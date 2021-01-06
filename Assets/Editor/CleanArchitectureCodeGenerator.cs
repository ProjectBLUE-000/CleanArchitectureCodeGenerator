using System.IO;
using ProjectBlue.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace Editor
{

    public enum ArchitectureType
    {
        Pattern1, Pattern2
    }
    
    public class CleanArchitectureCodeGenerator : EditorWindow
    {

        [MenuItem("ProjectBLUE/Architecture/Clean Architecture Code Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CleanArchitectureCodeGenerator>();
            window.titleContent = new GUIContent("CA Code Generator");
            window.Show();
        }

        private string baseScriptPath = "Assets/Scripts";
        private string className = "SampleClass";
        private string nameSpace = "SampleNamespace";
        private ArchitectureType archType = ArchitectureType.Pattern1;
        
        private void OnGUI()
        {

            baseScriptPath = EditorGUILayout.TextField("Base Script Path", baseScriptPath);
            nameSpace = EditorGUILayout.TextField("Namespace", nameSpace);
            className = EditorGUILayout.TextField("Class Name", className);

            archType = (ArchitectureType)EditorGUILayout.EnumPopup("Architecture Type", archType);

            if (GUILayout.Button("Generate"))
            {
                GenerateCodes();
            }

        }

        private int step = 0;
        
        private void GenerateCodes()
        {

            if (archType == ArchitectureType.Pattern1)
            {
                Pattern1();
            }
            else
            {
                Pattern2();
            }
            
        }

        private void Pattern1()
        {
            step = 0;
            
            // Model
            Generate(new ModelTemplate(nameSpace, className));
            
            // View
            Generate(new ViewTemplate(nameSpace, className));
            
            // Presenter
            Generate(new ViewInterfaceTemplate(nameSpace, className));
            Generate(new PresenterTemplate(nameSpace, className));
            
            // DataStore
            Generate(new DataStoreTemplate(nameSpace, className));
            
            // Repository
            Generate(new DataStoreInterfaceTemplate(nameSpace, className));
            Generate(new RepositoryTemplate(nameSpace, className));
            
            // UseCase
            Generate(new RepositoryInterfaceTemplate(nameSpace, className));
            Generate(new PresenterInterfaceTemplate(nameSpace, className));
            Generate(new UseCaseTemplate(nameSpace, className));
            
            EditorUtility.ClearProgressBar();
        }

        private void Pattern2()
        {
            
        }

        private void Generate(CodeTemplateBase codeTemplate)
        {
            EditorUtility.DisplayProgressBar ("Generating CA codes...", codeTemplate.FileName, step/10f);
            
            var folderPath = Path.GetDirectoryName(Path.Combine(baseScriptPath+"/", codeTemplate.FolderPath));
            CreateFolder(folderPath);
            
            var assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, codeTemplate.FileName));
            File.WriteAllText(assetPath, codeTemplate.GetCode());
            AssetDatabase.Refresh();
            
            step++;
        }

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