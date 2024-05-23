using Context.Models;
using DTOS.dtos;
using Lucene.Net.Analysis;
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
    public class PublicationSuggestSearch : IDisposable
    {
        const LuceneVersion Version = LuceneVersion.LUCENE_48;
        RAMDirectory Directory;
        Lucene.Net.Analysis.Analyzer Analyzer;
        IndexWriterConfig WriterConfig;
        public string FieldName { get; }
        public int MaxResults { get; set; }
        public PublicationSuggestSearch(string fieldName, int maxResults)
        {
            FieldName = fieldName;
            MaxResults = maxResults;
            Directory = new RAMDirectory();
            Analyzer = new EnglishAnalyzer(Version);
            WriterConfig = new IndexWriterConfig(Version, Analyzer);
            WriterConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
        }
        private void PublicationIndexDoc(IndexWriter writer, PublicationSearchDTO term)
        {
            Document doc = new Document();

            doc.Add(new StringField(nameof(term.title), term.title, Field.Store.YES));

            doc.Add(new StringField(nameof(term.publicationId), term.publicationId.ToString(), Field.Store.YES));
            writer.AddDocument(doc);
            writer.Commit();
        }
        public void AddListForPublication(IEnumerable<PublicationSearchDTO> publications)
        {
            using (var writer = new IndexWriter(Directory, WriterConfig))
            {
                foreach (var term in publications)
                {
                    PublicationIndexDoc(writer, term);

                }
            }
        }
        public List<PublicationGlobalSearchDTO> PublicationSearch(string term, List<Tag> listOfTags, List<PublicationSearchDTO> publications, List<PublicationTag> publicationTags)
        {

            var result = new List<PublicationGlobalSearchDTO>();
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
                    var resultFromPublications = publications
                                            .OrderByDescending(s => s.publicationId)
                                            .Where(c => c.title.Contains(item.ToLower(), StringComparison.OrdinalIgnoreCase))
                                            .Select(s => new PublicationSearchDTO
                                            {
                                                publicationId = s.publicationId,
                                                title = s.title,
                                            }).Take(5).ToList();

                    var tags = listOfTags
                                        .OrderByDescending(c => c.Id)
                                        .Where(c => c.TagName.Contains(item.ToLower(), StringComparison.OrdinalIgnoreCase))
                                        .Select(s => new PublicationTagsSearchDTO
                                        {
                                            tagId = s.Id,
                                            title = s.TagName,
                                        }).Take(5).ToList();

                    var resultFromTags = new List<PublicationTagSuggestSearchDTO>();
                    foreach (var tag in tags)
                    {
                        resultFromTags.Add(new PublicationTagSuggestSearchDTO
                        {
                            tagId = tag.tagId,
                            title = tag.title,
                            publicationId = publicationTags.Where(c => c.TagId == tag.tagId)
                                                           .Select(c => c.PublicationId)
                                                           .FirstOrDefault()
                        });
                    }

                    result.Add(new PublicationGlobalSearchDTO
                    {
                        publicationTags = resultFromTags,
                        publications = resultFromPublications,

                    });
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
