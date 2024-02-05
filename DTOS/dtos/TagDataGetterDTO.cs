using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class TagDataGetterDTO
    {
        public long Id { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<TagTranslationDataGetterDTO> TagTranslation { get; set; }
    }
}
