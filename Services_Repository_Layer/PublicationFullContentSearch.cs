using Context.Models;
using DTOS.dtos;
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
    public class PublicationFullContentSearch
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private readonly StandardAnalyzer _analyzer;
        private readonly RAMDirectory _directory;
        private readonly IndexWriter _writer;
        private readonly QueryParser _queryParser;
        public PublicationFullContentSearch()
        {
            _analyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            var config = new IndexWriterConfig(version, _analyzer);
            _writer = new IndexWriter(_directory, config);
            _queryParser = new QueryParser(LuceneVersion.LUCENE_48, "Name", _analyzer);
        }
        public void AddPublicationsToIndex(List<PublicationGetterDTO> publication)
        {
            foreach (var item in publication)
            {
                _writer.AddDocument(new Document
                {
                    new TextField(nameof(item.Name), item.Name, Field.Store.YES),
                    new TextField(nameof(item.Id), item.Id.ToString(), Field.Store.YES),

                });

            }
            _writer.Commit();
        }

        public Tuple<List<PublicationGetterDTO>, List<PublicationTagsSearchDTO>> FullSearch(string searchTerm, List<Tag> listOfTags, List<PublicationGetterDTO> publications, List<PublicationTag> publicationTags)
        {
            var list = new List<PublicationGlobalSearchDTO>();
            var resultFromPublications = new List<PublicationGetterDTO>();
            var resultFromTags = new List<PublicationTagsSearchDTO>();
            var directoryReader = DirectoryReader.Open(_directory);
            var indexSearcher = new IndexSearcher(directoryReader);
            var fuzzyQuery = new FuzzyQuery(new Term("Name", searchTerm), 2);
            var queryParser2 = _queryParser.Parse(searchTerm);
            var booleanQuery = new BooleanQuery
            {
                 { queryParser2, Occur.SHOULD },
                 { fuzzyQuery, Occur.SHOULD }
            };
            var hits = indexSearcher.Search(booleanQuery, 100000).ScoreDocs;
            List<string> searchValues = new List<string>();
            foreach (var item in hits)
            {
                var document = indexSearcher.Doc(item.Doc);
                foreach (var field in document.Fields)
                {
                    searchValues.Add(field.GetStringValue());
                }

            }
            foreach (var item in searchValues)
            {

                var publicationFilters = publications
                                               .OrderByDescending(s => s.Id)
                                               .Where(c => c.Name.Contains(item, StringComparison.OrdinalIgnoreCase))
                                               .Select(s => new PublicationGetterDTO
                                               {
                                                   Id = s.Id,
                                                   Name = s.Name,
                                                   PeriodicId = s.PeriodicId,
                                                   PublicationTypeId = s.PublicationTypeId,
                                                   SubSubjectId = s.SubSubjectId,
                                               }).ToList();
                resultFromPublications.AddRange(publicationFilters);

                var tags = listOfTags
                             .OrderByDescending(c => c.Id)
                             .Where(c => c.TagName.Contains(item, StringComparison.OrdinalIgnoreCase))
                             .Select(s => new PublicationTagsSearchDTO
                             {
                                 tagId = s.Id,
                                 title = s.TagName,

                             }).ToList();
                resultFromTags.AddRange(tags);


            }
            var uniquePublicationResults = resultFromPublications.Select(c => c.Id).Distinct();
            var uniqueTagsResults = resultFromTags.Select(c => c.tagId).Distinct();

            resultFromPublications = publications
                                               .OrderByDescending(s => s.Id)
                                               .Where(c => uniquePublicationResults.Contains(c.Id))
                                               .Select(s => new PublicationGetterDTO
                                               {
                                                   Id = s.Id,
                                                   Name = s.Name,
                                                   PeriodicId = s.PeriodicId,
                                                   PublicationTypeId = s.PublicationTypeId,
                                                   SubSubjectId = s.SubSubjectId,
                                               }).Take(5).ToList();

            resultFromTags = listOfTags
                             .OrderByDescending(c => c.Id)
                             .Where(c => uniqueTagsResults.Contains(c.Id))
                             .Select(s => new PublicationTagsSearchDTO
                             {
                                 tagId = s.Id,
                                 title = s.TagName,

                             }).Take(5).ToList();


            return Tuple.Create(resultFromPublications, resultFromTags);
        }


    }
}
