using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAcessLayer.Abstract;
using EntityLayer.Entities;
using EntityLayer.Paginate;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public async Task CreateAsync(Comment t)
        {
            await _commentDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            await _commentDal.DeleteAsync(Id);
        }

        public async Task<Comment> GetByIdAsync(string Id)
        {
           return await _commentDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<Comment>> GetListAsync(int size, int page)
        {
           return await _commentDal.GetListAsync(size, page);
        }

        public async Task<List<Comment>> GetListAsync()
        {
            return await _commentDal.GetListAsync();
        }

        public async Task UpdateAsync(Comment t, string Id)
        {
            await _commentDal.UpdateAsync(t, Id);
        }
    }
}
