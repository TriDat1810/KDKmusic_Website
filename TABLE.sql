USE master

CREATE DATABASE KDKMusic
ON 
( NAME = 'KDKMusic_dat', FILENAME = 'C:\Users\Public\DATA\KDKMusicdat.mdf')
LOG ON
( NAME = 'KDKMusic_log', FILENAME = 'C:\Users\Public\DATA\KDKMusicLog.ldf')
GO

USE [KDKMusic]
GO

Create table [Admin]
(
	[User_Id] INT IDENTITY,
	[User_name] VARCHAR(100) UNIQUE,
	[Password] VARCHAR(100) NOT NULL,
	[E_mail] VARCHAR(100),
	[FullName] NVARCHAR(100),
	[is_Admin] BIT, --1 is admin and 0 is not admin
	PRIMARY KEY ([User_Id])
) 
go

Create table [User]
(
	[User_Id] INT IDENTITY,
	[User_name] VARCHAR(100) UNIQUE,
	[Password] VARCHAR(100) NOT NULL,
	[E_mail] VARCHAR(100),
	PRIMARY KEY ([User_Id])
) 
go

Create table [Country]
(
	[Country_Id] INT IDENTITY,
	[Country_Name] NVARCHAR(100),
	PRIMARY KEY ([Country_Id])
) 
go

Create table [Music_Genre]
(
	[Genre_Id] INT IDENTITY,
	[Genre_Name] NVARCHAR(100),
	Primary Key ([Genre_Id])
)
go

Create table [Artist]
(
	[Artist_Id] INT IDENTITY,
	[Country_Id] INT,
	[Artist_Name] Nvarchar(100),
	[Artist_Image] VARCHAR(MAX),
	[Artist_Info] Nvarchar(MAX),
	Primary Key ([Artist_Id]),
	Foreign Key ([Country_Id]) References [Country]([Country_Id]) ON UPDATE CASCADE
) 
go

Create table [Albums]
(
	[Album_Id] INT IDENTITY,
	[Album_Name] NVARCHAR(100),
	[Artist_Id] INT,
	Primary Key ([Album_Id]),
	Foreign Key ([Artist_Id]) References [Artist]([Artist_Id]) ON DELETE CASCADE
) 
go

Create table [Song]
(
	[Song_Id] INT IDENTITY,
	[Genre_Id] INT NULL,
	[Artist_Id] INT,
	[Album_Id] INT NULL,
	[Song_Name] NVARCHAR(100),
	[Song_Path] VARCHAR(MAX),
	[Lyrics] Nvarchar(MAX),
	[Create_at] DATETIME DEFAULT GETDATE(),
	[Song_Image] VARCHAR(MAX),
	PRIMARY KEY([Song_Id]),
	FOREIGN KEY ([Artist_Id]) REFERENCES [Artist]([Artist_Id]) ON DELETE CASCADE,
	FOREIGN KEY ([Album_Id]) REFERENCES [Albums]([Album_Id]),
	FOREIGN KEY ([Genre_Id]) REFERENCES [Music_Genre]([Genre_Id])
) 
go

Create table [Interaction]
(
	[Interaction_Id] Bigint Identity,
	[Liked] INT,
	[Play_Count] BIGINT,
	[User_Id] INT,
	[Song_Id] INT,
	Primary Key ([Interaction_Id]),
	Foreign Key ([User_Id]) References [User]([User_Id]) ON DELETE CASCADE,
	Foreign Key ([Song_Id]) References [Song]([Song_Id]) ON DELETE CASCADE
) 
go

Create table [Playlist]
(
	[Playlist_Id] INT IDENTITY,
	[Playlist_Name] NVARCHAR(100),
	[Create_At] DATETIME Default GetDate(),
	[User_Id] INT,
	Primary Key ([Playlist_Id]),
	Foreign Key ([User_Id]) References [User]([User_Id]) ON DELETE CASCADE
) 
go

Create table [Playlist_Song]
(
	[Playlist_Id] INT,
	[Song_Id] INT,
	Primary Key ([Playlist_Id],[Song_Id]),
	Foreign Key ([Song_Id]) References [Song]([Song_Id]) ON DELETE CASCADE,
	Foreign Key ([Playlist_Id]) References [Playlist]([Playlist_Id]) ON DELETE CASCADE
) 
go