using System;

[Serializable]
public class InventorySlot
{
    public SeedData seed;
    public int quantity;
    public InventorySlot(SeedData seed, int amount)
    {
        this.seed = seed;
        quantity = amount;
    }
}
