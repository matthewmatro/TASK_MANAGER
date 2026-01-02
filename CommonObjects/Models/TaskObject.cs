using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonObjects.Models
{
    public class TaskObject
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? type { get; set; }
        public DateTime? due_date { get; set; }
        public bool? completed { get; set; }

        public TaskObject(int id,
            string? name, string? description, string? type,
            DateTime? due_date, bool? completed)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.type = type;
            this.due_date = due_date;
            this.completed = completed;
        }
    }
}
