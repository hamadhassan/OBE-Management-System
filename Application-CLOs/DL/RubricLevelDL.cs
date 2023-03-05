using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace Application_CLOs
{
    public class RubricLevelDL
    {
        private static List<RubricLevel> rubricLevelsList = new List<RubricLevel>();

        public static void AddIntoRubricLevelsList(RubricLevel rubricLevel)
        {
            rubricLevelsList.Add(rubricLevel);
        }
        public static void LoadDataIntoList()
        {
            rubricLevelsList.Clear();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int rubricId=reader.GetInt32(1);
                string detail=reader.GetString(2);
                int measurmentLevel=reader.GetInt32(3);
                RubricLevel rubricLevel = new RubricLevel(id, rubricId, detail, measurmentLevel);
                AddIntoRubricLevelsList(rubricLevel);

            }
            reader.Close();
        }
        public static List<int> GetMeasurementLevelFromRubricId(int rubricId)
        {
            LoadDataIntoList();
            List<int> measurementLevel = new List<int>();
            foreach(RubricLevel r in rubricLevelsList)
            {
                if(r.RubricId == rubricId)
                {
                    measurementLevel.Add(r.MeasurmentLevel);
                }
            }
            return measurementLevel;   
        }
        public static bool isMeasuremntLevelExist(int rubricId)
        {
            LoadDataIntoList();
            foreach (RubricLevel r in rubricLevelsList)
            {
                if (r.RubricId == rubricId)
                {
                   return true;
                }
            }
            return false;

        }
        public static int GetRubricLevelId(int rubricId,int measurmentLevel)
        {
            LoadDataIntoList();
            int id = -1;
            foreach (RubricLevel s in rubricLevelsList)
            {
                if (s.RubricId == rubricId && s.MeasurmentLevel == measurmentLevel)
                {
                    return s.Id;
                }
            }
            return id;
        }
        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T foundChild = FindVisualChild<T>(child);
                    if (foundChild != null)
                    {
                        return foundChild;
                    }
                }
            }
            return null;
        }

    }
}
