SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertMsmqSubscription]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[InsertMsmqSubscription](
   @subscriptionId uniqueidentifier,
   @host nvarchar(255),
   @workflowInstance uniqueidentifier,
   @msmqQueue nvarchar(2048),
   @wfQueueName varbinary(1024)
)
as
begin
   insert into MsmqSubscriptions values (
      @subscriptionId,
      @host,
      getdate(),
      @workflowInstance,
      @msmqQueue,
      @wfQueueName
   )
end
   
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllMsmqSubscriptions]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[GetAllMsmqSubscriptions] (
   @host nvarchar(255)
)
as
begin
   select 
	   SubscriptionId,
	   DateCreated,
	   WorkflowInstance,
	   MsmqQueue,
	   WfQueueName
	from MsmqSubscriptions
   where Host = @host
end

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveMsmqSubscription]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[RemoveMsmqSubscription](
	@subscriptionId uniqueidentifier
)
as
begin
	delete MsmqSubscriptions 
   where SubscriptionId = @subscriptionId
end

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MsmqSubscriptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MsmqSubscriptions](
	[SubscriptionId] [uniqueidentifier] NOT NULL,
	[Host] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[WorkflowInstance] [uniqueidentifier] NOT NULL,
	[MsmqQueue] [nvarchar](2048) NOT NULL,
	[WfQueueName] [varbinary](1024) NOT NULL,
 CONSTRAINT [PK__MsmqSubscription__04E4BC85] PRIMARY KEY CLUSTERED 
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE NONCLUSTERED INDEX [IX_MsmqSubscriptions_Host] ON [dbo].[MsmqSubscriptions] 
(
	[Host] ASC
)WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
END
