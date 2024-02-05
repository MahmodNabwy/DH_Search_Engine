using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class StudyPeriodDataGetterDTO  
    {
        public string Name { get; set; }
        public long id { get; set; }
        public List<StudyPeriodTranslationDataGetterDTO> StudyPeriodTranslations { get; set; }
    }
}
