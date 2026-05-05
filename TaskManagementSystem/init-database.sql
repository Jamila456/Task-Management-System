-- Create Database
CREATE DATABASE TaskManagementDB;
GO

USE TaskManagementDB;
GO

-- Users Table
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Email] NVARCHAR(100) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Role] NVARCHAR(20) DEFAULT 'User',
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [LastLogin] DATETIME NULL
);

-- Projects Table
CREATE TABLE [dbo].[Projects] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [StartDate] DATETIME NULL,
    [EndDate] DATETIME NULL,
    [Status] NVARCHAR(20) DEFAULT 'Active',
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Tasks Table
CREATE TABLE [dbo].[Tasks] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ProjectId] INT NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Priority] NVARCHAR(20) DEFAULT 'Medium',
    [Status] NVARCHAR(20) DEFAULT 'Pending',
    [DueDate] DATETIME NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CompletedAt] DATETIME NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
);

-- Task Comments Table
CREATE TABLE [dbo].[TaskComments] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [TaskId] INT NOT NULL,
    [Comment] NVARCHAR(500) NOT NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
);