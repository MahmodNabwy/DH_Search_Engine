using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record SuggestSearchDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public int type { get; set; }
        public long? subSubjectId { get; set; }
        public long? mainSubjectId { get; set; }
    }
}
