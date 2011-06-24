using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Husk.Model
{
    public class JobViewModel
    {
        public dynamic Job { get; set; }
    }

    public class JobsViewModel
    {
        public IEnumerable<dynamic> Jobs { get; set; }
    }
}