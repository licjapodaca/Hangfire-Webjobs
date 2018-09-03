using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webjobs.API.Repositories.Contracts
{
	public interface IJobRepository
	{
		Task ExecuteAndDisconnectFromJobAsync();
	}
}
