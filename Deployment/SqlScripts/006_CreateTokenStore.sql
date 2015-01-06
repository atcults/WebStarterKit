SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[TokenStore]', N'U') IS NOT NULL
DROP TABLE [dbo].[TokenStore]
GO

/****** Object:  Table [dbo].[TokenStore]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE TABLE [dbo].[TokenStore](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](256) NULL,
	[ClientId] [uniqueidentifier] NULL,
	[ProtectedTicket] [varchar](256) NULL,
	[IssuedUtc] [datetime] NULL,
	[ExpiresUtc] [datetime] NULL,
CONSTRAINT [PK_TokenStore] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[TokenStoreView]', 'V') IS NOT NULL
DROP VIEW [dbo].[TokenStoreView]
GO

/****** Object:  View [dbo].[TokenStoreView]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE VIEW [dbo].[TokenStoreView]
AS
	SELECT
	TS.[Id]
	,TS.[Name]
	,TS.[ClientId]
	,TS.[ProtectedTicket]
	,TS.[IssuedUtc]
	,TS.[ExpiresUtc]
	From TokenStore TS

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_TokenStoreInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TokenStoreInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_TokenStoreInsert]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE PROC [dbo].[usp_TokenStoreInsert]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@ClientId [uniqueidentifier]  = NULL
	,@ProtectedTicket [varchar](256)  = NULL
	,@IssuedUtc [datetime]  = NULL
	,@ExpiresUtc [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[TokenStore] ([Id], [Name], [ClientId], [ProtectedTicket], [IssuedUtc], [ExpiresUtc])
	SELECT @Id,@Name, @ClientId, @ProtectedTicket, @IssuedUtc, @ExpiresUtc

	COMMIT;

	SELECT
	TS.[Id]
	,TS.[Name]
	,TS.[ClientId]
	,TS.[ProtectedTicket]
	,TS.[IssuedUtc]
	,TS.[ExpiresUtc]
	From TokenStore TS
	WHERE TS.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TokenStoreUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TokenStoreUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_TokenStoreUpdate]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE PROC [dbo].[usp_TokenStoreUpdate]
	@Id  [uniqueidentifier]
	,@Name [varchar](256)  = NULL
	,@ClientId [uniqueidentifier]  = NULL
	,@ProtectedTicket [varchar](256)  = NULL
	,@IssuedUtc [datetime]  = NULL
	,@ExpiresUtc [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[TokenStore]
	SET [Id] = @Id, [Name] = @Name, [ClientId] = @ClientId, [ProtectedTicket] = @ProtectedTicket, [IssuedUtc] = @IssuedUtc, [ExpiresUtc] = @ExpiresUtc	WHERE [Id] = @Id


	COMMIT;

	SELECT
	TS.[Id]
	,TS.[Name]
	,TS.[ClientId]
	,TS.[ProtectedTicket]
	,TS.[IssuedUtc]
	,TS.[ExpiresUtc]
	From TokenStore TS
	WHERE TS.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TokenStoreDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TokenStoreDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_TokenStoreDelete]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE PROC [dbo].[usp_TokenStoreDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[TokenStore] WHERE  [Id] = @Id
COMMIT;
GO

