
DECLARE @schemaName nvarchar(255)
DECLARE @tableName nvarchar(255)

SET @schemaName = 'dbo'
SET @tableName = '_____MySchema'


;WITH CTE AS 
(
	SELECT 
		 'CREATE TABLE ' + QUOTENAME(@schemaName) + '.' + QUOTENAME(@tableName) AS SQL 
		,1 AS SortOrder 
		,0 AS ORDINAL_POSITION 
		

	UNION ALL 

	SELECT 
		 '(' 
		,2 AS SortOrder 
		,0 AS ORDINAL_POSITION 


	UNION ALL 

	SELECT 
		CASE 
			WHEN ORDINAL_POSITION = 1 THEN '     '
			ELSE '    ,'
		END 
		+ COLUMN_NAME 
		+ ' ' + DATA_TYPE 
		+ ISNULL('(' +CAST(CHARACTER_MAXIMUM_LENGTH AS varchar(20)) + ')', '') 
		+ CASE 
			WHEN IS_NULLABLE = 'YES' THEN ' NULL' 
			ELSE ' NOT NULL' 
		  END 
		 AS SQL 
		,3 AS SortOrder 
		,ORDINAL_POSITION 
	FROM INFORMATION_SCHEMA.COLUMNS AS isc 
	WHERE (1=1) 
	AND isc.TABLE_SCHEMA = @schemaName 
	AND isc.TABLE_NAME = @tableName 
	
	
	UNION ALL 
	
	
	SELECT 
		');'  AS SQL 
		,4 AS SortOrder 
		,0 AS ORDINAL_POSITION 
)
SELECT SQL 
FROM CTE 
ORDER BY SortOrder, ORDINAL_POSITION 
