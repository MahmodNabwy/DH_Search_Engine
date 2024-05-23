using DTOS.dtos;
using IServices_Repository_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DH_Search_Engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly IIndicatorsRepository _indicatorsRepository;

        public HomeController(INewsRepository newsRepository, IPublicationRepository publicationRepository, IIndicatorsRepository indicatorsRepository)
        {
            _newsRepository = newsRepository;
            _publicationRepository = publicationRepository;
            _indicatorsRepository = indicatorsRepository;
        }

        [HttpGet]
        [Route("NewsSearchTest")]
        public async Task<IActionResult> NewsSearchTest(string searchTerm)
        {
            var result = await _newsRepository.SuggestSearch(searchTerm);
            return Ok(result);
        }

        /// <summary>
        /// Suggest Search Using Lucene_Search_Engine       
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("ContentTitleSearch")]
        public async Task<IActionResult> ContentTitleSearchAsync(string searchTerm)
        {
            List<bool> lIndicators = new List<bool>();
            var _holderOfDTO = new Dictionary<string, object>();

            try
            {

                var finalResponse = new List<SuggestSearchDTO>();
                var newsResult = await _newsRepository.SuggestSearch(searchTerm);
                var publicationTagsResult = await _publicationRepository.SuggestSearch(searchTerm);
                var indicatorsResult = await _indicatorsRepository.SuggestSearch(searchTerm);
                finalResponse.AddRange(newsResult);
                finalResponse.AddRange(indicatorsResult);

                foreach (var item in publicationTagsResult)
                {
                    foreach (var publication in item.publications)
                    {
                        finalResponse.Add(new SuggestSearchDTO
                        {
                            id = publication.publicationId,
                            title = publication.title,
                            type = 2


                        });
                    }
                    foreach (var tag in item.publicationTags)
                    {
                        finalResponse.Add(new SuggestSearchDTO
                        {
                            id = tag.tagId,
                            title = tag.title,
                            type = 2

                        });
                    }

                }
                _holderOfDTO.Add("data", finalResponse);
                lIndicators.Add(true);
            }
            catch (Exception ex)
            {

                throw;
            }
            var state = lIndicators.All(x => x);
            _holderOfDTO.Add("state", state);
            return Ok(_holderOfDTO);

        }

        /// <summary>
        /// Full Content Title Search Using  Lucene_Search_Engine  
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("FullContentTitleSearch")]
        public async Task<IActionResult> FullContentTitleSearchAsync(string searchTerm)
        {
            List<bool> lIndicators = new List<bool>();
            var _holderOfDTO = new Dictionary<string, object>();

            try
            {
                var newsResult = await _newsRepository.FullContentSearch(searchTerm);
                var publicationResults = await _publicationRepository.FullContentTitleSearch(searchTerm);
                var indicatorsResult = await _indicatorsRepository.FullContentTitleSearch(searchTerm);
                var finalResponse = new List<GlobalSearchDTO>();
                finalResponse.Add(new GlobalSearchDTO
                {
                    news = newsResult,
                    publications = new PublicationFullSearchDTO
                    {
                        publications = publicationResults.Item1,
                        publicationTags = publicationResults.Item2
                    },
                    indicators = indicatorsResult
                });

                _holderOfDTO.Add("data", finalResponse);


                lIndicators.Add(true);
            }
            catch (Exception ex)
            {
                throw;

            }
            var state = lIndicators.All(x => x);
            _holderOfDTO.Add("state", state);
            return Ok(_holderOfDTO);

        }


    }
}
