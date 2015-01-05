SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Lead]', N'U') IS NOT NULL
DROP TABLE [dbo].[Lead]
GO

/****** Object:  Table [dbo].[Lead]    Script Date: 5/5/2014 1:15:00 PM ******/
CREATE TABLE [dbo].[Lead](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CompanyName] [nvarchar](128) NULL,
	[Email] [varchar](128) NULL,
	[Phone] [varchar](16) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Lead] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[Lead_Log]', N'U') IS NOT NULL
DROP TABLE [dbo].[Lead_Log]
GO

/****** Object:  Table [dbo].[Lead_Log]    Script Date: 5/5/2014 1:15:00 PM ******/

CREATE TABLE [dbo].[Lead_Log](
	[Sr] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CompanyName] [nvarchar](128) NULL,
	[Email] [varchar](128) NULL,
	[Phone] [varchar](16) NULL,
	[Description] [nvarchar](2048) NULL,
	[ImageData] [varchar](max) NULL,
	[RevisionNumber] [int] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
CONSTRAINT [PK_Lead_Log] PRIMARY KEY CLUSTERED 
	([Sr] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


IF OBJECT_ID('dbo.[LeadView]', 'V') IS NOT NULL
DROP VIEW [dbo].[LeadView]
GO

/****** Object:  View [dbo].[LeadView]    Script Date: 5/5/2014 1:15:00 PM ******/
CREATE VIEW [dbo].[LeadView]
AS
	SELECT
	L.[Id]
	,L.[Name]
	,L.[CompanyName]
	,L.[Email]
	,L.[Phone]
	,L.[Description]
	,L.[ImageData]
	,L.[RevisionNumber]
	,L.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,L.[CreatedOn]
	,L.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,L.[ModifiedOn]
	From Lead L
	LEFT JOIN Contact CC ON L.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON L.[ModifiedBy] = MC.[Id]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_LeadInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_LeadInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_LeadInsert]    Script Date: 5/5/2014 1:15:00 PM ******/
CREATE PROC [dbo].[usp_LeadInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@CompanyName [nvarchar](128)  = NULL
	,@Email [varchar](128)  = NULL
	,@Phone [varchar](16)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Lead] ([Id], [Name], [CompanyName], [Email], [Phone], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT @Id, @Name, @CompanyName, @Email, @Phone, @Description, @ImageData, 0, @UserId, GETDATE(), NULL, NULL

	INSERT INTO [dbo].[Lead_log] ([Id], [Name], [CompanyName], [Email], [Phone], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	L.[Id]
	,L.[Name]
	,L.[CompanyName]
	,L.[Email]
	,L.[Phone]
	,L.[Description]
	,L.[ImageData]
	,L.[RevisionNumber]
	,L.[CreatedBy]
	,L.[CreatedOn]
	,L.[ModifiedBy]
	,L.[ModifiedOn]
	From Lead L

	WHERE L.[Id] = @Id


	COMMIT;

	SELECT
	L.[Id]
	,L.[Name]
	,L.[CompanyName]
	,L.[Email]
	,L.[Phone]
	,L.[Description]
	,L.[ImageData]
	,L.[RevisionNumber]
	,L.[CreatedBy]
	,L.[CreatedOn]
	,L.[ModifiedBy]
	,L.[ModifiedOn]
	From Lead L
	WHERE L.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_LeadUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_LeadUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_LeadUpdate]    Script Date: 5/5/2014 1:15:00 PM ******/
CREATE PROC [dbo].[usp_LeadUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@CompanyName [nvarchar](128)  = NULL
	,@Email [varchar](128)  = NULL
	,@Phone [varchar](16)  = NULL
	,@Description [nvarchar](2048)
	,@ImageData [varchar](max)
	,@UserID  [uniqueidentifier]

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Lead]
	SET [Id] = @Id, [Name] = @Name, [CompanyName] = @CompanyName, [Email] = @Email, [Phone] = @Phone, [Description] = @Description, [ImageData] = @ImageData, [RevisionNumber] = [RevisionNumber] + 1, [ModifiedBy] = @UserId, [ModifiedOn] = GETDATE()	WHERE [Id] = @Id


	INSERT INTO [dbo].[Lead_log] ([Id], [Name], [CompanyName], [Email], [Phone], [Description], [ImageData], [RevisionNumber], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
	SELECT
	L.[Id]
	,L.[Name]
	,L.[CompanyName]
	,L.[Email]
	,L.[Phone]
	,L.[Description]
	,L.[ImageData]
	,L.[RevisionNumber]
	,L.[CreatedBy]
	,L.[CreatedOn]
	,L.[ModifiedBy]
	,L.[ModifiedOn]
	From Lead L

	WHERE L.[Id] = @Id


	COMMIT;

	SELECT
	L.[Id]
	,L.[Name]
	,L.[CompanyName]
	,L.[Email]
	,L.[Phone]
	,L.[Description]
	,L.[ImageData]
	,L.[RevisionNumber]
	,L.[CreatedBy]
	,L.[CreatedOn]
	,L.[ModifiedBy]
	,L.[ModifiedOn]
	From Lead L
	WHERE L.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_LeadDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_LeadDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_LeadDelete]    Script Date: 5/5/2014 1:15:00 PM ******/
CREATE PROC [dbo].[usp_LeadDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Lead] WHERE  [Id] = @Id
COMMIT;
GO

