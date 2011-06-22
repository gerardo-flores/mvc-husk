//
//  Copyright Info
//

using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Controllers;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;
using MVC_Husk.MsTests.Stubs;

namespace MVC_Husk.MsTests.ControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        Users _db;
        UserController _controller;
        AccountController _act;
     
        public UserControllerTests()
        {
            StubIdStore store = new StubIdStore();
            _db = new Users();
            _controller = new UserController(store, new NLogger());
            _act = new AccountController(store, new NLogger());
        }

        [TestInitialize]
        public void Init()
        {
            _db.Delete(where: "1=@0", args: 1);
        }

        [TestMethod]
        public void admin_page_should_not_be_available_to_non_admins()
        {
            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");

            var result = _controller.Index();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void admin_page_should_be_accessable_to_admins()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");

            var result = _controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void suspending_a_user_will_suspend_them_and_redirect_back_to_index()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");

            var userToSuspend = _db.Query("SELECT * FROM USERS WHERE IsActive = 1").FirstOrDefault();
            var result = _controller.Suspend(userToSuspend.ID.ToString());

            var suspended = _db.Single(userToSuspend.ID);
            Assert.IsFalse(suspended.IsActive);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void non_admin_will_redirect_to_home_suspend()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");

            var userToSuspend = _db.Query("SELECT * FROM USERS WHERE IsActive = 1").FirstOrDefault();
            var result = _controller.Suspend(userToSuspend.ID.ToString());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }

        [TestMethod]
        public void non_admin_will_redirect_to_home_activate()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");

            var userToActivate = _db.Query("SELECT * FROM USERS WHERE IsActive = 1").FirstOrDefault();
            var result = _controller.Activate(userToActivate.ID.ToString());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }

        [TestMethod]
        public void activating_a_user_will_enabled_them_and_redirect_back_to_index()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");

            var userToActivate = _db.Query("SELECT * FROM USERS WHERE IsActive = 1").FirstOrDefault();
            _db.Update(new { IsActive = 0 }, userToActivate.ID.ToString());            
            var result = _controller.Activate(userToActivate.ID.ToString());

            var activated = _db.Single(userToActivate.ID);
            Assert.IsTrue(activated.IsActive);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void on_delete_non_admin_will_redirect_to_home()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");

            var userToDelete = _db.Query("SELECT * FROM USERS WHERE IsAdmin = 0").FirstOrDefault();
            var result = _controller.Delete(userToDelete.ID.ToString());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void delete_will_remove_user()
        {
            register_a_bunch_of_users();

            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");

            var userToDelete = _db.Query("SELECT * FROM USERS WHERE Email = @0", "test01@test.com").FirstOrDefault();
            var result = _controller.Delete(userToDelete.ID.ToString());

            var deleted = _db.Single(userToDelete.ID);
            Assert.IsNull(deleted);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void create_new_user_view_should_redirect_if_not_admin()
        {
            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");
            var result = _controller.Create();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void create_new_user_view_should_show_view_if_admin()
        {
            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");
            var result = _controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void should_not_allow_none_admin_to_create_user()
        {
            _act.Register("test@test.com", "password", "password");
            _act.LogOn("test@test.com", "password");
            var result = _controller.Create("newTest@test.com");

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void create_user_should_create_new_user_and_redirect()
        {
            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");
            var result = _controller.Create("newTest@test.com");

            var user = _db.Query("SELECT * FROM USERS WHERE Email = @0", "newTest@test.com");

            Assert.AreEqual(1, user.Count());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void search_for_user_redirects_to_index()
        {
            _act.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _act.CurrentUser.ID);
            _act.LogOn("test@test.com", "password");

            var result = _controller.Search(search: "Test");
        }

        private void register_a_bunch_of_users()
        {
            _act.Register("test01@test.com", "password", "password");
            _act.Register("test02@test.com", "password", "password");
            _act.Register("test03@test.com", "password", "password");
            _act.Register("test04@test.com", "password", "password");
            _act.Register("test05@test.com", "password", "password");
            _act.Register("test06@test.com", "password", "password");
            _act.Register("test07@test.com", "password", "password");
            _act.Register("test08@test.com", "password", "password");
            _act.Register("test09@test.com", "password", "password");

            _act.Register("someother01@other.com", "password", "password");
            _act.Register("someother02@other.com", "password", "password");
            _act.Register("someother03@other.com", "password", "password");
            _act.Register("someother04@other.com", "password", "password");
            _act.Register("someother05@other.com", "password", "password");
            _act.Register("someother06@other.com", "password", "password");
            _act.Register("someother07@other.com", "password", "password");
            _act.Register("someother08@other.com", "password", "password");
            _act.Register("someother09@other.com", "password", "password");
        }
    }
}
