[System.Serializable]
public class ItemIdentifier
{
    public ItemType type;
    public string id;
    
    public ItemIdentifier(ItemType type, string id)
    {
        this.type = type;
        this.id = id;
    }

    public override string ToString()
    {
        return $"{type}:{id}";
    }
}
