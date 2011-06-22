//
//  Copyright Info
//

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Model;

namespace MVC_Husk.MsTests
{
    [TestClass]
    public class MembershipSpecs
    {
        Users _users;

        public MembershipSpecs()
        {
            _users = new Users();
        }

        [TestInitialize]
        public void Init()
        {
            _users.Delete(where: "1=@0", args: 1);
        }

        [TestMethod]
        public void registration_should_not_accept_invalid_email_addresses()
        {
            var result = _users.Register("testattest.com", "password", "password");
            Assert.IsFalse(result.Success); 
        }

        [TestMethod]
        public void registration_should_not_accept_passwords_less_than_6_characters()
        {
            var result = _users.Register("test@test.com", "pass", "pass");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void registration_should_not_accept_passwords_and_confirmation_that_do_not_match()
        {
            var result = _users.Register("test@test.com", "password", "confirm");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void registration_should_only_accept_unique_email_addresses()
        {
            var result = _users.Register("test@test.com", "password", "password");
            var result2 = _users.Register("test@test.com", "password2", "password2");

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result2.Success);
        }

        [TestMethod]
        public void duplicate_email_address_should_return_duplicate_message()
        {
            var result = _users.Register("test@test.com", "password", "password");
            var result2 = _users.Register("test@test.com", "password2", "password2");

            Assert.IsFalse(result2.Success);
            Assert.AreEqual(result2.Message, "This user email address already exists");
        }

        [TestMethod]
        public void valid_user_should_be_registered()
        {
            var result = _users.Register("test@test.com", "password", "password");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, _users.All("RequirePasswordChange = 0").Count());
        }

        [TestMethod]
        public void valid_users_should_be_registered_with_password_change_if_required()
        {
            var result = _users.Register("test@test.com", "password", "password", true);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, _users.All(where: "RequirePasswordChange = 1").Count());
        }

        [TestMethod]
        public void valid_users_should_be_registered_with_email_if_required()
        {
            new Emails().Delete(where: "1 = 1");
            var result = _users.Register("test@test.com", "password", "password", true, true);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, new Emails().All(where: "EmailTo = 'test@test.com'").Count());
        }

        [TestMethod]
        public void valid_login_should_return_user()
        {
            _users.Register("test@test.com", "password", "password");
            var result = _users.Login("test@test.com", "password");

            Assert.IsTrue(result.Authenticated);
            Assert.AreEqual("test@test.com", result.User.Email);
        }

        [TestMethod]
        public void invalid_login_should_return_failure()
        {
            _users.Register("test@test.com", "password", "password");
            var result = _users.Login("test@test.com", "password2");

            Assert.IsFalse(result.Authenticated);
            Assert.AreEqual("Invalid Email or Password", result.Message);
        }

        [TestMethod]
        public void change_passwords_should_require_user_know_current_password()
        {
            _users.Register("test@test.com", "password", "password");
            var result = _users.ChangePassword("test@test.com", "messedup", "newPassword", "newPassword");

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void change_password_should_require_user_confirms_their_new_password()
        {
            _users.Register("test@test.com", "password", "password");
            var result = _users.ChangePassword("test@test.com", "password", "newPassword", "newWrongPassword");

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void successful_change_password_should_return_user_when_relogging_in()
        {
            _users.Register("test@test.com", "password", "password");
            var result = _users.ChangePassword("test@test.com", "password", "newPassword", "newPassword");

            Assert.IsTrue(result.Success);

            result = _users.Login("test@test.com", "newPassword");

            Assert.IsFalse(result.User.RequirePasswordChange);
            Assert.AreEqual("test@test.com", result.User.Email);
        }
    }
}
