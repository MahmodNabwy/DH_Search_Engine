using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class NewsSearchGetterDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Brief { get; set; }
        public string Image { get; set; }
        public DateTime? PublishDate { get; set; }
        public List<NewsTranslationSearchGetterDTO> NewsTranslation { get; set; }
        public PublicationLookupGetterDTO Publication { get; set; }
    }
}
