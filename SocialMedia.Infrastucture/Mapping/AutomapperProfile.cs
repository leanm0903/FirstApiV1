using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastucture.Mapping
{
    class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDTO>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<PostDTO, Post>();


        }
    }
}
