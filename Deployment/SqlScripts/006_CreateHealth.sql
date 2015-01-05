SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('dbo.[Health]', N'U') IS NOT NULL
DROP TABLE [dbo].[Health]
GO

/****** Object:  Table [dbo].[Health]    Script Date: 4/29/2014 2:30:45 PM ******/
CREATE TABLE [dbo].[Health](
	[Id] [uniqueidentifier] NOT NULL,
	[HealthTypeValue] [char](2) NOT NULL,
	[Value] [nvarchar](2048) NULL,
	[RecordTime] [datetime] NOT NULL,
CONSTRAINT [PK_Health] PRIMARY KEY CLUSTERED 
	([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


IF OBJECT_ID('dbo.[HealthView]', 'V') IS NOT NULL
DROP VIEW [dbo].[HealthView]
GO

/****** Object:  View [dbo].[HealthView]    Script Date: 4/29/2014 2:30:45 PM ******/
CREATE VIEW [dbo].[HealthView]
AS
	SELECT
	H.[Id]
	,H.[HealthTypeValue]
	,H.[Value]
	,H.[RecordTime]
	From Health H

GO
SET ANSI_PADDING OFF 
GO


IF OBJECT_ID('dbo.[usp_HealthInsert]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_HealthInsert]
GO

/****** Object:  StoredProcedure [dbo].[usp_HealthInsert]    Script Date: 4/29/2014 2:30:45 PM ******/
CREATE PROC [dbo].[usp_HealthInsert]
	@Id  [uniqueidentifier]
	,@HealthTypeValue [char](2) 
	,@Value [nvarchar](2048)  = NULL
	,@RecordTime [datetime] 

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [dbo].[Health] ([Id], [HealthTypeValue], [Value], [RecordTime])
	SELECT @Id, @HealthTypeValue, @Value, @RecordTime

	COMMIT;

	SELECT
	H.[Id]
	,H.[HealthTypeValue]
	,H.[Value]
	,H.[RecordTime]
	From Health H
	WHERE H.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_HealthUpdate]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_HealthUpdate]
GO

/****** Object:  StoredProcedure [dbo].[usp_HealthUpdate]    Script Date: 4/29/2014 2:30:45 PM ******/
CREATE PROC [dbo].[usp_HealthUpdate]
	@Id  [uniqueidentifier]
	,@HealthTypeValue [char](2) 
	,@Value [nvarchar](2048)  = NULL
	,@RecordTime [datetime] 

AS
	SET NOCOUNT ON 
	SET XACT_ABORT ON

	BEGIN TRAN

	UPDATE [dbo].[Health]
	SET [Id] = @Id, [HealthTypeValue] = @HealthTypeValue, [Value] = @Value, [RecordTime] = @RecordTime	WHERE [Id] = @Id


	COMMIT;

	SELECT
	H.[Id]
	,H.[HealthTypeValue]
	,H.[Value]
	,H.[RecordTime]
	From Health H
	WHERE H.[Id] = @Id

GO


IF OBJECT_ID('dbo.[usp_HealthDelete]', N'P') IS NOT NULL
DROP PROCEDURE[dbo].[usp_HealthDelete]
GO

/****** Object:  StoredProcedure [dbo].[usp_HealthDelete]    Script Date: 4/29/2014 2:30:45 PM ******/
CREATE PROC [dbo].[usp_HealthDelete]
	@Id uniqueidentifier
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRAN
	DELETE FROM [dbo].[Health] WHERE  [Id] = @Id
COMMIT;
GO

