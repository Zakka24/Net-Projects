using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfacce;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        // Ritorna tutte le stock
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var stocks = await _stockRepo.GetAllAsync(query);
                var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

                return Ok(stockDto);
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        // Ritorna lo stock che ha un certo ID
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                // Cerco una certa Stock nel mio database
                var stock = await _stockRepo.GetByIdAsync(id);

                // Se non la trovo ritorno 'null' altrimenti ritorno la stock
                if(stock == null){
                    return NotFound();
                }

                return Ok(stock.ToStockDto());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var stockModel = stockDto.ToStockFromCreateDTO();
                await _stockRepo.CreateAsync(stockModel);
                
                return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPut("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int stockId, [FromBody] UpdateStockRequestDTO updateDTO){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var stockModel = await _stockRepo.UpdateAsync(stockId, updateDTO);

                if(stockModel == null){
                    return NotFound();
                }

                return Ok(stockModel.ToStockDto());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpDelete("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int stockId){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var stockModel = await _stockRepo.DeleteAsync(stockId);

                if(stockModel == null){
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }
    }
}