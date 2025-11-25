public enum AmmoType
{
    Soft,
    FMJ,
    HP,
    AP
}

public class AmmoBox : Pickup<AmmoType, AmmoType>
{
    
    public string Caliber { get; protected set; }
    public int Count { get; protected set; }
    public int Damage { get; protected set; }
    
    public override AmmoType HandlePickup()
    {
        gameObject.SetActive(false);
        return Type;
    }
}