public class Rat : Enemy  // Ärver av Enemy
{
    public Rat(int x, int y, Random random) : base(x, y, 'r', ConsoleColor.Red)
    {
        this.Name = "Rat";
        this.Health = 10;
        this.StartHealth = Health;
        this.IsFighting = false;
        AttackDice = new Dice(1, 6, 3, random);
        DefenceDice = new Dice(1, 6, 1, random);
    }

    public override void Update(Direction dir)
    {
        Move(dir);
    }
}