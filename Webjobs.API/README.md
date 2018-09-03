# Hangfire-Webjobs

Hangfire Webjobs for long running tasks

## Clean database objects

```sql
TRUNCATE TABLE [HangFire].[AggregatedCounter]
go
TRUNCATE TABLE [HangFire].[Counter]
go
TRUNCATE TABLE [HangFire].[JobParameter]
go
TRUNCATE TABLE [HangFire].[JobQueue]
go
TRUNCATE TABLE [HangFire].[List]
go
TRUNCATE TABLE [HangFire].[State]
go
DELETE FROM [HangFire].[Job]
go
DBCC CHECKIDENT ('[HangFire].[Job]', reseed, 0)
go
UPDATE [HangFire].[Hash] SET Value = 1 WHERE Field = 'LastJobId'
go
```
