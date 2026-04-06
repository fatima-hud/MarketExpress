IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'identity') IS NULL EXEC(N'CREATE SCHEMA [identity];');
GO

IF SCHEMA_ID(N'dto') IS NULL EXEC(N'CREATE SCHEMA [dto];');
GO

CREATE TABLE [identity].[AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [identity].[AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Discriminator] nvarchar(13) NOT NULL,
    [FirstName] nvarchar(50) NULL,
    [LastName] nvarchar(50) NULL,
    [ProfilePhoto] varbinary(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dto].[Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(100) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [Rating] int NOT NULL,
    [Season] int NOT NULL,
    [Gender] int NOT NULL,
    [Image] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [identity].[AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [identity].[AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [identity].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [identity].[AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [identity].[AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [identity].[AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [identity].[AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [identity].[AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [identity].[AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [identity].[AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [identity].[AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [identity].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251130103031_InitialCreate', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] ADD [Type] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251130110022_InitialCreatee', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [identity].[OtpCode] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [Expiry] datetime2 NOT NULL,
    CONSTRAINT [PK_OtpCode] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OtpCode_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_OtpCode_UserId] ON [identity].[OtpCode] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251201182117_x', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[OtpCode] ADD [Code] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251201202418_xx', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[OtpCode] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251202173727_addColumnCreatedAt', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[OtpCode] DROP CONSTRAINT [FK_OtpCode_AspNetUsers_UserId];
GO

ALTER TABLE [identity].[OtpCode] DROP CONSTRAINT [PK_OtpCode];
GO

EXEC sp_rename N'[identity].[OtpCode]', N'OtpCodes';
ALTER SCHEMA [dto] TRANSFER [identity].[OtpCodes];
GO

EXEC sp_rename N'[dto].[OtpCodes].[IX_OtpCode_UserId]', N'IX_OtpCodes_UserId', N'INDEX';
GO

ALTER TABLE [dto].[OtpCodes] ADD CONSTRAINT [PK_OtpCodes] PRIMARY KEY ([Id]);
GO

ALTER TABLE [dto].[OtpCodes] ADD CONSTRAINT [FK_OtpCodes_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251202174037_UpdateSchemeOtpCode', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203094521_AddTableCategory', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[Stories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Logo] varbinary(max) NOT NULL,
    [CreatedAT] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [Latitude] decimal(18,2) NOT NULL,
    [Longitude] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Stories] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203111258_aaaaa', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [identity].[StoreCategory] (
    [Id] int NOT NULL IDENTITY,
    [StoreId] int NOT NULL,
    [CategoryId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_StoreCategory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StoreCategory_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dto].[Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StoreCategory_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_StoreCategory_CategoryId] ON [identity].[StoreCategory] ([CategoryId]);
GO

CREATE INDEX [IX_StoreCategory_StoreId] ON [identity].[StoreCategory] ([StoreId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203125128_df', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[StoreCategory] DROP CONSTRAINT [FK_StoreCategory_Categories_CategoryId];
GO

ALTER TABLE [identity].[StoreCategory] DROP CONSTRAINT [FK_StoreCategory_Stories_StoreId];
GO

ALTER TABLE [identity].[StoreCategory] DROP CONSTRAINT [PK_StoreCategory];
GO

EXEC sp_rename N'[identity].[StoreCategory]', N'StoreCategories';
ALTER SCHEMA [dto] TRANSFER [identity].[StoreCategories];
GO

EXEC sp_rename N'[dto].[StoreCategories].[IX_StoreCategory_StoreId]', N'IX_StoreCategories_StoreId', N'INDEX';
GO

EXEC sp_rename N'[dto].[StoreCategories].[IX_StoreCategory_CategoryId]', N'IX_StoreCategories_CategoryId', N'INDEX';
GO

ALTER TABLE [dto].[StoreCategories] ADD CONSTRAINT [PK_StoreCategories] PRIMARY KEY ([Id]);
GO

ALTER TABLE [dto].[StoreCategories] ADD CONSTRAINT [FK_StoreCategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dto].[Categories] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[StoreCategories] ADD CONSTRAINT [FK_StoreCategories_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203125758_dfh', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] ADD [StoreCategoryId] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_Products_StoreCategoryId] ON [dto].[Products] ([StoreCategoryId]);
GO

ALTER TABLE [dto].[Products] ADD CONSTRAINT [FK_Products_StoreCategories_StoreCategoryId] FOREIGN KEY ([StoreCategoryId]) REFERENCES [dto].[StoreCategories] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204090446_bbbb', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] ADD [DiscountSettingId] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [dto].[DiscountSettings] (
    [Id] int NOT NULL IDENTITY,
    [DiscountPercentage] decimal(18,2) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [DiscountSettingId] int NULL,
    CONSTRAINT [PK_DiscountSettings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DiscountSettings_DiscountSettings_DiscountSettingId] FOREIGN KEY ([DiscountSettingId]) REFERENCES [dto].[DiscountSettings] ([Id])
);
GO

CREATE INDEX [IX_Products_DiscountSettingId] ON [dto].[Products] ([DiscountSettingId]);
GO

CREATE INDEX [IX_DiscountSettings_DiscountSettingId] ON [dto].[DiscountSettings] ([DiscountSettingId]);
GO

ALTER TABLE [dto].[Products] ADD CONSTRAINT [FK_Products_DiscountSettings_DiscountSettingId] FOREIGN KEY ([DiscountSettingId]) REFERENCES [dto].[DiscountSettings] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204101645_bbbbb', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] ADD [PriceAfterDiscount] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204103543_bbbsbb', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[Ratings] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [ProductId] int NOT NULL,
    [RatingValue] int NOT NULL,
    [Comment] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Ratings] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204114147_bbgbsbb', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[Carts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Carts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dto].[CartItems] (
    [Id] int NOT NULL IDENTITY,
    [CartId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Size] nvarchar(max) NOT NULL,
    [Color] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [dto].[Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dto].[Products] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_CartItems_CartId] ON [dto].[CartItems] ([CartId]);
GO

CREATE INDEX [IX_CartItems_ProductId] ON [dto].[CartItems] ([ProductId]);
GO

CREATE INDEX [IX_Carts_UserId] ON [dto].[Carts] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251206090732_addCart', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[Orders] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [StoreId] int NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Statue] int NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dto].[OrderItems] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Size] nvarchar(max) NOT NULL,
    [Color] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dto].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dto].[Products] ([Id])
);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [dto].[OrderItems] ([OrderId]);
GO

CREATE INDEX [IX_OrderItems_ProductId] ON [dto].[OrderItems] ([ProductId]);
GO

CREATE INDEX [IX_Orders_StoreId] ON [dto].[Orders] ([StoreId]);
GO

CREATE INDEX [IX_Orders_UserId] ON [dto].[Orders] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251206113109_addorder', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251206125045_addorrder', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[identity].[AspNetUsers]') AND [c].[name] = N'PhoneNumber');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [identity].[AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [identity].[AspNetUsers] ALTER COLUMN [PhoneNumber] nvarchar(10) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251206200617_t', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
GO

ALTER TABLE [identity].[AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
GO

ALTER TABLE [identity].[AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
GO

ALTER TABLE [identity].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
GO

ALTER TABLE [identity].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
GO

ALTER TABLE [identity].[AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
GO

ALTER TABLE [dto].[Carts] DROP CONSTRAINT [FK_Carts_AspNetUsers_UserId];
GO

ALTER TABLE [dto].[Orders] DROP CONSTRAINT [FK_Orders_AspNetUsers_UserId];
GO

ALTER TABLE [dto].[OtpCodes] DROP CONSTRAINT [FK_OtpCodes_AspNetUsers_UserId];
GO

ALTER TABLE [identity].[AspNetUserTokens] DROP CONSTRAINT [PK_AspNetUserTokens];
GO

ALTER TABLE [identity].[AspNetUsers] DROP CONSTRAINT [PK_AspNetUsers];
GO

ALTER TABLE [identity].[AspNetUserRoles] DROP CONSTRAINT [PK_AspNetUserRoles];
GO

ALTER TABLE [identity].[AspNetUserLogins] DROP CONSTRAINT [PK_AspNetUserLogins];
GO

ALTER TABLE [identity].[AspNetUserClaims] DROP CONSTRAINT [PK_AspNetUserClaims];
GO

ALTER TABLE [identity].[AspNetRoles] DROP CONSTRAINT [PK_AspNetRoles];
GO

ALTER TABLE [identity].[AspNetRoleClaims] DROP CONSTRAINT [PK_AspNetRoleClaims];
GO

EXEC sp_rename N'[identity].[AspNetUserTokens]', N'UserTokens';
GO

EXEC sp_rename N'[identity].[AspNetUsers]', N'Users';
GO

EXEC sp_rename N'[identity].[AspNetUserRoles]', N'UserRoles';
GO

EXEC sp_rename N'[identity].[AspNetUserLogins]', N'UserLogins';
GO

EXEC sp_rename N'[identity].[AspNetUserClaims]', N'UserClaims';
GO

EXEC sp_rename N'[identity].[AspNetRoles]', N'Roles';
GO

EXEC sp_rename N'[identity].[AspNetRoleClaims]', N'RoleClaims';
GO

EXEC sp_rename N'[identity].[UserRoles].[IX_AspNetUserRoles_RoleId]', N'IX_UserRoles_RoleId', N'INDEX';
GO

EXEC sp_rename N'[identity].[UserLogins].[IX_AspNetUserLogins_UserId]', N'IX_UserLogins_UserId', N'INDEX';
GO

EXEC sp_rename N'[identity].[UserClaims].[IX_AspNetUserClaims_UserId]', N'IX_UserClaims_UserId', N'INDEX';
GO

EXEC sp_rename N'[identity].[RoleClaims].[IX_AspNetRoleClaims_RoleId]', N'IX_RoleClaims_RoleId', N'INDEX';
GO

ALTER TABLE [identity].[UserTokens] ADD CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]);
GO

ALTER TABLE [identity].[Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY ([Id]);
GO

ALTER TABLE [identity].[UserRoles] ADD CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]);
GO

ALTER TABLE [identity].[UserLogins] ADD CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]);
GO

ALTER TABLE [identity].[UserClaims] ADD CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]);
GO

ALTER TABLE [identity].[Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]);
GO

ALTER TABLE [identity].[RoleClaims] ADD CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[Orders] ADD CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[OtpCodes] ADD CONSTRAINT [FK_OtpCodes_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[RoleClaims] ADD CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[UserClaims] ADD CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[UserLogins] ADD CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[UserRoles] ADD CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[UserRoles] ADD CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [identity].[UserTokens] ADD CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251206201921_tt', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] DROP CONSTRAINT [FK_Products_DiscountSettings_DiscountSettingId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Products]') AND [c].[name] = N'DiscountSettingId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Products] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [dto].[Products] ALTER COLUMN [DiscountSettingId] int NULL;
GO

ALTER TABLE [dto].[Products] ADD CONSTRAINT [FK_Products_DiscountSettings_DiscountSettingId] FOREIGN KEY ([DiscountSettingId]) REFERENCES [dto].[DiscountSettings] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251208193253_kj', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Stories]') AND [c].[name] = N'Logo');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Stories] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [dto].[Stories] ALTER COLUMN [Logo] nvarchar(max) NOT NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Products]') AND [c].[name] = N'Image');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Products] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [dto].[Products] ALTER COLUMN [Image] nvarchar(max) NOT NULL;
GO

CREATE TABLE [dto].[clothingitems] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [Color] nvarchar(max) NOT NULL,
    [Size] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_clothingitems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_clothingitems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dto].[Products] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_clothingitems_ProductId] ON [dto].[clothingitems] ([ProductId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251209121740_addclothingitem', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[clothingitems] ADD [IsFoundProduct] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251209121933_addclothingditem', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[Users] ADD [BirthDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251214185322_addcolumnBirthDate', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[UserRefreshTokens] (
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NOT NULL,
    [RefreshToken] nvarchar(max) NOT NULL,
    [IsUsed] bit NOT NULL,
    [IsRevoked] bit NOT NULL,
    [AddedTime] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserRefreshTokens_UserId] ON [dto].[UserRefreshTokens] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251215195446_f', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [identity].[RefreshToken] (
    [UserId] nvarchar(450) NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NOT NULL,
    [ExpiresOn] datetime2 NOT NULL,
    [CreatesOn] datetime2 NOT NULL,
    [RevokeOn] datetime2 NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([UserId], [Id]),
    CONSTRAINT [FK_RefreshToken_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251216123847_addTabelUserRefreshToken', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251216125946_addTabelRefreshToken', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dto].[userRoleStores] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [StoreId] int NOT NULL,
    CONSTRAINT [PK_userRoleStores] PRIMARY KEY ([UserId], [RoleId], [StoreId]),
    CONSTRAINT [FK_userRoleStores_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]),
    CONSTRAINT [FK_userRoleStores_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_userRoleStores_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_userRoleStores_RoleId] ON [dto].[userRoleStores] ([RoleId]);
GO

CREATE INDEX [IX_userRoleStores_StoreId] ON [dto].[userRoleStores] ([StoreId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251222105800_addUserRoleStore', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Carts] ADD [StoreId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [dto].[Carts] ADD [StoreId1] int NULL;
GO

ALTER TABLE [dto].[Carts] ADD [UserId1] nvarchar(450) NULL;
GO

CREATE INDEX [IX_Carts_StoreId] ON [dto].[Carts] ([StoreId]);
GO

CREATE INDEX [IX_Carts_StoreId1] ON [dto].[Carts] ([StoreId1]);
GO

CREATE INDEX [IX_Carts_UserId1] ON [dto].[Carts] ([UserId1]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Stories_StoreId1] FOREIGN KEY ([StoreId1]) REFERENCES [dto].[Stories] ([Id]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Users_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [identity].[Users] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251222120731_addcolumnstoreIdincart', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[DiscountSettings] ADD [UserId] nvarchar(450) NOT NULL DEFAULT N'';
GO

CREATE INDEX [IX_DiscountSettings_UserId] ON [dto].[DiscountSettings] ([UserId]);
GO

ALTER TABLE [dto].[DiscountSettings] ADD CONSTRAINT [FK_DiscountSettings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251222122129_addcolumnUserIdInDisxountsetting', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] DROP CONSTRAINT [FK_Products_StoreCategories_StoreCategoryId];
GO

EXEC sp_rename N'[dto].[Products].[StoreCategoryId]', N'StoreCategoryID', N'COLUMN';
GO

EXEC sp_rename N'[dto].[Products].[IX_Products_StoreCategoryId]', N'IX_Products_StoreCategoryID', N'INDEX';
GO

ALTER TABLE [dto].[Products] ADD CONSTRAINT [FK_Products_StoreCategories_StoreCategoryID] FOREIGN KEY ([StoreCategoryID]) REFERENCES [dto].[StoreCategories] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251223094835_updatestorescategory', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251223094924_addstorescategory', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [identity].[RefreshToken];
GO

DROP TABLE [dto].[userRoleStores];
GO

ALTER TABLE [identity].[Users] ADD [IsBanned] bit NULL;
GO

ALTER TABLE [identity].[Users] ADD [ResetPasswordCode] nvarchar(max) NULL;
GO

ALTER TABLE [identity].[Users] ADD [ResetPasswordCodeExpiry] datetime2 NULL;
GO

ALTER TABLE [identity].[UserRoles] ADD [Discriminator] nvarchar(34) NOT NULL DEFAULT N'';
GO

ALTER TABLE [identity].[UserRoles] ADD [StoreId] int NULL;
GO

ALTER TABLE [identity].[Roles] ADD [Discriminator] nvarchar(21) NOT NULL DEFAULT N'';
GO

CREATE TABLE [dto].[UserRefreshTokens] (
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NOT NULL,
    [RefreshToken] nvarchar(max) NOT NULL,
    [ExpiresOn] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    [CreatesOn] datetime2 NOT NULL,
    [RevokeOn] datetime2 NULL,
    [UserId] nvarchar(450) NOT NULL,
    [IsRevoked] bit NOT NULL,
    CONSTRAINT [PK_UserRefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserRoles_StoreId] ON [identity].[UserRoles] ([StoreId]);
GO

CREATE INDEX [IX_UserRefreshTokens_UserId] ON [dto].[UserRefreshTokens] ([UserId]);
GO

ALTER TABLE [identity].[UserRoles] ADD CONSTRAINT [FK_UserRoles_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251229112029_c', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251229112455_h', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251229113152_ha', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251229113302_hay', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[OtpCodes] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251230210654_addcolumnIsactiveinotpcode', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251231131556_removecolumn', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[UserRefreshTokens]') AND [c].[name] = N'IsUsed');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [dto].[UserRefreshTokens] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [dto].[UserRefreshTokens] DROP COLUMN [IsUsed];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260102175807_removecolumnIsUsed', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[identity].[Users]') AND [c].[name] = N'ProfilePhoto');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [identity].[Users] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [identity].[Users] ALTER COLUMN [ProfilePhoto] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260102185007_updatecolumnProfilephoto', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Ratings]') AND [c].[name] = N'RatingValue');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Ratings] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [dto].[Ratings] ALTER COLUMN [RatingValue] int NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Ratings]') AND [c].[name] = N'Comment');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Ratings] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [dto].[Ratings] ALTER COLUMN [Comment] nvarchar(100) NULL;
GO

ALTER TABLE [dto].[Products] ADD [DiscountEndDate] datetime2 NULL;
GO

ALTER TABLE [dto].[Products] ADD [DiscountPercentage] decimal(18,2) NULL;
GO

ALTER TABLE [dto].[Products] ADD [DiscountStartDate] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260103130025_discount', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Stories_StoreId];
GO

DROP INDEX [IX_UserRoles_StoreId] ON [identity].[UserRoles];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[identity].[UserRoles]') AND [c].[name] = N'Discriminator');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [identity].[UserRoles] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [identity].[UserRoles] DROP COLUMN [Discriminator];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[identity].[UserRoles]') AND [c].[name] = N'StoreId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [identity].[UserRoles] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [identity].[UserRoles] DROP COLUMN [StoreId];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[identity].[Roles]') AND [c].[name] = N'Discriminator');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [identity].[Roles] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [identity].[Roles] DROP COLUMN [Discriminator];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Products]') AND [c].[name] = N'PriceAfterDiscount');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Products] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [dto].[Products] ALTER COLUMN [PriceAfterDiscount] decimal(18,2) NULL;
GO

CREATE TABLE [dto].[UserStoreRoles] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [StoreId] int NOT NULL,
    [StoreId1] int NULL,
    [UserId1] nvarchar(450) NULL,
    CONSTRAINT [PK_UserStoreRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserStoreRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserStoreRoles_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserStoreRoles_Stories_StoreId1] FOREIGN KEY ([StoreId1]) REFERENCES [dto].[Stories] ([Id]),
    CONSTRAINT [FK_UserStoreRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserStoreRoles_Users_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [identity].[Users] ([Id])
);
GO

CREATE INDEX [IX_UserStoreRoles_RoleId] ON [dto].[UserStoreRoles] ([RoleId]);
GO

CREATE INDEX [IX_UserStoreRoles_StoreId] ON [dto].[UserStoreRoles] ([StoreId]);
GO

CREATE INDEX [IX_UserStoreRoles_StoreId1] ON [dto].[UserStoreRoles] ([StoreId1]);
GO

CREATE UNIQUE INDEX [IX_UserStoreRoles_UserId_RoleId_StoreId] ON [dto].[UserStoreRoles] ([UserId], [RoleId], [StoreId]);
GO

CREATE INDEX [IX_UserStoreRoles_UserId1] ON [dto].[UserStoreRoles] ([UserId1]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109211819_addentityUserstoreRole', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_UserStoreRoles_UserId_RoleId_StoreId] ON [dto].[UserStoreRoles];
GO

CREATE INDEX [IX_UserStoreRoles_UserId] ON [dto].[UserStoreRoles] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109211956_UpdateentityUserstoreRole', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_UserStoreRoles_UserId] ON [dto].[UserStoreRoles];
GO

CREATE UNIQUE INDEX [IX_UserStoreRoles_UserId_RoleId_StoreId] ON [dto].[UserStoreRoles] ([UserId], [RoleId], [StoreId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109212235_UpdateentitdyUserstoreRole', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109212511_UpdateentitdyUserstoreRolde', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Roles_RoleId];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Stories_StoreId];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Stories_StoreId1];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Users_UserId];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Users_UserId1];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [PK_UserStoreRoles];
GO

DROP INDEX [IX_UserStoreRoles_StoreId1] ON [dto].[UserStoreRoles];
GO

DROP INDEX [IX_UserStoreRoles_UserId_RoleId_StoreId] ON [dto].[UserStoreRoles];
GO

DROP INDEX [IX_UserStoreRoles_UserId1] ON [dto].[UserStoreRoles];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[UserStoreRoles]') AND [c].[name] = N'StoreId1');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [dto].[UserStoreRoles] DROP COLUMN [StoreId1];
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[UserStoreRoles]') AND [c].[name] = N'UserId1');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [dto].[UserStoreRoles] DROP COLUMN [UserId1];
GO

EXEC sp_rename N'[dto].[UserStoreRoles]', N'UserStoreRole';
GO

EXEC sp_rename N'[dto].[UserStoreRole].[IX_UserStoreRoles_StoreId]', N'IX_UserStoreRole_StoreId', N'INDEX';
GO

EXEC sp_rename N'[dto].[UserStoreRole].[IX_UserStoreRoles_RoleId]', N'IX_UserStoreRole_RoleId', N'INDEX';
GO

ALTER TABLE [dto].[UserStoreRole] ADD CONSTRAINT [PK_UserStoreRole] PRIMARY KEY ([Id]);
GO

CREATE INDEX [IX_UserStoreRole_UserId] ON [dto].[UserStoreRole] ([UserId]);
GO

ALTER TABLE [dto].[UserStoreRole] ADD CONSTRAINT [FK_UserStoreRole_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[UserStoreRole] ADD CONSTRAINT [FK_UserStoreRole_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[UserStoreRole] ADD CONSTRAINT [FK_UserStoreRole_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109212733_UpdateentitdyUserstoreRolxde', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[UserStoreRole] DROP CONSTRAINT [FK_UserStoreRole_Roles_RoleId];
GO

ALTER TABLE [dto].[UserStoreRole] DROP CONSTRAINT [FK_UserStoreRole_Stories_StoreId];
GO

ALTER TABLE [dto].[UserStoreRole] DROP CONSTRAINT [FK_UserStoreRole_Users_UserId];
GO

ALTER TABLE [dto].[UserStoreRole] DROP CONSTRAINT [PK_UserStoreRole];
GO

DROP INDEX [IX_UserStoreRole_UserId] ON [dto].[UserStoreRole];
GO

EXEC sp_rename N'[dto].[UserStoreRole]', N'UserStoreRoles';
GO

EXEC sp_rename N'[dto].[UserStoreRoles].[IX_UserStoreRole_StoreId]', N'IX_UserStoreRoles_StoreId', N'INDEX';
GO

EXEC sp_rename N'[dto].[UserStoreRoles].[IX_UserStoreRole_RoleId]', N'IX_UserStoreRoles_RoleId', N'INDEX';
GO

ALTER TABLE [dto].[UserStoreRoles] ADD CONSTRAINT [PK_UserStoreRoles] PRIMARY KEY ([Id]);
GO

CREATE UNIQUE INDEX [IX_UserStoreRoles_UserId_RoleId_StoreId] ON [dto].[UserStoreRoles] ([UserId], [RoleId], [StoreId]);
GO

ALTER TABLE [dto].[UserStoreRoles] ADD CONSTRAINT [FK_UserStoreRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[Roles] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [dto].[UserStoreRoles] ADD CONSTRAINT [FK_UserStoreRoles_Stories_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stories] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [dto].[UserStoreRoles] ADD CONSTRAINT [FK_UserStoreRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[Users] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260109213215_adddx', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Carts] DROP CONSTRAINT [FK_Carts_Stories_StoreId];
GO

ALTER TABLE [dto].[Carts] DROP CONSTRAINT [FK_Carts_Stories_StoreId1];
GO

ALTER TABLE [dto].[Orders] DROP CONSTRAINT [FK_Orders_Stories_StoreId];
GO

ALTER TABLE [dto].[StoreCategories] DROP CONSTRAINT [FK_StoreCategories_Stories_StoreId];
GO

ALTER TABLE [dto].[UserStoreRoles] DROP CONSTRAINT [FK_UserStoreRoles_Stories_StoreId];
GO

ALTER TABLE [dto].[Stories] DROP CONSTRAINT [PK_Stories];
GO

EXEC sp_rename N'[dto].[Stories]', N'Stores';
GO

ALTER TABLE [dto].[Stores] ADD CONSTRAINT [PK_Stores] PRIMARY KEY ([Id]);
GO

CREATE TABLE [dto].[StoreBusinessHours] (
    [Id] int NOT NULL IDENTITY,
    [StoreId] int NOT NULL,
    [DayOfWeek] int NOT NULL,
    [OpenTime] time NOT NULL,
    [CloseTime] time NOT NULL,
    [IsClosed] bit NOT NULL,
    CONSTRAINT [PK_StoreBusinessHours] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StoreBusinessHours_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_StoreBusinessHours_StoreId] ON [dto].[StoreBusinessHours] ([StoreId]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]);
GO

ALTER TABLE [dto].[Carts] ADD CONSTRAINT [FK_Carts_Stores_StoreId1] FOREIGN KEY ([StoreId1]) REFERENCES [dto].[Stores] ([Id]);
GO

ALTER TABLE [dto].[Orders] ADD CONSTRAINT [FK_Orders_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[StoreCategories] ADD CONSTRAINT [FK_StoreCategories_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dto].[UserStoreRoles] ADD CONSTRAINT [FK_UserStoreRoles_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260114115907_addstoreBusinessHours', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[StoreBusinessHours] DROP CONSTRAINT [FK_StoreBusinessHours_Stores_StoreId];
GO

ALTER TABLE [dto].[StoreBusinessHours] DROP CONSTRAINT [PK_StoreBusinessHours];
GO

EXEC sp_rename N'[dto].[StoreBusinessHours]', N'StoreBusinessHourss';
GO

EXEC sp_rename N'[dto].[StoreBusinessHourss].[IX_StoreBusinessHours_StoreId]', N'IX_StoreBusinessHourss_StoreId', N'INDEX';
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[StoreBusinessHourss]') AND [c].[name] = N'OpenTime');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [dto].[StoreBusinessHourss] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [dto].[StoreBusinessHourss] ALTER COLUMN [OpenTime] time NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[StoreBusinessHourss]') AND [c].[name] = N'CloseTime');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [dto].[StoreBusinessHourss] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [dto].[StoreBusinessHourss] ALTER COLUMN [CloseTime] time NULL;
GO

ALTER TABLE [dto].[StoreBusinessHourss] ADD CONSTRAINT [PK_StoreBusinessHourss] PRIMARY KEY ([Id]);
GO

ALTER TABLE [dto].[StoreBusinessHourss] ADD CONSTRAINT [FK_StoreBusinessHourss_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260114150047_addstoreBusinesssHours', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[StoreBusinessHourss] DROP CONSTRAINT [FK_StoreBusinessHourss_Stores_StoreId];
GO

ALTER TABLE [dto].[StoreBusinessHourss] DROP CONSTRAINT [PK_StoreBusinessHourss];
GO

EXEC sp_rename N'[dto].[StoreBusinessHourss]', N'StoreBusinessHours';
GO

EXEC sp_rename N'[dto].[StoreBusinessHours].[IX_StoreBusinessHourss_StoreId]', N'IX_StoreBusinessHours_StoreId', N'INDEX';
GO

ALTER TABLE [dto].[StoreBusinessHours] ADD CONSTRAINT [PK_StoreBusinessHours] PRIMARY KEY ([Id]);
GO

ALTER TABLE [dto].[StoreBusinessHours] ADD CONSTRAINT [FK_StoreBusinessHours_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dto].[Stores] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260114150156_addstoreBusinesHours', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Stores] ADD [DeletedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [dto].[Stores] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [dto].[Products] ADD [DeletedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [dto].[Products] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260115204519_addcolumnIsDelete', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[UserStoreRoles] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260115210953_addcolumnIsActive', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260117223934_gh', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [identity].[Users] ADD [Gender] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260131192047_addcolumnGendertoTableUser', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [dto].[Products] DROP CONSTRAINT [FK_Products_DiscountSettings_DiscountSettingId];
GO

DROP TABLE [dto].[DiscountSettings];
GO

DROP INDEX [IX_Products_DiscountSettingId] ON [dto].[Products];
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dto].[Products]') AND [c].[name] = N'DiscountSettingId');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [dto].[Products] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [dto].[Products] DROP COLUMN [DiscountSettingId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206174429_ds', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206191921_d', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206192239_InitialCreated', N'8.0.0');
GO

COMMIT;
GO

