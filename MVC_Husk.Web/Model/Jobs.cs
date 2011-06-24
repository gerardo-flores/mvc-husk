//
//  Copyright Info
//
using System.Web;
using MVC_Husk.Data;
using System.Dynamic;
using System;

namespace MVC_Husk.Model
{
    public class Jobs : DynamicModel
    {

       public Jobs()
            : base("Template")
        {
            PrimaryKeyField = "ID";
        }

       public dynamic CreateJob(string description, int priority, DateTime createdAt, long userID)
       {
           dynamic result = new ExpandoObject();
           result.Success = false;

           if (!string.IsNullOrEmpty(description) && ValidatePriority(priority))
           {
               try
               {
                   result.JobId = this.Insert(new { Description = description, Priority = priority, CreatedAt = createdAt, UploadedBy = userID, Status = "Queued"});
                   result.Success = true;
                   result.Message = "job added";
               }
               catch (Exception ex)
               {
                   result.Message = "No name, priority, or create date was supplied for job";
               }
           }
           else
           {
               result.Message = "The job was invalid";
           }


           return result;
       }

       private bool ValidatePriority(int priority)
       {
           if (priority > 0 && priority < 5)
               return true;
           else
               return false;
       }


    }
}