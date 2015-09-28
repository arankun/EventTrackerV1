USE [EventTrackerDB]
GO

ALTER TABLE [dbo].[HouseHolds] DROP CONSTRAINT [FK_HouseHolds_LeaderMemberId]
GO

/****** Object:  Table [dbo].[HouseHolds]    Script Date: 9/28/2015 1:07:24 AM ******/
DROP TABLE [dbo].[HouseHolds]
GO

/****** Object:  Table [dbo].[HouseHolds]    Script Date: 9/28/2015 1:07:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[HouseHolds](
	[HouseHoldId] [int] IDENTITY(1,1) NOT NULL,
	[HouseHoldLeaderMemberId] [int] NOT NULL,
	[Area] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_HouseHolds] PRIMARY KEY CLUSTERED 
(
	[HouseHoldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[HouseHolds]  WITH CHECK ADD  CONSTRAINT [FK_HouseHolds_LeaderMemberId] FOREIGN KEY([HouseHoldLeaderMemberId])
REFERENCES [dbo].[Members] ([MemberId])
GO

ALTER TABLE [dbo].[HouseHolds] CHECK CONSTRAINT [FK_HouseHolds_LeaderMemberId]
GO

