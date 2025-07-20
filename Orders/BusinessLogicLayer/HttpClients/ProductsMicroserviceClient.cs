

using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;
using System.Net.Http.Json;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductsMicroserviceClient> _logger;
    public ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    public async Task<ProductDTO?> GetProductByProductIDAsync(Guid userId)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/products/search/product-id/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException($"Bad request", null, System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    throw new HttpRequestException($"Bad request", null, System.Net.HttpStatusCode.BadRequest);
                }
            }

            ProductDTO? user = await response.Content.ReadFromJsonAsync<ProductDTO>();

            if (user == null)
            {
                throw new HttpRequestException($"Invalid Product ID", null);
            }

            return user;
        }
        catch(BulkheadRejectedException ex)
        {
            _logger.LogError(ex, "Bulkhead isolation bloacks the reuquest since the request queue is full");

            return new ProductDTO(
                ProductID: Guid.Empty,
                ProductName: "Temporarily Unavailable (Bulkhead)",
                Category: "Temporarily Unavailable (Bulkhead)",
                UnitPrice: 0,
                QuantityInStock: 0
            );
        }
        
    }
}
