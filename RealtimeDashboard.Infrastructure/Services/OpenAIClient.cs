using RealtimeDashboard.Application.Interfaces;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace RealtimeDashboard.Infrastructure.Services;

public class OpenAIClient(HttpClient http, string model = "gpt-4o-mini") : ILLMClient
{
    public async Task<string> CompleteAsync(string prompt, CancellationToken ct = default)
    {
        var body = new
        {
            model = model,
            messages = new[] { new { role = "user", content = prompt } }
        };

        var response = await http.PostAsJsonAsync("chat/completions", body, ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>(ct);

        return result
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;
    }

    public async IAsyncEnumerable<string> StreamAsync(string prompt, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var body = new
        {
            model = model,
            stream = true,
            message = new[] { new { role = "user", content = prompt } }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };

        var response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct);

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var reader = new StreamReader(stream);

        while (!ct.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(ct);
            if (line is null)
            {
                break;
            }

            if (string.IsNullOrEmpty(line) || !line.StartsWith("data: "))
            {
                continue;
            }

            if (line == "data: [DONE]")
            {
                break;
            }

            var json = JsonSerializer.Deserialize<JsonElement>(line[6..]);
            var delta = json
                .GetProperty("choices")[0]
                .GetProperty("delta");

            if (delta.TryGetProperty("content", out var content))
            {
                yield return content.GetString() ?? string.Empty;
            }
        }
    }
}

