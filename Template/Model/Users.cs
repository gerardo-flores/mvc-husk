using Template.ORM;

namespace Template.Model
{
    public class Users : DynamicModel
    {
        public Users()
            : base("TemplateTest")
        {
            PrimaryKeyField = "UserId"; 
        }
    }
}