
/****** Object:  Schema [Auth]    Script Date: 4/27/2020 6:30:05 PM ******/
CREATE SCHEMA [Auth]
GO
/****** Object:  Table [Auth].[Apps]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [Auth].[Apps](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](150) NULL,
	[DisplayName] [nvarchar](150) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_TenantApps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[Domains]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[Domains](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[ResourceActions]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[ResourceActions](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](150) NULL,
	[ResourceId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_ResourceActions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[ResourceCollections]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[ResourceCollections](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[ResourceId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_ResourceCollections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[Resources]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[Resources](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](150) NULL,
	[DomainId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_Resources_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[RoleResourceActions]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[RoleResourceActions](
	[Id] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ResourceActionId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_RoleResourceActions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Auth].[RoleResources]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[RoleResources](
	[Id] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ResourceId] [bigint] NOT NULL,
	[CanInsert] [bit] NOT NULL,
	[CanUpdate] [bit] NOT NULL,
	[CanDelete] [bit] NOT NULL,
	[CanViewDetails] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
	[CollectionId] [bigint] NULL,
 CONSTRAINT [PK_RoleResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Auth].[Roles]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[Roles](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[TenantDomainId] [bigint] NULL,
	[Description] [nvarchar](300) NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
	[IsUserRole] [bit] NOT NULL CONSTRAINT [DF_Roles_IsUserRole]  DEFAULT ((0)),
	[TenantAppId] [bigint] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Auth].[Tenants]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[Tenants](
	[Id] [bigint] NOT NULL,
	[Code] [varchar](100) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [ntext] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[UserEntityLinks]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[UserEntityLinks](
	[Id] [bigint] NOT NULL,
	[EntityName] [varchar](60) NULL,
	[EntityId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_UserEntityLinks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Auth].[UserRoles]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[UserRoles](
	[Id] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Auth].[Users]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Auth].[Users](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[LogonName] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[PersonId] [bigint] NULL,
	[UserType] [int] NOT NULL CONSTRAINT [DF_Users_UserType]  DEFAULT ((0)),
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Mobile] [varchar](50) NULL,
	[Email] [varchar](200) NULL,
	[Photo] [nvarchar](300) NULL,
	[Gender] [bit] NULL,
	[BirthDate] [datetime] NULL,
	[TenantId] [bigint] NULL,
	[AppId] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [Auth].[ResourceActions]  WITH CHECK ADD  CONSTRAINT [FK_ResourceActions_Resources] FOREIGN KEY([ResourceId])
REFERENCES [Auth].[Resources] ([Id])
GO
ALTER TABLE [Auth].[ResourceActions] CHECK CONSTRAINT [FK_ResourceActions_Resources]
GO
ALTER TABLE [Auth].[ResourceCollections]  WITH CHECK ADD  CONSTRAINT [FK_ResourceCollections_Resources] FOREIGN KEY([ResourceId])
REFERENCES [Auth].[Resources] ([Id])
GO
ALTER TABLE [Auth].[ResourceCollections] CHECK CONSTRAINT [FK_ResourceCollections_Resources]
GO
ALTER TABLE [Auth].[Resources]  WITH CHECK ADD  CONSTRAINT [FK_Resources_Domains] FOREIGN KEY([DomainId])
REFERENCES [Auth].[Domains] ([Id])
GO
ALTER TABLE [Auth].[Resources] CHECK CONSTRAINT [FK_Resources_Domains]
GO
ALTER TABLE [Auth].[RoleResourceActions]  WITH CHECK ADD  CONSTRAINT [FK_RoleResourceActions_ResourceActions] FOREIGN KEY([ResourceActionId])
REFERENCES [Auth].[ResourceActions] ([Id])
GO
ALTER TABLE [Auth].[RoleResourceActions] CHECK CONSTRAINT [FK_RoleResourceActions_ResourceActions]
GO
ALTER TABLE [Auth].[RoleResourceActions]  WITH CHECK ADD  CONSTRAINT [FK_RoleResourceActions_Roles] FOREIGN KEY([RoleId])
REFERENCES [Auth].[Roles] ([Id])
GO
ALTER TABLE [Auth].[RoleResourceActions] CHECK CONSTRAINT [FK_RoleResourceActions_Roles]
GO
ALTER TABLE [Auth].[RoleResources]  WITH CHECK ADD  CONSTRAINT [FK_RoleResources_ResourceCollections] FOREIGN KEY([CollectionId])
REFERENCES [Auth].[ResourceCollections] ([Id])
GO
ALTER TABLE [Auth].[RoleResources] CHECK CONSTRAINT [FK_RoleResources_ResourceCollections]
GO
ALTER TABLE [Auth].[RoleResources]  WITH CHECK ADD  CONSTRAINT [FK_RoleResources_Resources] FOREIGN KEY([ResourceId])
REFERENCES [Auth].[Resources] ([Id])
GO
ALTER TABLE [Auth].[RoleResources] CHECK CONSTRAINT [FK_RoleResources_Resources]
GO
ALTER TABLE [Auth].[RoleResources]  WITH CHECK ADD  CONSTRAINT [FK_RoleResources_Roles] FOREIGN KEY([RoleId])
REFERENCES [Auth].[Roles] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [Auth].[RoleResources] CHECK CONSTRAINT [FK_RoleResources_Roles]
GO
ALTER TABLE [Auth].[Roles]  WITH CHECK ADD FOREIGN KEY([TenantAppId])
REFERENCES [Auth].[Apps] ([Id])
GO
ALTER TABLE [Auth].[UserEntityLinks]  WITH CHECK ADD  CONSTRAINT [FK_UserEntityLinks_Users] FOREIGN KEY([UserId])
REFERENCES [Auth].[Users] ([Id])
GO
ALTER TABLE [Auth].[UserEntityLinks] CHECK CONSTRAINT [FK_UserEntityLinks_Users]
GO
ALTER TABLE [Auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [Auth].[Roles] ([Id])
GO
ALTER TABLE [Auth].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [Auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId])
REFERENCES [Auth].[Users] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [Auth].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
ALTER TABLE [Auth].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Apps] FOREIGN KEY([AppId])
REFERENCES [Auth].[Apps] ([Id])
GO
ALTER TABLE [Auth].[Users] CHECK CONSTRAINT [FK_Users_Apps]
GO
ALTER TABLE [Auth].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Tenants] FOREIGN KEY([TenantId])
REFERENCES [Auth].[Tenants] ([Id])
GO
ALTER TABLE [Auth].[Users] CHECK CONSTRAINT [FK_Users_Tenants]
GO
/****** Object:  StoredProcedure [dbo].[AddAuditingColumns]    Script Date: 4/27/2020 6:30:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAuditingColumns]

AS
BEGIN
	declare @command nvarchar(max);
	declare @tables TABLE(ID int,TABLE_NAME varchar(60));
	declare @i int=0;
	declare @table varchar(60);

	insert into @tables (ID,TABLE_NAME)
	select 
		ROW_NUMBER() over (order by TABLE_NAME) ,
		TABLE_SCHEMA+'.'+ TABLE_NAME 
	from INFORMATION_SCHEMA.TABLES
	where 
		TABLE_NAME!='sysdiagrams' AND
		TABLE_SCHEMA+'.'+TABLE_NAME NOT IN (SELECT TABLE_SCHEMA+'.'+TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME='CreatedOn');

	select @i=0;
	while(@i<(select count(*) from @tables))
	begin
		select @i=@i+1;
		select @table=TABLE_NAME from @tables where ID=@i;
		select @command='ALTER TABLE '+@table+' ADD CreatedOn Datetime null;'
		print @command;
		EXEC sp_executesql @command;
	end;
	delete from @tables

	
	insert into @tables (ID,TABLE_NAME)
	select 
		ROW_NUMBER() over (order by TABLE_NAME) ,
		TABLE_SCHEMA+'.'+ TABLE_NAME 
	from INFORMATION_SCHEMA.TABLES
	where 
		TABLE_NAME!='sysdiagrams' AND
		TABLE_SCHEMA+'.'+TABLE_NAME NOT IN (SELECT TABLE_SCHEMA+'.'+TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME='CreatedBy');

	select @i=0;
	while(@i<(select count(*) from @tables))
	begin
		select @i=@i+1;
		select @table=TABLE_NAME from @tables where ID=@i;
		select @command='ALTER TABLE '+@table+' ADD CreatedBy bigint null;'
		print @command;
		EXEC sp_executesql @command;
	end;
	delete from @tables

	insert into @tables (ID,TABLE_NAME)
	select 
		ROW_NUMBER() over (order by TABLE_NAME) ,
		TABLE_SCHEMA+'.'+ TABLE_NAME 
	from INFORMATION_SCHEMA.TABLES
	where 
		TABLE_NAME!='sysdiagrams' AND
		TABLE_SCHEMA+'.'+TABLE_NAME NOT IN (SELECT TABLE_SCHEMA+'.'+TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME='UpdatedOn');

	select @i=0;
	while(@i<(select count(*) from @tables))
	begin
		select @i=@i+1;
		select @table=TABLE_NAME from @tables where ID=@i;
		select @command='ALTER TABLE '+@table+' ADD UpdatedOn Datetime null;';
		print @command;
		EXEC sp_executesql @command;
	end;
	delete from @tables

	insert into @tables (ID,TABLE_NAME)
	select 
		ROW_NUMBER() over (order by TABLE_NAME) ,
		TABLE_SCHEMA+'.'+ TABLE_NAME 
	from INFORMATION_SCHEMA.TABLES
	where 
		TABLE_NAME!='sysdiagrams' AND
		TABLE_SCHEMA+'.'+TABLE_NAME NOT IN (SELECT TABLE_SCHEMA+'.'+TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME='UpdatedBy');

	select @i=0;
	while(@i<(select count(*) from @tables))
	begin
		select @i=@i+1;
		select @table=TABLE_NAME from @tables where ID=@i;
		select @command='ALTER TABLE '+@table+' ADD UpdatedBy bigint null;';
		print @command;
		EXEC sp_executesql @command;
	end;
	delete from @tables

END;

GO
