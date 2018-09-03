using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webjobs.API.Services.Contracts
{
	public interface IJobService
	{
		Task<string> ExecuteAndDisconnectFromJobAsync(string importance);
	}
}
