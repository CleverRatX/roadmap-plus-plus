using System.Text.RegularExpressions;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    internal static partial class ModuleSchema
    {
        public static string Of( Type contextType )
        {
            string assembly = contextType.Assembly.GetName().Name!;

            Match match = ModuleAssemblyName().Match( assembly );
            if ( !match.Success )
            {
                throw new InvalidOperationException( $"{contextType.Name} лежит в сборке {assembly}, а не в модуле RoadmapPlusPlus.Modules.*: схема БД неизвестна." );
            }

            return match.Groups[ "module" ].Value.ToLowerInvariant();
        }

        [GeneratedRegex( @"^RoadmapPlusPlus\.Modules\.(?<module>\w+)$" )]
        private static partial Regex ModuleAssemblyName();
    }
}
