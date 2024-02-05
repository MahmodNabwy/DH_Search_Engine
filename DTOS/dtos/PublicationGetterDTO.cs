using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationGetterDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long PeriodicId { get; set; }
        public long PublicationTypeId { get; set; }
        public long PublicAdministrationId { get; set; }
        public long SubSubjectId { get; set; }
        public long YearTypeId { get; set; }

        public decimal? CDPrice { get; set; }
        public decimal? PaperPrice { get; set; }
        public DateTime? NextPublishDate { get; set; }
        public List<PublicationTagGetterDTO> PublicationTags { get; set; }
        public List<PublicationTranslationGetterDTO> PublicationTranslations { get; set; }
    }
}
