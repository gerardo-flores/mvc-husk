//
//  Copyright Info
//

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Model;

namespace MVC_Husk.MsTests.Specs
{
    [TestClass]
    public class EmailSpecs
    {
        Emails _emails;

        public EmailSpecs()
        {
            _emails = new Emails();
        }

        [TestInitialize]
        public void Init()
        {
            _emails.Delete(where: "1=@0", args: 1);
        }

        [TestMethod]
        public void requires_valid_emailto_address()
        {
            var result = _emails.Send("testtest.com", "Sender@Sender.com", "Some Subject", "Some Message");

            Assert.AreEqual("Either the 'From' or 'To' Email address was invalid", result.Message);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void requires_valid_emailfrom_address()
        {
            var result = _emails.Send("test@test.com", "SenderSender.com", "Some Subject", "Some Message");

            Assert.AreEqual("Either the 'From' or 'To' Email address was invalid", result.Message);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void valid_data_should_queue_message_in_db()
        {
            var result = _emails.Send("test@test.com", "Sender@Sender.com", "Some Subject", "Some Message");

            Assert.AreEqual(1, _emails.All().Count());
            Assert.AreEqual("Email Queued Successfully", result.Message);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void send_thirdparty_setup_email_will_send_user_password()
        {
            var result = _emails.SendThirdPartySignup("customer@test.com", "1234567");

            Assert.AreEqual(1, _emails.All(where: "Message like '%1234567%'").Count());
            Assert.IsTrue(result.Success);
        }
    }
}
