using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
     public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        
        public PostRepository(SocialMediaContext context):base(context)
        {

        }
        public async Task<IEnumerable<Post>> GetPostsByUser(int userID)
        {
            return await _entities.Where(x => x.UserId == userID).ToListAsync();
        }
    }
}
