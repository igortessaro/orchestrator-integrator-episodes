using System.Text.Json.Serialization;

namespace Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects
{
    public sealed class EpisodeDto
    {
        public EpisodeDto()
        {
            this.Characters = new List<Uri>();
        }

        [JsonPropertyName("Id")]
        public short Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("AirDate")]
        public string AirDate { get; set; } = string.Empty;
        [JsonPropertyName("Episode")]
        public string Episode { get; set; } = string.Empty;
        [JsonPropertyName("Characters")]
        public IReadOnlyCollection<Uri> Characters { get; set; }
        [JsonPropertyName("Url")]
        public Uri? Url { get; set; }
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }
    }
}