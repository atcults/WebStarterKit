
IF OBJECT_ID('dbo.[AuditedCommand]', N'U') IS NOT NULL
DROP TABLE [dbo].[AuditedCommand]
GO

CREATE TABLE [dbo].[AuditedCommand](
	[SequenceNumber] [int] IDENTITY(1,1) NOT NULL,
	[AggregateId] [uniqueidentifier] NULL,
	[CommandText] [varchar](max) NULL,
	[CommandName] [varchar](255) NULL,
	[PerformingUserId] [uniqueidentifier] NULL,
	[PerformingUserName] [varchar](255) NULL,
	[PerformedOn] [datetime] NULL,
	
 CONSTRAINT [PK_Command] PRIMARY KEY CLUSTERED 
(
	[SequenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF OBJECT_ID('dbo.[AuditedCommandView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AuditedCommandView]
GO

CREATE VIEW [dbo].[AuditedCommandView]
	AS
	SELECT [SequenceNumber]
      ,[AggregateId] as Id
      ,[CommandText]
      ,[CommandName]
      ,[PerformingUserId]
      ,[PerformingUserName]
      ,[PerformedOn]
	  ,NULL as OriginalId
	  ,'true' as IsActive
  FROM [dbo].[AuditedCommand]

GO

IF OBJECT_ID('dbo.[CoreConfiguration]', N'U') IS NOT NULL
DROP TABLE [dbo].[CoreConfiguration]
GO

CREATE TABLE [dbo].[CoreConfiguration](
	[ConfigName] [varchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[ConfigName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

-- =============================================
-- Author:		Sunny
-- Create date: 4.14.2013
-- Description:	Person Name Formatter
-- =============================================

IF OBJECT_ID('dbo.[FormatName]', N'FN') IS NOT NULL
DROP FUNCTION[dbo].[FormatName]

GO

CREATE FUNCTION [dbo].FormatName
(
	@FirstName VARCHAR(50),
	@MiddleInitial VARCHAR(50),
	@LastName VARCHAR(50)
)
RETURNS VARCHAR(100)
AS
BEGIN
	
	RETURN LTRIM(RTRIM(
    LTRIM(RTRIM(ISNULL(@LastName, ''))) + ', ' +
    LTRIM(RTRIM(ISNULL(@FirstName, ''))) + ' ' + 
    LTRIM(ISNULL(@MiddleInitial, ''))
    ))

END
GO

IF OBJECT_ID('dbo.[usp_CustomSelect]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_CustomSelect]
GO

CREATE PROC [dbo].[usp_CustomSelect] 
	@TableName AS VARCHAR(255),
	@PropertyName AS VARCHAR(255),
	@PropertyVal AS NVARCHAR(255)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	DECLARE @SQLQuery AS NVARCHAR(255)

	SET @SQLQuery = 'SELECT * FROM ' + @Tablename + ' WHERE ' + QUOTENAME(@PropertyName) + ' = @v';

	EXEC sp_executesql @SQLQuery, N'@v VARCHAR(255)', @PropertyVal;

GO



