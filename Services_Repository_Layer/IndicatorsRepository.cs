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
    public class IndicatorsRepository : GenericRepository<CapmasTestContext, Indicator>, IIndicatorsRepository
    {
        private readonly CapmasTestContext _db;
        public IndicatorsRepository(CapmasTestContext db)
        {
            _db = db;
        }

        public async Task<List<SuggestSearchDTO>> SuggestSearch(string searchTerm)
        {


            var publicationIndicators = await _db.PublicationIndicators
                                      .AsNoTracking()
                                      .Where(c => !c.IsDeleted && c.IsPublished)
                                      .Select(c => c.IndicatorId)
                                      .ToListAsync();


            var listOfIndicators = await (from q in _db.Indicators.AsNoTracking()
                                          where publicationIndicators.Contains(q.Id)
                                          let subjects = _db.PublicationIndicators.AsNoTracking()
                                                                                                .Where(c => c.IndicatorId == q.Id)
                                                                                                .Select(c => new SubjectsDTO
                                                                                                {
                                                                                                    mainSubjectId = c.PublicationDetail.Publication.Subsubject.MainSubjectId,
                                                                                                    subSubjectId = c.PublicationDetail.Publication.SubsubjectId
                                                                                                }).FirstOrDefault()
                                          select new IndicatorSuggestSearchDTO
                                          {
                                              id = q.Id,
                                              title = q.Name.ToLower(),
                                              mainSubjectId = subjects.mainSubjectId,
                                              SubSubjectId = subjects.subSubjectId,
                                              type = 3,

                                          }).ToListAsync();

            var index = new IndicatorSuggestSearch("title", 1000000);
            index.AddListForIndicator(listOfIndicators);
            var matches = index.IndicatorSearch(searchTerm, listOfIndicators);

            return matches;

        }
        public async Task<List<IndicatorsGlobalSearchDTO>> FullContentTitleSearch(string searchTerm)
        {
            var engine = new IndicatorFullContentSearch();
            var publicationIndicators = await _db.PublicationIndicators
                                    .AsNoTracking()
                                    .Where(c => !c.IsDeleted && c.IsPublished)
                                    .Select(c => c.IndicatorId)
                                    .Distinct()
                                    .ToListAsync();

            var listOfIndicators = await (from q in _db.Indicators.AsNoTracking()
                                          where publicationIndicators.Contains(q.Id)
                                          let subjects = _db.PublicationIndicators
                                                             .AsNoTracking()
                                                             .Where(c => c.IndicatorId == q.Id)
                                                             .Select(c => new SubjectsDTO
                                                             {
                                                                 //SubSubject Refer To Subject Table -__-
                                                                 mainSubjectId = c.PublicationDetail.Publication.Subsubject.MainSubjectId,
                                                                 subSubjectId = c.PublicationDetail.Publication.SubsubjectId
                                                             }).FirstOrDefault()
                                          let translations = _db.IndicatorTranslations
                                                                .AsNoTracking()
                                                                .Where(c => c.IndicatorId == q.Id)
                                                                .Select(c => new LookupTranslationGetterDTO
                                                                {
                                                                    Locale = c.Locale,
                                                                    Name = c.Name
                                                                }).ToList()

                                          select new IndicatorsGlobalSearchDTO
                                          {
                                              id = q.Id,
                                              name = q.Name.ToLower(),
                                              mainSubjectId = subjects.mainSubjectId,
                                              SubSubjectId = subjects.subSubjectId,
                                              Translations = translations
                                          }).ToListAsync();
            engine.AddIndicatorsToIndex(listOfIndicators);
            var resuls = engine.FullSearch(searchTerm, listOfIndicators);
            return resuls;

        }
    }
}
