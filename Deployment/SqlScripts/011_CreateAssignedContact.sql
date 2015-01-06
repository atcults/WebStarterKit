SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[AssignedContact]', N'U') IS NOT NULL
DROP TABLE [dbo].[AssignedContact]
GO

/****** Object:  Table [dbo].[AssignedContact]    Script Date: 4/29/2014 2:53:26 PM ******/
CREATE TABLE [dbo].[AssignedContact](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL,
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
CONSTRAINT [PK_AssignedContact] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[AssignedContact_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[AssignedContact_Log]
GO

/****** Object:  Table [dbo].[AssignedContact_Log]    Script Date: 4/29/2014 2:53:26 PM ******/

CREATE TABLE [dbo].[AssignedContact_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL,
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
CONSTRAINT [PK_AssignedContact_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[AssignedContactView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AssignedContactView]
GO

/****** Object:  View [dbo].[AssignedContactView]    Script Date: 4/29/2014 2:53:26 PM ******/
CREATE VIEW [dbo].[AssignedContactView]
AS
	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[ContactId]
	,CO.[Name] as ContactName
	,CO.[Mobile]
	,CO.[Email]
	,CO.[GenderValue]
	,CO.[ContactTypeValue]
	,CO.[PrimaryLanguageValue]
	,CO.[SecondaryLanguageValue]
	,AC.[EntityTypeValue]
	,AC.[ReferenceId]
	,AC.[ReferenceName]
	,AC.[Description]
	,AC.[ImageData]
	,AC.[RevisionNumber]
	,AC.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,AC.[CreatedOn]
	,AC.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,AC.[ModifiedOn]
	From AssignedContact AC
	INNER JOIN Contact CO ON CO.[Id] = AC.[ContactId]
	LEFT JOIN Contact CC ON AC.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON AC.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_AssignedContactInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AssignedContactInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_AssignedContactInsert]    Script Date: 4/29/2014 2:53:26 PM ******/
CREATE PROC [dbo].[usp_AssignedContactInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@ContactId [uniqueidentifier] 
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

	INSERT INTO [dbo].[AssignedContact] ([Id], [Name], [ContactId], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @ContactId, @EntityTypeValue, @ReferenceId, @ReferenceName, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[AssignedContact_log] ([Id], [Name], [ContactId], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[ContactId]
	,AC.[EntityTypeValue]
	,AC.[ReferenceId]
	,AC.[ReferenceName]
	,AC.[Description]
	,AC.[ImageData]
	,AC.[RevisionNumber]
	,AC.[CreatedBy]
	,AC.[CreatedOn]
	,AC.[ModifiedBy]
	,AC.[ModifiedOn]
	From AssignedContact AC

	WHERE AC.[Id] = @Id


	COMMIT;

	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[ContactId]
	,AC.[EntityTypeValue]
	,AC.[ReferenceId]
	,AC.[ReferenceName]
	,AC.[Description]
	,AC.[ImageData]
	,AC.[RevisionNumber]
	,AC.[CreatedBy]
	,AC.[CreatedOn]
	,AC.[ModifiedBy]
	,AC.[ModifiedOn]
	From AssignedContact AC
	WHERE AC.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AssignedContactUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AssignedContactUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_AssignedContactUpdate]    Script Date: 4/29/2014 2:53:26 PM ******/
CREATE PROC [dbo].[usp_AssignedContactUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@ContactId [uniqueidentifier] 
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

	UPDATE [dbo].[AssignedContact]
	SET [Id] = @Id, [Name] = @Name, [ContactId] = @ContactId, [EntityTypeValue] = @EntityTypeValue, [ReferenceId] = @ReferenceId, [ReferenceName] = @ReferenceName, [Description] = @Description, [ImageData] = @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[AssignedContact_log] ([Id], [Name], [ContactId], [EntityTypeValue], [ReferenceId], [ReferenceName], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[ContactId]
	,AC.[EntityTypeValue]
	,AC.[ReferenceId]
	,AC.[ReferenceName]
	,AC.[Description]
	,AC.[ImageData]
	,AC.[RevisionNumber]
	,AC.[CreatedBy]
	,AC.[CreatedOn]
	,AC.[ModifiedBy]
	,AC.[ModifiedOn]
	From AssignedContact AC

	WHERE AC.[Id] = @Id


	COMMIT;

	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[ContactId]
	,AC.[EntityTypeValue]
	,AC.[ReferenceId]
	,AC.[ReferenceName]
	,AC.[Description]
	,AC.[ImageData]
	,AC.[RevisionNumber]
	,AC.[CreatedBy]
	,AC.[CreatedOn]
	,AC.[ModifiedBy]
	,AC.[ModifiedOn]
	From AssignedContact AC
	WHERE AC.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AssignedContactDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AssignedContactDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_AssignedContactDelete]    Script Date: 4/29/2014 2:53:26 PM ******/
CREATE PROC [dbo].[usp_AssignedContactDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[AssignedContact] WHERE  [Id] = @Id
COMMIT;
GO

