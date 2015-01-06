SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[AppClient]', N'U') IS NOT NULL
DROP TABLE [dbo].[AppClient]
GO

/****** Object:  Table [dbo].[AppClient]    Script Date: 1/5/2015 5:17:42 PM ******/
CREATE TABLE [dbo].[AppClient](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](256) NULL,
	[Secret] [varchar](256) NULL,
	[ApplicationTypeValue] [char](2) NULL,
	[RefreshTokenLifeTime] [integer] NULL,
	[AllowedOrigin] [varchar](256) NULL,
	[IsActive] [bit] NULL,
CONSTRAINT [PK_AppClient] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[AppClientView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AppClientView]
GO

/****** Object:  View [dbo].[AppClientView]    Script Date: 1/5/2015 5:17:42 PM ******/
CREATE VIEW [dbo].[AppClientView]
AS
	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[Secret]
	,AC.[ApplicationTypeValue]
	,AC.[RefreshTokenLifeTime]
	,AC.[AllowedOrigin]
	,AC.[IsActive]
	From AppClient AC

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_AppClientInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppClientInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppClientInsert]    Script Date: 1/5/2015 5:17:42 PM ******/
CREATE PROC [dbo].[usp_AppClientInsert]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@Secret [varchar](256)  = NULL
	,@ApplicationTypeValue [char](2)  = NULL
	,@RefreshTokenLifeTime [integer]  = NULL
	,@AllowedOrigin [varchar](256)  = NULL
	,@IsActive [bit]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[AppClient] ([Id], [Name], [Secret], [ApplicationTypeValue], [RefreshTokenLifeTime], [AllowedOrigin], [IsActive])
	SELECT @Id, @Name, @Secret, @ApplicationTypeValue, @RefreshTokenLifeTime, @AllowedOrigin, @IsActive

	COMMIT;

	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[Secret]
	,AC.[ApplicationTypeValue]
	,AC.[RefreshTokenLifeTime]
	,AC.[AllowedOrigin]
	,AC.[IsActive]
	From AppClient AC
	WHERE AC.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppClientUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppClientUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppClientUpdate]    Script Date: 1/5/2015 5:17:42 PM ******/
CREATE PROC [dbo].[usp_AppClientUpdate]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@Secret [varchar](256)  = NULL
	,@ApplicationTypeValue [char](2)  = NULL
	,@RefreshTokenLifeTime [integer]  = NULL
	,@AllowedOrigin [varchar](256)  = NULL
	,@IsActive [bit]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[AppClient]
	SET [Id] = @Id, [Name] = @Name, [Secret] = @Secret, [ApplicationTypeValue] = @ApplicationTypeValue, [RefreshTokenLifeTime] = @RefreshTokenLifeTime, [AllowedOrigin] = @AllowedOrigin, [IsActive] = @IsActive	WHERE [Id] = @Id


	COMMIT;

	SELECT
	AC.[Id]
	,AC.[Name]
	,AC.[Secret]
	,AC.[ApplicationTypeValue]
	,AC.[RefreshTokenLifeTime]
	,AC.[AllowedOrigin]
	,AC.[IsActive]
	From AppClient AC
	WHERE AC.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppClientDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppClientDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppClientDelete]    Script Date: 1/5/2015 5:17:42 PM ******/
CREATE PROC [dbo].[usp_AppClientDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[AppClient] WHERE  [Id] = @Id
COMMIT;
GO

