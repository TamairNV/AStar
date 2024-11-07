using Raylib_cs;

namespace AStar;

public class DrawObject
{
    private static List<DrawObject> ObjectsToDraw = new List<DrawObject>();
    private Rectangle rect;

    public DrawObject()
    {
        ObjectsToDraw.Add(this);
        
    }
}