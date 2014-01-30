using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlagiarsigmDetector
{
    class BipartiteMatching
    {
        public List<Edge> maxWeightBipartiteMatching(List<Vertex> A, List<Vertex> B, List<Edge> edges, int p, int q)
        {
            //Init

            //Each node is either free = true or matched = false
            Dictionary<Vertex, Boolean> free = new Dictionary<Vertex, Boolean>(p + q);

            //Both these data structures are used in the shortest path computation
            //The predecessor edge of a vertex. Either an edge or null
            Dictionary<Vertex, Edge> pred = new Dictionary<Vertex, Edge>(p + q);
            //The distance to this vertex
            Dictionary<Vertex, Int32> dist = new Dictionary<Vertex, Int32>(p + q);

            //The potential of this vertex. Cf. the book for more information
            Dictionary<Vertex, Int32> pot = new Dictionary<Vertex, Int32>(p + q);

            //For all vertices in A and B
            foreach (Vertex a in A)
            {
                free.Add(a, true); //a is free. 
                pred.Add(a, (Edge)null); //No predecessor
                dist.Add(a, 0); //distance is zero
                pot.Add(a, 0); //potential is zero
            }

            foreach (Vertex b in B)
            {
                free.Add(b, true);
                pred.Add(b, (Edge)null);
                dist.Add(b, 0);
                pot.Add(b, 0);
            }

            //List for the edges that are part of the max weight bip. matching
            List<Edge> result = new List<Edge>(q);

            FibHeap PQ = new FibHeap(q);

            //Naive heuristic used to set the potential of the vertices in A

            int C = 0;

            //Find the edge with the heaviest cost
            foreach (Edge e in edges)
            {

                if (e.getCost() > C)
                {
                    C = e.getCost();
                }

            }

            //Set the potential to all vertices in A to the heaviest cost
            foreach (Vertex a in A)
            {
                pot[a] = C;
            }

            //Augment from all vertices in A
            foreach (Vertex a in A)
            {
                if (free[a])
                {
                    augment(a, free, pred, dist, pot, PQ);
                }
            }

            foreach (Vertex b in B)
            {
                //add all out edges from b (these are the matching edges. all matching edges goes from B to A)
                Edge e = b.getFirstAdjEdge();

                if (e != null)
                {
                    result.Add(e);
                }
            }

            //All edges that are part of the max bip. weight matching goes from B to A. We therefore need to reverse them.
            foreach (Edge e in result)
            {
                e.reverseEdge();
            }

            return result;
        }

        private void augment(Vertex a, Dictionary<Vertex, Boolean> free,
            Dictionary<Vertex, Edge> pred, Dictionary<Vertex, Int32> dist,
            Dictionary<Vertex, Int32> pot, FibHeap PQ)
        {
            //init

            //dist from a is zero
            dist[a] = 0;

            //a is the best vertex in A
            Vertex bestVertexInA = a;

            //potential of a
            int minA = pot[a];
            int delta;

            //make a stack for nodes in A that are visited during the shortest path comp.
            Stack<Vertex> RA = new Stack<Vertex>();
            RA.Push(a);

            //make a strack for the nodes in B that are visited during the shortest path comp.
            Stack<Vertex> RB = new Stack<Vertex>();

            Vertex a1 = a;

            //relax all edges out of a1

            List<Edge> adjEdges = a1.getAdjList();

            if(adjEdges == null)
                return;

            foreach(Edge e in adjEdges){
                Vertex b = e.getTarget();
                //set the distance from a1 to all its neighbors. 
                int db = dist[a1] + (pot[a1] + pot[b] - e.getCost());
                dist[b] = db;
                pred[b] = e; //predecessor for b is e

                RB.Push(b); //push to stack
                PQ.insert(b, db); //insert in priority queue
            
            }

            //select from PQ the vertex b with min distance db

            while(true){
                Vertex b;
                int db = 0;

                if(PQ.empty()){
                    b = null;
                }

                else {
                    b = (Vertex) PQ.extractMin();
                    db = dist[b];
                }

                //distinguish three cases
                if(b == null || db >= minA){
                    //We have a node v in A with potential zero. augment a path from a to v.
                    delta = minA;

                    //augmentation by path to best vertex in A
                    augmentPathTo(bestVertexInA, pred);
                    free[a] = false;
                    free[bestVertexInA] = true;
                    break;
                }

                else {
                    //b is a free node, can be matched
                    if(free[b]){
                        delta = db; 

                        //augmentation by path to b
                        augmentPathTo(b, pred);
                        free[a] = false;
                        free[b] = false;
                        break;
                    }

                    //Neither of the above. b is matched
                    else {
                        //continue shortest-path computation
                        Edge e = b.getFirstAdjEdge();        

                        a1 = e.getTarget(); //target of b 
                        pred[a1] = e;
                        RA.Push(a1);
                        dist[a1] = db;

                        //if better than minA
                        if((db + pot[a1]) < minA){
                            bestVertexInA = a1;
                            minA = db + pot[a1];
                        }

                        //relax all edges out of a1
                        adjEdges = a1.getAdjList();

                        foreach(Edge e1 in adjEdges){
                            b = e1.getTarget();

                            db = dist[a1] + (pot[a1] + pot[b] - e1.getCost());

                            if(pred[b] == null){
                                dist[b] = db;
                                pred[b] = e1;
    
                                RB.Push(b);
                                PQ.insert(b, db);
                            }

                            //b is already in the priority queue
                            else {
                                if(db < dist[b]){
                                    dist[b] = db;
                                    pred[b] = e1;

                                    PQ.decreaseKey(b, db);
                                }
                            }
                        }
                    }
                }
            }

            //augment: potential update and reinit

            //Update the potential and remove the nodes from the stack
            while(RA.Count > 0){
                Vertex tmp = RA.Pop();
                pred[tmp] = null;
                int potChange = delta - dist[tmp];

                if(potChange <= 0){
                    continue;
                }

                pot[tmp] = (pot[tmp] - potChange);
            }

            //Update the potential and remove the nodes from the stack.
            while(RB.Count > 0){
                Vertex tmp = RB.Pop();
                pred[tmp] = (Edge) null;

                //if b is in the heap
                if(PQ.member(tmp)){
                    PQ.delete(tmp);
                }

                int potChange = delta - dist[tmp];

                if(potChange <= 0){
                    continue;
                }

                pot[tmp] = (pot[tmp] + potChange);
            }
        }

        //We reverse all the edges on the path when we augment from a node v
        private void augmentPathTo(Vertex v, Dictionary<Vertex, Edge> pred)
        {
            Edge e = pred[v];

            while (e != null)
            {
                e.reverseEdge();
                e = pred[e.getTarget()];//NOT SOURCE
            }
        }
    }
}