﻿using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        PageList<Post> GetPosts(PostQueryFilters filters);
        Task<Post> GetPost(int id);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int postId);


          
  }
}