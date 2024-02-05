using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class IndicatorsGlobalSearchDTO
    {
        public long id { get; init; }

        public string name { get; init; }
        public long? SubSubjectId { get; init; }
        public long? mainSubjectId { get; init; }

        public List<LookupTranslationGetterDTO> Translations { get; set; }
    }
}
