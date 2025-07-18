
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using System.Net.Http.Json;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient
{
    private readonly HttpClient _httpClient;
    public ProductsMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ProductDTO?> GetProductByProductIDAsync(Guid userId)
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
}
