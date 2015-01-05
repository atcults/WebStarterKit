SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Template]', N'U') IS NOT NULL
DROP TABLE [dbo].[Template]
GO

/****** Object:  Table [dbo].[Template]    Script Date: 4/29/2014 2:18:08 PM ******/
CREATE TABLE [dbo].[Template](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[MailBody] [nvarchar](4000) NOT NULL,
	[SmsBody] [nvarchar](250) NOT NULL,
CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[TemplateView]', 'V') IS NOT NULL
DROP VIEW [dbo].[TemplateView]
GO

/****** Object:  View [dbo].[TemplateView]    Script Date: 4/29/2014 2:18:08 PM ******/
CREATE VIEW [dbo].[TemplateView]
AS
	SELECT
	T.[Id]
	,T.[Name]
	,T.[MailBody]
	,T.[SmsBody]
	From Template T

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_TemplateInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TemplateInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_TemplateInsert]    Script Date: 4/29/2014 2:18:08 PM ******/
CREATE PROC [dbo].[usp_TemplateInsert]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@MailBody [nvarchar](4000)
	,@SmsBody [nvarchar](250)

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Template] ([Id], [Name], [MailBody],[SmsBody])
	SELECT @Id, @Name, @MailBody, @SmsBody

	COMMIT;

	SELECT
	T.[Id]
	,T.[Name]
	,T.[MailBody]
	,T.[SmsBody]
	From Template T
	WHERE T.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TemplateUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TemplateUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_TemplateUpdate]    Script Date: 4/29/2014 2:18:08 PM ******/
CREATE PROC [dbo].[usp_TemplateUpdate]
	@Id  [uniqueidentifier]
	,@Name [nvarchar](256)
	,@MailBody [nvarchar](4000) 
	,@SmsBody [nvarchar](250)

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Template]
	SET [Id] = @Id, [Name] = @Name, [MailBody] = @MailBody, [SmsBody] = @SmsBody WHERE [Id] = @Id


	COMMIT;

	SELECT
	T.[Id]
	,T.[Name]
	,T.[MailBody]
	,T.[SmsBody]
	From Template T
	WHERE T.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TemplateDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TemplateDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_TemplateDelete]    Script Date: 4/29/2014 2:18:08 PM ******/
CREATE PROC [dbo].[usp_TemplateDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Template] WHERE  [Id] = @Id
COMMIT;
GO

