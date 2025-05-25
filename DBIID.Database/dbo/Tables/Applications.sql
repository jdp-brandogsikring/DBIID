CREATE TABLE [dbo].[Applications] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (255) NOT NULL,
    [Token]  NVARCHAR (255) NULL,
    [Url] NVARCHAR (255) NULL,
    [PushUrl] NVARCHAR (255)  NULL DEFAULT(''),
    [EnablePush] BIT  NULL DEFAULT(0),
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

