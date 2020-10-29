using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastucture.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastucture.Service
{
    public class UrlService : IUrlService
    {
        private readonly string _baseUrl;
        public UrlService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Uri GetPaginationUrl(PostQueryFilters filters, string actionrl)
        {
            string baseUri = $"{_baseUrl}{actionrl}";
            return new Uri(baseUri);
        }
    }
}
