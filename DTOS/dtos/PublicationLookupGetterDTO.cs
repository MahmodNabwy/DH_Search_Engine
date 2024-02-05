using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationLookupGetterDTO
    {
        public long id { get; set; }
        public List<PublicationTranslationDataGetterDTO> PublicationTranslations { get; set; }

    }
}
