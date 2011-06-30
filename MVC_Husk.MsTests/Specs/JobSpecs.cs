using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Model;
using System.IO;

namespace MVC_Husk.MsTests.Specs
{
    [TestClass]
    public class JobSpecs
    {
        Jobs _jobs;
        long valid_user_id;

        public JobSpecs()
        {
            _jobs = new Jobs();
        }

        [TestInitialize]
        public void Init()
        {
            _jobs.Delete(where: "1=@0", args: 1);
            Users user = new Users();
            var result = user.Register("validtester@mckinsey.com", "password", "password");
            valid_user_id = (long)result.UserId;
        }

        [TestMethod]
        public void create_job_requieres_description()
        {
            var result = _jobs.CreateJob(null, 1, DateTime.Now, valid_user_id);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void create_job_requires_priority_between_higher_than_1_lower_than_5()
        {
            var result = _jobs.CreateJob("description", 0, DateTime.Now, valid_user_id);
            Assert.IsFalse(result.Success);

            var result1 = _jobs.CreateJob("description", 3, DateTime.Now, valid_user_id);
            Assert.IsTrue(result1.Success);

            var result2 = _jobs.CreateJob("description", 99, DateTime.Now, valid_user_id);
            Assert.IsFalse(result2.Success);
        }

        [TestMethod]
        public void valid_user_has_to_request_job()
        {
            var result = _jobs.CreateJob("description", 1, DateTime.Now, -11111);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void create_date_has_to_be_in_past()
        {
            var result = _jobs.CreateJob("description", 1, DateTime.Today.AddDays(1), valid_user_id);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void job_can_be_created_with_all_valid_data()
        {
            var result = _jobs.CreateJob("description", 1, DateTime.Now, valid_user_id);
            Assert.IsTrue(result.Success);
        }
    }
}
