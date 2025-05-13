CREATE TABLE [dbo].[LinkUserCompanies]
(
	[UserId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,

	constraint [PK_LinkUserCompanies] primary key clustered ([UserId], [CompanyId]),
	constraint [FK_LinkUserCompanies_Users] foreign key ([UserId]) references [dbo].[Users] ([Id]),
	constraint [FK_LinkUserCompanies_Companies] foreign key ([CompanyId]) references [dbo].[Companies] ([Id])
)
