namespace CollegeApp.Model
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } =
        new List<Student>(){
                new Student
                {
                    ID = 1,
                    StudentName = "Nevzat",
                    Email = "ncirpicioglu@gmail.com",
                    Address = "İstanbul"
                },
                new Student
                {
                    ID = 2,
                    StudentName = "Yahya",
                    Email = "ycirpicioglu@gmail.com",
                    Address = "İstanbul"
                }
                };
    }
}
