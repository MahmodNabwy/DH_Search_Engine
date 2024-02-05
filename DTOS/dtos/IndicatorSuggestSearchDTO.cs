using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class IndicatorSuggestSearchDTO
    {
        public long id { get; init; }
        public long? SubSubjectId { get; set; }
        public long? mainSubjectId { get; set; }
        public string title { get; init; }
        public int type { get; init; }
    }
}
