﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Application.Common.Requests
{
    public class BooksPagedFilterRequest
    {
        public string? Genre { get; set; }
        public string? Title { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
