CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MatchDays](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[When] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MatchDays] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MatchDayParticipants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MatchDayId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_MatchDayParticipants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MatchDayParticipants]  WITH CHECK ADD  CONSTRAINT [FK_MatchDayParticipants_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO

ALTER TABLE [dbo].[MatchDayParticipants] CHECK CONSTRAINT [FK_MatchDayParticipants_Accounts]
GO

ALTER TABLE [dbo].[MatchDayParticipants]  WITH CHECK ADD  CONSTRAINT [FK_MatchDayParticipants_MatchDays] FOREIGN KEY([MatchDayId])
REFERENCES [dbo].[MatchDays] ([Id])
GO

ALTER TABLE [dbo].[MatchDayParticipants] CHECK CONSTRAINT [FK_MatchDayParticipants_MatchDays]
GO

CREATE TABLE [dbo].[Matches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[MatchDayId] [int] NOT NULL,
	[CreatedByParticipantId] [int] NOT NULL,
	[Participant1Id] [int] NOT NULL,
	[Participant2Id] [int] NOT NULL,
	[Participant1Points] [int] NOT NULL,
	[Participant2Points] [int] NOT NULL,
	[StartTime] [datetime2](7) NULL,
	[EndTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Matches] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Matches] ADD  CONSTRAINT [DF_Matches_Participant1Points]  DEFAULT ((0)) FOR [Participant1Points]
GO

ALTER TABLE [dbo].[Matches] ADD  CONSTRAINT [DF_Matches_Participant2Points]  DEFAULT ((0)) FOR [Participant2Points]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_MatchDayParticipants_CreatedBy] FOREIGN KEY([CreatedByParticipantId])
REFERENCES [dbo].[MatchDayParticipants] ([Id])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_MatchDayParticipants_CreatedBy]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_MatchDayParticipants_Participant1] FOREIGN KEY([Participant1Id])
REFERENCES [dbo].[MatchDayParticipants] ([Id])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_MatchDayParticipants_Participant1]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_MatchDayParticipants_Participant2] FOREIGN KEY([Participant2Id])
REFERENCES [dbo].[MatchDayParticipants] ([Id])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_MatchDayParticipants_Participant2]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_MatchDays] FOREIGN KEY([MatchDayId])
REFERENCES [dbo].[MatchDays] ([Id])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_MatchDays]