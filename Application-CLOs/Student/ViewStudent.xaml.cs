using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for ViewStudent.xaml
    /// </summary>
    public partial class ViewStudent : Window
    {
        public ViewStudent()
        {
            InitializeComponent();
           
        }
       
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT S.id,S.FirstName,S.lastname,S.Contact,S.email,S.RegistrationNumber,L.Name AS Status FROM Student S JOIN Lookup  L ON L.LookupId=S.Status WHERE  L.Category='STUDENT_STATUS'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgStudent.ItemsSource = dt.DefaultView;
            //if (dgStudent.Columns.Count > 1)
            //{
            //    dgStudent.Columns[2].Visibility = Visibility.Hidden;
            //}
        }
        public void UpdateDataGrid()
        {
            DataView dataView = (DataView)dgStudent.ItemsSource;
            DataTable dataTable = dataView.Table;
            bindDataGrid();
            dgStudent.ItemsSource = null;
            dgStudent.ItemsSource = dataTable.DefaultView;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            int id = int.Parse(dataRowView[0].ToString());
            string firstName = dataRowView[1].ToString();
            string lastName = dataRowView[2].ToString();
            string contact = dataRowView[3].ToString();
            string email=dataRowView[4].ToString();
            string RegistrationNumber = dataRowView[5].ToString();
            string status =dataRowView[6].ToString();
            AddStudent addStudent = new AddStudent(id,firstName,lastName,contact,email,RegistrationNumber,status);
            addStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgStudent.SelectedIndex >= 0 && dgStudent.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int id = int.Parse(dataRowView[0].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("delete from student where id='" + id.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    bindDataGrid();
                   
                }
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field","Information",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            bindDataGrid();
            GenerateReport();
            MessageBox.Show("PDF report generated successfully.");
        }
        public void GenerateReport()
        {
            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("report.pdf", FileMode.Create));
            document.AddCreationDate();
            document.AddAuthor("Your Name");
            document.AddTitle("Report Title");
            document.Open();

            // Create a header table
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.Border = 0;

            // Add text to the header
            PdfPCell headerCell = new PdfPCell(new Phrase("My Report"));
            headerCell.Border = 0;
            headerTable.AddCell(headerCell);

            // Add the header table to the document
            document.Add(headerTable);


            PdfPTable table = new PdfPTable(4);
            table.AddCell("FirstName");
            table.AddCell("LastName");
            table.AddCell("RegistrationNumber");
            table.AddCell("Contact");


            //Paragraph paragraph = new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec vitae nulla euismod, tristique elit ut, euismod elit. Nam vel enim quis mi bibendum tincidunt.");
            //document.Add(paragraph);



            //// Add the image to the header
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("graphic.png");
            //logo.ScalePercent(50);
            //PdfPCell logoCell = new PdfPCell(logo);
            //logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //logoCell.Border = PdfPCell.NO_BORDER;
            //table.AddCell(logoCell);

            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT S.FirstName,S.LastName,S.RegistrationNumber,S.Contact FROM Student S", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                table.AddCell(reader["FirstName"].ToString());
                table.AddCell(reader["LastName"].ToString());
                table.AddCell(reader["RegistrationNumber"].ToString());
                table.AddCell(reader["Contact"].ToString());
            }
            reader.Close();
            document.Add(table);

            // Add an image to the header
            string imagePath = "graphic.png";
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
            PdfPCell logoCell = new PdfPCell(logo, true);
            logoCell.Border = 0;
            headerTable.AddCell(logoCell);

            // Add some content to the PDF document
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec vitae nulla euismod, tristique elit ut, euismod elit. Nam vel enim quis mi bibendum tincidunt.");
            document.Add(paragraph);

            // Create a footer table
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.Border = 0;


            // Add text to the footer
            PdfPCell footerCell = new PdfPCell(new Phrase("Page " + writer.PageNumber));
            footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCell.Border = 0;
            footerTable.AddCell(footerCell);

            // Add the footer table to the document
            document.Add(footerTable);
            // Set the background color of the PDF document
            BaseColor backgroundColor = new BaseColor(255, 255, 204);
            PdfContentByte canvas = writer.DirectContentUnder;
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(document.PageSize);
            rect.BackgroundColor = backgroundColor;
            canvas.Rectangle(rect);

            document.Close();
        }
    }
}
