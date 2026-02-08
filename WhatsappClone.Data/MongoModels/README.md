# MongoDB Data Models

This folder contains all MongoDB-specific data models for the WhatsApp Clone application.

## Purpose

These models are designed to work with MongoDB.Driver for .NET and use attributes from the `MongoDB.Bson` namespace for proper serialization.

## Models Overview

### Core Message Models

- **MongoMessage** - Main messages collection (direct + group) with embedded reactions, read status, and draft support (isDraft flag)

### Message Features

- **MongoMessageThread** - Message thread roots for reply chains
- **MongoMessageDeletion** - DEPRECATED: Deletion metadata embedded in MongoMessage
- **MongoMessageMention** - User mentions in messages (@username)

### Advanced Features

- **MongoScheduledMessage** - Messages scheduled for future sending
- **MongoMessageSearch** - Optimized for full-text search
- **MongoLinkPreview** - DEPRECATED: Now embedded in MongoMessage.LinkPreview
- **MongoDraft** - DEPRECATED: Draft messages now stored in messages collection with isDraft=true
- **MongoMessageMediaIndex** - Quick media lookup by type
- **MongoStarredMessage** - User-starred messages
- **MongoStatus** - User status/stories (24-hour expiry)

## Usage

### Requirements

Install the MongoDB.Driver NuGet package:

```bash
dotnet add package MongoDB.Driver
```

### Connection Setup

```csharp
var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("whatsapp_clone");
var messagesCollection = database.GetCollection<MongoMessage>("messages");
```

### Example Operations

```csharp
// Insert a message with reactions and read status
var message = new MongoMessage
{
    ChatType = "direct",
    ChatId = Guid.NewGuid(),
    SenderId = "user123",
    ReceiverId = "user456",
    Content = "Hello!",
    MessageType = "text",
    Reactions = new List<MessageReaction>
    {
        new MessageReaction { UserId = "user456", Emoji = "👍" }
    },
    ReadStatus = new List<MessageReadStatus>
    {
        new MessageReadStatus { UserId = "user456", Status = "delivered" }
    }
};
await messagesCollection.InsertOneAsync(message);

// Query messages by chat
var messages = await messagesCollection
    .Find(m => m.ChatId == chatId)
    .SortByDescending(m => m.CreatedAt)
    .Limit(50)
    .ToListAsync();

// Update reaction (add emoji)
var filter = Builders<MongoMessage>.Filter.Eq(m => m.MessageId, messageId);
var update = Builders<MongoMessage>.Update.Push(m => m.Reactions,
  **Reactions and Read Status are embedded in message documents** to avoid joins and improve read performance
-   new MessageReaction { UserId = "user789", Emoji = "❤️" });
await messagesCollection.UpdateOneAsync(filter, update);

// Update read status
var statusUpdate = Builders<MongoMessage>.Update.Set("readStatus.$[elem].status", "read");
var arrayFilter = new BsonDocumentArrayFilterDefinition<BsonDocument>(
    new BsonDocument("elem.userId", userId));
await messagesCollection.UpdateOneAsync(filter, statusUpdate,
    new UpdateOptions { ArrayFilters = new[] { arrayFilter } });
```

## Index Creation

Run the MongoDB index creation script located at:
`Database/MongoDB_CreateIndexes.js`

## Architecture Notes

- All models use `string Id` with `[BsonId]` attribute for MongoDB's ObjectId
- GUIDs are stored as strings with `[BsonRepresentation(BsonType.String)]`
- DateTime fields use UTC timezone
- All collections are denormalized for read performance
- Foreign key relationships are maintained via UUID fields (not MongoDB references)

## Related Files

- MongoDB Schema: `Database/MongoDB_Schema.json`
- Index Creation Script: `Database/MongoDB_CreateIndexes.js`
- SQL Server Schema: `Database/SQLServer_Schema.sql`
