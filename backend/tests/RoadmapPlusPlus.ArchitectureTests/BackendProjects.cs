using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RoadmapPlusPlus.ArchitectureTests
{
    /// <summary>
    /// Проекты <c>backend/src</c> и их <c>ProjectReference</c>, прочитанные из .csproj.
    /// Скомпилированные сборки для графа не годятся: компилятор выбрасывает ссылки на сборки, типы которых не используются.
    /// </summary>
    internal static partial class BackendProjects
    {
        public const string BuildingBlocks = "RoadmapPlusPlus.BuildingBlocks";

        public const string Api = "RoadmapPlusPlus.Api";

        public static IReadOnlyList<BackendProject> All { get; } = Load();

        public static string ContractsOf( string module )
        {
            return $"RoadmapPlusPlus.Modules.{module}.Contracts";
        }

        private static List<BackendProject> Load()
        {
            string src = Path.Combine( FindBackendRoot(), "src" );

            return Directory.EnumerateFiles( src, "*.csproj", SearchOption.AllDirectories )
                .Select( Parse )
                .OrderBy( p => p.Name, StringComparer.Ordinal )
                .ToList();
        }

        private static BackendProject Parse( string csprojPath )
        {
            string name = Path.GetFileNameWithoutExtension( csprojPath );

            List<string> references = XDocument.Load( csprojPath )
                .Descendants( "ProjectReference" )
                .Select( e => Path.GetFileNameWithoutExtension( ( ( string )e.Attribute( "Include" )! ).Replace( '\\', '/' ) ) )
                .ToList();

            if ( name == BuildingBlocks )
            {
                return new BackendProject( name, ProjectKind.BuildingBlocks, null, references );
            }

            if ( name == Api )
            {
                return new BackendProject( name, ProjectKind.Bootstrapper, null, references );
            }

            Match match = ModuleProjectName().Match( name );
            if ( !match.Success )
            {
                throw new InvalidOperationException( $"Неизвестный вид проекта: {csprojPath}. Добавьте его в {nameof( BackendProjects )}." );
            }

            ProjectKind kind = match.Groups[ "contracts" ].Success ? ProjectKind.Contracts : ProjectKind.Module;
            return new BackendProject( name, kind, match.Groups[ "module" ].Value, references );
        }

        private static string FindBackendRoot()
        {
            DirectoryInfo? dir = new( AppContext.BaseDirectory );
            while ( dir is not null && !File.Exists( Path.Combine( dir.FullName, "RoadmapPlusPlus.slnx" ) ) )
            {
                dir = dir.Parent;
            }

            return dir?.FullName ?? throw new InvalidOperationException( $"RoadmapPlusPlus.slnx не найден выше {AppContext.BaseDirectory}." );
        }

        [GeneratedRegex( @"^RoadmapPlusPlus\.Modules\.(?<module>\w+)(?<contracts>\.Contracts)?$" )]
        private static partial Regex ModuleProjectName();
    }
}
