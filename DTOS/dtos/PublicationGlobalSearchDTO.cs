using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record PublicationGlobalSearchDTO
    {
        public List<PublicationSearchDTO> publications { get; init; }
        public List<PublicationTagSuggestSearchDTO> publicationTags { get; init; }
    }
}
