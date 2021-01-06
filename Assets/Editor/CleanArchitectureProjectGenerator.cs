using System;
using UnityEditor;
using UnityEngine;

namespace ProjectBlue.ProjectGenerator
{
    
    public enum ArchitectureType
    {
        Pattern1, Pattern2
    }
    
    public class CleanArchitectureProjectGenerator : EditorWindow
    {
        
        [MenuItem("ProjectBLUE/Architecture/Clean Architecture Project Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CleanArchitectureProjectGenerator>();
            window.titleContent = new GUIContent("CA Project Generator");
            window.Show();
        }

        private int step = 0;
        
        private string baseScriptPath = "Assets/Scripts";
        private string nameSpaceName = "ProjectBlue.Product";
        private ArchitectureType archType = ArchitectureType.Pattern1;

        private void OnGUI()
        {

            baseScriptPath = EditorGUILayout.TextField("Base script path", baseScriptPath);
            nameSpaceName = EditorGUILayout.TextField("Namespace name", nameSpaceName);
            archType = (ArchitectureType)EditorGUILayout.EnumPopup("Architecture Type", archType);
            
            if (GUILayout.Button("Generate"))
            {
                GenerateAssemblyDefinitions();
            }
            
        }

        private void GenerateAssemblyDefinitions()
        {

            if (archType == ArchitectureType.Pattern1)
            {
                Pattern1.Run(baseScriptPath, nameSpaceName);
            }
        }
        
        

        
        
        
        
        
       
        
    }
}