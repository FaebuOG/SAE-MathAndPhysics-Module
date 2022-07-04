using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Range(0, 100)] public int ShopPopularityPercent;
    public bool StoreIsOpen;
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
