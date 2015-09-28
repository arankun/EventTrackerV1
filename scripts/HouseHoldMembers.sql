USE [EventTrackerDB]
GO

ALTER TABLE [dbo].[HouseHoldMembers] DROP CONSTRAINT [FK_HouseHoldMembers_Member]
GO

ALTER TABLE [dbo].[HouseHoldMembers] DROP CONSTRAINT [FK_HouseHoldMembers_HouseHold]
GO

/****** Object:  Table [dbo].[HouseHoldMembers]    Script Date: 9/28/2015 1:07:16 AM ******/
DROP TABLE [dbo].[HouseHoldMembers]
GO

/****** Object:  Table [dbo].[HouseHoldMembers]    Script Date: 9/28/2015 1:07:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HouseHoldMembers](
	[HouseHoldMemberId] [int] IDENTITY(1,1) NOT NULL,
	[HouseHoldId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_HouseHoldMembers] PRIMARY KEY CLUSTERED 
(
	[HouseHoldMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[HouseHoldMembers]  WITH CHECK ADD  CONSTRAINT [FK_HouseHoldMembers_HouseHold] FOREIGN KEY([HouseHoldId])
REFERENCES [dbo].[HouseHolds] ([HouseHoldId])
GO

ALTER TABLE [dbo].[HouseHoldMembers] CHECK CONSTRAINT [FK_HouseHoldMembers_HouseHold]
GO

ALTER TABLE [dbo].[HouseHoldMembers]  WITH CHECK ADD  CONSTRAINT [FK_HouseHoldMembers_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([MemberId])
GO

ALTER TABLE [dbo].[HouseHoldMembers] CHECK CONSTRAINT [FK_HouseHoldMembers_Member]
GO

