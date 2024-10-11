
public class LevelData
{
    // Instance variables
    private List<LevelElement> _elements;  // field
    private Player player;  // field
    private int intTurns;
    private readonly Random random = new Random();

    // Properties
    public Player Player { get { return player; } }
    public List<LevelElement> Elements { get { return _elements; } } // propfull  (public readonly property - Elements)
    public bool IsGameRunning { get; set; }
    public Sound Sound { get; set; }

    public void Load(string fileName)
    {
        _elements = new List<LevelElement>();

        List<string> linesFile = LoadFileLevel(fileName);

        for (int y = 0; y < linesFile.Count; y++)
        {
            string line = linesFile[y];
            for (int x = 0; x < line.Length; x++)
            {
                char ch = line[x];
                switch (ch)
                {
                    case '#': { _elements.Add(new Wall(x, y)); break; }
                    case 'r': { _elements.Add(new Rat(x, y, random)); break; }
                    case 's': { _elements.Add(new Snake(x, y, random)); break; }
                    case '@': { player = new Player(x, y, random); break; }
                }
            }
        }
    }

    private List<string> LoadFileLevel(string fileName)
    {
        StreamReader streamReader = null;
        List<string> listFileLines = new List<string>();

        if (!File.Exists(fileName)) return listFileLines;

        try
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            streamReader = new StreamReader(fileStream);

            while (!streamReader.EndOfStream) listFileLines.Add(streamReader.ReadLine());
            return listFileLines;
        }
        catch
        {
            return listFileLines;
        }
        finally
        {
            if (streamReader != null) streamReader.Close();
        }
    }

    public void Initialize()
    {
        IsGameRunning = true;
        intTurns = 0;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        WritePlayer_Name();
        WritePlayer_Health();
        WritePlayer_Turns();
        Update_Visibility();
    }

    private void WritePlayer_Name()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Name: Tina");
    }

    private void WritePlayer_Health()
    {
        Console.SetCursorPosition(18, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Health:{player.Health,3}/{player.StartHealth,3}");
    }

    public void WritePlayer_Turns()
    {
        Console.SetCursorPosition(57, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Turns:{intTurns,4}");
    }

    public LevelElement GetElementAt(LevelElement element, Direction dir)
    {
        int x = element.X;
        int y = element.Y;

        switch (dir)
        {
            case Direction.Up: { y--; break; }
            case Direction.Down: { y++; break; }
            case Direction.Left: { x--; break; }
            case Direction.Right: { x++; break; }
        }

        foreach (var item in _elements) { if (item.X == x && item.Y == y) return item; }

        return new EmptyElement(x, y);
    }

    public void MovePlayer(Direction dir)
    {
        LevelElement e = GetElementAt(player, dir);
        if (e is EmptyElement)
        {
            intTurns++;
            player.Move(dir);
        }
        if (e is EmptyElement || e is Wall)
        {
            Console.SetCursorPosition(40, 0);
            Console.Write($"            ");
            RemoveAttackRow1();
            RemoveAttackRow2();
        }
        if (e is Rat) Attack((Rat)e, true);
        if (e is Snake) Attack((Snake)e, true);
        if (e is Enemy && ((Enemy)e).Health == 0) RemoveEnemy(e);
    }

    private void RemoveEnemy(LevelElement enemy)
    {
        enemy.ForceRemoveOld();
        _elements.Remove(enemy);
        Sound = Sound.Kill;
    }

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.Kill: Console.Beep(600, 80); Console.Beep(1080, 60); break;
            case Sound.Attack: Console.Beep(500, 80); break;
            case Sound.GameOver: Console.Beep(600, 100); Console.Beep(400, 100); break;
        }
        Sound = Sound.None;
    }

    private void Attack(Enemy enemy, bool isPlayerAttacking)
    {
        enemy.IsFighting = true;
        enemy.ThrowDice();
        player.ThrowDice();

        if (isPlayerAttacking)
        {
            intTurns++;
            Attack_PlayerToEnemy(enemy, 1);
            if (enemy.Health > 0) Attack_EnemyToPlayer(enemy, 2); else RemoveAttackRow2();
        }
        else
        {
            Attack_EnemyToPlayer(enemy, 1);
            if (player.Health > 0) Attack_PlayerToEnemy(enemy, 2); else RemoveAttackRow2();
        }

        WritePlayer_Health();
        WriteEnemy_Name(enemy);
        Sound = Sound.Attack;
    }

    private void WriteEnemy_Name(Enemy enemy)
    {
        Console.SetCursorPosition(40, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{enemy.Name}:{enemy.Health,2}/{enemy.StartHealth,2}");
    }

    private void RemoveAttackRow1()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write($"                                                                    ");
    }

    private void RemoveAttackRow2()
    {
        Console.SetCursorPosition(0, 2);
        Console.Write($"                                                                    ");
    }

    private void Attack_EnemyToPlayer(Enemy enemy, int pos)
    {
        int dmgToPlayer = enemy.AttackDice.Damage - player.DefenceDice.Damage;
        if (dmgToPlayer < 0) dmgToPlayer = 0;
        player.Health -= dmgToPlayer;
        if (player.Health <= 0) player.Health = 0;
        Console.SetCursorPosition(0, pos);
        Console.ForegroundColor = dmgToPlayer == 0 ? ConsoleColor.Gray : ConsoleColor.Red;
        Console.Write($"{enemy.GetString_Attack()} attack the {player.GetString_Defence()}");
    }

    private void Draw_GameOver()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(20, 8);
        Console.Write($"                            ");
        Console.SetCursorPosition(20, 9);
        Console.Write($"     <<<  Game Over  >>>    ");
        Console.SetCursorPosition(20, 10);
        Console.Write($"                            ");

        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(22, 13);
        Console.Write($"                        ");
        Console.SetCursorPosition(22, 14);
        Console.Write($"   Press [Backspace]    ");
        Console.SetCursorPosition(22, 15);
        Console.Write($"                        ");
        Console.SetCursorPosition(22, 16);
        Console.Write($"      to continue       ");
        Console.SetCursorPosition(22, 17);
        Console.Write($"                        ");

        PlaySound(Sound.GameOver);

        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Backspace) return;
        }
    }

    private void Attack_PlayerToEnemy(Enemy enemy, int pos)
    {
        int dmgToEnemy = player.AttackDice.Damage - enemy.DefenceDice.Damage;
        if (dmgToEnemy < 0) dmgToEnemy = 0;
        Console.SetCursorPosition(0, pos);
        Console.ForegroundColor = dmgToEnemy == 0 ? ConsoleColor.Gray : ConsoleColor.Green;
        Console.Write($"{player.GetString_Attack()} attack the {enemy.GetString_Defence()}");
        enemy.Health -= dmgToEnemy;
        if (enemy.Health <= 0) enemy.Health = 0;
    }

    public void RemoveDeadEnemies()
    {
        foreach (var item in _elements)
        {
            if (item is Enemy && ((Enemy)item).Health == 0)
            {
                RemoveEnemy(item);
                return;
            }
        }
    }

    public void MoveAllEnemies()
    {
        foreach (var item in _elements)
        {
            if (player.X == item.X && player.Y == item.Y) continue;

            if (item is Rat) Move_Rat((Rat)item);
            if (item is Snake) Move_Snake((Snake)item);
        }
    }

    private void Move_Rat(Rat rat)
    {
        if (rat.IsFighting) { rat.IsFighting = false; return; }

        List<Direction> possibleDir = new List<Direction>();
        if (GetElementAt(rat, Direction.Up) is EmptyElement) possibleDir.Add(Direction.Up);
        if (GetElementAt(rat, Direction.Down) is EmptyElement) possibleDir.Add(Direction.Down);
        if (GetElementAt(rat, Direction.Left) is EmptyElement) possibleDir.Add(Direction.Left);
        if (GetElementAt(rat, Direction.Right) is EmptyElement) possibleDir.Add(Direction.Right);

        if (possibleDir.Count == 0) return;
        possibleDir.Add(Direction.None);
        int rnd = random.Next(0, possibleDir.Count);
        LevelElement e = GetElementAt(rat, possibleDir[rnd]);
        if (player.X == e.X && player.Y == e.Y) Attack(rat, false); else rat.Update(possibleDir[rnd]);
    }

    private void Move_Snake(Snake snake)
    {
        if (snake.IsFighting) { snake.IsFighting = false; return; }

        int distance = GetPlayerDistanceTo(snake);
        if (distance > 2) return;

        List<Direction> possibleDir = new List<Direction>();
        LevelElement e;
        bool isMovePossible;

        e = GetElementAt(snake, Direction.Up);
        isMovePossible = e is EmptyElement && GetPlayerDistanceTo(e) >= distance;
        if (isMovePossible) possibleDir.Add(Direction.Up);

        e = GetElementAt(snake, Direction.Down);
        isMovePossible = e is EmptyElement && GetPlayerDistanceTo(e) >= distance;
        if (isMovePossible) possibleDir.Add(Direction.Down);

        e = GetElementAt(snake, Direction.Left);
        isMovePossible = e is EmptyElement && GetPlayerDistanceTo(e) >= distance;
        if (isMovePossible) possibleDir.Add(Direction.Left);

        e = GetElementAt(snake, Direction.Right);
        isMovePossible = e is EmptyElement && GetPlayerDistanceTo(e) >= distance;
        if (isMovePossible) possibleDir.Add(Direction.Right);

        if (possibleDir.Count == 0) return;
        int rnd = random.Next(0, possibleDir.Count);
        e = GetElementAt(snake, possibleDir[rnd]);
        if (player.X == e.X && player.Y == e.Y) Attack(snake, false); else snake.Update(possibleDir[rnd]);
    }

    private int GetPlayerDistanceTo(LevelElement item)
    {
        return Math.Abs(item.X - player.X) + Math.Abs(item.Y - player.Y);
    }

    public void Update_Visibility()
    {
        foreach (var item in _elements)
        {
            double distance = Math.Sqrt(Math.Pow(item.X - player.X, 2) + Math.Pow(item.Y - player.Y, 2));
            bool inSight = distance <= 6; // Om avståndet är mindre än eller lika med 6

            if (item is Wall && inSight) item.SetVisibility(true);
            if (item is Enemy) item.SetVisibility(inSight);
        }
    }

    public void CheckGameOver()
    {
        if (player.Health <= 0)
        {
            player.Health = 0;
            WritePlayer_Health();
            IsGameRunning = false;
            Draw_GameOver();
        }
    }
}