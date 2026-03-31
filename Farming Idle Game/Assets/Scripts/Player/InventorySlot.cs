using System;

[Serializable]
public class InventorySlot
{
    public SeedData seed;
    public int quantity;

    public TendingDevice device;
    public int level;
    public InventorySlot(SeedData seed, int amount)
    {
        this.seed = seed;
        quantity = amount;
    }

    public InventorySlot(TendingDevice device, int level)
    {
        this.device = device;
        this.level = level;
    }
}
