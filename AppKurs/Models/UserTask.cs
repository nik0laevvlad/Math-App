using System.Collections.Generic;

namespace AppKurs.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskText { get; set; }
        public string TaskTopic { get; set; }
        public string TaskAnswer { get; set; }
        public string TaskUser { get; set; }
        public int TaskRating { get; set; }

        public ICollection<SolvedTask> SolvedTasks { get; set; }
    }
}
