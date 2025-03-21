CREATE TABLE [dbo].[OtpTransactions] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [UserId]        INT              NOT NULL,
    [TransactionId] uniqueidentifier NOT NULL,
    [OtpCode]       NVARCHAR (6)     NOT NULL,
    [Expire]        DATETIME         NOT NULL,
    [IsUsed]        BIT              NOT NULL,
    [Type]          INT             NOT NULL,

    CONSTRAINT [PK_OtpTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OtpTransactions_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

