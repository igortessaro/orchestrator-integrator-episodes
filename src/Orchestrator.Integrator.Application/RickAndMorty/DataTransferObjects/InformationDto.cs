using System.Text.Json.Serialization;

namespace Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects
{
    public sealed class InformationDto
    {
        [JsonPropertyName("Count")]
        public short Count { get; set; }
        [JsonPropertyName("Pages")]
        public short Pages { get; set; }
        [JsonPropertyName("Next")]
        public Uri? NextPage { get; set; }
        [JsonPropertyName("Prev")]
        public Uri? PreviousPage { get; set; }
    }
}
