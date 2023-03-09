using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.draw;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Documents;
using System.Dynamic;

namespace Application_CLOs
{
    public class Reports
    {
        private static Reports _instance;
        public static Reports getInstance()
        {
            if (_instance == null)
                _instance = new Reports();
            return _instance;
        }
        private PdfPTable ReportHeader()
        {
            // create a header table
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.WidthPercentage = 100;
            // add a cell with the header text
            PdfPCell headerCell = new PdfPCell(new Phrase("University of Engineering and Technology Lahore,Pakistan", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(headerCell);
            return headerTable;
        }
        private PdfPTable ReportSubHeader(string reportName)
        {
            PdfPTable subheaderTable = new PdfPTable(1);
            PdfPCell subheaderCell1 = new PdfPCell(new Phrase(reportName, new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
            subheaderCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            subheaderCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            subheaderTable.AddCell(subheaderCell1);
            return subheaderTable;
        }
        private PdfPTable ReportFooter(Document document, PdfWriter writer)
        {
            // Create a footer table
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.Border = 0;

            // Add text to the footer
            PdfPCell footerCell = new PdfPCell(new Phrase("Page " + writer.PageNumber));
            footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCell.Border = 0;
            footerTable.AddCell(footerCell);
            return footerTable;
        }

        private iTextSharp.text.Paragraph ReportDate()
        {
            iTextSharp.text.Paragraph dateParagraph = new iTextSharp.text.Paragraph(DateTime.Now.ToString("D"));
            dateParagraph.Alignment = Element.ALIGN_RIGHT;
            dateParagraph.SpacingBefore = 10f;

            return dateParagraph;
        }
        public void GenerateCLOWiseResultReport(SaveFileDialog saveFileDialog,int cloid)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("CLOs Wise Result"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth= 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Student Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Assignment", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell5 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell6 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            
            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell6.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);
            table.AddCell(cell5);
            table.AddCell(cell6);

            var con = Configuration.getInstance().getConnection();
            string query = "SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [Student Name],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks],\r\nC.Name AS [CLO Name]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\n" +
                " WHERE C.Id="+ cloid +
                " GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks,C.Name\r\nORDER BY C.Name ASC\r\n";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
                table.AddCell(reader.GetInt32(3).ToString());
                table.AddCell(reader.GetDouble(4).ToString());
                table.AddCell(reader.GetString(5));
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateFailCLOWiseResultReport(SaveFileDialog saveFileDialog,int thershouldLevel)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("CLOs Fail Result"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Student Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Assignment", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell5 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell6 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell6.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);
            table.AddCell(cell5);
            table.AddCell(cell6);

            var con = Configuration.getInstance().getConnection();
            string query = "SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks],\r\nC.Name AS [CLOName]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\nGROUP BY S.RegistrationNumber,A.Title,A.TotalMarks,C.Name\r\n" +
                "HAVING  ((SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks))/A.TotalMarks)*100<=" + thershouldLevel +
                "\r\nORDER BY C.Name ASC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
                table.AddCell(reader.GetInt32(3).ToString());
                table.AddCell(reader.GetDouble(4).ToString());
                table.AddCell(reader.GetString(5));
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateAssessmentWiseResultReport(SaveFileDialog saveFileDialog, int assessmentId)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Assessment Wise Result"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Student Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Assignment", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell5 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);
            table.AddCell(cell5);

            var con = Configuration.getInstance().getConnection();
            string query = "\r\nSELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\n" +
                " WHERE A.Id=" +assessmentId+
                " GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks\r\nORDER BY  A.Title ASC\r\n";
            
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
                table.AddCell(reader.GetInt32(3).ToString());
                table.AddCell(reader.GetDouble(4).ToString());
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateAssessmentComponentWiseResultReport(SaveFileDialog saveFileDialog, int assessmentComponentId)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Question Wise Result"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Student Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Assignment", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell5 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell6 = new PdfPCell(new Phrase("Question Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell6.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);
            table.AddCell(cell5);
            table.AddCell(cell6);


            var con = Configuration.getInstance().getConnection();
           
            string query = "SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,AC.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks]\r\n,AC.Name AS QuestionName\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\n" +
                " WHERE AC.Id=" +assessmentComponentId+
                " GROUP BY S.RegistrationNumber,A.Title,AC.TotalMarks,AC.Name\r\nORDER BY  AC.Name ASC\r\n";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
                table.AddCell(reader.GetInt32(3).ToString());
                table.AddCell(reader.GetDouble(4).ToString());
                table.AddCell(reader.GetString(5));
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateTop10StudentReport(SaveFileDialog saveFileDialog)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Top 10 Students Result"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Student Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Assignment", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell5 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);
            table.AddCell(cell5);


            var con = Configuration.getInstance().getConnection();
            string query = "SELECT TOP 10 CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nGROUP BY S.RegistrationNumber,A.Title,A.TotalMarks\r\nORDER BY [Obtained Marks] DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
                table.AddCell(reader.GetInt32(3).ToString());
                table.AddCell(reader.GetDouble(4).ToString());
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateIndividualStudentReport(SaveFileDialog saveFileDialog,int studentId,string name,string registrationNumber)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Students Result Card"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable info = new PdfPTable(4);
            info.WidthPercentage = 100;
            info.SetWidths(new[] { 1f, 1f , 1f, 1f });

            PdfPCell cell1Info = new PdfPCell(new Phrase("Student Name: "));
            PdfPCell cell2Info = new PdfPCell(new Phrase(name));
            PdfPCell cell3Info = new PdfPCell(new Phrase("Registration Number: "));
            PdfPCell cell4Info = new PdfPCell(new Phrase(registrationNumber));

            cell1Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell2Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell3Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell4Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            
            info.AddCell(cell1Info);
            info.AddCell(cell2Info);
            info.AddCell(cell3Info);
            info.AddCell(cell4Info);

            document.Add(info);

            document.Add(space);

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Assignemt Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Total Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell4 = new PdfPCell(new Phrase("Obtained Marks", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell3);
            table.AddCell(cell4);


            var con = Configuration.getInstance().getConnection();
            string query = "SELECT A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\n" +
                " WHERE S.Id=" +studentId+
                " GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks\r\nORDER BY S.RegistrationNumber ASC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetInt32(1).ToString());
                table.AddCell(reader.GetDouble(2).ToString());
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateIndividualStudentAttendanceReport(SaveFileDialog saveFileDialog, int studentId, string name, string registrationNumber)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Students Attendance Report"));

            document.Add(ReportDate());

            document.Add(space);

            PdfPTable info = new PdfPTable(4);
            info.WidthPercentage = 100;
            info.SetWidths(new[] { 1f, 1f, 1f, 1f });

            PdfPCell cell1Info = new PdfPCell(new Phrase("Student Name: "));
            PdfPCell cell2Info = new PdfPCell(new Phrase(name));
            PdfPCell cell3Info = new PdfPCell(new Phrase("Registration Number: "));
            PdfPCell cell4Info = new PdfPCell(new Phrase(registrationNumber));

            cell1Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell2Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell3Info.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell4Info.Border = iTextSharp.text.Rectangle.NO_BORDER;

            info.AddCell(cell1Info);
            info.AddCell(cell2Info);
            info.AddCell(cell3Info);
            info.AddCell(cell4Info);

            document.Add(info);

            document.Add(space);

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Date", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Status", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell3);


            var con = Configuration.getInstance().getConnection();
            string query = "SELECT  CONVERT(date,  C.AttendanceDate, 101) AS Date ,L.Name\r\nFROM Student S\r\nJOIN StudentAttendance ST\r\nON S.Id=ST.StudentId\r\nJOIN ClassAttendance C\r\nON C.Id=ST.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=ST.AttendanceStatus\r\n" +
                "WHERE S.Id="+studentId +
                "\r\nORDER BY DATE DESC\r\n";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetDateTime(0).ToLongDateString());
                table.AddCell(reader.GetString(1));
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
        public void GenerateClassAttendanceReport(SaveFileDialog saveFileDialog,string date)
        {

            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
            document.AddCreationDate();
            document.Open();
            iTextSharp.text.Chunk space = new iTextSharp.text.Chunk(new VerticalPositionMark(), 50, true);
            document.Add(space);

            document.Add(space);

            document.Add(ReportHeader());

            document.Add(space);

            document.Add(ReportSubHeader("Class Attendance Report"));

            document.Add(ReportDate());
            
            document.Add(space);

            //Date
            PdfPTable Selecteddate = new PdfPTable(2);
            Selecteddate.WidthPercentage = 100;
            Selecteddate.SetWidths(new[] { 1f, 1f });

            PdfPCell cell1Selecteddate = new PdfPCell(new Phrase("Attendance Date: "));
            PdfPCell cell2Selecteddate = new PdfPCell(new Phrase(date));


            cell1Selecteddate.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell2Selecteddate.Border = iTextSharp.text.Rectangle.NO_BORDER;

            Selecteddate.AddCell(cell1Selecteddate);
            Selecteddate.AddCell(cell2Selecteddate);

            document.Add(Selecteddate);




            document.Add(space);

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 100;
            //table.SetWidths(new[] { 1f , 1f , 1f , 1f , 1f, 1f});
            table.WidthPercentage = 100;
            // Create the header cells
            PdfPCell cell1 = new PdfPCell(new Phrase("Name", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell2 = new PdfPCell(new Phrase("Registration Number", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            PdfPCell cell3 = new PdfPCell(new Phrase("Status", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);


            var con = Configuration.getInstance().getConnection();
            string query = "SELECT CONCAT(S.FirstName,' ',S.LastName)AS Name,S.RegistrationNumber,L.Name\r\nFROM Student S\r\nJOIN StudentAttendance ST\r\nON S.Id=ST.StudentId\r\nJOIN ClassAttendance C\r\nON C.Id=ST.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=ST.AttendanceStatus\r\nWHERE L.Category='ATTENDANCE_STATUS' " +
                "AND C.AttendanceDate='"+date+"'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader.GetString(0));
                table.AddCell(reader.GetString(1));
                table.AddCell(reader.GetString(2));
            }
            reader.Close();
            document.Add(table);

            document.Add(ReportFooter(document, writer));

            document.Close();
        }
    }
}
