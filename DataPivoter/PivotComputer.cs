
namespace DataPivoter
{


    public class PivotComputer
    { 
    
        public System.Data.DataTable data;




    }


    public class cRowGroup
    {
        public System.Data.DataTable data;

    }


    public class cColumnGroup
    {
        public object parent;


        public System.Data.DataTable data;

        public System.Collections.Generic.List<string> lsFieldList = new System.Collections.Generic.List<string>();

        public System.Collections.Generic.List<object> lsDistinctValues = new System.Collections.Generic.List<object>();



    }


    public class cDistinctValue
    {
        public object Value;


    }



}
