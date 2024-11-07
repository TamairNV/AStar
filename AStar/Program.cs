using System.Numerics;
using AStar;
using Raylib_cs;

public class Program
{
    public static void Main()
    {
        Raylib.InitWindow(950, 751, "AStar");
        Raylib.SetTargetFPS(60);
        Grid grid = new Grid(75,75,10);
        grid.createGridLines();
        Button saveButton = new Button(new Vector2(750, 10), new Vector2(150, 100), Save,"Save");
        Painter painter = new Painter(grid);
        while (!Raylib.WindowShouldClose())
        {
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            grid.drawGridLines(Color.Black);
            painter.UpdatePainter();
            Button.DrawButtons();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static void Save()
    {
        Console.WriteLine("save");
    }
}