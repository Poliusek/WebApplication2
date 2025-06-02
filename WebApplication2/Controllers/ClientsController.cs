using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientsService;

        public ClientsController(IClientService clientsService)
        {
            _clientsService = clientsService;
        }
        
        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var result = await _clientsService.DeleteClient(idClient);
            if (!result)
                return BadRequest("Błąd request. Client not found or has associated trips.");
            return Ok("Deleted client successfully.");
        }
    }
}
