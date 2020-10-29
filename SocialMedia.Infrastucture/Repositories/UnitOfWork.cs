using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext _context;
        //private readonly IRepository<Post> _postRepository; se cambia a un repositorio propio con metodos propios ,este se usaba de manera general  
        private readonly IPostRepository _postRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Comment> _commentRepository;

        public UnitOfWork(SocialMediaContext context)
        {
            _context = context;
        }
        public IPostRepository PostRepository => _postRepository ?? new PostRepository(_context);

        public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(_context);
        public IRepository<Comment> CommentRepository => _commentRepository ?? new BaseRepository<Comment>(_context);
        public void Dispose()
        {
            if (_context!=null)
            {
                _context.Dispose();
            }
        }

        public void saveChanges()
        {
            _context.SaveChanges();
        }

        public async Task saveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
