using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.CSharp;

namespace CodePlagiarsigmDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            var root1 = GetRoot("..\\..\\ToParse\\Person1.cs");
            var root2 = GetRoot("..\\..\\ToParse\\Person2.cs");

            AstSimLcs sim = new AstSimLcs();
            double result = sim.run(root1, root2) / 2;
            Console.WriteLine(result);
        }

        static Node GetRoot(string path)
        {
            var code = new StreamReader(path).ReadToEnd();

            SyntaxTree tree = SyntaxTree.ParseText(code);
            SyntaxNode root = (CompilationUnitSyntax)tree.GetRoot();

            return new Node(root);
        }
    }
}
