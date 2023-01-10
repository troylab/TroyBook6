/*
DROP TABLE IdentityUserClaims
DROP TABLE IdentityUserLogins
DROP TABLE IdentityUserTokens
DROP TABLE IdentityUsers
DROP TABLE IdentityUserRoles
DROP TABLE IdentityRoleClaims
DROP TABLE IdentityRoles
DROP TABLE AccountIdentityUser

SELECT * FROM IdentityUsers
SELECT * FROM IdentityUserClaims
SELECT * FROM IdentityUserLogins
SELECT * FROM IdentityUserTokens
SELECT * FROM IdentityRoles
SELECT * FROM IdentityUserRoles
SELECT * FROM IdentityRoleClaims
SELECT * FROM AccountIdentityUser
*/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------------------- IdentityUsers -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityUsers](
	[Id] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdentityUsers] ADD  CONSTRAINT [PK_IdentityUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[IdentityUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[IdentityUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

----------------------------------------------------------------- IdentityUserClaims -----------------------------------------------------------------

CREATE TABLE [dbo].[IdentityUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [UNIQUEIDENTIFIER](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserClaims] ADD  CONSTRAINT [PK_IdentityUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_IdentityUserClaims_UserId] ON [dbo].[IdentityUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserClaims_IdentityUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityUserClaims] CHECK CONSTRAINT [FK_IdentityUserClaims_IdentityUsers_UserId]
GO

----------------------------------------------------------------- IdentityUserLogins -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [UNIQUEIDENTIFIER] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdentityUserLogins] ADD  CONSTRAINT [PK_IdentityUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_IdentityUserLogins_UserId] ON [dbo].[IdentityUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserLogins_IdentityUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityUserLogins] CHECK CONSTRAINT [FK_IdentityUserLogins_IdentityUsers_UserId]
GO

----------------------------------------------------------------- IdentityUserTokens -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityUserTokens](
	[UserId] [UNIQUEIDENTIFIER] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdentityUserTokens] ADD  CONSTRAINT [PK_IdentityUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserTokens_IdentityUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityUserTokens] CHECK CONSTRAINT [FK_IdentityUserTokens_IdentityUsers_UserId]
GO

----------------------------------------------------------------- IdentityRoles -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityRoles](
	[Id] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdentityRoles] ADD  CONSTRAINT [PK_IdentityRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[IdentityRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

----------------------------------------------------------------- IdentityUserRoles -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityUserRoles](
	[UserId] [UNIQUEIDENTIFIER] NOT NULL,
	[RoleId] [UNIQUEIDENTIFIER] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdentityUserRoles] ADD  CONSTRAINT [PK_IdentityUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_IdentityUserRoles_RoleId] ON [dbo].[IdentityUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserRoles_IdentityRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[IdentityRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityUserRoles] CHECK CONSTRAINT [FK_IdentityUserRoles_IdentityRoles_RoleId]
GO
ALTER TABLE [dbo].[IdentityUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_IdentityUserRoles_IdentityUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityUserRoles] CHECK CONSTRAINT [FK_IdentityUserRoles_IdentityUsers_UserId]
GO

----------------------------------------------------------------- IdentityRoleClaims -----------------------------------------------------------------
CREATE TABLE [dbo].[IdentityRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [UNIQUEIDENTIFIER] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityRoleClaims] ADD  CONSTRAINT [PK_IdentityRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_IdentityRoleClaims_RoleId] ON [dbo].[IdentityRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_IdentityRoleClaims_IdentityRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[IdentityRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityRoleClaims] CHECK CONSTRAINT [FK_IdentityRoleClaims_IdentityRoles_RoleId]
GO

----------------------------------------------------------------- AccountIdentityUser -----------------------------------------------------------------

CREATE TABLE [dbo].[AccountIdentityUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[IdentityUserId] [UNIQUEIDENTIFIER] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountIdentityUser] ADD  CONSTRAINT [PK_AccountIdentityUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

----------------------------------------------------------------- admin@test.com -----------------------------------------------------------------

/* admin / Aa123456 */

DECLARE @adminId NVARCHAR(50) = LOWER(NEWID())
SELECT @adminId

INSERT INTO IdentityUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES(@adminId, N'admin@test.com', N'ADMIN@TEST.COM', N'admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAEAACcQAAAAEKB8m3rKvOAWV93l/MnVyTHn81feZvw2K4zivRoXYnLpBysGuLyBlNQGu26y3Ayf5w==', N'NM64BNVF7JMGTBV54KLSTH3KEINN5YF7', N'558a624c-de29-4cd4-8383-1703b9442efa', NULL, 0, 0, NULL, 1, 0);
INSERT INTO IdentityUserClaims (UserId, ClaimType, ClaimValue)
VALUES(@adminId, N'IsAdmin', N'true');
INSERT INTO IdentityUserClaims (UserId, ClaimType, ClaimValue)
VALUES(@adminId , N'CmsAccountId', N'999999999');
GO
