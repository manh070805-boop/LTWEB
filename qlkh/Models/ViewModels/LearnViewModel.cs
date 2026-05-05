namespace qlkh.Models.ViewModels
{
    public class LearnViewModel
    {
         public Enrollment Enrollment { get; set; } = new Enrollment();

        public List<Lesson> Lessons { get; set; } = new List<Lesson>();

        public Lesson? CurrentLesson { get; set; }
    }
}