﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Model.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
