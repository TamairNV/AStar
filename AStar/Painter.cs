using System.Numerics;
using System.Runtime.Intrinsics.X86;
using Raylib_cs;
namespace AStar;

public class Painter
{
    public List<Vector2> Targets;
    public List<Vector2> Walls;
    public Dictionary<Vector2, PathingObject> Pathers;
 
    private Rectangle screenRect;
    private int brush = 0;
    private Grid grid;
    private int cellSize;




    public Painter(Grid grid_)
    {
        
        grid = grid_;
        
        cellSize = grid.cellSize;
        screenRect = new Rectangle(0, 0, grid.gridSize.X * cellSize, grid_.gridSize.Y * cellSize);
        Targets = new List<Vector2>();
        Walls = new List<Vector2>();
        Pathers = new Dictionary<Vector2, PathingObject>();
        Button wallsButton = new Button(new Vector2(770,300),new Vector2(100,70),changeToWallsBrush,"wall");
        Button targetButton = new Button(new Vector2(770,375 ),new Vector2(100,70),changeToTargetsBrush,"Target");
        Button playerButton = new Button(new Vector2(770,450),new Vector2(100,70),changeToPlayersBrush,"Pather");
        Button deleteButton = new Button(new Vector2(770,525),new Vector2(100,70),changeToDeleteBrush,"Delete");
        
    }

    public void UpdatePainter()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        Rectangle mouseRect = new Rectangle(mousePosition, 1, 1);
        if (Raylib.CheckCollisionRecs(mouseRect, screenRect))
        {
            int normPosX = (Convert.ToInt16(mousePosition.X) / cellSize) * cellSize;
            int normPosY = (Convert.ToInt16(mousePosition.Y) / cellSize) * cellSize;
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                HandleBrush(new Vector2(normPosX, normPosY));
            } 
        }

        DrawObjects();


    }

    private void DrawObjects()
    {
        foreach (var target in Targets)
        {
            Raylib.DrawRectangle(Convert.ToInt16(target.X),Convert.ToInt16(target.Y),cellSize,cellSize,Color.Green);
            
        }
        foreach (var wall in Walls)
        {
            Raylib.DrawRectangle(Convert.ToInt16(wall.X),Convert.ToInt16(wall.Y),cellSize,cellSize,Color.Black);
        }

        foreach (var pathers in Pathers.Values)
        {
            Raylib.DrawRectangle(Convert.ToInt16(pathers.realPosition.X),Convert.ToInt16(pathers.realPosition.Y),cellSize,cellSize,Color.Gold);

        }
    }

    private void HandleBrush(Vector2 pos)
    {
        switch (brush)
        {
            case 1:
                Walls.Add(pos);
                break;
            case 2:
                Targets.Add(pos);
                break;
            case 3:
                Pathers[pos] = new PathingObject(grid, new Vector2(pos.X/cellSize,pos.Y/cellSize));
                break;
            case 4:
                int[] intPos = new int[2]
                {
                    Convert.ToInt16(pos.X/cellSize),
                    Convert.ToInt16(pos.Y/cellSize)
                };
                grid.grid[intPos[0], intPos[1]] = 0;
                if (Targets.Contains(pos))
                {
                    Targets.Remove(pos);
                    break;
                }

                if (Walls.Contains(pos))
                {
                    
                    Walls.Remove(pos);
                    break;
                    Console.WriteLine("sdkgjh");
                }
                
                
                if (Pathers.ContainsKey(pos))
                {
                    Pathers.Remove(pos);
                }
                break;
                
            default:
                break;
            
        }
    }
    
    

    private void changeToWallsBrush() => brush = 1;
    private void changeToTargetsBrush() => brush = 2;
    private void changeToPlayersBrush() => brush = 3;
    private void changeToDeleteBrush() => brush = 4;

}