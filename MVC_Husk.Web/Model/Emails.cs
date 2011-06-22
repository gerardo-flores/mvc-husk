//
//  Copyright Info
//

using System;
using System.Dynamic;
using MVC_Husk.Data;

namespace MVC_Husk.Model
{
    public class Emails : DynamicModel
    {
        public Emails()
            : base("Template")
        {
            PrimaryKeyField = "ID";
        }

        /// <summary>
        /// Used to queue up an Email in the Database
        /// </summary>
        /// <param name="emailTo">The Email Address the Email is going to</param>
        /// <param name="emailFrom">The Email Address the Email is from</param>
        /// <param name="subject">The Subject of the Email</param>
        /// <param name="message">The Message itself</param>
        /// <returns>A dynamic object with the results of the queue process</returns>
        public dynamic Send(string emailTo, string emailFrom, string subject, string message)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            if (Users.ValidationEmailFormat(emailTo) && Users.ValidationEmailFormat(emailFrom))
            {
                result.EmailID = this.Insert(new { EmailTo = emailTo, EmailFrom = emailFrom, Subject = subject, Message = message });
                result.Success = true;
                result.Message = "Email Queued Successfully";
            }
            else
            {
                result.Success = false;
                result.Message = "Either the 'From' or 'To' Email address was invalid";
            }

            return result;
        }

        /// <summary>
        /// Sends an email to a user who an Admin has signed up
        /// </summary>
        /// <param name="emailTo">Email address of the signed up user</param>
        /// <param name="password">Temporary password of the signed up user</param>
        /// <returns>Dynamic object with results</returns>
        public dynamic SendThirdPartySignup(string emailTo, string password)
        {
            return this.Send(emailTo, "sender@mck.com", "Husk Automated Signup", SignupEmailMessage(emailTo, password));
        }

        private string SignupEmailMessage(string emailTo, string password)
        {
            string message = "You have been signed up for Husk " + Environment.NewLine;
            message += "The website can be found at http://www.huskistheword.com/ and your credentials are as follows:" + Environment.NewLine;
            message += "User Name: " + emailTo + Environment.NewLine;
            message += "Password: " + password + Environment.NewLine;
            message += Environment.NewLine;
            message += "You will be prompted to change your password on login";

            return message;
        }
    }
}