using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;  
using System.IO;  

public class UpgradeManager : MonoBehaviour
{
    // Dictionary with upgrade name and multiplier value
    public Dictionary<string, float> upgradesDictionary = new Dictionary<string, float>();

    void Start()
    {
        LoadUpgrades();
    }

    void LoadUpgrades()
    {
        // get XML file
        TextAsset xmlFile = Resources.Load<TextAsset>("Upgrades");

        if (xmlFile == null)
        {
            Debug.LogError("Could not find Upgrades.xml");
            return;
        }

        // XmlReader
        using (XmlReader reader = XmlReader.Create(new StringReader(xmlFile.text)))
        {
            
            string currentName = "";
            float currentValue = 0f;

            while (reader.Read())
            {
                // start element, looks for upgrade's name and multiplier value
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            currentName = reader.ReadElementContentAsString();
                            break;

                        case "multiplierValue":
                            currentValue = reader.ReadElementContentAsFloat();
                            break;
                    }
                }
                // end element, looks for end of an upgrade
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Upgrade")
                {
                    //add upgrade to dictionary
                    if (currentName != "")
                    {
                        upgradesDictionary.Add(currentName, currentValue);
                        Debug.Log($"New Upgrade: {currentName}, Value: {currentValue}");

                        currentName = "";
                        currentValue = 0f;
                    }
                }
            }
        }

        Debug.Log($"Total upgrades: {upgradesDictionary.Count}");
    }
}