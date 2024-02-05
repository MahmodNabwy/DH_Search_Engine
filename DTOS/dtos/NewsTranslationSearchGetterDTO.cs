using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class NewsTranslationSearchGetterDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Brief { get; set; }
        public string Locale { get; set; }
        
    }
}
