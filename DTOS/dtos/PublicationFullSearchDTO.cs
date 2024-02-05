using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record PublicationFullSearchDTO
    {
        public List<PublicationSearchDTO> publications { get; init; }
        public List<PublicationTagsSearchDTO> publicationTags { get; init; }
    }
}
