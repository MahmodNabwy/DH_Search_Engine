using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PeriodicDataGetterDTO  
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<PeriodicTranslationDataGetterDTO> PeriodicTranslations { get; set; }
    }
}
