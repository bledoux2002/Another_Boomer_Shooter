public enum ArmorType
{
    Normal,
    Overload
}

public class Armor : Pickup<ArmorType, int>
{

    public int Amount { get; private set; } //possible make protected set, might have two subclasses of armor for over-loading, or just implement here

    public override int HandlePickup()
    {
        gameObject.SetActive(false);
        return Amount;
    }

}