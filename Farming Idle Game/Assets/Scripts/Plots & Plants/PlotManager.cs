using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    // Drag ALL your PlotSign objects (IDs 1–31) into this list in inspector
    [SerializeField] private List<PlotSign> plotSigns = new List<PlotSign>();

    private HashSet<int> purchasedPlots = new HashSet<int>();

    public void RegisterPurchase(int id)
    {
        purchasedPlots.Add(id);
    }

    // Save system uses this
    public List<int> GetPurchasedPlotIDs()
    {
        return new List<int>(purchasedPlots);
    }

    // Load system uses this
    public void LoadPurchasedPlots(List<int> ids)
    {
        purchasedPlots = new HashSet<int>(ids);

        foreach (int id in purchasedPlots)
        {
            PlotSign sign = plotSigns.FirstOrDefault(s => s.IDnum == id);

            if (sign != null)
            {
                sign.ForceLoadPurchase();
            }
            else
            {
                Debug.LogWarning("No PlotSign found with ID: " + id);
            }
        }
    }
}