using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_CLOs
{
    public class AssessmentDL
    {
        private static List<Assessment> assessmentsList = new List<Assessment>();

        public static List<Assessment> AssessmentsList { get => assessmentsList; set => assessmentsList = value; }

        public static void AddIntoAssessementList(Assessment assessment)
        {
           assessmentsList.Add(assessment);
        }
        public static void LoadDataIntoList()
        {
            assessmentsList.Clear();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM Assessment A\r\n", con);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                int id=reader1.GetInt32(0);
                string title= reader1.GetString(1);
                DateTime dateCreated=reader1.GetDateTime(2);
                int totalMarks= reader1.GetInt32(3);
                int totalWeighatage= reader1.GetInt32(4); ;
                Assessment assessment = new Assessment(id, title, dateCreated, totalMarks, totalWeighatage);
                AddIntoAssessementList(assessment);
                
            }
            reader1.Close();
        }
        public static List<string> GetAssessmentTitle()
        {
            LoadDataIntoList();
            List<string> title = new List<string>();    
            foreach(Assessment s in assessmentsList)
            {
                title.Add(s.Title);
            }
            return title;

        }
        public static int GetAssessemntIdFromTitle(string title)
        {
            LoadDataIntoList();
            int id = -1;
            foreach (Assessment s in assessmentsList)
            {
                if (s.Title == title)
                {
                    id=s.Id;
                    break;
                }
            }
            return id;
        }
    }
}
