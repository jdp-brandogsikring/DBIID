CREATE TABLE [dbo].[Users] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [GivenName]  NVARCHAR (255) NOT NULL,
    [FamilyName] NVARCHAR (255) NOT NULL,
    [Email] NVARCHAR (255) NOT NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

