﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class ClientVM:AppUserVM
    {
        public List<CaseVM> Cases { get; set; }
    }
}