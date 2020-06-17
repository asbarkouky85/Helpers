
CREATE TABLE [dbo].[Localizables](
	[Id] [bigint] NOT NULL,
	[EntityId] [bigint] NOT NULL,
	[EntityType] [varchar](100) NOT NULL,
	[LocaleId] [int] NOT NULL,
	[ColumnName] [varchar](70) NULL,
	[Value] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_Localizables_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
