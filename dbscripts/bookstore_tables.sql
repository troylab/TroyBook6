
CREATE TABLE [dbo].[Book](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[BookName] [nvarchar](450) NOT NULL,
	[Author] [nvarchar](100) NULL,
	[Price] [int] NULL,
	[CreateOn] [datetime2](3) NULL,
	[UpdateOn] [datetime2](3) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[BookImage](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[BookId] [int] NOT NULL,
	[FilePath] [nvarchar](450) NOT NULL,
	[CreateOn] [datetime2](3) NULL,
	[UpdateOn] [datetime2](3) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BookImage] WITH CHECK ADD  CONSTRAINT [FK_BookImage_Book] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([Id])
ON DELETE CASCADE
GO

