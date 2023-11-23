namespace Biz1PosApi.Models
{
    public class Student
    {
        public string Name { get; set; }
        public Student()
        {
            Name = "Qwerty";
        }
        public static string Address()
        {
            return "Address";
        }
    }
}
