using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationTagGetterDTO
    {
        public TagGetterDTO Tag { get; set; }
        public PublicationGetterDTO Publication { get; set; }
    }
}
