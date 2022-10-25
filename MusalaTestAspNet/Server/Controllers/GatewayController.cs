using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusalaTestAspNet.Shared.Models;

namespace MusalaTestAspNet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GatewayController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Gateway>>> Get()
        {
            var gateways = await _context.Gateways.ToListAsync();
            return Ok(gateways);
        }

        [HttpGet("getperipherials/{id}")]
        public async Task<ActionResult<List<Peripheral>>> GetPeripherials(int id)
        {
            var periphs = await _context.Peripherals.Where(g => g.GatewayId == id).Include(w => w.Gateway).ToListAsync();
            return Ok(periphs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> Get(int id)
        {
            var gtw = await _context.Gateways.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(gtw);
        }

        [HttpPost]
        public async Task<ActionResult<Gateway>> Post(Gateway gateway)
        {
            MyResponse<List<Gateway>> orepsuesta = new MyResponse<List<Gateway>>();

            try
            {
                _context.Gateways.Add(gateway);
                await _context.SaveChangesAsync();
                orepsuesta.Code = Code.Success;
                orepsuesta.Message = "Gateway inserted";
                orepsuesta.Data = await GetDBGatways();
            }
            catch (Exception ex)
            {
                orepsuesta.Message = ex.Message;
                orepsuesta.Code = Code.Error;
                orepsuesta.Data = await GetDBGatways();
            }
            return Ok(orepsuesta);

        }

        private async Task<List<Gateway>> GetDBGatways()
        {
            return await _context.Gateways.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Gateway>> Put(Gateway warehouse)
        {
            MyResponse<List<Gateway>> orepsuesta = new MyResponse<List<Gateway>>();
            try
            {
                _context.Entry(warehouse).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                orepsuesta.Code = Code.Success;
                orepsuesta.Message = "Gateway updated";
                orepsuesta.Data = await GetDBGatways();
            }
            catch (Exception ex)
            {
                orepsuesta.Message = ex.Message;
            }
            return Ok(orepsuesta);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Gateway>> Delete(int Id)
        {
            MyResponse<List<Gateway>> orepsuesta = new MyResponse<List<Gateway>>();
            var item = await _context.Gateways.FirstOrDefaultAsync(r => r.Id == Id);
            if (item == null)
            {
                orepsuesta.Code = Code.Error;
                orepsuesta.Message = "NotFound";
            }
            else
            {
                try
                {
                    _context.Gateways.Remove(item);
                    await _context.SaveChangesAsync();
                    orepsuesta.Code = Code.Success;
                    orepsuesta.Message = "Gateway deleted";
                    orepsuesta.Data = await GetDBGatways();
                }
                catch (Exception ex)
                {
                    orepsuesta.Code = Code.Error;
                    orepsuesta.Message = ex.Message;
                }
            }
            return Ok(orepsuesta);
        }
    }
}
