public enum KeyType
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Key : Pickup
{

    public KeyType KeyColor { get; private set; }

}