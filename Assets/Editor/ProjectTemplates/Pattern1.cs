using System;
using System.Collections.Generic;
using UnityEditor;

namespace ProjectBlue.ProjectGenerator
{
    public static class Pattern1
    {
        private static float process = 0f;
        private const float totalProcessNum = 11f;
        
        public static void Run(string baseScriptPath, string nameSpaceName)
        {

            process = 0f;
            
            var generator = new AsmdefGenerator(baseScriptPath, nameSpaceName);
            
            // Model
            var modelGuid = Generate(generator, "Domain/Model", new List<string>());

            // Infrastructure
            var infraGuid = Generate(generator, "Infrastracture",  new List<string>());
            
            // DataStoreInterface
            var dataStoreInterfaceGuid = Generate(generator, "Data/Repository/DataStoreInterfaces", 
                new List<string>() {modelGuid, infraGuid});
            // ViewInterface
            var viewInterfaceGuid = Generate(generator, "Presentation/Presenter/ViewInterfaces",
                new List<string>() {modelGuid, infraGuid});
            // RepositoryInterface
            var repositoryInterfaceGuid = Generate(generator, "Domain/UseCase/RepositoryInterfaces",
                new List<string>() {modelGuid, infraGuid});
            // PresenterInterface
            var presenterInterfaceGuid = Generate(generator, "Domain/UseCase/PresenterInterfaces",
                new List<string>() {modelGuid, infraGuid});
            
            // DataStore
            var dataStoreGuid = Generate(generator, "Data/DataStore", 
                new List<string>() {modelGuid, infraGuid, dataStoreInterfaceGuid});
            
            // Repository
            var repositoryGuid = Generate(generator, "Data/Repository", 
                new List<string>() {modelGuid, infraGuid, dataStoreInterfaceGuid, repositoryInterfaceGuid});
            
            // View
            var viewGuid = Generate(generator, "Presentation/View",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid});

            // Presenter
            var presenterGuid = Generate(generator, "Presentation/Presenter",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid, presenterInterfaceGuid});

            // UseCase
            var useCaseGuid = Generate(generator, "Domain/UseCase",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid, presenterInterfaceGuid, repositoryInterfaceGuid});
            
            EditorUtility.ClearProgressBar();
        }

        private static string Generate(AsmdefGenerator generator, string directoryPath, List<string> referenceGuids)
        {
            var generated = generator.Generate(directoryPath, referenceGuids);
            process++;
            EditorUtility.DisplayProgressBar("Generating CA Project...", directoryPath, process/totalProcessNum);
            return generated;
        }
        
    }

}

