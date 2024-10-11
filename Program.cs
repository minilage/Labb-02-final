Console.Title = "Labb 02 - Tina";  

GameLoop gameloop = new GameLoop();

while (true)
{
    LevelData level = new LevelData();
    level.Load("Levels\\Level1.txt");

    if (level.Elements.Count < 10)
    {
        Console.WriteLine("Kan inte ladda in Level1.");
        Console.WriteLine();
        Console.Write("<<< Tryck tangent för att avsluta >>>");
        Console.ReadKey();
        return;
    }

    gameloop.Start(level);
}