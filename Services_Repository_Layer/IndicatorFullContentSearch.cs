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
    public class IndicatorFullContentSearch
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private readonly StandardAnalyzer _analyzer;
        private readonly RAMDirectory _directory;
        private readonly IndexWriter _writer;
        private readonly QueryParser _queryParser;
        public IndicatorFullContentSearch()
        {
            _analyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            var config = new IndexWriterConfig(version, _analyzer);
            _writer = new IndexWriter(_directory, config);
            _queryParser = new QueryParser(LuceneVersion.LUCENE_48, "name", _analyzer);
        }
        public void AddIndicatorsToIndex(List<IndicatorsGlobalSearchDTO> news)
        {
            foreach (var item in news)
            {
                _writer.AddDocument(new Document
                {
                    new TextField(nameof(item.name), item.name, Field.Store.YES),
                    new TextField(nameof(item.id), item.id.ToString(), Field.Store.YES),

                });

            }
            _writer.Commit();
        }

        public List<IndicatorsGlobalSearchDTO> FullSearch(string searchTerm, List<IndicatorsGlobalSearchDTO> indicators)
        {
            var finalResult = new List<IndicatorsGlobalSearchDTO>();
            var directoryReader = DirectoryReader.Open(_directory);
            var indexSearcher = new IndexSearcher(directoryReader);
            var fuzzyQuery = new FuzzyQuery(new Term("name", searchTerm), 2);
            var queryParser = _queryParser.Parse(searchTerm);
            var booleanQuery = new BooleanQuery
              {
                 { queryParser, Occur.SHOULD },
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
                var filteredIndicators = indicators
                  .Where(s => s.name.Contains(item, StringComparison.OrdinalIgnoreCase))
                  .OrderByDescending(s => s.id)
                  .Select(s => new IndicatorsGlobalSearchDTO
                  {
                      id = s.id,
                      name = s.name,
                      SubSubjectId = s.SubSubjectId,
                      mainSubjectId = s.mainSubjectId,
                      Translations = s.Translations
                  }).ToList();
                finalResult.AddRange(filteredIndicators);
            }
            var uniqueIndicatorsIds = finalResult.Select(c => c.id).Distinct();
            finalResult = indicators
                   .Where(s => uniqueIndicatorsIds.Contains(s.id))
                   .OrderByDescending(s => s.id)
                   .Select(s => new IndicatorsGlobalSearchDTO
                   {
                       id = s.id,
                       name = s.name,
                       SubSubjectId = s.SubSubjectId,
                       mainSubjectId = s.mainSubjectId,
                       Translations = s.Translations
                   })
                   .Take(15).ToList();

            return finalResult;
        }

    }
}
