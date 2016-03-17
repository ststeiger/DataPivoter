CREATE TABLE [dbo].[_____aMySchema]
(
     SO_UID uniqueidentifier NOT NULL
    ,SO_Nr varchar(4) NOT NULL
    ,SO_Bezeichnung varchar(200) NULL
    ,GB_UID uniqueidentifier NULL
    ,GB_Nr varchar(10) NULL
    ,GB_Bezeichnung varchar(100) NULL
    ,GS_UID uniqueidentifier NULL
    ,GS_GB_UID uniqueidentifier NULL
    ,GS_GST_UID uniqueidentifier NULL
    ,GS_Nr varchar(2) NULL
    ,GST_Kurz_DE varchar(50) NULL
    ,GS_Kurz_DE varchar(52) NULL
    ,GS_Sort bigint NULL
    ,GS_Sort1 bigint NULL
    ,RM_UID uniqueidentifier NULL
    ,RM_Nr varchar(25) NULL
    ,NA_UID uniqueidentifier NULL
    ,NA_LANG_DE varchar(255) NULL
    ,NA_Sort int NULL
    ,NA_Code varchar(5) NULL
    ,NA_CodeNr float NULL
    ,NAG_UID uniqueidentifier NULL
    ,NAG_LANG_DE varchar(255) NULL
    ,NAG_Sort int NULL
    ,RM_DatumVon datetime NULL
    ,RM_DatumBis datetime NULL
	,ZO_RMFlaeche_Flaeche float NULL
);
