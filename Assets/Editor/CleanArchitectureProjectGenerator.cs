using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CleanArchitectureProjectGenerator : EditorWindow
    {
        
        [MenuItem("stu/Clean Architecture Project Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CleanArchitectureProjectGenerator>();
            window.titleContent = new GUIContent("Clean Architecture Project Generator");
            window.Show();
        }

        private int step = 0;
        
        private string baseScriptPath = "Assets/Scripts";
        private string nameSpaceName = "ProjectBlue.Product";

        private void OnGUI()
        {

            baseScriptPath = EditorGUILayout.TextField("Base script path", baseScriptPath);
            nameSpaceName = EditorGUILayout.TextField("Namespace name", nameSpaceName);
            
            if (GUILayout.Button("Generate"))
            {
                GenerateAssemblyDefinitions();
            }
            
        }

        private void GenerateAssemblyDefinitions()
        {
            
            // Model
            var modelGuid = Generate("Domain/Model", new List<string>());
            
            // Infrastructure
            var infraGuid = Generate("Infrastracture",  new List<string>());
            
            // DataStoreInterface
            var dataStoreInterfaceGuid = Generate("Repository/DataStoreInterfaces", 
                new List<string>() {modelGuid, infraGuid});
            // ViewInterface
            var viewInterfaceGuid = Generate("Presentation/Presenter/ViewInterfaces",
                new List<string>() {modelGuid, infraGuid});
            // RepositoryInterface
            var repositoryInterfaceGuid = Generate("Domain/UseCase/RepositoryInterfaces",
                new List<string>() {modelGuid, infraGuid});
            // PresenterInterface
            var presenterInterfaceGuid = Generate("Domain/UseCase/PresenterInterfaces",
                new List<string>() {modelGuid, infraGuid});
            
            // DataStore
            var dataStoreGuid = Generate("Data/DataStore", 
                new List<string>() {modelGuid, infraGuid, dataStoreInterfaceGuid});
            
            // Repository
            var repositoryGuid = Generate("Data/Repository", 
                new List<string>() {modelGuid, infraGuid, dataStoreInterfaceGuid, repositoryInterfaceGuid});
            
            // View
            var viewGuid = Generate("Presentation/View",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid});

            // Presenter
            var presenterGuid = Generate("Presentation/Presenter",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid, presenterInterfaceGuid});

            // UseCase
            var useCaseGuid = Generate("Domain/UseCase",
                new List<string>() {modelGuid, infraGuid, viewInterfaceGuid, presenterInterfaceGuid, repositoryInterfaceGuid});
            

            EditorUtility.ClearProgressBar();
        }

        private string Generate(string directoryPath, List<string> referenceGuids)
        {
            
            directoryPath = directoryPath.EndsWith("/") ? directoryPath : $"{directoryPath}/";

            var fileName = directoryPath.Substring(0,directoryPath.Length-1).Replace("/", ".");
            Debug.Log(fileName);

            fileName = string.IsNullOrEmpty(nameSpaceName) ? fileName : $"{nameSpaceName}.{fileName}";
            
            EditorUtility.DisplayProgressBar ("Generating CA project...", fileName, step/11f);

            var folderPath = Path.GetDirectoryName(Path.Combine(baseScriptPath+"/", directoryPath));
            CreateFolder(folderPath);

            var referenceGuidStr = "";
            for (var i = 0; i < referenceGuids.Count; i++)
            {
                referenceGuidStr += $"\"GUID:{referenceGuids[i]}\"";
                if (i != referenceGuids.Count - 1) referenceGuidStr += ",\n";
            }
            
            var generatedText = asmdefTemplate.Replace("##FILE_NAME##", fileName).Replace("##REFERENCE##",referenceGuidStr);
            
            var assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, fileName+".asmdef"));
            File.WriteAllText(assetPath, generatedText);
            AssetDatabase.Refresh();

            step++;
            
            return AssetDatabase.AssetPathToGUID(assetPath);
        }
        
        
        private string asmdefTemplate = @"
{
    ""name"": ""##FILE_NAME##"",
    ""references"": [
        ##REFERENCE##
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": false,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}
";
        
        private static void CreateFolder(string path)
        {
            var target = "";
            var splitChars = new char[]{ Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
            foreach (var dir in path.Split(splitChars)) {
                var parent = target;
                target = Path.Combine(target, dir);
                if (!AssetDatabase.IsValidFolder(target)) {
                    AssetDatabase.CreateFolder(parent, dir);
                }
            }
        }
        
    }
}