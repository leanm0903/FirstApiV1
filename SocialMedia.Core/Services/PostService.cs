using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        //private readonly IPostRepository _postRepository; se cambia ea uno generico
        //private readonly IUserRepository _userRepository;

        //private readonly IRepository<Post> _postRepository;se cambia por un UNitOfWork (engloba todos los repositorios)
        //private readonly IRepository<User> _userRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOption _pagination;

        public PostService(IUnitOfWork unitOfWork,IOptions<PaginationOption> options)
        {
            _unitOfWork = unitOfWork;
            _pagination = options.Value;
        }

        public async Task<bool> DeletePost(int postId)
        {
            await _unitOfWork.PostRepository.Delete(postId);
            return true;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public  PageList<Post> GetPosts(PostQueryFilters filters)
        {
            filters.pageNumber = filters.pageNumber == 0 ? _pagination.DefaultPageNumber : filters.pageNumber;
            filters.pageSize = filters.pageSize == 0 ? _pagination.DefaultPageSize : filters.pageSize;


            var posts = _unitOfWork.PostRepository.GetAll();
            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description));
            }
            var pagePOsts = PageList<Post>.Create(posts, filters.pageNumber, filters.pageSize);
            return pagePOsts;

        }

        public async Task InsertPost(Post post)
         {
            var user = await _unitOfWork.PostRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessExceptions("el usuario no es valido");
            }
            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(user.Id);
            if(userPost.Count()<10)
            {
                var lastPost = userPost.LastOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                    throw new BusinessExceptions("el Usuario no tiene permito publicar");
            }
            if(post.Description.Contains("sexo"))
            {
                throw new BusinessExceptions("the description contain sexo");
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.saveChangesAsync();
        }

        public async Task <bool> UpdatePost(Post post)

        {
             _unitOfWork.PostRepository.Update(post);
             await _unitOfWork.saveChangesAsync();
             return true;

        }
    }
}
