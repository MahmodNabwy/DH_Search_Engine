using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationTagDataGetterDTO
    {
        public long id { get; set; }
        public TagDataGetterDTO Tag { get; set; }
        public PublicationDataGetterDTO Publication { get; set; }
    }
}
