using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Interfacce
{
    public interface ICommentRepository
    {
        Task <List<Comment>> GetAllAsync();
        Task <Comment?> GetByIdAsync(int id); 
        Task <Comment> CreateAsync(Comment commentModel);
        Task <Comment?> UpdateAsync(int commentId, UpdateCommentRequestDTO commentDTO);
        Task<Comment?> DeleteAsync(int commentId);
    }
}