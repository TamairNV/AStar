using System.Numerics;
using System;
namespace AStar;

public class PathingObject
{
    public static int pathCount = 0;
    
    private int[,] baseGrid;
    private Node[,] grid;
    private Vector2 position;
    public Vector2 realPosition;
    public float speed = 1000;
    private List<Vector2> path;
    private int currentWaypointIndex = 0;
    private List<Vector2> targets;
    public Vector2 currentTarget = Vector2.Zero;
    private static Random rnd = new Random();
    public bool completedPath = false;
    private int cellSize;
    public bool isRunning = false;

    public PathingObject(Grid baseGrid_,Vector2 position_,List<Vector2> targets_)
    {
        cellSize = baseGrid_.cellSize;
        targets = targets_;
        baseGrid = baseGrid_.grid;
        position = position_;
        realPosition = new Vector2(position.X *cellSize, position.Y * cellSize);
        grid = new Node[baseGrid.GetLength(0),baseGrid.GetLength(1)];
        for (int x = 0; x < baseGrid.GetLength(0); x++)
        {
            for (int y = 0; y < baseGrid.GetLength(1); y++)
            {
                grid[x, y] = new Node(x,y,baseGrid[x,y]);
            }
        }
    }


    public void PathToTarget(int targetX,int targetY)
    {
        position = new Vector2(realPosition.X / cellSize, realPosition.Y / cellSize);
        Node startNode = grid[Convert.ToInt16(position.X), Convert.ToInt16(position.Y)];
        Node endNode = grid[targetX, targetY];
        path = FindPath(startNode, endNode);
        completedPath = false;

    }

    public void Run()
    {
        Vector2 newTarget = FindNewTarget();
        PathToTarget(Convert.ToInt16(newTarget.X/cellSize),Convert.ToInt16(newTarget.Y/cellSize));
    }
    
    public void StepPath(float deltaTime,int movementSpeed = 1500)
    {
        
        if (path == null || path.Count == 0 || currentWaypointIndex >= path.Count)
            return;

        Vector2 target = new Vector2(path[currentWaypointIndex].X*cellSize,path[currentWaypointIndex].Y*cellSize);
        
        Vector2 direction = Vector2.Normalize(target - realPosition);
        if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
        {
            direction = Vector2.Zero; // or set to a default direction
        }

        Vector2 movement = direction * movementSpeed * deltaTime;
        
        
        if (Vector2.Distance(realPosition, target) <= movement.Length())
        {
            
            realPosition = new Vector2(target.X,target.Y);
            position = new Vector2(target.X/cellSize,target.Y/cellSize);
            currentWaypointIndex ++;
        }
        else
        {
            realPosition += movement;
        }

        if (currentWaypointIndex >= path.Count)
        {
            currentWaypointIndex = 0;
            completedPath = true;
            realPosition = new Vector2(target.X, target.Y);
            position = new Vector2(target.X / cellSize, target.Y / cellSize);

            // Start `Run` in a new thread
            Thread runThread = new Thread(Run);
            runThread.Start();
        }
    }

    public Vector2 FindNewTarget()
    {
        Vector2 randomTarget = currentTarget;
        int safety = 0;
        while (currentTarget == randomTarget || safety >= 100)
        {
            safety += 1;
            randomTarget = targets[rnd.Next(0, targets.Count)];
        }

        if (safety >= 100)
        {
            Console.WriteLine("uh-Oh");
        }

        return randomTarget;
    }

    public List<Vector2> FindPath(Node startNode,Node endNode)
    {
        pathCount += 1;
        endNode.ResetNode(0);
        startNode.ResetNode(0);
        endNode.nodeID = pathCount;
        startNode.nodeID = pathCount;
        PriorityQueue<Node,float> priorityQueue = new PriorityQueue<Node,float>();
        priorityQueue.Enqueue(startNode,0);
        
        while (priorityQueue.Count > 0)
        {
            var node = priorityQueue.Dequeue();  
            
            if (node.ExpandParent(grid, baseGrid, priorityQueue, endNode))
            {
                break; 
            }
        }
        endNode.path.Reverse();
        return endNode.path;


    }
    
}