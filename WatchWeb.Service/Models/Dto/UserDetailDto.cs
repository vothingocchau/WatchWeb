﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchWeb.Service.Models.Dto
{
    public class UserDetailDto : UserSimpleDto
    {
        public List<string> Roles { get; set; }
    }
}
