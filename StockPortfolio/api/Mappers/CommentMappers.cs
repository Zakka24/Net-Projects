using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel){
            return new CommentDTO{
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.AppUser.UserName,
                StockID = commentModel.StockID
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO commentDto, int stockId){
            return new Comment{
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockID = stockId
            };
        }
    }
}