using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float Strenght;
    public Vector3 Direction;

    // Wind force in knots "sailing"
    public int knots_X = 0;
    
    
    public void IncreaseStrenght()
    {
        if (knots_X < 1000)
        {
            knots_X += 100; 
            Direction = new Vector3(knots_X, 0, 0);
        }
    }
    public void DecreaseStrenght()
    {
        if (knots_X > -1000)
        {
            knots_X -= 100; 
            Direction = new Vector3(knots_X, 0, 0);
        }
    }
}
