using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationDataGetterDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long PeriodicId { get; set; }
        public long PublicationTypeId { get; set; }
        public long PublicAdministrationId { get; set; }
        public long SubSubjectId { get; set; }
        public long YearTypeId { get; set; }

        public DateTime? NextPublishDate { get; set; }
        public decimal? CDPrice { get; set; }
        public decimal? PaperPrice { get; set; }


        public List<PublicationTagDataGetterDTO> PublicationTags { get; set; }
        public List<PublicationTranslationGetterDTO> PublicationTranslations { get; set; }
    }
}
