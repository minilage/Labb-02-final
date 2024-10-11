public class Snake : Enemy  // Ärver av Enemy
{
    public Snake(int x, int y, Random random) : base(x, y, 's', ConsoleColor.Green)
    {
        this.Name = "Snake";
        this.Health = 25;
        this.StartHealth = Health;
        this.IsFighting = false;
        AttackDice = new Dice(3, 4, 2, random);
        DefenceDice = new Dice(1, 8, 5, random);
    }

    public override void Update(Direction dir)
    {
        Move(dir);
    }
}