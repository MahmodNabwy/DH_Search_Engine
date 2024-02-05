using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record NewsContentTitleDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
}
