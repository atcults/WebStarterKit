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
	[ClientName] [varchar](256) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [varchar](256) NULL,
	[AccessTokenHash] [varchar](256) NULL,
	[AccessTicket] [varchar](1024) NULL,
	[AccessTokenIssuedUtc] [datetime] NULL,
	[AccessTokenExpiresUtc] [datetime] NULL,
	[RefreshTokenHash] [varchar](256) NULL,
	[RefreshTicket] [varchar](1024) NULL,
	[RefreshTokenIssuedUtc] [datetime] NULL,
	[RefreshTokenExpiresUtc] [datetime] NULL,
	[TimesTokenGiven] [integer] NULL	
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
	,TS.[ClientName]
	,TS.[UserId]
	,TS.[UserName]
	,TS.[AccessTokenHash]
	,TS.[AccessTicket]
	,TS.[AccessTokenIssuedUtc]
	,TS.[AccessTokenExpiresUtc]
	,TS.[RefreshTokenHash]
	,TS.[RefreshTicket]
	,TS.[RefreshTokenIssuedUtc]
	,TS.[RefreshTokenExpiresUtc]
	,TS.[TimesTokenGiven]
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
	,@ClientName [varchar](256)  = NULL
	,@UserId  [uniqueidentifier]
	,@UserName [varchar](256)  = NULL
	,@AccessTokenHash [varchar](256)  = NULL
	,@AccessTicket [varchar](1024)  = NULL
	,@AccessTokenIssuedUtc [datetime]  = NULL
	,@AccessTokenExpiresUtc [datetime]  = NULL
	,@RefreshTokenHash [varchar](256)  = NULL
	,@RefreshTicket [varchar](1024)  = NULL
	,@RefreshTokenIssuedUtc [datetime]  = NULL
	,@RefreshTokenExpiresUtc [datetime]  = NULL
	,@TimesTokenGiven [integer]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[TokenStore] ([Id], [ClientName], [UserId], [UserName], [AccessTokenHash], [AccessTicket], [AccessTokenIssuedUtc], [AccessTokenExpiresUtc], [RefreshTokenHash], [RefreshTicket], [RefreshTokenIssuedUtc], [RefreshTokenExpiresUtc], [TimesTokenGiven])

	SELECT @Id, @ClientName, @UserId, @UserName, @AccessTokenHash, @AccessTicket, @AccessTokenIssuedUtc, @AccessTokenExpiresUtc, @RefreshTokenHash, @RefreshTicket, @RefreshTokenIssuedUtc, @RefreshTokenExpiresUtc, @TimesTokenGiven
	
	COMMIT;

	SELECT
	TS.[Id]
	,TS.[ClientName]
	,TS.[UserId]
	,TS.[UserName]
	,TS.[AccessTokenHash]
	,TS.[AccessTicket]
	,TS.[AccessTokenIssuedUtc]
	,TS.[AccessTokenExpiresUtc]
	,TS.[RefreshTokenHash]
	,TS.[RefreshTicket]
	,TS.[RefreshTokenIssuedUtc]
	,TS.[RefreshTokenExpiresUtc]
	,TS.[TimesTokenGiven]
	From TokenStore TS
	WHERE TS.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_TokenStoreUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_TokenStoreUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_TokenStoreUpdate]    Script Date: 1/5/2015 5:21:22 PM ******/
CREATE PROC [dbo].[usp_TokenStoreUpdate]
	@Id  [uniqueidentifier]
	,@ClientName [varchar](256)  = NULL
	,@UserId  [uniqueidentifier]
	,@UserName [varchar](256)  = NULL
	,@AccessTokenHash [varchar](256)  = NULL
	,@AccessTicket [varchar](1024)  = NULL
	,@AccessTokenIssuedUtc [datetime]  = NULL
	,@AccessTokenExpiresUtc [datetime]  = NULL
	,@RefreshTokenHash [varchar](256)  = NULL
	,@RefreshTicket [varchar](1024)  = NULL
	,@RefreshTokenIssuedUtc [datetime]  = NULL
	,@RefreshTokenExpiresUtc [datetime]  = NULL
	,@TimesTokenGiven [integer]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[TokenStore]
	SET [Id] = @Id, [ClientName] = @ClientName, [UserId] = @UserId, [UserName] = @UserName, [AccessTokenHash] = @AccessTokenHash, [AccessTicket] = @AccessTicket, [AccessTokenIssuedUtc] = @AccessTokenIssuedUtc, [AccessTokenExpiresUtc] = @AccessTokenExpiresUtc, [RefreshTokenHash] = @RefreshTokenHash, [RefreshTicket] = @RefreshTicket, [RefreshTokenIssuedUtc] = @RefreshTokenIssuedUtc, [RefreshTokenExpiresUtc] = @RefreshTokenExpiresUtc, [TimesTokenGiven] = @TimesTokenGiven WHERE [Id] = @Id

	COMMIT;

	SELECT
	TS.[Id]
	,TS.[ClientName]
	,TS.[UserId]
	,TS.[UserName]
	,TS.[AccessTokenHash]
	,TS.[AccessTicket]
	,TS.[AccessTokenIssuedUtc]
	,TS.[AccessTokenExpiresUtc]
	,TS.[RefreshTokenHash]
	,TS.[RefreshTicket]
	,TS.[RefreshTokenIssuedUtc]
	,TS.[RefreshTokenExpiresUtc]
	,TS.[TimesTokenGiven]
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