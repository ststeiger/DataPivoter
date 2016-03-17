
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


        private void btnOK_Click(object sender, System.EventArgs e)
        {
            // m_dtSampleData = DataPivoter.SQL.GetSampleData();
            //DataPivoter.JsonHelpers.SerializeToFile(@"d:\data.txt", m_dtSampleData);

            // m_dtSampleData = DataPivoter.SQL.GetJsonSampleDataNoReportingDate(); // 4780
            m_dtSampleData = DataPivoter.SQL.GetJsonSampleDataReportingDate(); // 4761

            this.dgvSource.DataSource = m_dtSampleData;

            // DataPivoter.JsonHelpers.SerializeToFile("file", dt);

            this.dgvPivot.DataSource = null;
        }


    }


}
