using UnityEditor;

namespace ProjectBlue.CodeGenerator
{
    public static class Pattern1
    {

        private static int _step;
        private const float TotalStepNum = 10f;

        public static void Run(string baseScriptPath, string nameSpace, string className)
        {

            _step = 0;
            
            // Model
            Generate(baseScriptPath, new ModelTemplate(nameSpace, className));
            
            // View
            Generate(baseScriptPath, new ViewTemplate(nameSpace, className));
            
            // Presenter
            Generate(baseScriptPath, new ViewInterfaceTemplate(nameSpace, className));
            Generate(baseScriptPath, new PresenterTemplate(nameSpace, className));
            
            // DataStore
            Generate(baseScriptPath, new DataStoreTemplate(nameSpace, className));
            
            // Repository
            Generate(baseScriptPath, new DataStoreInterfaceTemplate(nameSpace, className));
            Generate(baseScriptPath, new RepositoryTemplate(nameSpace, className));
            
            // UseCase
            Generate(baseScriptPath, new RepositoryInterfaceTemplate(nameSpace, className));
            Generate(baseScriptPath, new PresenterInterfaceTemplate(nameSpace, className));
            Generate(baseScriptPath, new UseCaseTemplate(nameSpace, className));
            
            EditorUtility.ClearProgressBar();
        }

        private static void Generate(string baseScriptPath, CodeTemplateBase codeTemplateBase)
        {
            CodeGenerator.Generate(baseScriptPath, codeTemplateBase);
            _step++;
            EditorUtility.DisplayProgressBar("Generating CA codes...", codeTemplateBase.FileName, _step/TotalStepNum);
        }
        
    }
}