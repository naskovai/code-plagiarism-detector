using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlagiarsigmDetector
{
    public class Vertex
    {
        //Max size of the adjecency list of this vertex. 
        int size = 0;

        List<Edge> adjList = null;

        public Vertex(int size)
        {
            this.size = size;
        }

        //Make a new edge that has this vertex as source and another vertex 
        //as target.
        public Edge insertEdge(Vertex target)
        {
            if (adjList == null)
                adjList = new List<Edge>(size);

            Edge e = new Edge(this, target);
            adjList.Add(e);

            return e;
        }

        //Insert a new edge into the adjecency list of this vertex.
        public Edge insertEdge(Edge e)
        {
            if (adjList == null)
                adjList = new List<Edge>(size);

            adjList.Add(e);

            return e;
        }

        //Remove an edge from the adjecency list of this vertex.
        public bool removeEdge(Edge e)
        {
            if (adjList != null)
                return adjList.Remove(e);
            return false;
        }

        //Return adjecency list.
        public List<Edge> getAdjList()
        {
            return adjList;
        }

        //Return the first element in the adjecency list.
        public Edge getFirstAdjEdge()
        {
            if (adjList != null && adjList.Count != 0)
                return adjList[0];
            return null;
        }
    }
}
