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
    public class NewsFullContentSearch
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private readonly StandardAnalyzer _analyzer;
        private readonly RAMDirectory _directory;
        private readonly IndexWriter _writer;
        private readonly QueryParser _queryParser;

        public NewsFullContentSearch()
        {
            _analyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            var config = new IndexWriterConfig(version, _analyzer);
            _writer = new IndexWriter(_directory, config);
            _queryParser = new QueryParser(LuceneVersion.LUCENE_48, "content", _analyzer);
        }
        public void AddNewsToIndex(List<NewsContentTitleDTO> news)
        {
            foreach (var item in news)
            {
                _writer.AddDocument(new Document
                {
                    new TextField(nameof(item.title), item.title, Field.Store.YES),
                    new TextField(nameof(item.content), item.content, Field.Store.YES),
                });

            }
            _writer.Commit();
        }

        public List<NewsGlobalSearchDTO> Search(string searchTerm, List<NewsContentTitleDTO> listOfNews)
        {
            var finalResult = new List<NewsGlobalSearchDTO>();
            var directoryReader = DirectoryReader.Open(_directory);
            var indexSearcher = new IndexSearcher(directoryReader);

            var fuzzyQuery = new FuzzyQuery(new Term("title", searchTerm), 2);
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

                var filteredResult = listOfNews
                      .Where(s => s.title.Contains(item, StringComparison.OrdinalIgnoreCase) ||
                      s.content.Contains(item, StringComparison.OrdinalIgnoreCase))
                      .OrderByDescending(s => s.id)
                      .Select(s => new NewsGlobalSearchDTO
                      {
                          id = s.id,
                          title = s.title,
                          content = s.content,

                      }).ToList();

                finalResult.AddRange(filteredResult);

            }
            var uniqueValues = finalResult.Select(c => c.id).Distinct();

            finalResult = listOfNews.Where(c => uniqueValues.Contains(c.id))
                        .OrderByDescending(s => s.id)
                        .Select(s => new NewsGlobalSearchDTO
                        {
                            id = s.id,
                            title = s.title,

                        })
                        .Take(3).ToList();

            return finalResult;
        }

    }
}
