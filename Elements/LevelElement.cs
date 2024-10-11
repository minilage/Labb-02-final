public abstract class LevelElement
{
    private bool isChanged;
    private int oldX;
    private int oldY;
    private bool isVisible;

    public int X { get; set; }
    public int Y { get; set; }
    protected char Icon { get; set; }

    protected ConsoleColor Color { get; set; }

    public LevelElement(int x, int y, char icon, ConsoleColor consoleColor)  // en constructor
    {
        this.X = x;
        this.Y = y;
        this.Icon = icon;
        this.Color = consoleColor;
        this.isChanged = true;
        this.oldX = x;
        this.oldY = y;
        this.isVisible = false;
    }

    public void SetVisibility(bool visible)
    {
        if (isVisible != visible)
        {
            isChanged = true; isVisible = visible;
        }
    }

    public void Draw()  // inga parametrar
    {
        // anropas för att skriva ut LevelElement med rätt tecken på rätt plats
        if (isChanged)
        {
            Console.SetCursorPosition(X, Y + 3);
            Console.ForegroundColor = isVisible ? Color : ConsoleColor.Black;
            Console.Write(Icon);
            isChanged = false;
            oldX = X;
            oldY = Y;
        }
    }

    public void RemoveOld()
    {
        if (isChanged)
        {
            Console.SetCursorPosition(oldX, oldY + 3);
            Console.Write(' ');
        }
    }

    public void ForceRemoveOld()
    {
        Console.SetCursorPosition(oldX, oldY + 3);
        Console.Write(' ');
    }

    public void Move(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: if (isVisible) isChanged = true; Y--; break;
            case Direction.Down: if (isVisible) isChanged = true; Y++; break;
            case Direction.Left: if (isVisible) isChanged = true; X--; break;
            case Direction.Right: if (isVisible) isChanged = true; X++; break;
        }
    }
}