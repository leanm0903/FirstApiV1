using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Apiv2.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastucture.Interfaces;
using SocialMedia.Infrastucture.Service;

namespace SocialMedia.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IUrlService _urlService;
        public PostController(IMapper map,IPostService service,IUrlService urlService)
        {
            _postService = service;
            _mapper = map;
            _urlService = urlService;
        }
        /// <summary>
        /// Devuelve todos los posts a partir de un filtro
        /// </summary>
        /// <param name="filter"> filters a aplicar </param>
        /// <returns> </returns>
        [HttpGet (Name =nameof(GetPosts))]
        //se utiliza para documentar la aplicacion con swagger 
        [ProducesResponseType((int)HttpStatusCode.OK,Type =typeof(ApiResponse<IEnumerable<PostDTO>>)) ]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery]PostQueryFilters filter)
        {
            var posts =  _postService.GetPosts(filter);
            var postsDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);
            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);
            var metaData = new MetaData()
            {
               TotalPages=posts.TotalPages,
               TotalCount=posts.TotalCount,
               CurrentPage=posts.CurrentPage,
               HasNextPage=posts.HasNextPage,
               PageSize=posts.PageSize,
               HasPreviousPage=posts.HasPreviousPage,
               NextUrlPage= _urlService.GetPaginationUrl(filter,Url.RouteUrl(nameof(GetPosts))) .ToString()
            };
            response.MetaData = metaData;
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
            return Ok(response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult>GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postsDTO = _mapper.Map<PostDTO>(post);
           var response = new ApiResponse<PostDTO>(postsDTO);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {

           var post = _mapper.Map<Post>(postDTO);
            await _postService.InsertPost(post);
            postDTO = _mapper.Map<PostDTO>(post);//se le asigna el id
            var response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        } 
        [HttpPut]
        public async Task<IActionResult>Put (PostDTO postDTO)
        {
            
            var post = _mapper.Map<Post>(postDTO);
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok();
        }
        [HttpDelete("{idPost}")]
        public async Task<IActionResult> Delete(int idPost)
        {
            var result = await _postService.DeletePost(idPost);
            var response = new ApiResponse<bool>(result);
            return Ok(response); 

        }




    }
}