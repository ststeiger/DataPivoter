
namespace foo
{
	var bar;
	var template:string = `Test
	123 345
	6789`;
	
	
	
	class Customer
	{}
	
	var map: { [email: string]: Customer; } = { };
	
	var mylist:{ [email: string]: Customer; }[] = [];
	map['col1'] = new Customer(); // OK
	map['col2'] = new Customer(); // OK
	
	mylist.push(map);
	
	
	
	var data = [
  {
    "SO_UID": "a256542f-b313-4448-8f4a-ff312c702d45",
    "SO_Nr": "7500",
    "SO_Bezeichnung": "St. Moritz",
    "GB_UID": "54505665-82ef-4388-a343-11bf5c80833c",
    "GB_Nr": "PS4",
    "GB_Bezeichnung": "PS4",
    "GS_UID": "3f4ab7a7-523c-4b26-923d-c758210a17cb",
    "GS_GB_UID": "54505665-82ef-4388-a343-11bf5c80833c",
    "GS_GST_UID": "5baf44cc-4d1c-4ea6-91d5-6ac7b4f3a915",
    "GS_Nr": "02",
    "GST_Kurz_DE": "OG",
    "GS_Kurz_DE": "OG02",
    "GS_Sort": 1,
    "GS_Sort1": 1,
    "RM_UID": "9a2614f4-5415-4686-ba80-26dde7c6ab9d",
    "RM_Nr": "3",
    "NA_UID": "ce0ca343-203d-4cd9-9b85-576125734c7f",
    "NA_LANG_DE": "Sanitär",
    "NA_Sort": 71,
    "NA_Code": "7.1",
    "NA_CodeNr": 7.1,
    "NAG_UID": "cc6bbe95-7d63-4d33-99f0-315d92d090be",
    "NAG_LANG_DE": "Sonstige Nutzen",
    "NAG_Sort": 7,
    "RM_DatumVon": "\/Date(1325372400000+0100)\/",
    "RM_DatumBis": "\/Date(32503590000000+0100)\/",
    "ZO_RMFlaeche_Flaeche": 2.5998928566105195
  },
  {
    "SO_UID": "a256542f-b313-4448-8f4a-ff312c702d45",
    "SO_Nr": "7500",
    "SO_Bezeichnung": "St. Moritz",
    "GB_UID": "54505665-82ef-4388-a343-11bf5c80833c",
    "GB_Nr": "PS4",
    "GB_Bezeichnung": "PS4",
    "GS_UID": "3f4ab7a7-523c-4b26-923d-c758210a17cb",
    "GS_GB_UID": "54505665-82ef-4388-a343-11bf5c80833c",
    "GS_GST_UID": "5baf44cc-4d1c-4ea6-91d5-6ac7b4f3a915",
    "GS_Nr": "02",
    "GST_Kurz_DE": "OG",
    "GS_Kurz_DE": "OG02",
    "GS_Sort": 1,
    "GS_Sort1": 1,
    "RM_UID": "1bfe110d-dbf9-4391-83aa-89a0a2c1de97",
    "RM_Nr": "4",
    "NA_UID": "ce0ca343-203d-4cd9-9b85-576125734c7f",
    "NA_LANG_DE": "Sanitär",
    "NA_Sort": 71,
    "NA_Code": "7.1",
    "NA_CodeNr": 7.1,
    "NAG_UID": "cc6bbe95-7d63-4d33-99f0-315d92d090be",
    "NAG_LANG_DE": "Sonstige Nutzen",
    "NAG_Sort": 7,
    "RM_DatumVon": "\/Date(1325372400000+0100)\/",
    "RM_DatumBis": "\/Date(32503590000000+0100)\/",
    "ZO_RMFlaeche_Flaeche": 1.96611563582905
  }
	];
	
	// console.log(data);
	
	
	
	
	var myrow: { [key: string]: any; } = { };
	var mytable:{ [key: string]: any; }[] = [];
	
	mytable = data;
	
	var str:string[] = [];
	var df:DocumentFragment = document.createDocumentFragment();
	
	for(var i = 0; i < mytable.length; ++i)
	{
		var dr = mytable[i];
		// console.log(dr);
		
		var tr = <HTMLTableRowElement> document.createElement("TR");
		
		for(var columnName in dr)
		{
			var td = <HTMLTableDataCellElement> document.createElement("TD");
			
			var val = dr[columnName];
			console.log(val);
			
			var tn = document.createTextNode(dr[columnName])
			td.appendChild(tn);
			
			tr.appendChild(td);
		}
		
		df.appendChild(tr);
	}
	
	// console.log(df.innerHTML)
	document.body.appendChild(df);
	
}
