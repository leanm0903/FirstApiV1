using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infrastucture.Interfaces
{ 
    public interface IUrlService
    {
        Uri GetPaginationUrl(PostQueryFilters filters, string actionrl);
    }
}