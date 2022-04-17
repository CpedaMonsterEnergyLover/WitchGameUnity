[System.Serializable]
public class ItemIdentifier
{
    public ItemType type;
    public string id;
    public ItemIdentifier(ItemType type)
    {
        this.type = type;
        id = "";
    }

    public override string ToString()
    {
        return $"{type}:{id}";
    }
}
