SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[AppUser]', N'U') IS NOT NULL
DROP TABLE [dbo].[AppUser]
GO

/****** Object:  Table [dbo].[AppUser]    Script Date: 12/6/2014 11:48:16 AM ******/
CREATE TABLE [dbo].[AppUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PasswordHash] [varchar](256) NULL,
	[PasswordSalt] [varchar](256) NULL,
	[FailedAttemptCount] [integer] NULL,
	[ProfileId] [uniqueidentifier] NULL,
	[UserStatusValue] [char](2) NULL,
	[PasswordRetrievalToken] [varchar](256) NULL,
	[LastLoginTime] [datetime] NULL,
	[PasswordRetrievalTokenExpirationDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[AppUserView]', 'V') IS NOT NULL
DROP VIEW [dbo].[AppUserView]
GO

/****** Object:  View [dbo].[AppUserView]    Script Date: 12/6/2014 11:48:16 AM ******/
CREATE VIEW [dbo].[AppUserView]
AS
	SELECT
	AU.[Id]
	,C.[Name]
	,C.[Mobile]
	,C.[Email]
	,AU.[PasswordHash]
	,AU.[PasswordSalt]
	,AU.[FailedAttemptCount]
	,AU.[ProfileId]
	,AP.[Name] as ProfileName
	,AU.[UserStatusValue]
	,AU.[PasswordRetrievalToken]
	,AU.[LastLoginTime]
	,AU.[PasswordRetrievalTokenExpirationDate]
	,AU.[LastPasswordChangedDate]
	,C.[Description]
	,C.[ImageData]
	,C.[RevisionNumber]
	,C.[CreatedBy]
	,CC.[Name] AS CreatedByName
	,C.[CreatedOn]
	,C.[ModifiedBy]
	,MC.[Name] AS ModifiedByName
	,C.[ModifiedOn]
	From AppUser AU
	LEFT JOIN Contact C ON C.[Id] = AU.[Id]
	LEFT JOIN Contact CC ON C.[CreatedBy] = CC.[Id]
	LEFT JOIN Contact MC ON C.[ModifiedBy] = MC.[Id]
	LEFT JOIN AppProfile AP ON AP.[Id] = AU.[ProfileId]

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_AppUserInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppUserInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppUserInsert]    Script Date: 12/6/2014 11:48:16 AM ******/
CREATE PROC [dbo].[usp_AppUserInsert]
	@Id  [uniqueidentifier]
	,@PasswordHash [varchar](256)  = NULL
	,@PasswordSalt [varchar](256)  = NULL
	,@FailedAttemptCount [integer]  = NULL
	,@ProfileId [uniqueidentifier]  = NULL
	,@UserStatusValue [char](2)  = NULL
	,@PasswordRetrievalToken [varchar](256)  = NULL
	,@LastLoginTime [datetime]  = NULL
	,@PasswordRetrievalTokenExpirationDate [datetime]  = NULL
	,@LastPasswordChangedDate [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[AppUser] ([Id], [PasswordHash], [PasswordSalt], [FailedAttemptCount], [ProfileId], [UserStatusValue], [PasswordRetrievalToken], [LastLoginTime], [PasswordRetrievalTokenExpirationDate], [LastPasswordChangedDate])
	SELECT @Id, @PasswordHash, @PasswordSalt, @FailedAttemptCount, @ProfileId, @UserStatusValue, @PasswordRetrievalToken, @LastLoginTime, @PasswordRetrievalTokenExpirationDate, @LastPasswordChangedDate

	COMMIT;

	SELECT
	AU.[Id]
	,AU.[PasswordHash]
	,AU.[PasswordSalt]
	,AU.[FailedAttemptCount]
	,AU.[ProfileId]
	,AU.[UserStatusValue]
	,AU.[PasswordRetrievalToken]
	,AU.[LastLoginTime]
	,AU.[PasswordRetrievalTokenExpirationDate]
	,AU.[LastPasswordChangedDate]
	From AppUser AU
	WHERE AU.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppUserUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppUserUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppUserUpdate]    Script Date: 12/6/2014 11:48:16 AM ******/
CREATE PROC [dbo].[usp_AppUserUpdate]
	@Id  [uniqueidentifier]
	,@PasswordHash [varchar](256)  = NULL
	,@PasswordSalt [varchar](256)  = NULL
	,@FailedAttemptCount [integer]  = NULL
	,@ProfileId [uniqueidentifier]  = NULL
	,@UserStatusValue [char](2)  = NULL
	,@PasswordRetrievalToken [varchar](256)  = NULL
	,@LastLoginTime [datetime]  = NULL
	,@PasswordRetrievalTokenExpirationDate [datetime]  = NULL
	,@LastPasswordChangedDate [datetime]  = NULL

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[AppUser]
	SET [Id] = @Id, [PasswordHash] = @PasswordHash, [PasswordSalt] = @PasswordSalt, [FailedAttemptCount] = @FailedAttemptCount, [ProfileId] = @ProfileId, [UserStatusValue] = @UserStatusValue, [PasswordRetrievalToken] = @PasswordRetrievalToken, [LastLoginTime] = @LastLoginTime, [PasswordRetrievalTokenExpirationDate] = @PasswordRetrievalTokenExpirationDate, [LastPasswordChangedDate] = @LastPasswordChangedDate	WHERE [Id] = @Id


	COMMIT;

	SELECT
	AU.[Id]
	,AU.[PasswordHash]
	,AU.[PasswordSalt]
	,AU.[FailedAttemptCount]
	,AU.[ProfileId]
	,AU.[UserStatusValue]
	,AU.[PasswordRetrievalToken]
	,AU.[LastLoginTime]
	,AU.[PasswordRetrievalTokenExpirationDate]
	,AU.[LastPasswordChangedDate]
	From AppUser AU
	WHERE AU.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_AppUserDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_AppUserDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_AppUserDelete]    Script Date: 12/6/2014 11:48:16 AM ******/
CREATE PROC [dbo].[usp_AppUserDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[AppUser] WHERE  [Id] = @Id
COMMIT;
GO

