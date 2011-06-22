//
//  Copyright Info
//

using System.Collections.Generic;

namespace MVC_Husk.Model
{
    public class SeasonalProductViewModel
    {
        public dynamic Product { get; set; }
    }

    public class SeasonalProductsViewModel
    {
        public IEnumerable<dynamic> Products { get; set; }
    }
}