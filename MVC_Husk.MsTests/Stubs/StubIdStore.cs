using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC_Husk.Infrastructure.IdStore;

namespace MVC_Husk.MsTests.Stubs
{
    /// <summary>
    /// Stubbed out ID Store used by Forms Authentication
    /// </summary>
    public class StubIdStore : IIdStore
    {
        string _id;

        public void SetClientAccess(string token)
        {
            _id = token;
        }

        public void RemoveClientAccess()
        {
            _id = string.Empty;
        }

        public string GetUserId()
        {
            return _id;
        }
    }
}
