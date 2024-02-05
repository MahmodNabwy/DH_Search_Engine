using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class GlobalSearchDTO
    {
        public PublicationFullSearchDTO publications { get; set; }
        public List<NewsSearchGetterDTO> news { get; set; }
        public List<IndicatorsGlobalSearchDTO> indicators { get; set; }
    }
}
