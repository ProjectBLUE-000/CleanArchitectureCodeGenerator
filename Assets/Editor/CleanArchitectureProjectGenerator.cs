using System;
using UnityEditor;
using UnityEngine;

namespace ProjectBlue.Generator
{

    public class CleanArchitectureProjectGenerator : EditorWindow
    {
        
        [MenuItem("ProjectBLUE/Architecture/Clean Architecture Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CleanArchitectureProjectGenerator>();
            window.titleContent = new GUIContent("CA Generator");
            window.Show();
        }

        private ArchitectureType _archType = ArchitectureType.Pattern1;
        private string _baseScriptPath;
        private string _nameSpaceName;
        private string _className;
        
        private void OnGUI()
        {

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                _archType = (ArchitectureType)EditorGUILayout.EnumPopup("Architecture Type", _archType);
            }
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                _baseScriptPath = EditorGUILayout.TextField("Base script path", _baseScriptPath);
                _nameSpaceName = EditorGUILayout.TextField("Namespace name", _nameSpaceName);
                
                if (GUILayout.Button("Build Project"))
                {
                    GenerateAssemblyDefinitions();
                }
            }
            
            GUILayout.Space(10);
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {

                EditorGUI.BeginDisabledGroup(_archType == ArchitectureType.Pattern2);
                
                _className = EditorGUILayout.TextField("Class Name", _className);
            
                if (GUILayout.Button("Generate Codes"))
                {
                    GenerateCodes();
                }
                
                EditorGUI.EndDisabledGroup();
            }
            
            
        }

        private void OnEnable()
        {
            _archType = (ArchitectureType)Enum.ToObject(typeof(ArchitectureType), EditorPrefs.GetInt("CAGEN_ARCHTYPE", 0));
            _baseScriptPath = EditorPrefs.GetString("CAGEN_BASEPATH", "Assets/Scripts");
            _nameSpaceName = EditorPrefs.GetString("CAGEN_NAMESPACE", "ProjectBlue.ProductName");
            _className = EditorPrefs.GetString("CAGEN_CLASSNAME", "SampleClass");
        }

        private void OnDestroy()
        {
            Save();
        }

        private void Save()
        {
            EditorPrefs.SetInt("CAGEN_ARCHTYPE", (int)_archType);
            EditorPrefs.SetString("CAGEN_BASEPATH", _baseScriptPath);
            EditorPrefs.SetString("CAGEN_NAMESPACE", _nameSpaceName);
            EditorPrefs.SetString("CAGEN_CLASSNAME", _className);
        }

        private void GenerateAssemblyDefinitions()
        {
            Save();

            if (_archType == ArchitectureType.Pattern1)
            {
                ProjectGenerator.Pattern1.Run(_baseScriptPath, _nameSpaceName);
            }
            else
            {
                ProjectGenerator.Pattern2.Run(_baseScriptPath, _nameSpaceName);
            }
        }

        private void GenerateCodes()
        {
            
            Save();

            if (_archType == ArchitectureType.Pattern1)
            {
                CodeGenerator.Pattern1.Run(_baseScriptPath, _nameSpaceName, _className);
            }
            
        }

    }
}