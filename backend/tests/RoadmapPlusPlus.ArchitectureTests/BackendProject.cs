namespace RoadmapPlusPlus.ArchitectureTests
{
    /// <summary>
    /// Проект из <c>backend/src</c>. <see cref="Module"/> — имя модуля для <see cref="ProjectKind.Module"/>
    /// и <see cref="ProjectKind.Contracts"/>, иначе <see langword="null"/>.
    /// </summary>
    internal sealed record BackendProject( string Name, ProjectKind Kind, string? Module, IReadOnlyList<string> References );
}
