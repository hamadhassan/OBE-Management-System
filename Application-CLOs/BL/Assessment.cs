using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_CLOs
{
    public class Assessment
    {
        int id;
        string title;
        DateTime dateCreated;
        int totalMarks;
        int totalWeighatage;
        public Assessment(int id, string title, DateTime dateCreated, int totalMarks, int totalWeighatage)
        {
            this.id = id;
            this.title = title;
            this.dateCreated = dateCreated;
            this.totalMarks = totalMarks;
            this.totalWeighatage = totalWeighatage;
        }
        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public int TotalMarks { get => totalMarks; set => totalMarks = value; }
        public int TotalWeighatage { get => totalWeighatage; set => totalWeighatage = value; }
    }
}
