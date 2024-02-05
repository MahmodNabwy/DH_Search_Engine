using Context.Models;
using DTOS.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices_Repository_Layer
{
    public interface INewsRepository : IGenericRepository<Publication>
    {
        Task<List<NewsContentTitleDTO>> ListAllNews();
        Task<List<NewsSearchGetterDTO>> FullContentSearch(string searchTerm);
        Task<List<SuggestSearchDTO>> SuggestSearch(string searchTerm);

    }
}
