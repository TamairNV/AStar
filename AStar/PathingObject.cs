using System.Numerics;

namespace AStar;

public class PathingObject
{
    private static int pathCount = 0;
    private int[,] baseGrid;
    private Node[,] grid;
    private Vector2 position;
    public Vector2 realPosition;
    private float speed = 10;
    private List<Vector2> path;
    private int currentWaypointIndex = 0;

    public PathingObject(Grid baseGrid_,Vector2 position_)
    {
        baseGrid = baseGrid_.grid;
        position = position_;
        realPosition = new Vector2(position.X * baseGrid_.cellSize, position.Y * baseGrid_.cellSize);
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
        
        Node startNode = grid[Convert.ToInt16(position.X), Convert.ToInt16(position.Y)];
        Node endNode = grid[targetX, targetY];
        path = FindPath(startNode, endNode);
        
    }
    
    public void StepPath(float deltaTime)
    {
        if (path == null || path.Count == 0 || currentWaypointIndex >= path.Count)
            return;

        Vector2 target = path[currentWaypointIndex];
        Vector2 direction = Vector2.Normalize(target - realPosition);
        Vector2 movement = direction * speed * deltaTime;
        
        if (Vector2.Distance(realPosition, target) <= movement.Length())
        {
            realPosition = target;
            position = target;
            currentWaypointIndex++;
        }
        else
        {
            realPosition += movement;
        }
    }

    public void FindNewTarget()
    {
        
    }

    public List<Vector2> FindPath(Node startNode,Node endNode)
    {
        pathCount += 1;
        endNode.ResetNode(0);
        startNode.ResetNode(0);
        endNode.nodeID = pathCount;
        PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();
        priorityQueue.Enqueue(0,startNode);
        while (!priorityQueue.Dequeue().ExpandParent(grid, baseGrid, priorityQueue, endNode))
        {
        }

        endNode.path.Reverse();
        return endNode.path;


    }
    
}