

CREATE TABLE [dbo].[Attachments](
	[Id] [bigint] NOT NULL IDENTITY(1,1),
	[Description] [nvarchar](300) NULL,
	[EntityType] [varchar](70) NULL,
	[FileType] [varchar](50) NULL,
	[FilePath] [nvarchar](300) NULL,
	[EntityId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



