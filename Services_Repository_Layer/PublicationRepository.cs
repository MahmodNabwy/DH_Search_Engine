using Context.Models;
using DTOS.dtos;
using IServices_Repository_Layer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Repository_Layer
{
    public class PublicationRepository : GenericRepository<CapmasTestContext, Publication>, IPublicationRepository
    {
        private readonly CapmasTestContext _db;
        public PublicationRepository(CapmasTestContext db)
        {
            _db = db;
        }

        public async Task<Tuple<List<PublicationSearchDTO>, List<PublicationTagsSearchDTO>>> FullContentTitleSearch(string searchTerm)
        {

            var engine = new PublicationFullContentSearch();
            List<PublicationGetterDTO> listOfPublications = await _db.Publications
                                                         .AsNoTracking()
                                                         .Where(q => q.IsPublished && !q.IsDeleted && q.PublishDate <= DateTime.Now)
                                                         .Select(c => new PublicationGetterDTO
                                                         {
                                                             Id = c.Id,
                                                             Name = c.Name.ToLower(),
                                                             PeriodicId = c.PeriodicId,
                                                             SubSubjectId = c.SubsubjectId,
                                                             PublicationTypeId = c.PublicationTypeId,
                                                         })
                                                         .ToListAsync();

            var listOfPublicationTags = await _db.PublicationTags.AsNoTracking()
                                                                  .Where(c => c.IsDeleted == false)
                                                                  .ToListAsync();

            var listOfTags = await _db.Tags.AsNoTracking()
                                           .Where(c => !c.IsDeleted)
                                           .ToListAsync();

            engine.AddPublicationsToIndex(listOfPublications);
            var result = engine.FullSearch(searchTerm, listOfTags, listOfPublications, listOfPublicationTags);
            var fullResult = new List<PublicationSearchDTO>();
            var finalResult = Tuple.Create(new List<PublicationSearchDTO>(), new List<PublicationTagsSearchDTO>());
            var periodicData = new PeriodicDataGetterDTO();
            var publicationTypeData = new PublicationTypeDataGetterDTO();
            var subjectData = new MainSubjectSearchDataGetterDTO();
            var periodicTranslation = new List<PeriodicTranslationDataGetterDTO>();
            var publicationTranslations = new List<PublicationTranslationDataGetterDTO>();
            var subjectTranslations = new List<SubjectTranslationDataGetterDTO>();

            foreach (var item in result.Item1)
            {
                var publicationDetails = await _db.PublicationDetails
                                            .AsNoTracking()
                                            .Where(c => c.PublicationId == item.Id)
                                            .ToListAsync();

                #region Periodic
                var publicationPeriodic = await _db.Periodics
                                                         .AsNoTracking()
                                                         .Where(c => c.Id == item.PeriodicId)
                                                         .Select(c => new PeriodicDataGetterDTO
                                                         {
                                                             Id = c.Id,
                                                             Name = c.Name,
                                                         }).FirstOrDefaultAsync();
                if (publicationPeriodic is not null)
                {

                    periodicTranslation = await _db.PeriodicTranslations
                                                      .AsNoTracking()
                                                      .Where(c => c.PeriodicId == publicationPeriodic.Id)
                                                      .Select(c => new PeriodicTranslationDataGetterDTO
                                                      {
                                                          Id = c.Id,
                                                          Locale = c.Locale,
                                                          Name = c.Name
                                                      }).ToListAsync();
                }
                #endregion

                #region Publication Translations
                publicationTranslations = await _db.PublicationTranslations
                                                         .AsNoTracking()
                                                         .Where(c => c.PublicationId == item.Id)
                                                         .Select(c => new PublicationTranslationDataGetterDTO
                                                         {
                                                             Id = c.Id,
                                                             Locale = c.Locale,
                                                             Name = c.Name,
                                                         }).ToListAsync();
                #endregion

                #region Publication Types
                var publicationType = await _db.PublicationTypes
                                               .AsNoTracking()
                                               .Where(c => c.Id == item.PublicationTypeId)
                                               .Select(c => new PublicationTypeDataGetterDTO
                                               {
                                                   Id = c.Id,
                                                   Name = c.Name,
                                               }).FirstOrDefaultAsync();
                var publicationTypesTranslations = new List<PublicationTypeTranslationDataGetterDTO>();

                if (publicationType is not null)
                {
                    publicationTypesTranslations = await _db.PublicationTypeTranslations
                                                            .AsNoTracking()
                                                            .Where(c => c.PublicationTypeId == publicationType.Id)
                                                            .Select(c => new PublicationTypeTranslationDataGetterDTO
                                                            {
                                                                Id = c.Id,
                                                                Locale = c.Locale,
                                                                Name = c.Name,
                                                            }).ToListAsync();

                }
                #endregion

                #region Subject
                var publicationSubjects = await _db.Subjects
                                             .AsNoTracking()
                                             .Where(c => c.Id == item.SubSubjectId)
                                             .Select(c => new MainSubjectSearchDataGetterDTO
                                             {
                                                 subSubjectId = c.Id,
                                                 MainSubjectId = c.MainSubjectId,
                                                 Title = c.Title,

                                             }).FirstOrDefaultAsync();
                if (publicationSubjects is not null)
                {

                    subjectTranslations = await _db.SubjectTranslations
                                                   .AsNoTracking()
                                                   .Where(c => c.SubjectId == publicationSubjects.subSubjectId)
                                                   .Select(c => new SubjectTranslationDataGetterDTO
                                                   {
                                                       Id = c.Id,
                                                       Locale = c.Locale,
                                                       Title = c.Title
                                                   }).ToListAsync();
                }
                #endregion


                foreach (var release in publicationDetails)
                {

                    finalResult.Item1.Add(new PublicationSearchDTO
                    {
                        Name = item.Name,
                        PublicationDetail = new PublicationDetailDataGetterDTO
                        {
                            Id = release.Id,
                            PublicationCover = release.PublicationCover,
                            ExcelUrl = release.ExcelUrl,
                            Month = release.Month,
                            PdfUrl = release.PdfUrl,
                            PublishDate = release.PublishDate ?? DateTime.Now,
                            Quarter = release.Quarter,
                            ReleaseDate = release.ReleaseDate,
                            Year = release.Year,
                        },
                        Periodic = publicationPeriodic != null ? new PeriodicDataGetterDTO
                        {
                            Id = publicationPeriodic.Id,
                            Name = publicationPeriodic.Name,
                            PeriodicTranslations = periodicTranslation
                        } : periodicData,

                        PublicationType = publicationType != null ? new PublicationTypeDataGetterDTO
                        {
                            Id = publicationType.Id,
                            Name = publicationType.Name,
                            PublicationTypeTranslations = publicationTypesTranslations
                        } : publicationTypeData,
                        PublicationTranslations = publicationTranslations,
                        Subject = publicationSubjects != null ? new MainSubjectSearchDataGetterDTO
                        {
                            subSubjectId = publicationSubjects.subSubjectId,
                            MainSubjectId = publicationSubjects.MainSubjectId,
                            Title = publicationSubjects.Title,
                            SubjectTranslations = subjectTranslations
                        } : subjectData
                    });
                }

            }

            foreach (var item in result.Item2)
            {
                var tagPublications = await _db.PublicationTags
                                         .AsNoTracking()
                                         .Where(c => c.TagId == item.tagId)
                                         .Select(c => new PublicationTagsSearchDTO
                                         {
                                             tagId = c.TagId,
                                             title = c.Tag.TagName,
                                             publications = new PublicationSearchDTO
                                             {
                                                 publicationId = c.PublicationId,
                                                 title = c.Publication.Name
                                             }
                                         }).ToListAsync();
                foreach (var pub_tag in tagPublications)
                {
                    var tagsTranslations = await _db.TagTranslations
                                              .AsNoTracking()
                                              .Where(c => pub_tag.tagId == c.TagId)
                                              .Select(c => new TagTranslationDataGetterDTO
                                              {
                                                  Id = c.Id,
                                                  Locale = c.Locale,
                                                  TagName = c.TagName
                                              }).ToListAsync();

                    finalResult.Item2.Add(new PublicationTagsSearchDTO
                    {
                        title = pub_tag.title,
                        tagId = pub_tag.tagId,
                        publications = pub_tag.publications,
                        tagsTranslations = tagsTranslations

                    });

                }
            }

            return finalResult;
        }


        public async Task<List<PublicationGlobalSearchDTO>> SuggestSearch(string searchTerm)
        {
            List<PublicationSearchDTO> listOfPublications = await _db.Publications.AsNoTracking()
                                                           .Where(q => q.IsPublished && !q.IsDeleted && q.PublishDate <= DateTime.Now)
                                                           .Select(c => new PublicationSearchDTO
                                                           {
                                                               publicationId = c.Id,
                                                               title = c.Name.ToLower(),
                                                           })
                                                           .ToListAsync();

            var listOfPublicationTags = await _db.PublicationTags.AsNoTracking()
                                                                  .Where(c => c.IsDeleted == false)
                                                                  .ToListAsync();

            var listOfTags = await _db.Tags.AsNoTracking()
                                           .Where(c => !c.IsDeleted)
                                           .ToListAsync();


            var index = new PublicationSuggestSearch("title", 1000000);

            index.AddListForPublication(listOfPublications);
            var matches = index.PublicationSearch(searchTerm, listOfTags, listOfPublications, listOfPublicationTags);


            return matches;
        }

    }
}
