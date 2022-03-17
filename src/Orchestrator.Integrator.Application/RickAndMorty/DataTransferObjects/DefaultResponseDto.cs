using System.Text.Json.Serialization;

namespace Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects
{
    public sealed class DefaultResponseDto<TData>
    {
        public DefaultResponseDto()
        {
            this.Results = new List<TData>();
        }

        [JsonPropertyName("Info")]
        public InformationDto? Information { get; set; }
        [JsonPropertyName("Results")]
        public IReadOnlyCollection<TData> Results { get; set; }
    }
}
