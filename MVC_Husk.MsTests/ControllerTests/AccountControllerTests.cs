//
//  Copyright Info
//

using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Controllers;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;
using MVC_Husk.MsTests.Stubs;

namespace MVC_Husk.MsTests.ControllerTests
{
    [TestClass]
    public class AccountControllerTests
    {
        Users _db;
        AccountController _controller;

        public AccountControllerTests()
        {
            _db = new Users();
            _controller = new AccountController(new StubIdStore(), new NLogger());
        }

        [TestInitialize]
        public void Init()
        {
            _db.Delete(where: "1=@0", args: 1);
        }

        [TestMethod]
        public void register_action_should_redirect_with_valid_email_password()
        {
            var result = _controller.Register("test@test.com", "password", "password");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("ClickThru", ((ViewResult)result).ViewName);

        }

        [TestMethod]
        public void unsuccessful_registration_should_redirect_back_to_login_screen()
        {
            var result = _controller.Register("testattest.com", "password", "password");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void get_register_should_redirect_to_register()
        {
            var result = _controller.Register();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void login_should_redirect_with_valid_email_password_if_user_has_read_legal()
        {
            _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { IsFirstVisit = 0 }, _controller.CurrentUser.ID);
            var result = _controller.LogOn("test@test.com", "password");

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void login_should_redirect_with_to_change_password_if_required()
        {
            _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { RequirePasswordChange = 1 }, _controller.CurrentUser.ID);
            var result = _controller.LogOn("test@test.com", "password");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void login_should_redirect_to_legal_with_valid_email_password_and_first_visit()
        {
            _controller.Register("test@test.com", "password", "password");
            var result = _controller.LogOn("test@test.com", "password");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("ClickThru", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void unsuccessful_login_should_redirect_back_to_login_screen()
        {
            _controller.Register("test@test.com", "password", "password");
            var result = _controller.LogOn("test@test.com", "password2");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void get_login_should_redirect_to_register()
        {
            var result = _controller.LogOn();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void logging_off_should_redirect_to_login()
        {
            _controller.Register("test@test.com", "password", "password");
            var result = _controller.LogOff();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void logged_in_user_should_redirect_to_change_password_page()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");
            
            var result = _controller.ChangePassword();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void change_password_should_require_a_user_to_be_logged_in()
        {
            var result = _controller.ChangePassword("test@test.com", "password", "newPassword", "messedUp");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void change_password_should_require_the_same_user_to_be_logged_in()
        {
            _controller.Register("test@test.com", "password", "password");
            var result = _controller.ChangePassword("test2@test.com", "password", "newPassword", "messedUp");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void failed_change_password_should_redirect_to_change_password()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ChangePassword("test@test.com", "password", "newPassword", "messedUp");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void successful_change_password_should_redirect_to_terms_if_required()
        {
            _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { IsFirstVisit = 1 }, _controller.CurrentUser.ID);
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ChangePassword("test@test.com", "password", "newPassword", "newPassword");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void successful_change_password_should_redirect_to_homepage()
        {
            _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { IsFirstVisit = 0 }, _controller.CurrentUser.ID);
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ChangePassword("test@test.com", "password", "newPassword", "newPassword");

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void get_change_password_success_should_redirect_to_change_password_success()
        {
            var result = _controller.ChangePasswordSuccess();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void guests_should_not_look_like_admins()
        {
            _controller.LogOff();
            Assert.IsFalse(_controller.IsAdmin);
        }

        [TestMethod]
        public void non_admin_users_should_not_look_like_admins()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");

            Assert.IsFalse(_controller.IsAdmin);
        }

        [TestMethod]
        public void admin_users_should_look_like_admins()
        {
            var result = _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { IsAdmin = 1 }, _controller.CurrentUser.ID);
            _controller.LogOn("test@test.com", "password");

            Assert.IsTrue(_controller.IsAdmin);
        }

        [TestMethod]
        public void users_who_have_read_message_redirected_to_home()
        {
            _controller.Register("test@test.com", "password", "password");
            new Users().Update(new { IsFirstVisit = 0 }, _controller.CurrentUser.ID);
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ClickThru();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void users_who_have_not_read_message_are_shown_view()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ClickThru();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void users_who_have_not_accepted_terms_will_get_logged_out_and_redirected_home()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ClickThru(null);

            Assert.IsNull(_controller.CurrentUser);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void users_who_have_accepted_terms_will_get_updated_and_redirected_home()
        {
            _controller.Register("test@test.com", "password", "password");
            _controller.LogOn("test@test.com", "password");

            var result = _controller.ClickThru("accept");

            Assert.IsFalse(_controller.CurrentUser.IsFirstVisit);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
