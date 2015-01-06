SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Contact]', N'U') IS NOT NULL
DROP TABLE [dbo].[Contact]
GO

/****** Object:  Table [dbo].[Contact]    Script Date: 12/4/2014 9:17:38 AM ******/
CREATE TABLE [dbo].[Contact](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[GenderValue] [char](2) NULL,
	[Mobile] [varchar](16) NULL,
	[Email] [varchar](256) NULL,
	[ContactTypeValue] [char](2) NULL,
	[PrimaryLanguageValue] [char](2) NULL,
	[SecondaryLanguageValue] [char](2) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[Contact_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[Contact_Log]
GO

/****** Object:  Table [dbo].[Contact_Log]    Script Date: 12/4/2014 9:17:38 AM ******/

CREATE TABLE [dbo].[Contact_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[GenderValue] [char](2) NULL,
	[Mobile] [varchar](16) NULL,
	[Email] [varchar](256) NULL,
	[ContactTypeValue] [char](2) NULL,
	[PrimaryLanguageValue] [char](2) NULL,
	[SecondaryLanguageValue] [char](2) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Contact_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[ContactView]', 'V') IS NOT NULL
DROP VIEW [dbo].[ContactView]
GO

/****** Object:  View [dbo].[ContactView]    Script Date: 12/4/2014 9:17:38 AM ******/
CREATE VIEW [dbo].[ContactView]
AS
	SELECT
	C.[Id]
	,C.[Name]
	,C.[GenderValue]
	,C.[Mobile]
	,C.[Email]
	,C.[ContactTypeValue]
	,C.[PrimaryLanguageValue]
	,C.[SecondaryLanguageValue]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,C.[ModifiedOn]
	From Contact C
	LEFT JOIN Contact CC ON C.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON C.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_ContactInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_ContactInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_ContactInsert]    Script Date: 12/4/2014 9:17:38 AM ******/
CREATE PROC [dbo].[usp_ContactInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@GenderValue [char](2)  = NULL
	,@Mobile [varchar](16)  = NULL
	,@Email [varchar](256)  = NULL
	,@ContactTypeValue [char](2) 
	,@PrimaryLanguageValue [char](2)  = NULL
	,@SecondaryLanguageValue [char](2)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Contact] ([Id], [Name], [GenderValue], [Mobile], [Email], [ContactTypeValue], [PrimaryLanguageValue], [SecondaryLanguageValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @GenderValue, @Mobile, @Email, @ContactTypeValue, @PrimaryLanguageValue, @SecondaryLanguageValue, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[Contact_log] ([Id], [Name], [GenderValue], [Mobile], [Email], [ContactTypeValue], [PrimaryLanguageValue], [SecondaryLanguageValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	C.[Id]
	,C.[Name]
	,C.[GenderValue]
	,C.[Mobile]
	,C.[Email]
	,C.[ContactTypeValue]
	,C.[PrimaryLanguageValue]
	,C.[SecondaryLanguageValue]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,C.[ModifiedOn]
	From Contact C

	WHERE C.[Id] = @Id


	COMMIT;

	SELECT
	C.[Id]
	,C.[Name]
	,C.[GenderValue]
	,C.[Mobile]
	,C.[Email]
	,C.[ContactTypeValue]
	,C.[PrimaryLanguageValue]
	,C.[SecondaryLanguageValue]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,C.[ModifiedOn]
	From Contact C
	WHERE C.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_ContactUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_ContactUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_ContactUpdate]    Script Date: 12/4/2014 9:17:38 AM ******/
CREATE PROC [dbo].[usp_ContactUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@GenderValue [char](2)  = NULL
	,@Mobile [varchar](16)  = NULL
	,@Email [varchar](256)  = NULL
	,@ContactTypeValue [char](2) 
	,@PrimaryLanguageValue [char](2)  = NULL
	,@SecondaryLanguageValue [char](2)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Contact]
	SET [Id] = @Id, [Name] = @Name, [GenderValue] = @GenderValue, [Mobile] = @Mobile, [Email] = @Email, [ContactTypeValue] = @ContactTypeValue, [PrimaryLanguageValue] = @PrimaryLanguageValue, [SecondaryLanguageValue] = @SecondaryLanguageValue, [Description] = @Description, [ImageData] = @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[Contact_log] ([Id], [Name], [GenderValue], [Mobile], [Email], [ContactTypeValue], [PrimaryLanguageValue], [SecondaryLanguageValue], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	C.[Id]
	,C.[Name]
	,C.[GenderValue]
	,C.[Mobile]
	,C.[Email]
	,C.[ContactTypeValue]
	,C.[PrimaryLanguageValue]
	,C.[SecondaryLanguageValue]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,C.[ModifiedOn]
	From Contact C

	WHERE C.[Id] = @Id


	COMMIT;

	SELECT
	C.[Id]
	,C.[Name]
	,C.[GenderValue]
	,C.[Mobile]
	,C.[Email]
	,C.[ContactTypeValue]
	,C.[PrimaryLanguageValue]
	,C.[SecondaryLanguageValue]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,C.[ModifiedOn]
	From Contact C
	WHERE C.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_ContactDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_ContactDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_ContactDelete]    Script Date: 12/4/2014 9:17:38 AM ******/
CREATE PROC [dbo].[usp_ContactDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Contact] WHERE  [Id] = @Id
COMMIT;
GO

