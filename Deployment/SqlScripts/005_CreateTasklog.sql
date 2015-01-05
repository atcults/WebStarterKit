SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[TaskLog]', N'U') IS NOT NULL
DROP TABLE [dbo].[TaskLog]
GO

/****** Object:  Table [dbo].[TaskLog]    Script Date: 5/8/2014 1:19:25 PM ******/
CREATE TABLE [dbo].[TaskLog](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[TaskStatusValue] [char](2) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_TaskLog] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[TaskLog_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[TaskLog_Log]
GO

/****** Object:  Table [dbo].[TaskLog_Log]    Script Date: 5/8/2014 1:19:25 PM ******/

CREATE TABLE [dbo].[TaskLog_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[TaskStatusValue] [char](2) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_TaskLog_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[TaskLogView]', 'V') IS NOT NULL
DROP VIEW [dbo].[TaskLogView]
GO

/****** Object:  View [dbo].[TaskLogView]    Script Date: 5/8/2014 1:19:25 PM ******/
CREATE VIEW [dbo].[TaskLogView]
AS
	SELECT
	TL.[Id]
	,TL.[Name]
	,TL.[StartTime]
	,TL.[EndTime]
	,TL.[TaskStatusValue]
	,TL.[Description]
	,TL.[ImageData]
	,TL.[RevisionNumber]
	,TL.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,TL.[CreatedOn]
	,TL.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,TL.[ModifiedOn]
	From TaskLog TL
	LEFT JOIN Contact CC ON TL.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON TL.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_TaskLogInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TaskLogInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_TaskLogInsert]    Script Date: 5/8/2014 1:19:25 PM ******/
CREATE PROC [dbo].[usp_TaskLogInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@StartTime [datetime]  = NULL
	,@EndTime [datetime]  = NULL
	,@TaskStatusValue [char](2)  = NULL
	,@Description [nvarchar](2048) = NULL
	,@ImageData [varchar](max) = NULL
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[TaskLog] ([Id], [Name], [StartTime], [EndTime], [TaskStatusValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @StartTime, @EndTime, @TaskStatusValue, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[TaskLog_log] ([Id], [Name], [StartTime], [EndTime], [TaskStatusValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	TL.[Id]
	,TL.[Name]
	,TL.[StartTime]
	,TL.[EndTime]
	,TL.[TaskStatusValue]
	,TL.[Description]
	,TL.[ImageData]
	,TL.[RevisionNumber]
	,TL.[CreatedBy]
	,TL.[CreatedOn]
	,TL.[ModifiedBy]
	,TL.[ModifiedOn]
	From TaskLog TL

	WHERE TL.[Id] = @Id


	COMMIT;

	SELECT
	TL.[Id]
	,TL.[Name]
	,TL.[StartTime]
	,TL.[EndTime]
	,TL.[TaskStatusValue]	
	,TL.[Description]
	,TL.[ImageData]
	,TL.[RevisionNumber]
	,TL.[CreatedBy]
	,TL.[CreatedOn]
	,TL.[ModifiedBy]
	,TL.[ModifiedOn]
	From TaskLog TL
	WHERE TL.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TaskLogUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TaskLogUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_TaskLogUpdate]    Script Date: 5/8/2014 1:19:25 PM ******/
CREATE PROC [dbo].[usp_TaskLogUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@StartTime [datetime]  = NULL
	,@EndTime [datetime]  = NULL
	,@TaskStatusValue [char](2)  = NULL	
	,@Description [nvarchar](2048) = Null
	,@ImageData [varchar](max) = Null
	,@UserID  [uniqueidentifier] = Null

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[TaskLog]
	SET [Id] = @Id, [Name] = @Name, [StartTime] = @StartTime, [EndTime] = @EndTime, [TaskStatusValue] = @TaskStatusValue, [Description] = @Description, [ImageData] = @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[TaskLog_log] ([Id], [Name], [StartTime], [EndTime], [TaskStatusValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	TL.[Id]
	,TL.[Name]
	,TL.[StartTime]
	,TL.[EndTime]
	,TL.[TaskStatusValue]	
	,TL.[Description]
	,TL.[ImageData]
	,TL.[RevisionNumber]
	,TL.[CreatedBy]
	,TL.[CreatedOn]
	,TL.[ModifiedBy]
	,TL.[ModifiedOn]
	From TaskLog TL

	WHERE TL.[Id] = @Id


	COMMIT;

	SELECT
	TL.[Id]
	,TL.[Name]
	,TL.[StartTime]
	,TL.[EndTime]
	,TL.[TaskStatusValue]	
	,TL.[Description]
	,TL.[ImageData]
	,TL.[RevisionNumber]
	,TL.[CreatedBy]
	,TL.[CreatedOn]
	,TL.[ModifiedBy]
	,TL.[ModifiedOn]
	From TaskLog TL
	WHERE TL.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TaskLogDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TaskLogDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_TaskLogDelete]    Script Date: 5/8/2014 1:19:25 PM ******/
CREATE PROC [dbo].[usp_TaskLogDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[TaskLog] WHERE  [Id] = @Id
COMMIT;
GO

