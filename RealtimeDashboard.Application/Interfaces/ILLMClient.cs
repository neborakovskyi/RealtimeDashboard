namespace RealtimeDashboard.Application.Interfaces;

    public interface ILLMClient
    {
        Task<string> CompleteAsync(string prompt,CancellationToken ct=default);
        IAsyncEnumerable<string> StreamAsync(string prompt,CancellationToken ct=default);
    }

