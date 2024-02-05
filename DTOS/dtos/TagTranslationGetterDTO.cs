using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class TagTranslationGetterDTO
    {
        public string TagName { get; set; }
        public string Locale { get; set; }
        public long TagId { get; set; }
    }
}
