using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationTagsSearchDTO
    {
        public long tagId { get; set; }
        public string title { get; set; }
        public PublicationSearchDTO publications { get; set; }
        public List<TagTranslationDataGetterDTO> tagsTranslations { get; set; }
    }
}
