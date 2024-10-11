public abstract class Enemy : LevelElement
{
    public bool IsFighting { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int StartHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public Enemy(int x, int y, char icon, ConsoleColor consoleColor) : base(x, y, icon, consoleColor)
    {

    }

    public string GetString_Attack()
    {
        return $"{Name} (ATK: {AttackDice.ToString()} -> {AttackDice.Damage,2} dmg)";
    } 

    public string GetString_Defence()
    {
        return $"{Name} (DEF: {DefenceDice.ToString()} -> {DefenceDice.Damage,2} dmg)";
    }

    public abstract void Update(Direction dir); // Update - det som fienden ska göra på sitt drag

    public void ThrowDice()
    {
        AttackDice.ThrowDice();
        DefenceDice.ThrowDice();
    }
}