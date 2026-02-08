-- Users
CREATE TABLE Users (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Phone NVARCHAR(32) NULL,
    FullName NVARCHAR(200) NOT NULL,
    About NVARCHAR(500) NULL,
    PicUrl NVARCHAR(500) NULL,
    LastSeen DATETIME2 NOT NULL,
    IsActive BIT NOT NULL DEFAULT(0),
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE())
);

-- Contacts
CREATE TABLE UserContacts (
    UserId NVARCHAR(450) NOT NULL,
    ContactId NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT PK_UserContacts PRIMARY KEY (UserId, ContactId),
    CONSTRAINT FK_UserContacts_User FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserContacts_Contact FOREIGN KEY (ContactId) REFERENCES Users(Id)
);

-- Groups
CREATE TABLE Groups (
    GroupId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    PictureUrl NVARCHAR(500) NULL,
    CreatorId NVARCHAR(450) NOT NULL,
    InviteCode NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    CanAddMembers BIT NOT NULL DEFAULT(1),
    EditGroupSettings BIT NOT NULL DEFAULT(1),
    AllowSendMessages BIT NOT NULL DEFAULT(1),
    ApproveMembers BIT NOT NULL DEFAULT(0),
    MembersCount BIGINT NOT NULL DEFAULT(0),
    CONSTRAINT FK_Groups_Creator FOREIGN KEY (CreatorId) REFERENCES Users(Id)
);

CREATE TABLE UserGroups (
    GroupId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Role NVARCHAR(32) NULL,
    JoinedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsApproved BIT NOT NULL DEFAULT(1),
    CONSTRAINT PK_UserGroups PRIMARY KEY (GroupId, UserId),
    CONSTRAINT FK_UserGroups_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),
    CONSTRAINT FK_UserGroups_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Messages (direct or group)
CREATE TABLE Messages (
    MessageId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SenderId NVARCHAR(450) NOT NULL,
    ReceiverId NVARCHAR(450) NULL, -- direct only
    GroupId UNIQUEIDENTIFIER NULL, -- group only
    Content NVARCHAR(MAX) NULL,
    MessageType NVARCHAR(32) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsEdited BIT NOT NULL DEFAULT(0),
    EditedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    ReplyToMessageId UNIQUEIDENTIFIER NULL,
    CONSTRAINT FK_Messages_Sender FOREIGN KEY (SenderId) REFERENCES Users(Id),
    CONSTRAINT FK_Messages_Receiver FOREIGN KEY (ReceiverId) REFERENCES Users(Id),
    CONSTRAINT FK_Messages_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),
    CONSTRAINT FK_Messages_Reply FOREIGN KEY (ReplyToMessageId) REFERENCES Messages(MessageId)
);

-- Read/Delivery receipts
CREATE TABLE MessageReadStatus (
    MessageId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Status NVARCHAR(16) NOT NULL, -- sent|delivered|read
    At DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT PK_MessageReadStatus PRIMARY KEY (MessageId, UserId),
    CONSTRAINT FK_MRS_Message FOREIGN KEY (MessageId) REFERENCES Messages(MessageId),
    CONSTRAINT FK_MRS_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Attachments
CREATE TABLE Attachments (
    AttachmentId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    MessageId UNIQUEIDENTIFIER NOT NULL,
    Url NVARCHAR(1000) NOT NULL,
    Type NVARCHAR(32) NOT NULL,
    Name NVARCHAR(255) NULL,
    Size BIGINT NULL,
    Mime NVARCHAR(128) NULL,
    Width INT NULL,
    Height INT NULL,
    Duration INT NULL,
    CONSTRAINT FK_Attachments_Message FOREIGN KEY (MessageId) REFERENCES Messages(MessageId)
);

-- Reactions
CREATE TABLE MessageReactions (
    MessageId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Emoji NVARCHAR(32) NOT NULL,
    At DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT PK_MessageReactions PRIMARY KEY (MessageId, UserId, Emoji),
    CONSTRAINT FK_Reactions_Message FOREIGN KEY (MessageId) REFERENCES Messages(MessageId),
    CONSTRAINT FK_Reactions_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Pinned
CREATE TABLE PinnedMessages (
    MessageId UNIQUEIDENTIFIER NOT NULL,
    GroupId UNIQUEIDENTIFIER NULL,
    PinnedBy NVARCHAR(450) NOT NULL,
    PinnedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsActive BIT NOT NULL DEFAULT(1),
    CONSTRAINT PK_PinnedMessages PRIMARY KEY (MessageId, PinnedBy),
    CONSTRAINT FK_Pinned_Message FOREIGN KEY (MessageId) REFERENCES Messages(MessageId),
    CONSTRAINT FK_Pinned_Group FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),
    CONSTRAINT FK_Pinned_User FOREIGN KEY (PinnedBy) REFERENCES Users(Id)
);

-- Starred
CREATE TABLE StarredMessages (
    MessageId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    StarredAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsActive BIT NOT NULL DEFAULT(1),
    CONSTRAINT PK_StarredMessages PRIMARY KEY (MessageId, UserId),
    CONSTRAINT FK_Starred_Message FOREIGN KEY (MessageId) REFERENCES Messages(MessageId),
    CONSTRAINT FK_Starred_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Indexes
CREATE INDEX IX_Messages_Sender_CreatedAt ON Messages(SenderId, CreatedAt DESC);
CREATE INDEX IX_Messages_Receiver_CreatedAt ON Messages(ReceiverId, CreatedAt DESC);
CREATE INDEX IX_Messages_Group_CreatedAt ON Messages(GroupId, CreatedAt DESC);
CREATE INDEX IX_MRS_User_At ON MessageReadStatus(UserId, At DESC);
CREATE INDEX IX_Attachments_Message ON Attachments(MessageId);
CREATE INDEX IX_Reactions_Message ON MessageReactions(MessageId);
CREATE INDEX IX_UserContacts_UserId ON UserContacts(UserId);
CREATE INDEX IX_UserContacts_ContactId ON UserContacts(ContactId);
CREATE INDEX IX_UserGroups_UserId ON UserGroups(UserId);