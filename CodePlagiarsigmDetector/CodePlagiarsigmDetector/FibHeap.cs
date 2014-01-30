using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlagiarsigmDetector
{
    //For more information about the Fibonacci heap see Introduction to Algorithms (2001) by Cormen et al.
    public class FibHeap
    {
        public class FibHeapNode
        {
            public Object @object;
            public int key;
            public FibHeapNode parent;
            public FibHeapNode leftSibling;
            public FibHeapNode rightSibling;
            public FibHeapNode child;

            //mark indicates wheter this node has lost a child since the last time this node was made the child of another node. 
            //A node can lose a child when we use the decreasekey operation.
            public bool mark;

            //The number of children of this nodes.
            public int degree;

            //A number that indicates when this node was inserted into the heap. Added by me.
            //The minimum node in the heap is the node with the smallest key. If we have two or more node with the smallest key, then
            //the min node is the node with the smallest key and the smallest insertNum. The nodes that are added to the heap are the 
            //nodes in B in BipartiteMatching.java. We always add the nodes that are closest to a node a in A first. By using insertNum 
            //then we do not need to do unordered matchings of the nodes in A and B if it is not absolutely necessary.
            public int insertNum;

            public FibHeapNode(Object @object, int key, int insertNum)
            {
                this.@object = @object;
                this.key = key;
                this.insertNum = insertNum;

                parent = child = null;
                leftSibling = rightSibling = this;

                mark = false;
                degree = 0;
            }
        }

        //minimum node
        private FibHeapNode min;
        //Hashmap that map an Object to a FibHeapNode
        private Dictionary<Object, FibHeapNode> objectToNode;
        //Number of nodes in the heap
        private int numberOfNodes;
        //Count how many nodes that have been added. Not the same as numberOfNodes since nodes can have been removed from the heap.
        private int insertNumCounter;

        public FibHeap(int maxSize)
        {
            min = null;
            numberOfNodes = 0;
            insertNumCounter = 0;
            objectToNode = new Dictionary<Object, FibHeapNode>(maxSize);
        }

        public void insert(Object @object, int key)
        {
            //Reset counter if number of nodes is zero, because then we start over again.
            if (numberOfNodes == 0)
            {
                insertNumCounter = 0;
            }

            FibHeapNode node = new FibHeapNode(@object, key, insertNumCounter);
            insertNumCounter++;

            objectToNode.Add(@object, node);

            if (min != null)
            {
                //insert node into the root list
                concatenate(node, min);

                //only test on key since node cannot have lower insertNum than min
                if (min.key > node.key)
                    min = node;
            }

            else
            {
                min = node;
            }

            numberOfNodes++;
        }

        //insert a x into a list of nodes
        public void concatenate(FibHeapNode x, FibHeapNode y)
        {
            x.leftSibling.rightSibling = y;
            y.leftSibling.rightSibling = x;

            FibHeapNode tmp = y.leftSibling;

            y.leftSibling = x.leftSibling;
            x.leftSibling = tmp;
        }

        //remove x from a list of nodes
        public void removeNode(FibHeapNode x)
        {
            x.rightSibling.leftSibling = x.leftSibling;
            x.leftSibling.rightSibling = x.rightSibling;
        }

        //extract the min node
        public Object extractMin()
        {
            FibHeapNode z = min;

            if (z != null)
            {
                //if z has children then insert all the children into the root list
                if (z.child != null)
                {
                    FibHeapNode tmp = z.child;

                    while (tmp.parent != null)
                    {
                        tmp.parent = null;
                        tmp = tmp.rightSibling;
                    }

                    concatenate(tmp, min);

                }

                //remove the min node from the root list
                removeNode(z);

                //if min is the only node in the root list
                if (z.rightSibling == z)
                {
                    if (numberOfNodes - 1 != 0)
                    {
                        throw new Exception("Error");
                    }

                    min = null;
                }

                //else set the rightSibling of min as the new min node and run a consolidation on the heap
                else
                {
                    min = z.rightSibling;
                    consolidate();
                }

                z.rightSibling = z.leftSibling = null;

                numberOfNodes--;
                objectToNode.Remove(z.@object);

                //return the object of the z node (former min node)
                return z.@object;
            }

            return null;
        }


        public void consolidate()
        {
            //the new size of the root list. see Cormen et al. for more information
            double phi = (1.0 + Math.Sqrt(5.0)) / 2.0;

            int size = (int)(Math.Floor(Math.Log((double)numberOfNodes) / Math.Log(phi)));

            //Array can contain trees of different sizes. We can have size trees of the 
            //sizes 1, 2, 4, 8, 16, and so on, and only one tree of each size.
            FibHeapNode[] array = new FibHeapNode[size + 1];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = null;
            }

            // Find the number of root nodes in the root list.
            int numRoots = 0;
            FibHeapNode x = min;

            if (x != null)
            {
                numRoots++;
                x = x.rightSibling;

                while (x != min)
                {
                    numRoots++;
                    x = x.rightSibling;
                }
            }

            // For each node in the root list
            while (numRoots > 0)
            {
                //The degree of x
                int d = x.degree;
                //Since we might move x we set its rightSibling as the next node that we will process
                FibHeapNode next = x.rightSibling;

                //is there a subtree with the same size in the root list, then make a bigger tree
                while (array[d] != null)
                {
                    FibHeapNode y = array[d];

                    //if x has a bigger key than y, or if the keys are equal but x has a bigger insertNum, then
                    //x becomes the child of y
                    if ((x.key > y.key) || ((x.key == y.key) && (x.insertNum > y.insertNum)))
                    {
                        FibHeapNode temp = y;
                        y = x;
                        x = temp;
                    }

                    //y becomes the child of x and get removed from the root list
                    link(y, x);

                    //there is no node with this degree anymore. we increase the degree and see if we can build an even larger tree 
                    array[d] = null;
                    d++;
                }

                //the tree rooted at x is inserted at d
                array[d] = x;

                //move forward through the list.
                x = next;
                numRoots--;
            }

            //set min to null since we need to find the new min node in the list        
            min = null;

            for (int i = 0; i <= size; i++)
            {
                if (array[i] != null)
                {
                    //array[i] is the new min node if min = null or if it has a smaller key than min or if its key equals min and insertNum of array[i] is smaller than 
                    //the insertNum of min.
                    if ((min == null) || (array[i].key < min.key) || ((array[i].key == min.key) && (array[i].insertNum < min.insertNum)))
                    {
                        min = array[i];
                    }
                }
            }
        }

        //used when we remove child from the root list and make it a child of parent
        public void link(FibHeapNode child, FibHeapNode parent)
        {
            //remove child from the root list
            removeNode(child);

            child.rightSibling = child.leftSibling = child;

            //if parent has children, then concatenate child with these 
            if (parent.child != null)
            {
                concatenate(child, parent.child);
            }

            //child is the new child of parent
            parent.child = child;
            parent.degree++;

            child.parent = parent;
            child.mark = false;
        }

        //decrease the key of object
        public void decreaseKey(Object @object, int key)
        {
            if (objectToNode.ContainsKey(@object))
            {

                //find the node
                FibHeapNode child = objectToNode[@object];

                if (key > child.key)
                {
                    return;
                }

                child.key = key;

                FibHeapNode parent = child.parent;

                //perform cut and cascading cut if parent != null and child has a lower key than parent or they have an equal key but child
                //has a smaller insertNum than parent.
                if ((parent != null) && ((child.key < parent.key) || ((child.key == parent.key) && (child.insertNum < parent.insertNum))))
                {
                    cut(child, parent);
                    cascadingCut(parent);
                }

                //check if child is the new min node
                if ((child.key < min.key) || ((child.key == min.key) && (child.insertNum < min.insertNum)))
                {
                    min = child;
                }
            }

            else
            {
                throw new Exception("Error: No such object.");
            }
        }

        public void cut(FibHeapNode child, FibHeapNode parent)
        {
            //remove child from the child list of parent       
            removeNode(child);

            //set the child list to a new node or null
            if (parent.child == child)
            {
                if (child.rightSibling != child)
                {
                    parent.child = child.rightSibling;
                }

                else
                {
                    parent.child = null;
                }

            }

            parent.degree--;

            child.parent = null;
            child.mark = false;

            child.rightSibling = child.leftSibling = child;

            //insert the child into the root list
            concatenate(child, min);
        }

        public void cascadingCut(FibHeapNode node)
        {
            FibHeapNode parent = node.parent;

            if (parent != null)
            {
                //if node's mark is false then set it to true. 
                //The next time we remove a child from node then node will be cut from its parent.
                if (node.mark == false)
                {
                    node.mark = true;
                }

                //cut the node and perform a cascading cut on the parent
                else
                {
                    cut(node, parent);
                    cascadingCut(parent);
                }
            }
        }

        //delete a node. the node is deleted by decreasing its key to Integer.MIN_VALUE and then by using extractMin
        public Object delete(Object @object)
        {
            if (objectToNode != null)
            {
                decreaseKey(@object, Int32.MinValue);
                return extractMin();
            }

            return null;
        }

        //check if a node with object is in the heap
        public bool member(Object @object)
        {
            if (objectToNode != null)
                return objectToNode.ContainsKey(@object);

            return false;
        }

        //check if the heap is empty
        public bool empty()
        {
            if (min == null)
                return true;

            return false;
        }
    }
}
