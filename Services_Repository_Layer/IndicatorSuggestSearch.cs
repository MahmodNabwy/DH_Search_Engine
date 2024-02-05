using DTOS.dtos;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
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
    public class IndicatorSuggestSearch : IDisposable
    {
        const LuceneVersion Version = LuceneVersion.LUCENE_48;
        RAMDirectory Directory;
        Lucene.Net.Analysis.Analyzer Analyzer;
        private readonly StandardAnalyzer _analyzer;
        private readonly IndexWriter _writer;

        IndexWriterConfig WriterConfig;

        private readonly QueryParser _queryParser;

        public string FieldName { get; }
        public int MaxResults { get; set; }
        public IndicatorSuggestSearch(string fieldName, int maxResults)
        {

            FieldName = fieldName;
            MaxResults = maxResults;
            Directory = new RAMDirectory();
            Analyzer = new EnglishAnalyzer(Version);
            WriterConfig = new IndexWriterConfig(Version, Analyzer);
            WriterConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
            _analyzer = new StandardAnalyzer(Version);
            var config = new IndexWriterConfig(Version, _analyzer);
            //_writer = new IndexWriter(Directory, config);
            _queryParser = new QueryParser(LuceneVersion.LUCENE_48, "title", _analyzer);
        }
        private void IndicatorIndexDoc(IndexWriter writer, IndicatorSuggestSearchDTO term)
        {
            Document doc = new Document();

            doc.Add(new StringField(nameof(term.title), term.title, Field.Store.YES));

            doc.Add(new StringField(nameof(term.id), term.id.ToString(), Field.Store.YES));
            writer.AddDocument(doc);
            writer.Commit();
        }
        public void AddListForIndicator(IEnumerable<IndicatorSuggestSearchDTO> indicators)
        {

            using (var writer = new IndexWriter(Directory, WriterConfig))
            {
                foreach (var term in indicators)
                {
                    IndicatorIndexDoc(writer, term);

                }
            }
        }
        public List<SuggestSearchDTO> IndicatorSearch(string term, List<IndicatorSuggestSearchDTO> indicators)
        {
            var result = new List<SuggestSearchDTO>();
            using (var reader = DirectoryReader.Open(Directory))
            {
                IndexSearcher searcher = new IndexSearcher(reader);
                var query = new PrefixQuery(new Term(FieldName, term));
                TopDocs foundDocs = searcher.Search(query, MaxResults);

                //Get Matched values
                var matches = foundDocs.ScoreDocs
                    .Select(scoreDoc => searcher.Doc(scoreDoc.Doc).Get(FieldName))
                    .ToArray();
                foreach (var item in matches)
                {
                    var filteredIndicators = indicators
                        .Where(s => s.title.Contains(item.ToLower(), StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(s => s.id)
                        .Select(s => new SuggestSearchDTO
                        {
                            id = s.id,
                            title = s.title,
                            subSubjectId = s.SubSubjectId,
                            mainSubjectId = s.mainSubjectId,
                            type = 3
                        }).Take(15).ToList();
                    foreach (var indicator in filteredIndicators)
                    {
                        result.Add(indicator);
                    }
                }
            }
            return result;
        }
        public void Dispose()
        {
            Directory.Dispose();
            Analyzer.Dispose();
        }
    }
}
