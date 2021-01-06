using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlue.CodeGenerator
{

    public class AsmdefAPI
    {

        private string fileName = "";
        private List<string> referenceGUIDs;
        
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

        public AsmdefAPI(string fileName, List<string> referenceGUIDs = null)
        {
            this.fileName = fileName;
            this.referenceGUIDs = referenceGUIDs ?? new List<string>();
        }

        public void Reset()
        {
            referenceGUIDs.Clear();
        }

        public void AddReferenceGUID(string guid)
        {
            referenceGUIDs.Add(guid);
        }

        public string Generate()
        {
            
            var referenceGuidStr = "";
            for (var i = 0; i < referenceGUIDs.Count; i++)
            {
                referenceGuidStr += $"\"GUID:{referenceGUIDs[i]}\"";
                if (i != referenceGUIDs.Count - 1) referenceGuidStr += ",\n";
            }
            
            var generatedText = asmdefTemplate.Replace("##FILE_NAME##", fileName).Replace("##REFERENCE##",referenceGuidStr);
            return generatedText;
        }
        
        
    }
    
}
