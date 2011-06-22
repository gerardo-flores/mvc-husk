using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Husk.Infrastructure.IdStore
{
    public interface IIdStore
    {
        void SetClientAccess(string token);
        void RemoveClientAccess();
        string GetUserId();
    }
}
