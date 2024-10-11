public class Dice
{
    private readonly int numberOfDice;
    private readonly int sidesPerDice;
    private readonly int modifier;
    private readonly Random random;
    public int Damage { get; set; }

    public Dice(int numberOfDice, int sidesPerDice, int modifier, Random random)
    {
        this.numberOfDice = numberOfDice;
        this.sidesPerDice = sidesPerDice;
        this.modifier = modifier;
        this.random = random;
    }

    public void ThrowDice()
    {
        int sum = 0;
        for (int i = 0; i < numberOfDice; i++) sum += random.Next(1, sidesPerDice + 1);
        Damage = sum + modifier;
    }

    public override string ToString()
    {
        return $"{numberOfDice}d{sidesPerDice}+{modifier}";
    }
}