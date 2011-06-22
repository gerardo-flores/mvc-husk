//
//  Copyright Info
//

using System;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using MVC_Husk.Data;

namespace MVC_Husk.Model
{
    public class Users : DynamicModel
    {
        /// <summary>
        /// Class representing the Users
        /// </summary>
        public Users()
            : base("Template")
        {
            PrimaryKeyField = "ID";
        }

        /// <summary>
        /// Method used to Register a new user with the Application
        /// </summary>
        /// <param name="email">Email, this will act as the user's User Name</param>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Password Confirmation</param>
        /// <returns>A Dynamic Results object containing status and any messages</returns>
        public dynamic Register(string email, string password, string confirmPassword, bool requirePasswordChange = false, bool sendEmail = false)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            if (ValidationEmailFormat(email) && PasswordComplexityCheck(password) && password.Equals(confirmPassword))
            {
                try
                {
                    result.UserId = this.Insert(new { Email = email, HashedPassword = Hash(password), RequirePasswordChange = requirePasswordChange });
                    result.Success = true;
                    result.Authenticated = true;
                    result.Message = "Registration Successful!";

                    if (sendEmail)
                    {
                        var emailResult = new Emails().SendThirdPartySignup(email, password);
                        if (emailResult.Success == false)
                        {
                            // Do something here to roll it back
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = "This user email address already exists";
                }
            }
            else
            {
                result.Message = "Either the Email was invalid, the Password wasn't long enough or the Password and the Confirmation didn't match";
            }

            return result;
        }

        /// <summary>
        /// Attempt so login a user
        /// </summary>
        /// <param name="email">the Email Address of the User</param>
        /// <param name="password">Password of the User</param>
        /// <returns>Dynamic Results object</returns>
        public dynamic Login(string email, string password)
        {
            dynamic result = new ExpandoObject();

            object[] queryargs = { email, Hash(password) };
            result.User = this.Query("select * from users where email = @0 AND hashedpassword = @1", args: queryargs).FirstOrDefault();
            result.Authenticated = result.User != null;

            if (result.Authenticated == false)
            {
                result.Message = "Invalid Email or Password";
            }

            return result;
        }

        /// <summary>
        /// This allows the user to change password
        /// </summary>
        /// <param name="email">the Email Address of the User</param>
        /// <param name="oldPassword">The Old Password</param>
        /// <param name="password">The New Password</param>
        /// <param name="confirmNewPassword">The New Password Confirmation</param>
        /// <returns>Dynamic Results Object</returns>
        public dynamic ChangePassword(string email, string password, string newPassword, string confirmNewPassword)
        {
            dynamic result = new ExpandoObject();

            object[] queryargs = { email, Hash(password) };
            result.User = this.Query("select * from users where email = @0 AND hashedpassword = @1", args: queryargs).FirstOrDefault();

            result.Success = false;
            if (result.User == null)
            {
                result.Message = "Invalid Password";
            }
            else
            {
                if (newPassword.Equals(confirmNewPassword) && PasswordComplexityCheck(newPassword))
                {
                    this.Update(new { HashedPassword = Hash(newPassword), RequirePasswordChange = false }, result.User.ID);
                    result.Success = true;
                    result.Message = "Password Change Successful";
                }
                else
                {
                    result.Message = "The New Password wasn't long enough or the New Password and Confirmation Password did not match";
                }
            }

            return result;
        }

        /// <summary>
        /// Method used to validation an email address via Regex.
        /// </summary>
        /// <param name="email">The email address in question</param>
        /// <returns>A Boolean indicating whether the email address is technically valid</returns>
        internal static bool ValidationEmailFormat(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        /// <summary>
        /// Used to Hash a password for storing in the database
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>The hash of the password</returns>
        private string Hash(string password)
        {
            // TODO: Do the Hashing of the Password
            return password;
        }

        /// <summary>
        /// Used to check password complexity when creating or updating users
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>The result</returns>
        private bool PasswordComplexityCheck(string password)
        {
            if (password.Length >= 6)
                return true;

            return false;
        }
    }
}