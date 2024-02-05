using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record PublicationSearchDTO
    {
        public long publicationId { get; set; }
        public string title { get; init; }
        public string Name { get; init; }
        public MainSubjectSearchDataGetterDTO Subject { get; set; }
        public PeriodicDataGetterDTO Periodic { get; set; }
        public PublicationDetailDataGetterDTO PublicationDetail { get; set; }
        public PublicationTypeDataGetterDTO PublicationType { get; set; }
        public List<PublicationTranslationDataGetterDTO> PublicationTranslations { get; set; }
        public List<PublicationTagDataGetterDTO> PublicationTags { get; set; }
    }
}
