SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[AppProfile]', N'U') IS NOT NULL
DROP TABLE [dbo].[AppProfile]
GO

/****** Object:  Table [dbo].[AppProfile]    Script Date: 1/5/2015 5:17:31 PM ******/
CREATE TABLE [dbo].[AppProfile](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](256) NULL,
	[IsActive] [bit] NULL,
CONSTRAINT [PK_AppProfile] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[AppProfileView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AppProfileView]
GO

/****** Object:  View [dbo].[AppProfileView]    Script Date: 1/5/2015 5:17:31 PM ******/
CREATE VIEW [dbo].[AppProfileView]
AS
	SELECT
	AP.[Id]
	,AP.[Name]
	,AP.[IsActive]
	From AppProfile AP

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_AppProfileInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppProfileInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppProfileInsert]    Script Date: 1/5/2015 5:17:31 PM ******/
CREATE PROC [dbo].[usp_AppProfileInsert]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@IsActive [bit]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[AppProfile] ([Id], [Name], [IsActive])
	SELECT @Id, @Name, @IsActive

	COMMIT;

	SELECT
	AP.[Id]
	,AP.[Name]
	,AP.[IsActive]
	From AppProfile AP
	WHERE AP.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppProfileUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppProfileUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppProfileUpdate]    Script Date: 1/5/2015 5:17:31 PM ******/
CREATE PROC [dbo].[usp_AppProfileUpdate]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@IsActive [bit]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[AppProfile]
	SET [Id] = @Id, [Name] = @Name, [IsActive] = @IsActive	WHERE [Id] = @Id


	COMMIT;

	SELECT
	AP.[Id]
	,AP.[Name]
	,AP.[IsActive]
	From AppProfile AP
	WHERE AP.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppProfileDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppProfileDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppProfileDelete]    Script Date: 1/5/2015 5:17:31 PM ******/
CREATE PROC [dbo].[usp_AppProfileDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[AppProfile] WHERE  [Id] = @Id
COMMIT;
GO

