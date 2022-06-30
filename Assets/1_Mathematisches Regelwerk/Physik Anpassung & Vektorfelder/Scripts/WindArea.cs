using System;
using UnityEngine;
using TMPro;

public class WindArea : MonoBehaviour
{
    public float Strenght;
    [NonSerialized] public Vector3 Direction;

    // Wind force in knots "sailing"
    [SerializeField] private int knots_X = 0;
    [SerializeField] private int knots_Y = 0;
    [SerializeField] private int knots_Z = 0;
    
    [SerializeField] private TextMeshProUGUI knotsStrenght;
    public void IncreaseStrenght()
    {
        if (knots_X < 1000)
        {
            knots_X += 100;
            Direction = new Vector3(knots_X, knots_Y, knots_Z);
            
            // UI
            knotsStrenght.text = knots_X.ToString();
        }
    }
    public void DecreaseStrenght()
    {
        if (knots_X > -1000)
        {
            knots_X -= 100; 
            Direction = new Vector3(knots_X, 0, 0);
            
            // UI
            knotsStrenght.text = knots_X.ToString();
        }
    }
}
