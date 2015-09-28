USE [EventTrackerDB]
GO

/****** Object:  Table [dbo].[Members]    Script Date: 9/28/2015 1:07:42 AM ******/
DROP TABLE [dbo].[Members]
GO

/****** Object:  Table [dbo].[Members]    Script Date: 9/28/2015 1:07:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Members](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [char](1) NULL,
	[Phone] [varchar](20) NULL,
	[Email] [varchar](50) NULL,
	[MembershipDate] [datetime] NULL,
	[SpouseMemberId] [int] NULL,
	[ParentMemberId] [int] NULL,
	[SpouseName] [varchar](50) NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

