﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record PublicationTagSuggestSearchDTO
    {
        public long tagId { get; init; }
        public long publicationId { get; init; }

        public string title { get; init; }
    }
}
