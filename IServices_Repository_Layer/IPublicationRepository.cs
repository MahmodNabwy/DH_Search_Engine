using Context.Models;
using DTOS.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices_Repository_Layer
{
    public interface IPublicationRepository : IGenericRepository<Publication>
    {
        Task<List<PublicationGlobalSearchDTO>> SuggestSearch(string searchTerm);
        Task<Tuple<List<PublicationSearchDTO>, List<PublicationTagsSearchDTO>>> FullContentTitleSearch(string searchTerm);
    }
}
