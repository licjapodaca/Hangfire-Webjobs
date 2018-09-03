using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;

namespace Webjobs.API.Filters
{
	public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
	{
		public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
		{
			context.JobExpirationTimeout = TimeSpan.FromSeconds(10);
		}

		public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
		{
			//context.JobExpirationTimeout = TimeSpan.FromSeconds(10);
		}
	}
}
