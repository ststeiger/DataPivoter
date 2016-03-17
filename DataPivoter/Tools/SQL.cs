
namespace DataPivoter
{


    public class SQL
    {


        public static System.IO.Stream GetEmbeddedFileAsStream(string name)
        {
            System.Reflection.Assembly ass = typeof(SQL).Assembly;
            string resourceName = null;

            foreach (string thisResourceName in ass.GetManifestResourceNames())
            {
                if (thisResourceName.EndsWith(name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    resourceName = thisResourceName;
                    break;
                } // End if (thisResourceName.EndsWith(name, System.StringComparison.InvariantCultureIgnoreCase)) 

            } // Next thisResourceName

            return ass.GetManifestResourceStream(resourceName);
        } // End Function GetEmbeddedFile 


        public static string GetEmbeddedFileText(string fileName)
        {
            string retVal = null;

            using (System.IO.Stream strm = GetEmbeddedFileAsStream(fileName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(strm))
                {
                    retVal = sr.ReadToEnd();
                    sr.Close();
                } // End Using xtrReader

                strm.Close();
            } // End Using strm 

            return retVal;
        } // End Function GetFile 

        public static System.Data.DataTable GetJsonSampleData(bool withReportingData)
        {
            string fn = "Data_NoReportingData.txt.enc";

            if (withReportingData)
                fn = "Data_ReportingData.txt.enc";
            
            string JSON = GetEmbeddedFileText(fn);
            JSON = DataPivoter.Cryptography.AES.DeCrypt(JSON);

            System.Data.DataTable dt = JsonHelper.Deserialize<System.Data.DataTable>(JSON);

            /*
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);


            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                if (dict.ContainsKey(dc.ColumnName))
                    throw new System.Exception("Result set contains column \"" + dc.ColumnName + "\" at least twice.");
                else
                    dict.Add(dc.ColumnName, null);
            }
            */

            return dt;
        }


        public static System.Data.DataTable GetSampleData(bool withReportingData)
        {
            string strSQL = @"
SELECT 
	 SO_UID
	,SO_Nr
	,SO_Bezeichnung
	,GB_UID
	,GB_Nr 
	,GB_Bezeichnung
	,GS_UID 
	,GS_GB_UID 
	,GS_GST_UID
	,GS_Nr 
	,GST_Kurz_DE
	,GST_Kurz_DE + RIGHT('00' + GS_Nr, 2) AS GS_Kurz_DE 
	,DENSE_RANK() OVER(PARTITION BY GS_GB_UID ORDER BY GST_Sort, GST_GS_NrMultiplikator * GS_Nr, GST_ZG_Sort) AS GS_Sort 
	,DENSE_RANK() OVER(PARTITION BY GS_GB_UID ORDER BY GS_IsAussengeschoss, GST_Sort, GST_GS_NrMultiplikator * GS_Nr, GST_ZG_Sort) AS GS_Sort1 
	,RM_UID
	,RM_Nr 
	,NA_UID
	,NA_LANG_DE
	,NA_Sort 
	,NA_Code 
	,CAST(NA_Code AS float) NA_CodeNr 
	,NAG_UID
	,NAG_LANG_DE
	,NAG_Sort
    ,RM_DatumVon 
    ,RM_DatumBis 
    ,ZO_RMFlaeche_Flaeche 
FROM T_AP_Standort 

LEFT JOIN T_AP_Gebaeude ON T_AP_Gebaeude.GB_SO_UID = T_AP_Standort.SO_UID 
LEFT JOIN T_AP_Geschoss ON T_AP_Geschoss.GS_GB_UID = T_AP_Gebaeude.GB_UID
LEFT JOIN T_AP_Ref_Geschosstyp ON T_AP_Ref_Geschosstyp.GST_UID = T_AP_Geschoss.GS_GST_UID 
LEFT JOIN T_AP_Raum ON T_AP_Raum.RM_GS_UID = T_AP_Geschoss.GS_UID 

LEFT JOIN T_ZO_AP_Raum_AP_Ref_Nutzungsart ON T_ZO_AP_Raum_AP_Ref_Nutzungsart.ZO_RMNA_RM_UID = T_AP_Raum.RM_UID 
AND CURRENT_TIMESTAMP BETWEEN T_ZO_AP_Raum_AP_Ref_Nutzungsart.ZO_RMNA_DatumVon AND T_ZO_AP_Raum_AP_Ref_Nutzungsart.ZO_RMNA_DatumBis
AND T_ZO_AP_Raum_AP_Ref_Nutzungsart.ZO_RMNA_Status = 1 
LEFT JOIN T_AP_Ref_Nutzungsart ON T_AP_Ref_Nutzungsart.NA_UID = ZO_RMNA_NA_UID 
LEFT JOIN T_AP_Ref_Nutzungsartgruppe ON T_AP_Ref_Nutzungsartgruppe.NAG_UID = T_AP_Ref_Nutzungsart.NA_NAG_UID
LEFT JOIN T_ZO_AP_Raum_Flaeche ON ZO_RMFlaeche_RM_UID = RM_UID AND ZO_RMFlaeche_Status = 1 --AND CURRENT_TIMESTAMP BETWEEN ZO_RMFlaeche_DatumVon AND ZO_RMFlaeche_DatumBis 

WHERE T_AP_Raum.RM_Status = 1 
"
;
            if (withReportingData)
                strSQL+=@"AND CURRENT_TIMESTAMP BETWEEN T_AP_Raum.RM_DatumVon AND T_AP_Raum.RM_DatumBis " + System.Environment.NewLine;

            return GetDataTable(strSQL);
        }


        public static string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = @"VMSTZHDB08\SZH_DBH_1";
            csb.InitialCatalog = "HBD_CAFM_V3";
            

            csb.DataSource = @"CORDB2008R2";
            csb.InitialCatalog = "RoomPlanning";
            

            csb.IntegratedSecurity = true;
            return csb.ConnectionString;
        }


        public static System.Data.DataTable GetDataTable(string strSQL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.Common.DbDataAdapter ada = new System.Data.SqlClient.SqlDataAdapter(strSQL, GetConnectionString()))
            {
                ada.Fill(dt);
            }

            return dt;
        }


        public static string EXEC_sp_RPT_DATA_REM_Zeitberechnung
        {
            get
            {
                return @"EXECUTE sp_RPT_DATA_REM_Zeitberechnung 
   @in_mandant = '0'
  ,@in_sprache = 'DE'
  ,@in_standort = '01F92978-6C73-4093-91DE-58088D6735EE' -- '00000000-0000-0000-0000-000000000000'
  ,@in_gebaeude = '00000000-0000-0000-0000-000000000000'
  ,@in_portofolio_raum = '00000000-0000-0000-0000-000000000000'
  ,@in_subportofolio_raum = '00000000-0000-0000-0000-000000000000'
  ,@in_strasse = 'Alle'
  ,@in_geschoss = '00000000-0000-0000-0000-000000000000'
  ,@in_ebene1 = '00000000-0000-0000-0000-000000000000'
;";
            }
        }


    }


}
