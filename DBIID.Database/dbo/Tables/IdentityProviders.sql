CREATE TABLE [dbo].[IdentityProviders] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (255) NOT NULL,
    [TenantId] NVARCHAR (255) NOT NULL,
    [Secret] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_IdentityProviders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

