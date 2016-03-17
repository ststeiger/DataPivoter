
using System.Windows.Forms;

namespace DataPivoterTestForm
{


    public partial class Form1 : Form
    {

        private System.Data.DataTable m_dtSampleData;


        public Form1()
        {
            InitializeComponent();
        }


        public static string MapDataPath(string FileName)
        {
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dir = System.IO.Path.Combine(dir, "../../../DataPivoter");
            dir = System.IO.Path.GetFullPath(dir);
            dir = System.IO.Path.Combine(dir, FileName);

            return dir;
        }


        public static void SaveData(string fn, System.Data.DataTable dt)
        {
            string path = MapDataPath(fn);
            
            DataPivoter.JsonHelpers.SerializeToFile(path, dt);
            DataPivoter.Cryptography.AES.EncryptFile(path);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        public static void SaveData(ref System.Data.DataTable data, bool withReportingDate)
        {
            data = DataPivoter.SQL.GetSampleData(withReportingDate);

            if (withReportingDate)
                SaveData("Data_ReportingData.txt", data);
            else
                SaveData("Data_NoReportingData.txt", data);
        }


        private void btnOK_Click(object sender, System.EventArgs e)
        {
            bool withReportingDate = true;
            // SaveData(ref m_dtSampleData, withReportingDate);
            m_dtSampleData = DataPivoter.SQL.GetJsonSampleData(withReportingDate);
            

            // Reporting Date:    4761
            // No Reporting Date: 4780
            // System.Console.WriteLine(m_dtSampleData.Rows.Count);
            
            this.dgvSource.DataSource = m_dtSampleData;

            this.dgvPivot.DataSource = null;
        }


    }


}
