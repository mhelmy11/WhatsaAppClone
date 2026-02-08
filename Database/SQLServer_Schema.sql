-- ============================================
-- WhatsApp Clone - SQL Server Schema
-- Relational Core: Users, Contacts, Groups, Auth
-- ============================================

-- Users
CREATE TABLE Users (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Phone NVARCHAR(32) NULL,
    FullName NVARCHAR(200) NOT NULL,
    About NVARCHAR(500) NULL DEFAULT 'Hey there! I''m using WhatsappClone!',
    PicUrl NVARCHAR(500) NULL,
    LastSeen DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- User Contacts (many-to-many)
CREATE TABLE UserContacts (
    UserId NVARCHAR(450) NOT NULL,
    ContactId NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_UserContacts PRIMARY KEY (UserId, ContactId),
    CONSTRAINT FK_UserContacts_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserContacts_Contact FOREIGN KEY (ContactId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- Groups
CREATE TABLE Groups (
    GroupId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    PictureUrl NVARCHAR(500) NULL,
    CreatorId NVARCHAR(450) NOT NULL,
    InviteCode NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CanAddMembers BIT NOT NULL DEFAULT 1,
    EditGroupSettings BIT NOT NULL DEFAULT 1,
    AllowSendMessages BIT NOT NULL DEFAULT 1,
    ApproveMembers BIT NOT NULL DEFAULT 0,
    MembersCount BIGINT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Groups_Creator FOREIGN KEY (CreatorId) REFERENCES Users(Id)
);

-- Group Members
CREATE TABLE UserGroups (
    GroupId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Role NVARCHAR(32) NULL DEFAULT 'member',
    JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsApproved BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_UserGroups PRIMARY KEY (GroupId, UserId),
    CONSTRAINT FK_UserGroups_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId) ON DELETE CASCADE,
    CONSTRAINT FK_UserGroups_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- Blacklists
CREATE TABLE Blacklists (
    UserId NVARCHAR(450) NOT NULL,
    BlockedUserId NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Blacklists PRIMARY KEY (UserId, BlockedUserId),
    CONSTRAINT FK_Blacklists_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Blacklists_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- Refresh Tokens
CREATE TABLE RefreshTokens (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(450) NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RevokedAt DATETIME2 NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_RefreshTokens_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- User Connections (SignalR - optional if using Redis)
CREATE TABLE UserConnections (
    UserId NVARCHAR(450) NOT NULL,
    ConnectionId NVARCHAR(200) NOT NULL,
    ConnectedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_UserConnections PRIMARY KEY (UserId, ConnectionId),
    CONSTRAINT FK_UserConnections_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Chats (direct + group metadata)
CREATE TABLE Chats (
    ChatId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    ChatType NVARCHAR(16) NOT NULL, -- direct|group
    GroupId UNIQUEIDENTIFIER NULL,
    UserAId NVARCHAR(450) NULL,
    UserBId NVARCHAR(450) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PinnedMessageId NVARCHAR(24) NULL, -- Mongo ObjectId string
    PinnedAt DATETIME2 NULL,
    PinnedByUserId NVARCHAR(450) NULL,
    LastMessageId NVARCHAR(24) NULL, -- Mongo ObjectId string
    CONSTRAINT FK_Chats_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId) ON DELETE NO ACTION,
    CONSTRAINT FK_Chats_UserA FOREIGN KEY (UserAId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Chats_UserB FOREIGN KEY (UserBId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Chats_PinnedBy FOREIGN KEY (PinnedByUserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- User Chat Settings
CREATE TABLE UserChatSettings (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(450) NOT NULL,
    ContactId NVARCHAR(450) NULL,
    GroupId UNIQUEIDENTIFIER NULL,
    IsMuted BIT NOT NULL DEFAULT 0,
    IsPinned BIT NOT NULL DEFAULT 0,
    PinnedAt DATETIME2 NULL,
    CONSTRAINT FK_UserChatSettings_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserChatSettings_Contact FOREIGN KEY (ContactId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserChatSettings_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId) ON DELETE NO ACTION
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
    CONSTRAINT FK_Communities_Creator FOREIGN KEY (CreatorId) REFERENCES Users(Id)
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
    CONSTRAINT FK_CommunityChannels_Creator FOREIGN KEY (CreatorId) REFERENCES Users(Id)
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
    CONSTRAINT FK_UserCommunities_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- ============================================
-- Indexes for Performance
-- ============================================

CREATE INDEX IX_Users_Phone ON Users(Phone);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);
CREATE INDEX IX_UserContacts_UserId ON UserContacts(UserId);
CREATE INDEX IX_UserContacts_ContactId ON UserContacts(ContactId);
CREATE INDEX IX_Groups_CreatorId ON Groups(CreatorId);
CREATE INDEX IX_Groups_InviteCode ON Groups(InviteCode);
CREATE INDEX IX_UserGroups_UserId ON UserGroups(UserId);
CREATE INDEX IX_UserGroups_GroupId ON UserGroups(GroupId);
CREATE INDEX IX_Blacklists_UserId ON Blacklists(UserId);
CREATE INDEX IX_Blacklists_BlockedUserId ON Blacklists(BlockedUserId);
CREATE INDEX IX_RefreshTokens_UserId ON RefreshTokens(UserId);
CREATE INDEX IX_RefreshTokens_Token ON RefreshTokens(Token);
CREATE INDEX IX_UserChatSettings_UserId ON UserChatSettings(UserId);
CREATE INDEX IX_Chats_GroupId ON Chats(GroupId);
CREATE INDEX IX_Chats_UserAId ON Chats(UserAId);
CREATE INDEX IX_Chats_UserBId ON Chats(UserBId);
CREATE INDEX IX_Communities_CreatorId ON Communities(CreatorId);
CREATE INDEX IX_Communities_IsActive ON Communities(IsActive);
CREATE INDEX IX_CommunityChannels_CommunityId ON CommunityChannels(CommunityId);
CREATE INDEX IX_CommunityChannels_CreatorId ON CommunityChannels(CreatorId);
CREATE INDEX IX_UserCommunities_UserId ON UserCommunities(UserId);
CREATE INDEX IX_UserCommunities_CommunityId ON UserCommunities(CommunityId);
