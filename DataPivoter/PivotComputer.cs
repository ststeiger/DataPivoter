
namespace DataPivoter
{

    internal static class MyDataTableExtensions
    {

        public static System.Data.DataTable Sort(this System.Data.DataTable table, string sort)
        {
            System.Data.DataTable newTable = table.Clone();
            System.Data.DataRow[] rowArray = table.Select(null, sort);


            for (long i = 0; i < rowArray.LongLength; ++i)
            {
                newTable.ImportRow(rowArray[i]);
            }

            return newTable;
        }


        private static void CopyColumns(this System.Data.DataTable source, System.Data.DataTable dest, params string[] columns)
        {
            foreach (System.Data.DataRow sourcerow in source.Rows)
            {
                System.Data.DataRow destRow = dest.NewRow();
                foreach (string colname in columns)
                {
                    destRow[colname] = sourcerow[colname];
                }
                dest.Rows.Add(destRow);
            }
        }

    }




    public abstract class cPivotBase
    {
        public object parent;
        public System.Data.DataTable data;
    }


    public class cPivotTable : cPivotBase
    { 
        public System.Collections.Generic.List<cRowGroup> RowGroups;
        public System.Collections.Generic.List<cColumnGroup> ColumnGroups;


        public cPivotTable(System.Data.DataTable dt)
        {
            this.data = dt;
            this.RowGroups = new System.Collections.Generic.List<cRowGroup>();
            this.ColumnGroups = new System.Collections.Generic.List<cColumnGroup>();
        }


        public cPivotTable(System.Data.DataTable dt, System.Collections.Generic.List<cRowGroup> rowGroups, System.Collections.Generic.List<cColumnGroup> columnGroups) 
        {
            this.data = dt;
            this.RowGroups = rowGroups;
            this.ColumnGroups = columnGroups;
        }


        public void Test()
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>() { 
                 "SO_UID"
                ,"GB_UID" 
                ,"GS_UID" 
                ,"RM_UID" 
            };
            

            var cg = new cColumnGroup(this, ls);
            this.ColumnGroups.Add(cg);
        }


    }


    public class cRowGroup : cPivotBase
    {

    }


    public class GroupedValuesList
    {
        public object value;
        public System.Collections.Generic.List<GroupedValuesList> ls;


        public GroupedValuesList(object pobj)
        {
            this.value = pobj;
            this.ls = new System.Collections.Generic.List<GroupedValuesList>();
        }


        public long Count
        {
            get
            {

                if (this.value != null && ls.Count == 0)
                    return 1;

                long total = 0;

                for (int i = 0; i < ls.Count; ++i)
                {
                    total += ls[i].Count;
                }

                return total;
            }
        }


        public static bool CompareObjects(object val1, object val2)
        {

            if (val1 != null && val2 != null)
            {

                if (object.ReferenceEquals(val1.GetType(), typeof(string)) && object.ReferenceEquals(val2.GetType(), typeof(string)))
                {
                    string str1 = val1.ToString();
                    string str2 = val2.ToString();
                    return string.Equals(str1, str2, System.StringComparison.InvariantCultureIgnoreCase);
                }

            }

            return object.Equals(val1, val2);
        }

    }


    public class cGroupedValues
    {
        public System.Collections.Generic.List<GroupedValuesList> DistinctValuesList;

        public cGroupedValues()
        {
            this.DistinctValuesList = new System.Collections.Generic.List<GroupedValuesList>();
        }


        public long Count
        {
            get
            {
                long total = 0;
                for (int i = 0; i < DistinctValuesList.Count; ++i)
                    total += DistinctValuesList[i].Count;

                return total;
            }
        }
    }


    public class cColumnGroup : cPivotBase
    {

        //public System.Collections.Generic.List<string> lsFieldList = new System.Collections.Generic.List<string>();
        public System.Collections.Generic.List<string> FieldsToGroupBy;
        public DotNet2Extensions.HashSet<object> DistinctValues = new DotNet2Extensions.HashSet<object>();
        // public System.Collections.Generic.List<cDistinctValue> DistinctValuesList = new System.Collections.Generic.List<cDistinctValue>();
        cGroupedValues GroupedValues = new cGroupedValues();



        public cColumnGroup(object objParent, System.Collections.Generic.List<string> lsFieldsToGroupBy)
        {
            this.FieldsToGroupBy = lsFieldsToGroupBy;
            this.parent = objParent;
            this.Init();
        }


        public void Init()
        {
            System.Data.DataTable dt = ((cPivotBase)this.parent).data;
            

            System.Data.DataTable dt2 = dt.Clone();

            for (int i = 0; i < dt.Rows.Count; ++i)
            {

                /*
                object val = dt.Rows[i][groupField];
                
                if (!this.DistinctValues.Contains(val))
                {
                    this.DistinctValues.Add(val);
                    dt2.ImportRow(dt.Rows[i]);
                }
                */

                System.Collections.Generic.List<GroupedValuesList> thisDistinctValuesList = GroupedValues.DistinctValuesList;

                for (int j = 0; j < this.FieldsToGroupBy.Count; ++j)
                {
                    object val = dt.Rows[i][this.FieldsToGroupBy[j]];

                    int myind = thisDistinctValuesList.FindIndex(delegate(GroupedValuesList nodeToCompare)
                    {
                        // return System.StringComparer.InvariantCultureIgnoreCase.Equals(nodeToCompare.value, "foo");
                        return GroupedValuesList.CompareObjects(val, nodeToCompare.value);
                    }
                    );


                    if (myind == -1)
                    {
                        GroupedValuesList newDistinctValue = new GroupedValuesList(val);
                        thisDistinctValuesList.Add(newDistinctValue);
                        thisDistinctValuesList = newDistinctValue.ls;

                        if(j == this.FieldsToGroupBy.Count -1)
                            dt2.ImportRow(dt.Rows[i]);
                    }
                    else
                    {
                        thisDistinctValuesList = thisDistinctValuesList[myind].ls;
                    }
                    
                    
                }

            }

            System.Data.DataTable dtt = dt2.Sort("SO_Nr ASC, GB_Nr ASC");

            System.Console.WriteLine(dtt.Rows.Count);
            System.Console.WriteLine(GroupedValues.Count);

            // System.Console.WriteLine(drs);
            // dt2.DefaultView.Sort = "SO_Nr ASC";
            // System.Data.DataView dv = dt2.DefaultView;
            // dt2 = dt2.DefaultView.ToTable();
        }




    }


    public class cDistinctValue
    {
        public object parent;
        public object Value;
        public System.Data.DataTable data;

        public cDistinctValue()
        { 
        
        }
        

    }



}
