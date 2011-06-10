using System.Collections.Generic;

namespace Template.Model
{
    public class UsersViewModel
    {
        public IEnumerable<dynamic> People { get; set; }
    }

    public class UserViewModel
    {
        public dynamic Person { get; set; }
    }
}