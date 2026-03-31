using System.Collections;
using System.Collections.Generic;
using UnityEngine;



  [CreateAssetMenu(menuName ="Seeds")]
  public class SeedData : ScriptableObject
    {
        public string seedName;
        public GameObject plantPrefab;
        public float growTime;
        public Sprite icon;
        public int price;
        public int sellPrice;
    }

