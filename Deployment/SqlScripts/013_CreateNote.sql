SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Note]', N'U') IS NOT NULL
DROP TABLE [dbo].[Note]
GO

/****** Object:  Table [dbo].[Note]    Script Date: 4/29/2014 2:24:05 PM ******/
CREATE TABLE [dbo].[Note](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[EntityTypeValue] [char](2) NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[ReferenceName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[Note_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[Note_Log]
GO

/****** Object:  Table [dbo].[Note_Log]    Script Date: 4/29/2014 2:24:05 PM ******/

CREATE TABLE [dbo].[Note_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[EntityTypeValue] [char](2) NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[ReferenceName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Note_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[NoteView]', 'V') IS NOT NULL
DROP VIEW [dbo].[NoteView]
GO

/****** Object:  View [dbo].[NoteView]    Script Date: 4/29/2014 2:24:05 PM ******/
CREATE VIEW [dbo].[NoteView]
AS
	SELECT
	N.[Id]
	,N.[Name]
	,N.[EntityTypeValue]
	,N.[ReferenceId]
	,N.[ReferenceName]
	,N.[Description]
	,N.[ImageData]
	,N.[RevisionNumber]
	,N.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,N.[CreatedOn]
	,N.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,N.[ModifiedOn]
	From Note N
	LEFT JOIN Contact CC ON N.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON N.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_NoteInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NoteInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_NoteInsert]    Script Date: 4/29/2014 2:24:05 PM ******/
CREATE PROC [dbo].[usp_NoteInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@EntityTypeValue [char](2) 
	,@ReferenceId [uniqueidentifier]
	,@ReferenceName [nvarchar](256)  
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Note] ([Id], [Name], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @EntityTypeValue, @ReferenceId, @ReferenceName, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[Note_log] ([Id], [Name], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	N.[Id]
	,N.[Name]
	,N.[EntityTypeValue]
	,N.[ReferenceId]
	,N.[ReferenceName]
	,N.[Description]
	,N.[ImageData]
	,N.[RevisionNumber]
	,N.[CreatedBy]
	,N.[CreatedOn]
	,N.[ModifiedBy]
	,N.[ModifiedOn]
	From Note N

	WHERE N.[Id] = @Id


	COMMIT;

	SELECT
	N.[Id]
	,N.[Name]
	,N.[EntityTypeValue]
	,N.[ReferenceId]
	,N.[ReferenceName]
	,N.[Description]
	,N.[ImageData]
	,N.[RevisionNumber]
	,N.[CreatedBy]
	,N.[CreatedOn]
	,N.[ModifiedBy]
	,N.[ModifiedOn]
	From Note N
	WHERE N.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_NoteUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NoteUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_NoteUpdate]    Script Date: 4/29/2014 2:24:05 PM ******/
CREATE PROC [dbo].[usp_NoteUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@EntityTypeValue [char](2) 
	,@ReferenceId [uniqueidentifier] 
	,@ReferenceName [nvarchar](256)
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Note]
	SET [Id] = @Id, [Name] = @Name, [EntityTypeValue] = @EntityTypeValue, [ReferenceId] = @ReferenceId, [ReferenceName] = @ReferenceName, [Description] = @Description, [ImageData]= @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[Note_log] ([Id], [Name], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	N.[Id]
	,N.[Name]
	,N.[EntityTypeValue]
	,N.[ReferenceId]
	,N.[ReferenceName]
	,N.[Description]
	,N.[ImageData]
	,N.[RevisionNumber]
	,N.[CreatedBy]
	,N.[CreatedOn]
	,N.[ModifiedBy]
	,N.[ModifiedOn]
	From Note N

	WHERE N.[Id] = @Id


	COMMIT;

	SELECT
	N.[Id]
	,N.[Name]
	,N.[EntityTypeValue]
	,N.[ReferenceId]
	,N.[ReferenceName]
	,N.[Description]
	,N.[ImageData]
	,N.[RevisionNumber]
	,N.[CreatedBy]
	,N.[CreatedOn]
	,N.[ModifiedBy]
	,N.[ModifiedOn]
	From Note N
	WHERE N.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_NoteDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NoteDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_NoteDelete]    Script Date: 4/29/2014 2:24:05 PM ******/
CREATE PROC [dbo].[usp_NoteDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Note] WHERE  [Id] = @Id
COMMIT;
GO

