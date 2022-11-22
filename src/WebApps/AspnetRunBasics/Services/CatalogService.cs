﻿using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CatalogService> _logger;
        public CatalogService(HttpClient client, ILogger<CatalogService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJson($"/Catalog", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadContentAs<CatalogModel>();
            } 
            else
            {
                throw new Exception("Something went wrong when calling the API.");
            }
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            _logger.LogInformation("Getting Catalog products from url: {url}", _client.BaseAddress);

            var response = await _client.GetAsync("/Catalog");
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");
            return await response.ReadContentAs<List<CatalogModel>>();
        }
    }
}
