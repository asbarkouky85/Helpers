
ALTER PROCEDURE [dbo].[ResetSequence]
	-- Add the parameters for the stored procedure here
	@sequenceName VARCHAR(60)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @query NVARCHAR(400);
	SELECT @query=N'DROP SEQUENCE ' + @sequenceName + N'; CREATE SEQUENCE '+@sequenceName+N' START WITH 1 INCREMENT BY 1'; 
	PRINT @query;
	EXECUTE sp_executesql @query;
END