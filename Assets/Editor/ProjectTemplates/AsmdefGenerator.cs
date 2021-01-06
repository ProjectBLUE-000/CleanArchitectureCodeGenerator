using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProjectBlue.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace ProjectBlue.ProjectGenerator
{
    
    public class AsmdefGenerator
    {

        private string baseScriptPath;
        private string nameSpaceName;
        
        /// <summary>
        /// Initialize asmdef generator
        /// </summary>
        /// <param name="baseScriptPath">your script directory path</param>
        /// <param name="nameSpaceName">Add namespace prefix to filename</param>
        public AsmdefGenerator(string baseScriptPath, string nameSpaceName)
        {
            this.baseScriptPath = baseScriptPath;
            this.nameSpaceName = nameSpaceName;
        }
        
        /// <summary>
        /// Creates directory and asmdef for the directory.
        /// </summary>
        /// <param name="directoryPath">Directory path to generate</param>
        /// <param name="referenceGuids"></param>
        /// <returns>generated asmdef's GUID</returns>
        public string Generate(string directoryPath, List<string> referenceGuids)
        {
            
            directoryPath = directoryPath.EndsWith("/") ? directoryPath : $"{directoryPath}/";

            var fileName = directoryPath.Substring(0,directoryPath.Length-1).Replace("/", ".");

            // remove prefix for filename
            if (fileName.Split('_').Length == 2)
            {
                fileName = fileName.Split('_')[1];
            }

            fileName = string.IsNullOrEmpty(nameSpaceName) ? fileName : $"{nameSpaceName}.{fileName}";

            var folderPath = Path.GetDirectoryName(Path.Combine(baseScriptPath+"/", directoryPath));
            CreateFolder(folderPath);

            var assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, fileName+".asmdef"));
            File.WriteAllText(assetPath, new AsmdefAPI(fileName, referenceGuids).Generate());
            AssetDatabase.Refresh();
            
            return AssetDatabase.AssetPathToGUID(assetPath);
        }
        
        public static void CreateFolder(string path)
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

