using Context.Models;
using DTOS.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices_Repository_Layer
{
    public interface IIndicatorsRepository : IGenericRepository<Indicator>
    {
        Task<List<SuggestSearchDTO>> SuggestSearch(string searchTerm);
        Task<List<IndicatorsGlobalSearchDTO>> FullContentTitleSearch(string searchTerm);
    }
}
