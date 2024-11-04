using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.Interfacce;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if(commentModel == null){
                return null;
            }

            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int commentId, UpdateCommentRequestDTO commentDTO)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if(existingComment == null){
                return null;
            }

            existingComment.Title = commentDTO.Title;
            existingComment.Content = commentDTO.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}