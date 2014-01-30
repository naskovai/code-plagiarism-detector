using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CodePlagiarsigmDetector
{
    public class AstSimLcs
    {
        //The root nodes that represents tree1 and tree2
        Node tree1, tree2;
        //Map B - All possible mappings
        Dictionary<Node, List<Node>> bMap;
        //Map M - The actual mapping
        Dictionary<Node, Node> mMap;

        public double run(Node tree1, Node tree2)
        {
            initialize(tree1, tree2);

            //The root nodes need to have the same label
            if (tree1.getId() == tree2.getId())
            {
                //Find the size of the isomorphism
                int res = topDownUnorderedMaxCommonSubtreeIso(tree1, tree2);

                //The similarity between the two trees.
                return similarity(tree1.Size, tree2.Size, res);
            }
            return 0;
        }

        private void initialize(Node tree1, Node tree2)
        {
            //We always find the mapping between the smallest tree and the largest tree. 
            //Here we might need to switch the root nodes.
            if (tree1.Size <= tree2.Size)
            {
                this.tree1 = tree1;
                this.tree2 = tree2;
            }

            else
            {
                this.tree1 = tree2;
                this.tree2 = tree1;
            }

            bMap = new Dictionary<Node, List<Node>>(tree1.Size);
            mMap = new Dictionary<Node, Node>(tree1.Size);
        }

        public int lcs(Node subTree1, Node subTree2)
        {
            //This hashmap stores the alignment between the two sequences. This alignment does not need to be the final alignment.
            Dictionary<Node, Node> tmp = new Dictionary<Node, Node>(subTree1.Size);

            List<Node> preorderSubTree1 = new List<Node>(subTree1.Size);
            List<Node> preorderSubTree2 = new List<Node>(subTree2.Size);

            subTree1.preorderTraversal(preorderSubTree1);
            subTree2.preorderTraversal(preorderSubTree2);

            int m = subTree1.Size;
            int n = subTree2.Size;

            int[,] c = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                c[i, 0] = 0;
            }

            for (int j = 0; j <= n; j++)
            {
                c[0, j] = 0;
            }

            //Find the size of an optimal alignment between preorderSubTree1 and preorderSubTree2
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    Node i1 = preorderSubTree1[i - 1];
                    Node j1 = preorderSubTree2[j - 1];

                    Node p1 = i1.Parent;
                    Node p2 = j1.Parent;

                    //Only align if i1 and j1 share the same label, and p1 and p2 share the same label or i1 and j1 have the label Block.
                    if ((i1.getId() == j1.getId()) && ((p1.getId() == p2.getId()) || i1.getId() == SyntaxKind.Block))
                    {
                        c[i, j] = c[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        c[i, j] = Math.Max(c[i, j - 1], c[i - 1, j]);
                    }
                }
            }

            int ii = m;
            int jj = n;

            //Find the actual alignment between preorderSubTree1 and preorderSubTree2
            while (ii > 0 && jj > 0)
            {
                Node i1 = preorderSubTree1[ii - 1];
                Node j1 = preorderSubTree2[jj - 1];

                Node p1 = i1.Parent;
                Node p2 = j1.Parent;

                //Only use i1 and j1 in the alignment if i1 and j1 share the same label, and p1 and p2 share the same label 
                //or i1 and j1 have the label Block.
                if ((i1.getId() == j1.getId()) && ((p1.getId() == p2.getId()) || i1.getId() == SyntaxKind.Block))
                {
                    tmp.Add(i1, j1);
                    ii--;
                    jj--;
                }
                else if (c[ii, jj - 1] > c[ii - 1, jj])
                {
                    jj--;
                }
                else
                {
                    ii--;
                }
            }

            int res = 0;

            //Used to store the final alignment
            Dictionary<Node, Node> alignment = new Dictionary<Node, Node>(subTree1.Size);

            //Here we remove nodes that cannot be part of the alignment
            foreach (Node n1 in preorderSubTree1)
            {
                if (tmp.ContainsKey(n1))
                {
                    Node n2 = tmp[n1];

                    //The root nodes of the subtrees that represents the method bodies are always part of the final alignment
                    if (n1 == subTree1)
                    {
                        alignment.Add(n1, n2);
                    }
                    else
                    {
                        Node p1 = n1.Parent;
                        Node p2 = n2.Parent;

                        //If the parents of n1 and n2 are aligned, then align n1
                        //and n2
                        if (alignment.ContainsKey(p1) && p2 == alignment[p1])
                        {
                            alignment.Add(n1, n2);
                        }

                        //If n1 and n2 are labeled block, then align them if at least one of the children 
                        //of n1 is aligned to a child of n2.
                        else if (n1.getId() == SyntaxKind.Block)
                        {
                            Node[] nodes1 = n1.Children.ToArray();

                            if (nodes1 != null)
                            {
                                foreach (Node node1 in nodes1)
                                {
                                    if (tmp.ContainsKey(node1))
                                    {
                                        Node node2 = tmp[node1];

                                        if (n2 == node2.Parent)
                                        {
                                            alignment.Add(n1, n2);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Insert n2 into the list of nodes that can be mapped to n1 if n1 is aligned to n2 in alignment.
                    if (alignment.ContainsKey(n1))
                    {
                        List<Node> nodeList;

                        if (bMap.ContainsKey(n1))
                        {
                            nodeList = bMap[n1];
                        }

                        else
                        {
                            nodeList = new List<Node>(100);
                        }

                        nodeList.Add(n2);
                        bMap[n1] = nodeList;

                        res += 1;
                    }
                }
            }
            return res;
        }

        public int topDownUnorderedMaxCommonSubtreeIso(Node r1, Node r2)
        {
            //Cannot find a isomophism when the labels differ
            if (r1.getId() != r2.getId())
            {
                return 0;
            }

            //The isomorphism has size 0 or 1 if one of the nodes are leaf nodes
            if (r1.isLeaf() || r2.isLeaf())
            {
                return (r1.getId() == r2.getId()) ? 1 : 0;
            }

            int result;

            Node rp1 = r1.Parent;
            Node rp2 = r2.Parent;

                //Use LCS if r1 and r2 are root nodes in subtrees that represents method bodies.
            if ((rp1 != null && r1.getId() == SyntaxKind.Block && rp1.getId() == SyntaxKind.MethodDeclaration)
                && (rp2 != null && r2.getId() == SyntaxKind.Block && rp2.getId() == SyntaxKind.MethodDeclaration))
            {
                int res = lcs(r1, r2);
                result = res;
            }
            else
            {
                int p = r1.Size;
                int q = r2.Size;

                //Each child of r1 has a corresponding vertex in the bipartite graph. A map from node to vertex.
                Dictionary<Node, Vertex> T1G = new Dictionary<Node, Vertex>(p);
                //Each child of r2 has a corresponding vertex in the bipartite graph. A map from node to vertex.
                Dictionary<Node, Vertex> T2G = new Dictionary<Node, Vertex>(q);
                //A map from vertex to node.
                Dictionary<Vertex, Node> GT = new Dictionary<Vertex, Node>(p + q);

                //There is maximum p*q edges in the bipartite graph.
                List<Edge> edges = new List<Edge>(p * q);

                //The vertices that represents the children of r1.
                List<Vertex> U = new List<Vertex>(p);

                foreach (Node v1 in r1.Children)
                {
                    //q is the number of neighbors that v can have in the bipartite graph.
                    Vertex v = new Vertex(q);

                    U.Add(v);

                    GT.Add(v, v1);
                    T1G.Add(v1, v);
                }

                //The vertices that represents the children of r2.
                List<Vertex> W = new List<Vertex>(q);

                foreach (Node v2 in r2.Children)
                {
                    //p is the number of neighbors that w can have in the bipartite graph.
                    Vertex w = new Vertex(p);

                    W.Add(w);

                    GT.Add(w, v2);
                    T2G.Add(v2, w);
                }

                //List of matched edges
                List<Edge> list = null;

                foreach (Node v1 in r1.Children)
                {
                    foreach (Node v2 in r2.Children)
                    {

                        //Find max common subtree between v1 and v2
                        int res = topDownUnorderedMaxCommonSubtreeIso(v1, v2);

                        //If max common subtree
                        if (res != 0)
                        {
                            Vertex v = T1G[v1];

                            //Insert edge between v1 and v2
                            Edge e = v.insertEdge(T2G[v2]);

                            //Set cost of edge to res (size of max common subtree)
                            e.setCost(res);

                            edges.Add(e);
                        }
                    }
                }

                //Find the children of r1 and r2 that are part of r1's and r2's max common subtree
                BipartiteMatching bm = new BipartiteMatching();
                list = bm.maxWeightBipartiteMatching(U, W, edges, p, q);

                //Can map r1 to r2
                int ress = 1;

                //Go through the mached edges in the bipartite graph
                foreach (Edge e in list)
                {
                    List<Node> nodeList;

                    //All edges goes from the children of r1 to the children of r2
                    Node v = GT[e.getSource()];
                    Node w = GT[e.getTarget()];

                    //v is already in B
                    if (bMap.ContainsKey(v))
                    {
                        nodeList = bMap[v];
                    }

                    //First time we insert v. Create a list for the nodes that can be mapped to v.
                    else
                    {
                        nodeList = new List<Node>(100);
                    }

                    //Insert w into the list
                    nodeList.Add(w);
                    bMap[v] = nodeList;

                    //Add the size of the max common subtree between v and w to res.
                    ress += e.getCost();
                }

                result = ress;
            }
            return result;
        }

        //Calculates the similarity between two ASTs.
        private double similarity(int size1, int size2, int sim)
        {
            return ((double)(2 * sim) / (double)(size1 + size2));
        }
    }
}
