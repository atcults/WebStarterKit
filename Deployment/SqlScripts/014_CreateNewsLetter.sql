SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[NewsLetter]', N'U') IS NOT NULL
DROP TABLE [dbo].[NewsLetter]
GO

/****** Object:  Table [dbo].[NewsLetter]    Script Date: 5/20/2014 5:05:22 PM ******/
CREATE TABLE [dbo].[NewsLetter](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [varchar](128) NULL,
	[IsActive] [bit] NULL,
	[InsertedDate] [datetime] NULL,
CONSTRAINT [PK_NewsLetter] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[NewsLetterView]', 'V') IS NOT NULL
DROP VIEW [dbo].[NewsLetterView]
GO

/****** Object:  View [dbo].[NewsLetterView]    Script Date: 5/20/2014 5:05:22 PM ******/
CREATE VIEW [dbo].[NewsLetterView]
AS
	SELECT
	NL.[Id]
	,NL.[Email]
	,NL.[IsActive]
	,NL.[InsertedDate]
	From NewsLetter NL

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_NewsLetterInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NewsLetterInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_NewsLetterInsert]    Script Date: 5/20/2014 5:05:22 PM ******/
CREATE PROC [dbo].[usp_NewsLetterInsert]
	@Id  [uniqueidentifier]
	,@Email [varchar](128)  = NULL
	,@IsActive [bit]  = NULL
	,@InsertedDate [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[NewsLetter] ([Id], [Email], [IsActive], [InsertedDate])
	SELECT @Id, @Email, @IsActive, @InsertedDate

	COMMIT;

	SELECT
	NL.[Id]
	,NL.[Email]
	,NL.[IsActive]
	,NL.[InsertedDate]
	From NewsLetter NL
	WHERE NL.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_NewsLetterUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NewsLetterUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_NewsLetterUpdate]    Script Date: 5/20/2014 5:05:22 PM ******/
CREATE PROC [dbo].[usp_NewsLetterUpdate]
	@Id  [uniqueidentifier]
	,@Email [varchar](128)  = NULL
	,@IsActive [bit]  = NULL
	,@InsertedDate [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[NewsLetter]
	SET [Id] = @Id, [Email] = @Email, [IsActive] = @IsActive, [InsertedDate] = @InsertedDate	WHERE [Id] = @Id


	COMMIT;

	SELECT
	NL.[Id]
	,NL.[Email]
	,NL.[IsActive]
	,NL.[InsertedDate]
	From NewsLetter NL
	WHERE NL.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_NewsLetterDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_NewsLetterDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_NewsLetterDelete]    Script Date: 5/20/2014 5:05:22 PM ******/
CREATE PROC [dbo].[usp_NewsLetterDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[NewsLetter] WHERE  [Id] = @Id
COMMIT;
GO

