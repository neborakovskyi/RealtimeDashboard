using RealtimeDashboard.Application.Interfaces;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RealtimeDashboard.Infrastructure.Services
{
    public class OllamaClient(HttpClient http, string model = "llama3.2") : ILLMClient
    {
        public async Task<string> CompleteAsync(string prompt, CancellationToken ct = default)
        {
            var body = new
            {
                model = model,
                prompt = prompt,
                stream = false
            };

            var response = await http.PostAsJsonAsync("api/generate", body, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>(ct);

            return result.GetProperty("response").GetString() ?? string.Empty;
        }

        public async IAsyncEnumerable<string> StreamAsync(string prompt, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var body = new
            {
                model = model,
                prompt,
                stream = true
            };

            var response = await http.PostAsJsonAsync("api/generate", body, ct);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(ct);
                if (line is null)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var json = JsonSerializer.Deserialize<JsonElement>(line);
                yield return json.GetProperty("response").GetString() ?? string.Empty;

                if (json.TryGetProperty("done", out var done) && done.GetBoolean())
                {
                    break;
                }
            }
        }
    }
}
