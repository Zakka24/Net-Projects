using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.DTOs.Stock;
using api.Extensions;
using api.Interfacce;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        // Ritorna tutti i commenti
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var comments = await _commentRepo.GetAllAsync();
                var commentDto = comments.Select(c => c.ToCommentDTO());

                return Ok(commentDto);
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpGet("{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int commentId){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var comment = await _commentRepo.GetByIdAsync(commentId);

                if(comment == null){
                    return NotFound();
                }

                return Ok(comment.ToCommentDTO());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                if(!await _stockRepo.StockExists(stockId)){
                    return BadRequest("Stock does not exist");
                }

                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);

                var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
                commentModel.AppUserId = appUser.Id;
                
                await _commentRepo.CreateAsync(commentModel);

                return CreatedAtAction(nameof(GetById), new {commentId = commentModel.Id}, commentModel.ToCommentDTO());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPut("{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDTO commentDTO){
            try{
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepo.UpdateAsync(commentId, commentDTO);

            if(commentModel == null){
                return NotFound();
            }

            return Ok(commentModel.ToCommentDTO());
            }
            catch (Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpDelete("{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int commentId){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var commentModel = await _commentRepo.DeleteAsync(commentId);

                if(commentModel == null){
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