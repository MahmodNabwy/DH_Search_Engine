

using DTOS.dtos;
using Lucene.Net.Analysis.En;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Repository_Layer
{
    public class NewsSuggestSearch : IDisposable
    {
        const LuceneVersion Version = LuceneVersion.LUCENE_48;
        RAMDirectory Directory;
        Lucene.Net.Analysis.Analyzer Analyzer;
        IndexWriterConfig WriterConfig;
        //private readonly CapmasDBContext _db;
        public string FieldName { get; }
        public int MaxResults { get; set; }

        public NewsSuggestSearch(string fieldName, int maxResults)
        {
            FieldName = fieldName;
            MaxResults = maxResults;
            Directory = new RAMDirectory();
            Analyzer = new EnglishAnalyzer(Version);
            WriterConfig = new IndexWriterConfig(Version, Analyzer);
            WriterConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
        }

        private void IndexDoc(IndexWriter writer, NewsContentTitleDTO term)
        {
            Document doc = new Document();

            doc.Add(new StringField(nameof(term.title), term.title, Field.Store.YES));
            doc.Add(new StringField(nameof(term.content), term.content, Field.Store.YES));
            doc.Add(new StringField(nameof(term.id), term.id.ToString(), Field.Store.YES));
            writer.AddDocument(doc);
            writer.Commit();

        }

        public void AddList(IEnumerable<NewsContentTitleDTO> news)
        {
            using (var writer = new IndexWriter(Directory, WriterConfig))
            {
                foreach (var term in news)
                {
                    IndexDoc(writer, term);

                }
            }
        }

        public List<SuggestSearchDTO> Search(string term, List<NewsContentTitleDTO> listOfNews)
        {
            var result = new List<SuggestSearchDTO>();
            using (var reader = DirectoryReader.Open(Directory))
            {
                IndexSearcher searcher = new IndexSearcher(reader);
                var query = new PrefixQuery(new Term(FieldName, term));
                TopDocs foundDocs = searcher.Search(query, MaxResults);

                //Get Matched strings
                var matches = foundDocs.ScoreDocs
                    .Select(scoreDoc => searcher.Doc(scoreDoc.Doc).Get(FieldName))
                    .ToArray();
                foreach (var item in matches)
                {
                    var filteredNews = listOfNews
                         .Where(s => s.title.Contains(item, StringComparison.OrdinalIgnoreCase) ||
                                s.content.Contains(item, StringComparison.OrdinalIgnoreCase))
                         .OrderByDescending(s => s.id)
                         .Select(s => new SuggestSearchDTO
                         {
                             id = s.id,
                             title = s.title,
                             type = 1
                         })
                         .Take(3).ToList();

                    foreach (var news in filteredNews)
                    {
                        result.Add(news);
                    }

                }

                return result;
            }
        }

        public void Dispose()
        {
            Directory.Dispose();
            Analyzer.Dispose();
        }

    }
}
