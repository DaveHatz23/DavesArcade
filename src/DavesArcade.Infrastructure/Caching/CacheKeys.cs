namespace DavesArcade.Infrastructure.Caching;

internal static class CacheKeys
{
    internal const string AllGames = "games:all";
    internal static string GameById(Guid id) => $"games:{id}";
}
