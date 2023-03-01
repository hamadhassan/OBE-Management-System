using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_CLOs
{
    public class AssessmentComponent
    {
        int id;
        string name;
        int rubricId;
        int totalMarks;
        DateTime dateCreated;
        DateTime dateUpdated;
        int assessmentId;
        public AssessmentComponent(int id, string name, int rubricId, int totalMarks, DateTime dateCreated, DateTime dateUpdated, int assessmentId)
        {
            this.id = id;
            this.name = name;
            this.rubricId = rubricId;
            this.totalMarks = totalMarks;
            this.dateCreated = dateCreated;
            this.dateUpdated = dateUpdated;
            this.assessmentId = assessmentId;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int RubricId { get => rubricId; set => rubricId = value; }
        public int TotalMarks { get => totalMarks; set => totalMarks = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime DateUpdated { get => dateUpdated; set => dateUpdated = value; }
        public int AssessmentId { get => assessmentId; set => assessmentId = value; }
    }
}
