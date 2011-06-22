//
//  Copyright Info
//

using System.Collections.Generic;

namespace MVC_Husk.Model
{
    public class UsersViewModel
    {
        public IEnumerable<dynamic> People { get; set; }
        public dynamic CurrentUser { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class UserViewModel
    {
        public dynamic Person { get; set; }
    }
}