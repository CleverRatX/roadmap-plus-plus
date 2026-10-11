namespace RoadmapPlusPlus.ArchitectureTests
{
    /// <summary>
    /// Разрешённые зависимости между модулями из ADR-0002: модуль может ссылаться только на Contracts перечисленных модулей.
    /// Меняется вместе с ADR.
    /// </summary>
    internal static class ModuleGraph
    {
        public static IReadOnlyDictionary<string, string[]> AllowedDependencies { get; } = new Dictionary<string, string[]>
        {
            [ "Identity" ] = [],
            [ "Planning" ] = [ "Identity" ],
            [ "Knowledge" ] = [ "Identity", "Planning" ],
            [ "Interchange" ] = [ "Identity", "Planning", "Knowledge" ],
            [ "Assistant" ] = [ "Identity", "Planning", "Interchange" ],
            [ "Notifications" ] = [ "Identity", "Planning" ],
        };
    }
}
