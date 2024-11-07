using System.Numerics;
using Raylib_cs;

namespace AStar;

public class Button
{
    public static List<Button> Buttons = new List<Button>();
    private Vector2 location;
    private Action function;
    private Vector2 buttonSize;
    private String text;


    private Rectangle buttonBody;

    public Button(Vector2 location_, Vector2 buttonSize_, Action function_,String text_)
    {
        location = location_;
        buttonSize = buttonSize_;
        function = function_;
        Buttons.Add(this);
        text = text_;
        buttonBody = new Rectangle(location, buttonSize);
    }
    public static void DrawTextCentered(Rectangle rect, string text,int fontSize = 20)
    {
        

        int textWidth = Raylib.MeasureText(text, fontSize);
        int textHeight = fontSize;  
        
        int xPosition = (int)(rect.X + (rect.Width - textWidth) / 2);
        int yPosition = (int)(rect.Y + (rect.Height - textHeight) / 2);

       
        Raylib.DrawText(text, xPosition, yPosition, fontSize, Color.Black);
    }

    public static void DrawButtons()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        Rectangle mouseRect = new Rectangle(mousePosition, 1, 1); 
        foreach (var button in Buttons)
        {
            
            Color buttonColour = Color.Gray;
            if (Raylib.CheckCollisionRecs(mouseRect, button.buttonBody))
            {
                buttonColour = Color.LightGray;
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    buttonColour = Color.DarkGray;
                    
                }

                if (Raylib.IsMouseButtonReleased(MouseButton.Left))
                {
                    button.function();
                }
            }
            Raylib.DrawRectangleRec(button.buttonBody,buttonColour);
            DrawTextCentered(button.buttonBody, button.text);
        }
        
    }
    
}