using Newtonsoft.Json;
using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public sealed class FindEpisodeService : IFindEpisodeService
    {
        private readonly HttpClient _httpClient;
        private string _resource;

        public FindEpisodeService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._resource = configuration["RickAndMortyApi:EpisodeResource"];
        }

        public async Task<IReadOnlyCollection<EpisodeDto>> GetAll()
        {
            using var responseMessage = await _httpClient.GetAsync(this._resource);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Unexpected error when calling the service GET {_httpClient.BaseAddress}/{_resource} with Http Status Code ${responseMessage.StatusCode}");
            }

            var response = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponseDto<EpisodeDto>>(response);

            return result != null ? result.Results : new List<EpisodeDto>();
        }
    }
}
