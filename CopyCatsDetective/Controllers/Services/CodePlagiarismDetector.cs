using CopyCatsDetective.Controllers.Services.CodeSimilarityMeasurer;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CopyCatsDetective.Controllers.Services
{
    public enum Languages
    {
        CSharp
    }

    public class CodePlagiarismDetectionResult
    {
        public Languages Language { get; private set; }
        public string FirstSourceCode { get; private set; }
        public string SecondSourceCode { get; private set; }
        public bool AreEqual { get; private set; }
        public double Similarity { get; private set; }

        public CodePlagiarismDetectionResult(Languages language, string firstSourceCode, string secondSourceCode,
            bool areEqual, double similarity)
        {
            Language = language;
            FirstSourceCode = firstSourceCode;
            SecondSourceCode = secondSourceCode;
            AreEqual = areEqual;
            Similarity = similarity;
        }
    }

    public class CodePlagiarismDetector
    {
        private AstSimLcs astSimLcs;

        public CodePlagiarismDetector()
        {
            astSimLcs = new AstSimLcs();
        }

        public async Task<CodePlagiarismDetectionResult> Compare(Languages language, string firstSourceCode, string secondSourceCode)
        {
            var result = await Task.Factory.StartNew(() =>
            {
                switch(language)
                {
                    case Languages.CSharp:
                        return CompareCShapr(firstSourceCode, secondSourceCode);
                    default:
                        return null;
                }
            });
            return result;
        }

        private CodePlagiarismDetectionResult CompareCShapr(string firstSourceCode, string secondSourceCode)
        {
            SyntaxNode firstRoot = SyntaxTree.ParseText(firstSourceCode).GetRoot();
            SyntaxNode secondRoot = SyntaxTree.ParseText(secondSourceCode).GetRoot();
            double result = astSimLcs.run(new Node(firstRoot), new Node(secondRoot));
            result = Math.Min(result, 1);
            return new CodePlagiarismDetectionResult(Languages.CSharp, firstSourceCode, secondSourceCode, result >= 0.7, result);
        }
    }
}