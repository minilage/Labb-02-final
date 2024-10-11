public class GameLoop
{
    public void Start(LevelData level)
    {
        level.Initialize();

        while (level.IsGameRunning)
        {
            Console.CursorVisible = false;

            level.Player.RemoveOld();
            foreach (LevelElement item in level.Elements) { item.RemoveOld(); item.Draw(); }
            level.Player.Draw();

            level.PlaySound(level.Sound);

            Direction dir = Direction.None;
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow: { dir = Direction.Up; break; }
                case ConsoleKey.DownArrow: { dir = Direction.Down; break; }
                case ConsoleKey.LeftArrow: { dir = Direction.Left; break; }
                case ConsoleKey.RightArrow: { dir = Direction.Right; break; }
                case ConsoleKey.Backspace: { level.IsGameRunning = false; break; }
            }

            level.MovePlayer(dir);
            level.MoveAllEnemies();
            level.RemoveDeadEnemies();
            level.WritePlayer_Turns();
            level.Update_Visibility();
            level.CheckGameOver();
        }
    }
}