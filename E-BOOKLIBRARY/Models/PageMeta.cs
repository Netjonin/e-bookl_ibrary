﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public class PageMeta
    {
        public int Page { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}
