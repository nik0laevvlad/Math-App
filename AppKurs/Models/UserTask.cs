using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public string ImageStorageName { get; set; }

        public ICollection<SolvedTask> SolvedTasks { get; set; }
    }
}
