IF OBJECT_ID('ArticlesToTags', 'U') IS NOT NULL DROP TABLE [dbo].[ArticlesToTags]
IF OBJECT_ID('Tags', 'U') IS NOT NULL DROP TABLE [dbo].[Tags]
IF OBJECT_ID('Articles', 'U') IS NOT NULL DROP TABLE [dbo].[Articles]

CREATE TABLE [dbo].[Tags]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[Articles]
(
	[Id] INT NOT NULL PRIMARY KEY, 
	[Title] VARCHAR(50) NOT NULL,
	[Body] VARCHAR(MAX) NOT NULL,
	[PublishTime] DATETIME NOT NULL,
	[AuthorID] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT(0)
)

CREATE TABLE [dbo].[ArticlesToTags]
(
	[ArticleID] INT NOT NULL FOREIGN KEY REFERENCES Articles(Id),
	[TagID] INT NOT NULL FOREIGN KEY REFERENCES Tags(Id),
	CONSTRAINT Artcile_Tag PRIMARY KEY (ArticleID, TagID)
)