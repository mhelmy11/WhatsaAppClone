# WhatsApp Clone - Status Migration to MongoDB

## Summary

Successfully migrated the Status/Stories feature from SQL Server to MongoDB for better performance and scalability.

## Why MongoDB for Status?

- **Temporary Data**: Stories expire after 24 hours (MongoDB TTL indexes handle auto-deletion)
- **High Volume**: Frequent reads/writes for viewing and posting stories
- **Media-Heavy**: Large attachments and media metadata
- **Simple Queries**: No complex relational joins needed
- **Flexible Schema**: Easy to add new fields without migrations

## Changes Made

### 1. SQL Server Schema (`Database/SQLServer_Schema.sql`)

- ✅ Removed `Statuses` table from SQL Server
- ✅ Removed related indexes

### 2. MongoDB Schema (`Database/MongoDB_Schema.json`)

- ✅ Added `statuses` collection with:
  - _id (ObjectId, auto-generated primary key)
  - userId (string reference to SQL Users)
  - content, mediaUrl, mediaType
  - backgroundColor, fontStyle (for text stories)
  - createdAt, expiresAt
  - viewCount and views array
  - isActive flag

### 3. MongoDB Indexes (`Database/MongoDB_CreateIndexes.js`)

- ✅ Created indexes for:
  - userId + createdAt (user's stories)
  - **expiresAt with TTL** (auto-delete after expiration)
  - isActive + expiresAt (active stories)
  - views.userId (view lookups)

### 4. Status Model (`WhatsappClone.Data\Models\Status.cs`)

- ✅ Added MongoDB BSON attributes
- ✅ Changed from Entity Framework to MongoDB model
- ✅ Added StatusView embedded document for tracking views
- ✅ Proper DateTime UTC handling
- ✅ Kept AppUser navigation property for API responses (not stored in MongoDB)

### 5. Infrastructure Layer

#### New Files Created:

- `MongoDbContext.cs` - MongoDB database context
- `MongoDbSettings.cs` - Configuration model
- `IStatusRepository.cs` - Status repository interface
- `StatusRepository.cs` - MongoDB implementation with methods:
  - GetByIdAsync
  - GetUserStatusesAsync
  - GetActiveStatusesAsync
  - GetContactsStatusesAsync
  - CreateAsync, UpdateAsync, DeleteAsync
  - AddViewAsync, GetStatusViewsAsync
  - DeactivateExpiredStatusesAsync

#### Updated Files:

- `Context.cs` - Removed DbSet<Status> (moved to MongoDB)
- `ModuleInfrastructureDependencies.cs` - Added MongoDB registration

### 6. Configuration (`appsettings.json`)

- ✅ Added MongoDB connection settings:

```json
"MongoDbSettings": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "whatsapp_clone"
}
```

## Required NuGet Packages

Add to `WhatsappClone.Infrastructure.csproj`:

```xml
<PackageReference Include="MongoDB.Driver" Version="2.24.0" />
<PackageReference Include="MongoDB.Bson" Version="2.24.0" />
```

## Next Steps

### 1. Install MongoDB Driver

```powershell
cd "c:\Whyred\Asp.NetCore (MVC & WebAPI)\WhatsappClone\WhatsappClone.Infrastructure"
dotnet add package MongoDB.Driver
dotnet add package MongoDB.Bson
```

### 2. Install MongoDB Server

- Download from: https://www.mongodb.com/try/download/community
- Or use Docker: `docker run -d -p 27017:27017 --name mongodb mongo:latest`

### 3. Create MongoDB Indexes

```powershell
mongo whatsapp_clone < Database/MongoDB_CreateIndexes.js
```

### 4. Update Status Service/Controller

- Update existing StatusService to use IStatusRepository instead of Context.Statuses
- Modify StatusController to work with new repository

### 5. Create Migration (Optional)

If you have existing status data in SQL Server, create a migration script to:

1. Export existing statuses from SQL
2. Import to MongoDB with proper format
3. Run: `dotnet ef migrations add RemoveStatusFromSql`

## Benefits Achieved

- ✅ **Auto-expiration**: MongoDB TTL index automatically deletes expired stories
- ✅ **Better Performance**: NoSQL optimized for read-heavy workloads
- ✅ **Scalability**: Easy horizontal scaling for high-volume stories
- ✅ **Flexibility**: Easy to add new fields without schema migrations
- ✅ **Embedded Views**: Views stored as embedded documents (no joins needed)

## Hybrid Architecture

- **SQL Server**: Users, Contacts, Groups, Messages, Auth (relational data)
- **MongoDB**: Status/Stories (temporary, high-volume data)
- **SignalR**: Real-time notifications for both

This creates an optimal hybrid architecture leveraging the best of both database types!
