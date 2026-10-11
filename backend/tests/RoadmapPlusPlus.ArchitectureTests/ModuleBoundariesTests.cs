using System.Reflection;
using RoadmapPlusPlus.BuildingBlocks;

namespace RoadmapPlusPlus.ArchitectureTests
{
    public sealed class ModuleBoundariesTests
    {
        [Fact]
        public void BuildingBlocksReferencesNoProjects()
        {
            BackendProject buildingBlocks = BackendProjects.All.Single( p => p.Kind == ProjectKind.BuildingBlocks );

            AssertNoViolations( buildingBlocks.References.Select( r => $"{buildingBlocks.Name} -> {r}" ) );
        }

        [Fact]
        public void EveryModuleIsInGraph()
        {
            AssertNoViolations( BackendProjects.All
                .Where( p => p.Module is not null && !ModuleGraph.AllowedDependencies.ContainsKey( p.Module ) )
                .Select( p => $"{p.Name}: module {p.Module} is missing in {nameof( ModuleGraph )}" ) );
        }

        [Fact]
        public void ModulesReferenceOnlyBuildingBlocksAndAllowedContracts()
        {
            AssertNoViolations( BackendProjects.All
                .Where( p => p.Kind is ProjectKind.Module or ProjectKind.Contracts )
                .SelectMany( p => p.References
                    .Where( r => !IsAllowedReference( p, r ) )
                    .Select( r => $"{p.Name} -> {r}" ) ) );
        }

        [Fact]
        public void ModuleGraphHasNoCycles()
        {
            AssertNoViolations( ModuleGraph.AllowedDependencies.Keys
                .Where( DependsOnItself )
                .Select( m => $"{m} depends on itself" ) );
        }

        [Fact]
        public void ApiReferencesEveryModule()
        {
            BackendProject api = BackendProjects.All.Single( p => p.Kind == ProjectKind.Bootstrapper );

            AssertNoViolations( BackendProjects.All
                .Where( p => p.Kind == ProjectKind.Module )
                .Select( p => p.Name )
                .Except( api.References )
                .Select( name => $"{api.Name} does not reference {name}" ) );
        }

        [Fact]
        public void ModulesExposeOnlyModuleClass()
        {
            List<string> violations = [];

            foreach ( BackendProject project in BackendProjects.All.Where( p => p.Kind == ProjectKind.Module ) )
            {
                Type[] exported = Assembly.Load( project.Name ).GetExportedTypes();
                Type[] moduleClasses = exported.Where( t => typeof( IModule ).IsAssignableFrom( t ) ).ToArray();

                if ( moduleClasses.Length != 1 )
                {
                    violations.Add( $"{project.Name}: {moduleClasses.Length} public {nameof( IModule )} types, expected 1" );
                }

                violations.AddRange( exported.Except( moduleClasses ).Select( t => $"{project.Name}: public {t.FullName}" ) );
            }

            AssertNoViolations( violations );
        }

        private static void AssertNoViolations( IEnumerable<string> violations )
        {
            List<string> list = violations.ToList();
            if ( list.Count > 0 )
            {
                Assert.Fail( string.Join( Environment.NewLine, list ) );
            }
        }

        private static bool IsAllowedReference( BackendProject project, string reference )
        {
            if ( reference == BackendProjects.BuildingBlocks )
            {
                return true;
            }

            string module = project.Module!;
            if ( project.Kind == ProjectKind.Module && reference == BackendProjects.ContractsOf( module ) )
            {
                return true;
            }

            return ModuleGraph.AllowedDependencies.GetValueOrDefault( module, [] )
                .Any( dependency => reference == BackendProjects.ContractsOf( dependency ) );
        }

        private static bool DependsOnItself( string module )
        {
            HashSet<string> visited = [];
            Queue<string> queue = new( ModuleGraph.AllowedDependencies[ module ] );

            while ( queue.TryDequeue( out string? next ) )
            {
                if ( next == module )
                {
                    return true;
                }

                if ( visited.Add( next ) )
                {
                    foreach ( string dependency in ModuleGraph.AllowedDependencies.GetValueOrDefault( next, [] ) )
                    {
                        queue.Enqueue( dependency );
                    }
                }
            }

            return false;
        }
    }
}
