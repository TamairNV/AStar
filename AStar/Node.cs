using System.Numerics;

namespace AStar;
using System;
using System.Collections.Generic;
public class Node
{
    
    private float gCost = 0;
    private float hCost = 0;
    private float fCost = 0;
    private int X;
    private int Y;
    public int nodeID = 0;
    private int state = 0;
    private Node parent;

    public List<Vector2>  path;
    //0 - empty
    //1 - wall
    //2 - discovered
    //3 - parent
    //4
    //5


    public Node(int x, int y,int state_)
    {
        X = x;
        Y = y;
        state = state_;
    }

    public void ResetNode(int state_)
    {
        state = state_;
        gCost = 0;
        parent = null;
        path = null;
    }

    private float CalDistance(int x1, int y1, int x2, int y2)
    {
        float x3 = Math.Abs( x1 - x2);
        float y3 = Math.Abs(y1 - y2);

        return Convert.ToSingle(Math.Sqrt(Math.Pow(x3, 2) + Math.Pow(y3, 2)));
    }

    private void TraceBack()
    {
        path = new List<Vector2>();
        Node currentNode = this;
        while (currentNode != null)
        {
            path.Add(new Vector2(currentNode.X,currentNode.Y));
            currentNode = currentNode.parent;
        }

    }


    public bool ExpandParent(Node[,] grid, int[,] baseGrid, PriorityQueue<Node> priorityQueue , Node endNode)
    {

        if (this == endNode)
        {
            TraceBack();
            return true;
        }

        if (nodeID != endNode.nodeID)
        {
            nodeID = endNode.nodeID;
            ResetNode(baseGrid[X,Y]);
        }

        state = 3;
        for (int x = X-1; x < X+2; x++)
        {
            for (int y = Y-1; y < Y+2; y++)
            {
                if ((x >= 0 && x < grid.GetLength(0)) && (Y >= 0 && Y < grid.GetLength(1)))
                {
                    if (grid[x, y] != this)
                    {
                        Node node = grid[x, y];
                        if (nodeID != endNode.nodeID)
                        {
                            node.ResetNode(baseGrid[x,y]);
                            node.nodeID = endNode.nodeID;
                        }
                        
                        if (node.state == 0)
                        {
                            node.state = 2;
                            node.gCost = gCost + CalDistance(X, Y, x, y);
                            node.hCost = CalDistance(X, Y, endNode.X, endNode.Y);
                            node.parent = this;
                            priorityQueue.Enqueue(gCost + hCost,node);
                        }
                        else if (node.state == 2 && node.parent.gCost > gCost)
                        {
                            node.gCost = gCost + CalDistance(X, Y, x, y);
                            node.parent = this;
                        }
                        
                        
                    }
                }
            }
        }

        return false;
    }
    
    
}