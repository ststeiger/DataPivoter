
INSERT INTO _____aMySchema
(
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
	,GS_Kurz_DE
	,GS_Sort
	,GS_Sort1
	,RM_UID
	,RM_Nr
	,NA_UID
	,NA_LANG_DE
	,NA_Sort
	,NA_Code
	,NA_CodeNr
	,NAG_UID
	,NAG_LANG_DE
	,NAG_Sort
	,RM_DatumVon
	,RM_DatumBis
	,ZO_RMFlaeche_Flaeche
)
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
	,GS_Kurz_DE
	,GS_Sort
	,GS_Sort1
	,RM_UID
	,RM_Nr
	,NA_UID
	,NA_LANG_DE
	,NA_Sort
	,NA_Code
	,NA_CodeNr
	,NAG_UID
	,NAG_LANG_DE
	,NAG_Sort
	,RM_DatumVon
	,RM_DatumBis
	,ZO_RMFlaeche_Flaeche
FROM _____MySchema
