public enum KeyType
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Key : Pickup<KeyType, KeyType>
{

    public override KeyType HandlePickup()
    {
        gameObject.SetActive(false);
        return Type;
    }
}