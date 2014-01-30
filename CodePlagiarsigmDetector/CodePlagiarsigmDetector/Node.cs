using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CodePlagiarsigmDetector
{
    public class Node : IEquatable<Node>
    {
        private IEnumerable<Node> children;
        private Node parent;
        private int size;

        public IEnumerable<Node> Children
        {
            get
            {
                if (children == null)
                {
                    children = SyntaxNodeOrToken.ChildNodesAndTokens().Select(child => new Node(child));
                }
                return children;
            }
        }

        public Node Parent
        {
            get
            {
                if (parent == null)
                {
                    if (SyntaxNodeOrToken.Parent == null)
                    {
                        return null;
                    }
                    parent = new Node(SyntaxNodeOrToken.Parent);
                }
                return parent;
            }
        }

        public int Size
        {
            get
            {
                if (SyntaxNodeOrToken.IsToken)
                {
                    size = 0;
                }
                else
                {
                    size = SyntaxNodeOrToken.AsNode().DescendantNodesAndSelf().Count();
                }
                return size;
            }
        }

        public SyntaxNodeOrToken SyntaxNodeOrToken { get; private set; }

        public bool IsMatched { get; set; }
        public bool IsChecked { get; set; }
        public bool IsRotated { get; set; }
        public bool IsLcsMatched { get; set; }
        public int ChildNumber { get; private set; }

        public int MatchNum { get; set; }

        public Node(SyntaxNodeOrToken syntaxNodeOrToken)
        {
            SyntaxNodeOrToken = syntaxNodeOrToken;
            IsMatched = false;
            IsChecked = false;
            IsRotated = false;
            IsLcsMatched = false;
            MatchNum = 0;

            if (SyntaxNodeOrToken.Parent != null)
            {
                int i = 0;
                foreach (var child in SyntaxNodeOrToken.Parent.ChildNodesAndTokens())
                {
                    if (child == SyntaxNodeOrToken)
                    {
                        ChildNumber = i;
                        break;
                    }
                    i++;
                }
            }
            else
            {
                ChildNumber = 0;
            }
        }

        public bool isLeaf()
        {
            return SyntaxNodeOrToken.IsToken;
        }

        public SyntaxKind getId()
        {
            return SyntaxNodeOrToken.Kind;
        }

        public void preorderTraversal(List<Node> list)
        {
            list.Add(this);

            if (Children.Any())
            {
                foreach (Node child in Children)
                {
                    child.preorderTraversal(list);
                }
            }
        }

        public override int GetHashCode()
        {
            return SyntaxNodeOrToken.GetHashCode();
        }

        public bool Equals(Node other)
        {
            return SyntaxNodeOrToken == other.SyntaxNodeOrToken;
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Node)obj);
        }
    }
}
