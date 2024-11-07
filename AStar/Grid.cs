using System.Numerics;
using Raylib_cs;
namespace AStar;


public class Grid
{
    public Vector2 gridSize;
    public int[,] grid;
    private List<Rectangle> lines = new List<Rectangle>();
    public int cellSize;

    public Grid(int width, int height,int cellSize_)
    {
        gridSize = new Vector2(width, height);
        grid = new int[width, height];
        cellSize = cellSize_;

    }

    public void createGridLines(int lineThickness = 2)
    {
        for (int x = 0; x <= gridSize.X; x++)
        {
            Rectangle rect = new Rectangle(x * cellSize, 0, lineThickness, gridSize.Y * cellSize+lineThickness);
            lines.Add(rect);
        }
        for (int x = 0; x <= gridSize.Y; x++)
        {
            Rectangle rect = new Rectangle(0,x*cellSize,gridSize.X*cellSize,lineThickness);
            lines.Add(rect);
        }
        
    }

    public void drawGridLines(Color colour)
    {
        foreach (var line in lines)
        {
            Raylib.DrawRectangleRec(line,colour);
        }
        
    }
    
    
    
    


}