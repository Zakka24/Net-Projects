using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfacce;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio(){
            try{
                var username = User.GetUsername();
                var AppUser = await _userManager.FindByNameAsync(username);
                var userPortfolio = await _portfolioRepo.GetUserPortfolio(AppUser);

                return Ok(userPortfolio);
            }
            catch(Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol){
            try{
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                var stock = await _stockRepo.GetBySymbolAsync(symbol);
    
                if(stock == null){
                    return BadRequest("Stock not found");
                }

                var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
                if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())){
                    return BadRequest("Cannot add same stock to portfolio");
                }

                var portfolioModel = new Portfolio{
                    StockId = stock.Id,
                    AppUserId = appUser.Id
                };

                await _portfolioRepo.CreateAsync(portfolioModel);
                if(portfolioModel == null){
                    return StatusCode(500, "Could not create");
                }
                
                return Created();
            }
            catch(Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol){
            try{
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                
                var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

                var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

                if(filteredStock.Count() == 1){
                    await _portfolioRepo.DeletePortfolioAsync(appUser, symbol);
                }
                else{
                    return BadRequest("Stock is not in your portfolio");
                }

                return NoContent();
            }
            catch(Exception e){
                return StatusCode(500, e);
            }
        }
    }
}