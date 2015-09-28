USE [EventTrackerDB]
GO

ALTER TABLE [dbo].[EventAttendances] DROP CONSTRAINT [FK_EventAttendances_LogBy]
GO

ALTER TABLE [dbo].[EventAttendances] DROP CONSTRAINT [FK_EventAttendance_Member]
GO

ALTER TABLE [dbo].[EventAttendances] DROP CONSTRAINT [FK_EventAttendance_Event]
GO

/****** Object:  Table [dbo].[EventAttendances]    Script Date: 9/28/2015 1:06:54 AM ******/
DROP TABLE [dbo].[EventAttendances]
GO

/****** Object:  Table [dbo].[EventAttendances]    Script Date: 9/28/2015 1:06:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventAttendances](
	[EventAttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
	[LogBy] [int] NULL,
	[LogDate] [datetime] NOT NULL CONSTRAINT [DF_EventAttendanceLog_LogDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_EventAttendanceLogId] PRIMARY KEY CLUSTERED 
(
	[EventAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EventAttendances]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendance_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([EventId])
GO

ALTER TABLE [dbo].[EventAttendances] CHECK CONSTRAINT [FK_EventAttendance_Event]
GO

ALTER TABLE [dbo].[EventAttendances]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendance_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([MemberId])
GO

ALTER TABLE [dbo].[EventAttendances] CHECK CONSTRAINT [FK_EventAttendance_Member]
GO

ALTER TABLE [dbo].[EventAttendances]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendances_LogBy] FOREIGN KEY([LogBy])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[EventAttendances] CHECK CONSTRAINT [FK_EventAttendances_LogBy]
GO

