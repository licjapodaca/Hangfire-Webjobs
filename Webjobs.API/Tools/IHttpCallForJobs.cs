using System.Threading.Tasks;

namespace Webjobs.API.Tools
{
	public interface IHttpCallForJobs
	{
		Task<string> GenerarMensaje(string importance);
		Task GenerarMensajeHigh();
		Task GenerarMensajeLow();
		Task GenerarMensajeNormal();
	}
}