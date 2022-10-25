using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusalaTestAspNet.Shared.Models;
using System;

namespace MusalaTestAspNet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeripherialController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PeripherialController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Peripheral>>> Get()
        {
            var peripherials = await _context.Peripherals.ToListAsync();
            return Ok(peripherials);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Peripheral>> Get(int id)
        {
            var ph = await _context.Peripherals.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(ph);
        }


        [HttpPost]
        public async Task<ActionResult<Peripheral>> Post(Peripheral ph)
        {
            MyResponse<List<Peripheral>> orepsuesta = new MyResponse<List<Peripheral>>();

            try
            {
                if (_context.Peripherals.Where(p => p.GatewayId == ph.GatewayId).Count() >= 10)
                {
                    orepsuesta.Code = Code.Error;
                    orepsuesta.Message = "Peripheral list full";
                }
                else
                {
                    ph.Gateway = null;
                    _context.Peripherals.Add(ph);
                    await _context.SaveChangesAsync();
                    orepsuesta.Code = Code.Success;
                    orepsuesta.Message = "Peripheral inserted";
                }                
                orepsuesta.Data = await GetDBPeripherals(ph.GatewayId);
            }
            catch (Exception ex)
            {
                orepsuesta.Message = ex.Message;
            }
            return Ok(orepsuesta);

        }

        private async Task<List<Peripheral>> GetDBPeripherals(int gatewayID)
        {
            return await _context.Peripherals.Where(r => r.GatewayId == gatewayID).Include(w => w.Gateway).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Peripheral>> Put(Peripheral ph)
        {
            MyResponse<List<Peripheral>> orepsuesta = new MyResponse<List<Peripheral>>();
            try
            {
                _context.Entry(ph).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                orepsuesta.Code = Code.Success;
                orepsuesta.Message = "Peripheral updated";
                orepsuesta.Data = await GetDBPeripherals(ph.GatewayId);
            }
            catch (Exception ex)
            {
                orepsuesta.Message = ex.Message;
            }
            return Ok(orepsuesta);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Peripheral>> Delete(int Id)
        {
            MyResponse<List<Peripheral>> orepsuesta = new MyResponse<List<Peripheral>>();
            var item = await _context.Peripherals.FirstOrDefaultAsync(r => r.Id == Id);
            if (item == null)
            {
                orepsuesta.Code = Code.Error;
                orepsuesta.Message = "NotFound";
            }
            else
            {
                int gatewayID = item.GatewayId;
                try
                {
                    _context.Peripherals.Remove(item);
                    await _context.SaveChangesAsync();
                    orepsuesta.Code = Code.Success;
                    orepsuesta.Message = "Peripheral deleted";
                    orepsuesta.Data = await GetDBPeripherals(gatewayID);
                }
                catch (Exception ex)
                {
                    orepsuesta.Code = Code.Error;
                    orepsuesta.Message = ex.Message;
                    orepsuesta.Data = await GetDBPeripherals(gatewayID);
                }
            }            
            return Ok(orepsuesta);
        }
    }
}
