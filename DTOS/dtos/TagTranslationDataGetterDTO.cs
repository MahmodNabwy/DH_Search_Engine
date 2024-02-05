using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class TagTranslationDataGetterDTO
    {
        public long Id { get; set; }
        public string TagName { get; set; }
        public string Locale { get; set; }
    }
}
