using System.Numerics;
using AStar;
using Raylib_cs;
using System.IO;
using System.Text.Json;

public class Program
{
    public static void Main()
    {
        Raylib.InitWindow(950, 751, "AStar");
        Raylib.SetTargetFPS(300);
        Grid grid = new Grid(75,75,10);
        grid.createGridLines();

        Painter painter = new Painter(grid);
        bool isPaused = false;
        bool isRunning = false;
        
        void Save()
        {
            SaveGridToJson(grid.grid,"save.json");
        }

        void Load()
        {
            grid.grid = LoadGridFromJson("save.json");
            for (int i = 0; i < grid.grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.grid.GetLength(1); j++)
                {
                    if (grid.grid[i, j] == 5)
                    {
                        painter.Targets.Add(new Vector2(i*grid.cellSize,j*grid.cellSize));
                    }
                    else if (grid.grid[i, j] == 1)
                    {
                        painter.Walls.Add(new Vector2(i*grid.cellSize,j*grid.cellSize));
                    }
                    else if (grid.grid[i, j] == 6)
                    {
                        painter.Pathers[new Vector2(i,j)] = new PathingObject(grid, new Vector2(i,j),painter.Targets);
                        painter.pathersCount += 1;
                    }
                }
            }
        }

        void RunPathing()
        {
            foreach (var pather in painter.Pathers.Values)
            {
                if (!pather.isRunning)
                {
                    pather.isRunning = true;
                    pather.Run();
                }

            }

            isRunning = true;
        }

        void Pause()
        {
            isPaused = !isPaused;
        }

        int currentSpeed = 1500;
        void increaseSpeed()
        {
            currentSpeed += 75;
        }

        void decreaseSpeed()
        {
            currentSpeed -= 75;
        }
    
        float pathsPerSecond = 0;
        void drawTextInfo()
        {
            Raylib.DrawText("FPS: " +  Raylib.GetFPS(),770,644,16,Color.Black);
            Raylib.DrawText("Object Count: " +  painter.pathersCount,770,670,16,Color.Black);
            Raylib.DrawText("Path Count: " +  PathingObject.pathCount,770,700,16,Color.Black);
            Raylib.DrawText("Path Per Second: " +  pathsPerSecond,770,730,16,Color.Black);
            Raylib.DrawText(currentSpeed.ToString(),835,257,16,Color.Black);
        }
        Button saveButton = new Button(new Vector2(800, 10), new Vector2(100, 70), Save,"Save");
        Button runButton = new Button(new Vector2(800, 89), new Vector2(100, 70), RunPathing,"Run");
        Button pauseButton = new Button(new Vector2(800, 170), new Vector2(100, 70), Pause,"Pause");
        Button loadButton = new Button(new Vector2(800, 560), new Vector2(100, 70), Load,"Load");
        Button increaseSpeedButton = new Button(new Vector2(870, 250), new Vector2(30, 30), increaseSpeed,"+");
        Button decreaseSpeedButton = new Button(new Vector2(800, 250), new Vector2(30, 30), decreaseSpeed,"-");

        float timer = 0;
        int pathsCountStart = 0;
        while (!Raylib.WindowShouldClose())
        {
            float deltaTime = Raylib.GetFrameTime();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            grid.drawGridLines(Color.Black);
            painter.UpdatePainter();
            timer += deltaTime;
            if (timer > 1.5)
            {
                timer = 0;
                int change = PathingObject.pathCount - pathsCountStart;
                pathsPerSecond =  change / 5;

            }

            if (timer == 0)
            {
                pathsCountStart = PathingObject.pathCount;
            }
            
            
            if (isRunning && !isPaused)
            {
                foreach (var pathers in painter.Pathers.Values)
                {
                    if(!pathers.completedPath)
                        pathers.StepPath(deltaTime,currentSpeed);
                }
            }

            drawTextInfo();
            Button.DrawButtons();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
    
    
    
    public static int[,] LoadGridFromJson(string filePath)
    {
        // Read JSON from the file
        string jsonString = File.ReadAllText(filePath);
        int[][] jaggedArray = JsonSerializer.Deserialize<int[][]>(jsonString);

        // Convert jagged array back to 2D array
        int rows = jaggedArray.Length;
        int cols = jaggedArray[0].Length;
        int[,] grid = new int[rows, cols];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = jaggedArray[i][j];
            }
        }

        return grid;
    }
    
    public static void SaveGridToJson(int[,] grid, string filePath)
    {
        // Convert the 2D array to a jagged array
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[][] jaggedArray = new int[rows][];
        
        for (int i = 0; i < rows; i++)
        {
            jaggedArray[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                jaggedArray[i][j] = grid[i, j];
            }
        }

        // Serialize to JSON and save
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(jaggedArray, jsonOptions);
        File.WriteAllText(filePath, jsonString);
    }




}