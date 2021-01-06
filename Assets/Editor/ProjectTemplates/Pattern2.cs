using System;
using System.Collections.Generic;
using UnityEditor;

namespace ProjectBlue.ProjectGenerator
{
    public static class Pattern2
    {
        private static float process = 0f;
        private const float totalProcessNum = 8f;
        
        public static void Run(string baseScriptPath, string nameSpaceName)
        {

            process = 0f;
            
            var generator = new AsmdefGenerator(baseScriptPath, nameSpaceName);
            
            // EnterpriseBusinessRules
            // Entity
            var entity = Generate(generator, "2_EnterpriseBusinessRules/Entities", new List<string>());

            var infrastructure = Generate(generator, "1_Infrastructure", new List<string>());
            
            // ApplicationBusinessRules
            // interfaces
            var abrInterfaces = Generate(generator, "3_ApplicationBusinessRules/Interfaces", new List<string>(){Config.UniRxGuid, Config.UniTaskGuild, entity, infrastructure});
            AsmdefGenerator.CreateFolder(baseScriptPath + "/3_ApplicationBusinessRules/Interfaces/InputBoundaries/");
            AsmdefGenerator.CreateFolder(baseScriptPath + "/3_ApplicationBusinessRules/Interfaces/OutputBoundaries/");
            // impl
            var abr = Generate(generator, "3_ApplicationBusinessRules/Implementations", new List<string>(){Config.UniRxGuid, Config.UniTaskGuild, abrInterfaces, entity, infrastructure});
            
            // InterfaceAdapters
            // interfaces
            var iaInterfaces = Generate(generator, "4_InterfaceAdapters/Interfaces", new List<string>(){Config.UniRxGuid, Config.UniTaskGuild, entity, infrastructure});
            AsmdefGenerator.CreateFolder(baseScriptPath + "/4_InterfaceAdapters/Interfaces/InputBoundaries/");
            AsmdefGenerator.CreateFolder(baseScriptPath + "/4_InterfaceAdapters/Interfaces/OutputBoundaries/");
            // impl
            var ia = Generate(generator, "4_InterfaceAdapters/Implementations", new List<string>(){Config.UniRxGuid, Config.UniTaskGuild, iaInterfaces, abrInterfaces, entity, infrastructure});

            // FrameworksAndDrivers
            var unityDependencies = Generate(generator, "5_UnityDependencies", new List<string>(){Config.UniRxGuid, Config.UniTaskGuild, iaInterfaces, entity, infrastructure});

            var installer = Generate(generator, "0_Installers",
                new List<string>()
                {
                    Config.ZenjectGuild, Config.UniTaskGuild, Config.UniRxGuid, abrInterfaces, abr, iaInterfaces, ia,
                    unityDependencies
                });

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

