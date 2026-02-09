-- ============================================
-- WhatsApp Clone - SQL Server Schema
-- Relational Core: Users, Contacts, Groups, Auth
-- Note: Uses ASP.NET Core Identity for Users (AspNetUsers table)
-- ============================================

-- IMPORTANT: Run "dotnet ef migrations add InitialCreate" and "dotnet ef database update"
-- to create the database with Identity tables and all custom columns

-- AspNetUsers (Identity + Custom Columns)
-- Note: Identity automatically creates AspNetUsers, AspNetRoles, AspNetUserRoles, etc.
-- The AppUser model extends IdentityUser with these additional custom columns:
-- - FullName, About, AboutUpdatedAt, ProfilePicUrl
-- - LastSeen, IsActive, CreatedAt, IsDeleted, AccountStatus
-- - Privacy Settings: AllowReadReceipts, WhoCanSeeMyStory, WhoCanAddMeToGroups, WhoCanSeeMyLastSeen, WhoCanViewProfilePic
-- - Preferences: PreferredLanguage, ThemePreference, MuteAllNotifications, MuteCallNotifications

-- User Contacts (many-to-many)
CREATE TABLE UserContacts (
    UserId NVARCHAR(450) NOT NULL,
    ContactId NVARCHAR(450) NOT NULL,
    DisplayName NVARCHAR(200) NULL,
    AddedOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT PK_UserContacts PRIMARY KEY (UserId, ContactId),
    CONSTRAINT FK_UserContacts_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserContacts_Contact FOREIGN KEY (ContactId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION
);

-- Groups
CREATE TABLE Groups (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    IconUrl NVARCHAR(500) NULL,
    CreatorId NVARCHAR(450) NOT NULL,
    InviteCode NVARCHAR(100) NULL,
    CanAddMembers BIT NOT NULL DEFAULT 1,
    CanEditGroupSettings BIT NOT NULL DEFAULT 1,
    CanSendMessages BIT NOT NULL DEFAULT 1,
    RequireApprovalToJoin BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Groups_Creator FOREIGN KEY (CreatorId) REFERENCES AspNetUsers(Id)
);

-- Group Members
CREATE TABLE UserGroups (
    GroupId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Member',
    IsApproved BIT NOT NULL DEFAULT 1,
    IsMember BIT NOT NULL DEFAULT 1,
    JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LeftAt DATETIME2 NULL,
    CONSTRAINT PK_UserGroups PRIMARY KEY (GroupId, UserId),
    CONSTRAINT FK_UserGroups_Group FOREIGN KEY (GroupId) REFERENCES Groups(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserGroups_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION
);

-- Blacklists
CREATE TABLE Blacklists (
    UserId NVARCHAR(450) NOT NULL,
    BlockedUserId NVARCHAR(450) NOT NULL,
    BlockedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Blacklists PRIMARY KEY (UserId, BlockedUserId),
    CONSTRAINT FK_Blacklists_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Blacklists_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION
);

-- Refresh Tokens Audit
CREATE TABLE RefreshTokenAudits (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IpAddress NVARCHAR(45) NULL,
    UserAgent NVARCHAR(500) NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,
    RevokedAt DATETIME2 NULL,
    CONSTRAINT FK_RefreshTokenAudits_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- User Connections (SignalR)
CREATE TABLE UserConnections (
    UserId NVARCHAR(450) NOT NULL,
    ConnectionId NVARCHAR(200) NOT NULL,
    ConnectedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_UserConnections PRIMARY KEY (UserId, ConnectionId),
    CONSTRAINT FK_UserConnections_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Chats (direct + group metadata)
CREATE TABLE Chats (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    ChatType NVARCHAR(50) NOT NULL DEFAULT '', -- Direct|Group
    SenderId NVARCHAR(450) NULL,
    ReceiverId NVARCHAR(450) NULL,
    GroupId UNIQUEIDENTIFIER NULL,
    LastMessageId NVARCHAR(24) NULL, -- MongoDB ObjectId
    LastMessageTime DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Chats_Sender FOREIGN KEY (SenderId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Chats_Receiver FOREIGN KEY (ReceiverId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Chats_Group FOREIGN KEY (GroupId) REFERENCES Groups(Id) ON DELETE NO ACTION
);

-- User Chat Settings
CREATE TABLE UserChatSettings (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    ChatId UNIQUEIDENTIFIER NOT NULL,
    IsPinned BIT NOT NULL DEFAULT 0,
    PinnedAt DATETIME2 NULL,
    IsArchived BIT NOT NULL DEFAULT 0,
    ArchivedAt DATETIME2 NULL,
    IsFavorite BIT NOT NULL DEFAULT 0,
    FavoritedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsMuted BIT NOT NULL DEFAULT 0,
    MutedUntil DATETIME2 NULL,
    CONSTRAINT FK_UserChatSettings_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserChatSettings_Chat FOREIGN KEY (ChatId) REFERENCES Chats(Id) ON DELETE NO ACTION
);

-- User Privacy Exceptions ("My Contacts Except" rules)
CREATE TABLE UserPrivacyExceptions (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    ExcludedUserId NVARCHAR(450) NOT NULL,
    PrivacyType NVARCHAR(50) NOT NULL, -- Status, ProfilePic, LastSeen, About
    CONSTRAINT FK_UserPrivacyExceptions_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserPrivacyExceptions_ExcludedUser FOREIGN KEY (ExcludedUserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION
);

-- Communities (WhatsApp Communities)
CREATE TABLE Communities (
    CommunityId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    PictureUrl NVARCHAR(500) NULL,
    CreatorId NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    MembersCount BIGINT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Communities_Creator FOREIGN KEY (CreatorId) REFERENCES AspNetUsers(Id)
);

-- Community Channels (channels within a community)
CREATE TABLE CommunityChannels (
    ChannelId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    CommunityId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500) NULL,
    CreatorId NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastMessageId NVARCHAR(24) NULL, -- Mongo ObjectId string
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_CommunityChannels_Community FOREIGN KEY (CommunityId) REFERENCES Communities(CommunityId) ON DELETE CASCADE,
    CONSTRAINT FK_CommunityChannels_Creator FOREIGN KEY (CreatorId) REFERENCES AspNetUsers(Id)
);

-- Community Members
CREATE TABLE UserCommunities (
    CommunityId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Role NVARCHAR(32) NOT NULL DEFAULT 'member', -- admin|moderator|member
    JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_UserCommunities PRIMARY KEY (CommunityId, UserId),
    CONSTRAINT FK_UserCommunities_Community FOREIGN KEY (CommunityId) REFERENCES Communities(CommunityId) ON DELETE CASCADE,
    CONSTRAINT FK_UserCommunities_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE NO ACTION
);

-- ============================================
-- Indexes for Performance
-- ============================================

-- AspNetUsers (AppUser) Indexes
CREATE INDEX IX_AspNetUsers_PhoneNumber ON AspNetUsers(PhoneNumber);
CREATE INDEX IX_AspNetUsers_IsActive ON AspNetUsers(IsActive);
CREATE INDEX IX_AspNetUsers_LastSeen ON AspNetUsers(LastSeen);
CREATE INDEX IX_AspNetUsers_IsDeleted ON AspNetUsers(IsDeleted);

-- UserContacts Indexes
CREATE INDEX IX_UserContacts_UserId ON UserContacts(UserId);
CREATE INDEX IX_UserContacts_ContactId ON UserContacts(ContactId);
CREATE INDEX IX_UserContacts_IsDeleted ON UserContacts(IsDeleted);

-- Groups Indexes
CREATE INDEX IX_Groups_CreatorId ON Groups(CreatorId);
CREATE UNIQUE INDEX IX_Groups_InviteCode ON Groups(InviteCode) WHERE InviteCode IS NOT NULL;
CREATE INDEX IX_Groups_CreatedAt ON Groups(CreatedAt);

-- UserGroups Indexes
CREATE INDEX IX_UserGroups_UserId ON UserGroups(UserId);
CREATE INDEX IX_UserGroups_GroupId ON UserGroups(GroupId);
CREATE INDEX IX_UserGroups_IsMember ON UserGroups(IsMember);
CREATE INDEX IX_UserGroups_GroupId_IsMember ON UserGroups(GroupId, IsMember);
CREATE INDEX IX_UserGroups_Role ON UserGroups(Role);

-- Blacklists Indexes
CREATE INDEX IX_Blacklists_UserId ON Blacklists(UserId);
CREATE INDEX IX_Blacklists_BlockedUserId ON Blacklists(BlockedUserId);

-- RefreshTokenAudits Indexes
CREATE INDEX IX_RefreshTokenAudits_UserId ON RefreshTokenAudits(UserId);
CREATE UNIQUE INDEX IX_RefreshTokenAudits_Token ON RefreshTokenAudits(Token);
CREATE INDEX IX_RefreshTokenAudits_ExpiresAt ON RefreshTokenAudits(ExpiresAt);
CREATE INDEX IX_RefreshTokenAudits_IsRevoked ON RefreshTokenAudits(IsRevoked);

-- Chats Indexes
CREATE INDEX IX_Chats_SenderId ON Chats(SenderId);
CREATE INDEX IX_Chats_ReceiverId ON Chats(ReceiverId);
CREATE INDEX IX_Chats_GroupId ON Chats(GroupId);
CREATE INDEX IX_Chats_LastMessageTime ON Chats(LastMessageTime);
CREATE INDEX IX_Chats_IsDeleted ON Chats(IsDeleted);

-- UserChatSettings Indexes
CREATE INDEX IX_UserChatSettings_UserId ON UserChatSettings(UserId);
CREATE INDEX IX_UserChatSettings_ChatId ON UserChatSettings(ChatId);
CREATE INDEX IX_UserChatSettings_UserId_ChatId ON UserChatSettings(UserId, ChatId);
CREATE INDEX IX_UserChatSettings_IsPinned ON UserChatSettings(IsPinned);
CREATE INDEX IX_UserChatSettings_IsArchived ON UserChatSettings(IsArchived);
CREATE INDEX IX_UserChatSettings_IsFavorite ON UserChatSettings(IsFavorite);
CREATE INDEX IX_UserChatSettings_IsDeleted ON UserChatSettings(IsDeleted);

-- UserPrivacyExceptions Indexes
CREATE INDEX IX_UserPrivacyExceptions_UserId ON UserPrivacyExceptions(UserId);
CREATE INDEX IX_UserPrivacyExceptions_ExcludedUserId ON UserPrivacyExceptions(ExcludedUserId);
CREATE INDEX IX_UserPrivacyExceptions_PrivacyType ON UserPrivacyExceptions(PrivacyType);

-- Communities Indexes
CREATE INDEX IX_Communities_CreatorId ON Communities(CreatorId);
CREATE INDEX IX_Communities_IsActive ON Communities(IsActive);

-- CommunityChannels Indexes
CREATE INDEX IX_CommunityChannels_CommunityId ON CommunityChannels(CommunityId);
CREATE INDEX IX_CommunityChannels_CreatorId ON CommunityChannels(CreatorId);

-- UserCommunities Indexes
CREATE INDEX IX_UserCommunities_UserId ON UserCommunities(UserId);
CREATE INDEX IX_UserCommunities_CommunityId ON UserCommunities(CommunityId);
