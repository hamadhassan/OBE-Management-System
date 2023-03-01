using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_CLOs
{
    public class RubricLevel
    {
        int id;
        int rubricId;
        string detail;
        int measurmentLevel;
       
        public RubricLevel(int id,int rubricId, string detail,int measuremntLevel)
        {
            this.id = id;
            this.rubricId= rubricId;
            this.detail = detail;
            this.measurmentLevel= measuremntLevel;
        }

        public int Id { get => id; set => id = value; }
        public int RubricId { get => rubricId; set => rubricId = value; }
        public string Detail { get => detail; set => detail = value; }
        public int MeasurmentLevel { get => measurmentLevel; set => measurmentLevel = value; }
    }
}
