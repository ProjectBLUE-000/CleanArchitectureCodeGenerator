namespace ProjectBlue.CodeGenerator
{

    public class ModelTemplate : CodeTemplateBase
    {

        public override string FolderPath => "Domain/Model/";
        public override string FileName => $"{className}Model.cs";

        public ModelTemplate(string nameSpaceName, string className) : base(nameSpaceName, className){}
        
        protected override string Template => @"

namespace #NAME_SPACE#.Domain.Model
{

    public class #CLASS_NAME#Model 
    {

        public #CLASS_NAME#Model()
        {
        }

    }
}
";

    }
    
}