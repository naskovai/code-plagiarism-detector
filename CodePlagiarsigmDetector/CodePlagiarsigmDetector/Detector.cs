using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlagiarsigmDetector
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
        public async Task<CodePlagiarismDetectionResult> Compare(string firstSourceCode, string secondSourceCode)
        {
            var result = await Task.Factory.StartNew(() =>
            {
                return new CodePlagiarismDetectionResult(Languages.CSharp, firstSourceCode, secondSourceCode, true, 0.96);
            });
            return result;
        }
    }
}
