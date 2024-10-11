public class Player : LevelElement
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int StartHealth { get; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public Player(int x, int y, Random random) : base(x, y, '@', ConsoleColor.White) // constructor chaining (# & color är hårdkodade)
    {
        Name = "Player";
        this.Health = 100;
        this.StartHealth = Health;
        AttackDice = new Dice(2, 6, 2, random);
        DefenceDice = new Dice(2, 6, 0, random);
        SetVisibility(true);
    }

    public string GetString_Attack()
    {
        return $"{Name} (ATK: {AttackDice.ToString()} -> {AttackDice.Damage,2} dmg)";
    }

    public string GetString_Defence()
    {
        return $"{Name} (DEF: {DefenceDice.ToString()} -> {DefenceDice.Damage,2} dmg)";
    }

    public void ThrowDice()
    {
        AttackDice.ThrowDice();
        DefenceDice.ThrowDice();
    }
}