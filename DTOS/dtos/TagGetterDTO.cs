using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class TagGetterDTO
    {
        public string TagName { get; set; }
        public virtual ICollection<TagTranslationGetterDTO> TagTranslation { get; set; }
    }
}
