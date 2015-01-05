SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Attachment]', N'U') IS NOT NULL
DROP TABLE [dbo].[Attachment]
GO

/****** Object:  Table [dbo].[Attachment]    Script Date: 5/12/2014 4:45:17 PM ******/
CREATE TABLE [dbo].[Attachment](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[FileType] [varchar](32) NULL,
	[FileSize] [decimal](9,2) NULL,
	[FileHashCode] [varchar](128) NULL,
	[FileData] [image] NULL,
	[EntityTypeValue] [char](2) NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[ReferenceName] [nvarchar](256) NOT NULL,
	[Tags] [nvarchar](1024) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[Attachment_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[Attachment_Log]
GO

/****** Object:  Table [dbo].[Attachment_Log]    Script Date: 5/12/2014 4:45:17 PM ******/

CREATE TABLE [dbo].[Attachment_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[FileType] [varchar](32) NULL,
	[FileSize] [decimal](9,2) NULL,
	[FileHashCode] [varchar](128) NULL,
	[FileData] [image] NULL,
	[EntityTypeValue] [char](2) NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[ReferenceName] [nvarchar](256) NOT NULL,
	[Tags] [nvarchar](1024) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Attachment_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[AttachmentView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AttachmentView]
GO

/****** Object:  View [dbo].[AttachmentView]    Script Date: 5/12/2014 4:45:17 PM ******/
CREATE VIEW [dbo].[AttachmentView]
AS
	SELECT
	A.[Id]
	,A.[Name]
	,A.[FileType]
	,A.[FileSize]
	,A.[FileHashCode]
	,A.[FileData]
	,A.[EntityTypeValue]
	,A.[ReferenceId]
	,A.[ReferenceName]
	,A.[Tags]
	,A.[Description]
	,A.[ImageData]
	,A.[RevisionNumber]
	,A.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,A.[CreatedOn]
	,A.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,A.[ModifiedOn]
	From Attachment A
	LEFT JOIN Contact CC ON A.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON A.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_AttachmentInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AttachmentInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_AttachmentInsert]    Script Date: 5/12/2014 4:45:17 PM ******/
CREATE PROC [dbo].[usp_AttachmentInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@FileType [varchar](32)  = NULL
	,@FileSize [decimal](9,2)  = NULL
	,@FileHashCode [varchar](128)  = NULL
	,@FileData [image]  = NULL
	,@EntityTypeValue [char](2) 
	,@ReferenceId [uniqueidentifier] 
	,@ReferenceName [nvarchar](256)
	,@Tags [nvarchar](1024)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Attachment] ([Id], [Name], [FileType], [FileSize], [FileHashCode], [FileData], [EntityTypeValue], [ReferenceId], [ReferenceName], [Tags], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @FileType, @FileSize, @FileHashCode, @FileData, @EntityTypeValue, @ReferenceId,  @ReferenceName, @Tags, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[Attachment_log] ([Id], [Name], [FileType], [FileSize], [FileHashCode], [FileData], [EntityTypeValue], [ReferenceId],  [ReferenceName], [Tags], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	A.[Id]
	,A.[Name]
	,A.[FileType]
	,A.[FileSize]
	,A.[FileHashCode]
	,A.[FileData]
	,A.[EntityTypeValue]
	,A.[ReferenceId]
	,A.[ReferenceName]
	,A.[Tags]
	,A.[Description]
	,A.[ImageData]
	,A.[RevisionNumber]
	,A.[CreatedBy]
	,A.[CreatedOn]
	,A.[ModifiedBy]
	,A.[ModifiedOn]
	From Attachment A

	WHERE A.[Id] = @Id


	COMMIT;

	SELECT
	A.[Id]
	,A.[Name]
	,A.[FileType]
	,A.[FileSize]
	,A.[FileHashCode]
	,A.[FileData]
	,A.[EntityTypeValue]
	,A.[ReferenceId]
	,A.[ReferenceName]
	,A.[Tags]
	,A.[Description]
	,A.[ImageData]
	,A.[RevisionNumber]
	,A.[CreatedBy]
	,A.[CreatedOn]
	,A.[ModifiedBy]
	,A.[ModifiedOn]
	From Attachment A
	WHERE A.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AttachmentUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AttachmentUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_AttachmentUpdate]    Script Date: 5/12/2014 4:45:17 PM ******/
CREATE PROC [dbo].[usp_AttachmentUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@FileType [varchar](32)  = NULL
	,@FileSize [decimal](9,2)  = NULL
	,@FileHashCode [varchar](128)  = NULL
	,@FileData [image]  = NULL
	,@EntityTypeValue [char](2) 
	,@ReferenceId [uniqueidentifier] 
	,@ReferenceName [nvarchar](256)
	,@Tags [nvarchar](1024)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Attachment]
	SET [Id] = @Id, [Name] = @Name, [FileType] = @FileType, [FileSize] = @FileSize, [FileHashCode] = @FileHashCode, [FileData] = @FileData, [EntityTypeValue] = @EntityTypeValue, [ReferenceId] = @ReferenceId, [ReferenceName] = @ReferenceName, [Tags] = @Tags, [Description] = @Description, [ImageData] = @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[Attachment_log] ([Id], [Name], [FileType], [FileSize], [FileHashCode], [FileData], [EntityTypeValue], [ReferenceId], [ReferenceName], [Tags], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	A.[Id]
	,A.[Name]
	,A.[FileType]
	,A.[FileSize]
	,A.[FileHashCode]
	,A.[FileData]
	,A.[EntityTypeValue]
	,A.[ReferenceId]
	,A.[ReferenceName]
	,A.[Tags]
	,A.[Description]
	,A.[ImageData]
	,A.[RevisionNumber]
	,A.[CreatedBy]
	,A.[CreatedOn]
	,A.[ModifiedBy]
	,A.[ModifiedOn]
	From Attachment A

	WHERE A.[Id] = @Id


	COMMIT;

	SELECT
	A.[Id]
	,A.[Name]
	,A.[FileType]
	,A.[FileSize]
	,A.[FileHashCode]
	,A.[FileData]
	,A.[EntityTypeValue]
	,A.[ReferenceId]
	,A.[ReferenceName]
	,A.[Tags]
	,A.[Description]
	,A.[ImageData]
	,A.[RevisionNumber]
	,A.[CreatedBy]
	,A.[CreatedOn]
	,A.[ModifiedBy]
	,A.[ModifiedOn]
	From Attachment A
	WHERE A.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AttachmentDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AttachmentDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_AttachmentDelete]    Script Date: 5/12/2014 4:45:17 PM ******/
CREATE PROC [dbo].[usp_AttachmentDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Attachment] WHERE  [Id] = @Id
COMMIT;
GO

