USE [EventTrackerDB]
GO

ALTER TABLE [dbo].[MemberMemberships] DROP CONSTRAINT [FK_MemberMemberships_Member]
GO

/****** Object:  Table [dbo].[MemberMemberships]    Script Date: 9/28/2015 1:07:33 AM ******/
DROP TABLE [dbo].[MemberMemberships]
GO

/****** Object:  Table [dbo].[MemberMemberships]    Script Date: 9/28/2015 1:07:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MemberMemberships](
	[MemberMembershipId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[MemberOf] [nchar](10) NOT NULL,
 CONSTRAINT [PK_KfcMembers] PRIMARY KEY CLUSTERED 
(
	[MemberMembershipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MemberMemberships]  WITH CHECK ADD  CONSTRAINT [FK_MemberMemberships_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([MemberId])
GO

ALTER TABLE [dbo].[MemberMemberships] CHECK CONSTRAINT [FK_MemberMemberships_Member]
GO

