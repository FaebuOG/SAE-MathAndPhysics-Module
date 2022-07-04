using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip : MonoBehaviour
{
    // Rb of pirateship
    private Rigidbody rigidbody;
    
    // Wind Zones
    private bool inWindZone = false;
    private GameObject windZone;

    // Cloth Components
    [SerializeField] private Cloth pirateFlag;
    [SerializeField] private Cloth sail1;
    [SerializeField] private Cloth sail2;
    [SerializeField] private Cloth sail3;
    [SerializeField] private Cloth sail4;
    [SerializeField] private Cloth sail5;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (inWindZone)
        {
            // pirate flag cloth
            pirateFlag.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
            
            // sails cloth
            sail1.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
            sail2.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
            sail3.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
            sail4.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
            sail5.externalAcceleration = windZone.GetComponent<WindArea>().Direction;
        }
    }
    private void FixedUpdate()
    {
        // When the object is in Windzone, a vector force should be applied to the rigidbody of the pirate ship.
        // We do this in the FixedUpdate bc it is physics based.
        // Note: For an advanced system I may need to calculate the center of mass and apply the force there.
        if (inWindZone)
        {
            rigidbody.AddForce(windZone.GetComponent<WindArea>().Direction * windZone.GetComponent<WindArea>().Strenght, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("windArea"))
        {
            windZone = other.gameObject;
            inWindZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("windArea"))
            inWindZone = false;
    }
}
