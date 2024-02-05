using Context.Models;
using DTOS.dtos;
using IServices_Repository_Layer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services_Repository_Layer
{
    public class NewsRepository : GenericRepository<CapmasTestContext, Publication>, INewsRepository
    {
        private readonly CapmasTestContext _db;
        public NewsRepository(CapmasTestContext db)
        {
            _db = db;

        }
        public async Task<List<NewsContentTitleDTO>> ListAllNews()
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);//Regex to remove anchor tags 
            var result = await _db.News.AsNoTracking()
                                   .Where(q => q.IsPublished && !q.IsDeleted && q.PublishDate <= DateTime.Now)
                                   .Select(c => new NewsContentTitleDTO
                                   {
                                       id = c.Id,
                                       title = c.Title.ToLower(),
                                       content = c.Content != null ? reg.Replace(c.Content.ToLower(), "").Replace("&nbsp;", " ") : "",
                                   })
                                   .ToListAsync();

            return result;
        }
        public async Task<List<NewsSearchGetterDTO>> FullContentSearch(string searchTerm)
        {

            var result = new List<NewsSearchGetterDTO>();
            var newsTranslations = new List<NewsTranslationSearchGetterDTO>();
            var engine = new NewsFullContentSearch();


            var newsList = await ListAllNews();

            engine.AddNewsToIndex(newsList);
            var engineResult = engine.Search(searchTerm, newsList);
            foreach (var item in engineResult)
            {
                var news = await _db.News.AsNoTracking()
                                    .Where(c => c.Id == item.id)
                                    .FirstOrDefaultAsync();
                if (news is not null)
                {
                    newsTranslations = await _db.NewsTranslations
                                          .AsNoTracking()
                                          .Where(c => c.NewsId == news.Id)
                                          .Select(c => new NewsTranslationSearchGetterDTO
                                          {
                                              Id = c.Id,
                                              Brief = c.Brief,
                                              Locale = c.Locale,
                                              Title = c.Title
                                          }).ToListAsync();

                    result.Add(new NewsSearchGetterDTO
                    {
                        Id = news.Id,
                        Brief = news.Brief,
                        Image = news.Image,
                        Title = news.Title,
                        Content = news.Content,
                        NewsTranslation = newsTranslations,
                        PublishDate = news.PublishDate

                    });
                }

            }
            return result;

        }

        public async Task<List<SuggestSearchDTO>> SuggestSearch(string searchTerm)
        {
            try
            {
                var news = await ListAllNews();
                var index = new NewsSuggestSearch("title", 1000000);

                index.AddList(news);

                var matches = index.Search(searchTerm, news);


                return matches;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                throw;
            }
        }
    }
}
