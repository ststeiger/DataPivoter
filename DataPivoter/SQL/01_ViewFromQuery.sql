/*
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[_____MySchema]'))
DROP VIEW [dbo].[_____MySchema]
GO



CREATE VIEW [dbo].[_____MySchema] AS 
*/
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