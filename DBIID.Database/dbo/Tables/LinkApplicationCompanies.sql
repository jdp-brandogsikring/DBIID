CREATE TABLE [dbo].[LinkApplicationCompanies]
(
	[ApplicationId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,

	constraint [PK_LinkApplicationCompanies] primary key clustered ([ApplicationId], [CompanyId]),
	constraint [FK_LinkApplicationCompanies_Applications] foreign key ([ApplicationId]) references [dbo].[Applications] ([Id]),
	constraint [FK_LinkApplicationCompanies_Companies] foreign key ([CompanyId]) references [dbo].[Companies] ([Id])
)
