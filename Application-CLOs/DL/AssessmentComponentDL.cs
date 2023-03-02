using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application_CLOs
{
    public class AssessmentComponentDL
    {
        private static List<AssessmentComponent> assessmentsComponentList = new List<AssessmentComponent>();

        public static List<AssessmentComponent> AssessmentsComponentList { get => assessmentsComponentList; set => assessmentsComponentList = value; }

        public static void AddIntoAssessementComponentList(AssessmentComponent assessmentComponent)
        {
            assessmentsComponentList.Add(assessmentComponent);
        }
        public static void LoadDataIntoList()
        {
            assessmentsComponentList.Clear();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent AC", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                int rubricId=reader.GetInt32(2);
                int totalMarks = reader.GetInt32(3);
                DateTime dateCreated = reader.GetDateTime(4);
                DateTime dateUpdated = reader.GetDateTime(5);
                int assessmentId = reader.GetInt32(6); ;
                AssessmentComponent assessmentComponent=new AssessmentComponent(id,name,rubricId,totalMarks,dateCreated,dateUpdated,assessmentId);
                AddIntoAssessementComponentList(assessmentComponent);
            }
            reader.Close();
        }
        public static List<string> GetAssessmentComponentNameFromAssessmentId(int assessmentId)
        {
            LoadDataIntoList();
            List<string> name = new List<string>();
            foreach (AssessmentComponent s in assessmentsComponentList)
            {
                if (s.AssessmentId == assessmentId)
                {
                    name.Add(s.Name);
                }
            }
            return name;
        }
        public static int GetRubricIdFromAssesmentComponentName(string name,int assessmentID)
        {
            LoadDataIntoList();
            int id = -1;
            foreach (AssessmentComponent s in assessmentsComponentList)
            {
                if (s.Name == name && s.AssessmentId==assessmentID)
                {
                    id = s.RubricId;
                    break;
                }
            }
            return id;
        }
        private static string queryData(string query)
        {
            string output = null;
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(query, con);
                output = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                
            }
            return output;

        }
        public static int GetRubricId(string AssessmentTitle,string AssessmentComponentName)
        {
            string query = "SELECT AC.RubricId\r\nFROM Assessment A\r\nJOIN AssessmentComponent AC\r\nON A.Id=AC.AssessmentId WHERE A.Title='" + AssessmentTitle+ "' AND AC.Name='"+AssessmentComponentName+"'";
            int rubricID = int.Parse(queryData(query));
            return rubricID;
        }
        public static int GetAssesmentComponetId(string name,int rubricId,int totalMarks)
        {
            LoadDataIntoList();
            int id = -1;
            foreach (AssessmentComponent s in assessmentsComponentList)
            {
                if (s.Name == name && s.RubricId == rubricId && s.TotalMarks==totalMarks)
                {
                    id = s.Id;
                    break;
                }
            }
            return id;
        }
    }
}
