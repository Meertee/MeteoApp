using MeteoApp.Core.Models;
using System.Threading.Tasks;
namespace MeteoApp.Core.Services
{
    public interface ILocationService
    {
        Task<Entry> GetCurrentLocationAsync();
    }
}
