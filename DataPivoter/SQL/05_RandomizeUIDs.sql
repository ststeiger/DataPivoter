
     
ALTER TABLE dbo._____aMySchema
	ADD 
	[SO_ID] [int] NULL,
	[GB_ID] [int] NULL,
	[GS_ID] [int] NULL,
	[RM_ID] [int] NULL,
	[NA_ID] [int] NULL,
	[NAG_ID] [int] NULL
	
GO 


;WITH CTE AS 
(
SELECT SO_UID
      ,SO_ID
      ,DENSE_RANK() OVER (ORDER BY SO_UID, SO_Nr) AS drSO 
      
      ,GB_UID
      ,GB_ID
      ,DENSE_RANK() OVER (ORDER BY GB_UID, GB_Nr) AS drGB 
      
      ,GS_UID
      ,GS_ID
      ,DENSE_RANK() OVER (ORDER BY GS_GB_UID, GS_UID, GS_Nr) AS drGS 
      
      ,RM_UID
      ,RM_ID
      ,DENSE_RANK() OVER (ORDER BY RM_UID, RM_Nr) AS drRM 
      
      ,NA_UID
      ,NA_ID
      ,DENSE_RANK() OVER (ORDER BY CAST(NA_Code AS float), NA_UID) AS drNA 
      
      
      ,NAG_UID
      ,NAG_ID
      ,DENSE_RANK() OVER (ORDER BY CAST(NAG_Sort AS float), NAG_UID) AS drNAG
FROM _____aMySchema
)
	UPDATE CTE 
	SET NAG_ID = drNAG 
	,NA_ID = drNA
	,RM_ID = drRM
	,GS_ID = drGS 
	,GB_ID = drGB
	,SO_ID = drSO 


GO 



UPDATE _____aMySchema
	SET  [SO_Nr] = CAST([SO_ID] AS varchar(4))
		,[GB_Nr] = CAST([GB_ID] AS varchar(4))
		,[RM_Nr] = CAST(RM_ID AS varchar(25))
		
		
GO 
UPDATE _____aMySchema 
	SET SO_Bezeichnung = 'Location ' + SO_Nr 
	,GB_Bezeichnung = 'Building ' + GB_Nr 
      
      
GO 

-- https://stackoverflow.com/questions/794637/how-to-update-rows-with-a-random-date

UPDATE _____aMySchema 
	SET RM_DatumVon = 
		DATEADD
		(
			 day
			,(ABS(CHECKSUM(NEWID())) % CEILING(115 * 365.25)) 
			,0
		)

GO 



UPDATE _____aMySchema 
	SET RM_DatumBis = 
		DATEADD
		(
			 day
			,(ABS(CHECKSUM(NEWID())) % CEILING(800 * 365.25)) 
			+ CEILING(200 * 365.25) 
			,RM_DatumVon
		)
GO 



UPDATE _____aMySchema 
	SET ZO_RMFlaeche_Flaeche = T_ZO_AP_Raum_Flaeche.ZO_RMFlaeche_Flaeche 
-- SELECT ZO_RMFlaeche_Flaeche 
FROM _____aMySchema 

LEFT JOIN T_ZO_AP_Raum_Flaeche 
	ON ZO_RMFlaeche_RM_UID = RM_UID 
	AND ZO_RMFlaeche_Status = 1 
	--AND CURRENT_TIMESTAMP BETWEEN ZO_RMFlaeche_DatumVon AND ZO_RMFlaeche_DatumBis 
	

GO 



DECLARE @imax bigint 
DECLARE @imin bigint 
SET @imin = 0 
SET @imax = 10 


DECLARE @fmax bigint 
DECLARE @fmin bigint 
SET @fmin = 0 
SET @fmax = 10000 -- 10E18 


;WITH CTE AS 
(
	SELECT 
		 ZO_RMFlaeche_Flaeche 
		,ABS(CHECKSUM(NEWID())) % (@imax-@imin + 1) + @imin AS ran -- Random integer ∈ [@min, @max] 
		 
		,
		CAST
		(
			STUFF
			(
				RIGHT
				(
					 REPLICATE
					 (
						'0'
						,1+ LEN(CAST(@fmax AS varchar(20)))
					 ) 
					 + CAST
					 (
						ABS(CHECKSUM(NEWID())) % (@fmax-@fmin + 1) + @fmin -- Random integer ∈ [@fmin, @fmax]
						AS varchar(20)
					 ) 
					,LEN(CAST(@fmax AS varchar(20))) 
				)  
				,2
				,0
				,'.'
			)
			AS float 
		)
		AS ranFloat 
	FROM _____aMySchema 
)
/*
SELECT 
	  ran -- = random integer [@imin, @imax] 
	 ,ranFloat  -- = random float (@fmin, @fmax] pad LEN(@fmax) 
	 ,ran*ranFloat 
FROM CTE 

--ORDER BY ran 
ORDER BY ranFloat 
*/

UPDATE CTE 
	SET ZO_RMFlaeche_Flaeche = ZO_RMFlaeche_Flaeche + ran*ranFloat 
	

GO 

