USE [EventTrackerDB]
GO

ALTER TABLE [dbo].[UserEvents] DROP CONSTRAINT [FK_UserEvents_User]
GO

/****** Object:  Table [dbo].[UserEvents]    Script Date: 9/28/2015 1:08:02 AM ******/
DROP TABLE [dbo].[UserEvents]
GO

/****** Object:  Table [dbo].[UserEvents]    Script Date: 9/28/2015 1:08:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserEvents](
	[UserEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[EventCode] [nvarchar](15) NOT NULL,
	[EventStatus] [nvarchar](10) NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[EventMessage] [varchar](50) NULL,
 CONSTRAINT [PK_UserEvents] PRIMARY KEY CLUSTERED 
(
	[UserEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserEvents]  WITH CHECK ADD  CONSTRAINT [FK_UserEvents_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[UserEvents] CHECK CONSTRAINT [FK_UserEvents_User]
GO

