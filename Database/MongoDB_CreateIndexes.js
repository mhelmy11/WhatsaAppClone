// MongoDB Index Creation Script
// Run this in MongoDB shell or using MongoDB driver

// use whatsapp_clone;

// messages collection
db.messages.createIndex({ "chatId": 1, "createdAt": -1 });
db.messages.createIndex({ "senderId": 1, "receiverId": 1, "createdAt": -1 });
db.messages.createIndex({ "groupId": 1, "createdAt": -1 });
db.messages.createIndex({ "isDraft": 1, "chatId": 1 }); // Quick draft lookup

// message_threads collection
db.message_threads.createIndex({ "chatId": 1, "rootMessageId": 1 }, { unique: true });
db.message_threads.createIndex({ "chatId": 1, "createdAt": -1 });

// message_mentions collection
db.message_mentions.createIndex({ "mentionedUserId": 1, "mentionedAt": -1 });
db.message_mentions.createIndex({ "messageId": 1, "mentionedUserId": 1 }, { unique: true });

// scheduled_messages collection
db.scheduled_messages.createIndex({ "status": 1, "scheduledAt": 1 });
db.scheduled_messages.createIndex({ "chatId": 1, "scheduledAt": -1 });

// message_search collection
db.message_search.createIndex({ "chatId": 1, "createdAt": -1 });
db.message_search.createIndex({ "content": "text" });

// drafts collection
db.drafts.createIndex({ "chatId": 1, "userId": 1 }, { unique: true });

// message_media_index collection
db.message_media_index.createIndex({ "chatId": 1, "mediaType": 1, "createdAt": -1 });
db.message_media_index.createIndex({ "messageId": 1 }, { unique: true });

// starred_messages collection
db.starred_messages.createIndex({ "userId": 1, "starredAt": -1 });
db.starred_messages.createIndex({ "messageId": 1, "userId": 1 }, { unique: true });

// statuses collection
db.statuses.createIndex({ "userId": 1, "createdAt": -1 });
db.statuses.createIndex({ "expiresAt": 1 }, { expireAfterSeconds: 0 }); // TTL index for auto-deletion
db.statuses.createIndex({ "isActive": 1, "expiresAt": 1 });
db.statuses.createIndex({ "views.userId": 1 });

print("All indexes created successfully!");
