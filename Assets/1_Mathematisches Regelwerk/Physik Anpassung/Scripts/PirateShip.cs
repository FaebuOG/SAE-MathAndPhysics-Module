using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip : MonoBehaviour
{
    public float Lifetime = 10f;
    public bool InWindZone = false;
    public GameObject WindZone;

    private Rigidbody rigidbody;

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
        if (InWindZone)
        {
            // pirate flag cloth
            pirateFlag.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
            
            // sails cloth
            sail1.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
            sail2.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
            sail3.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
            sail4.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
            sail5.externalAcceleration = WindZone.GetComponent<WindArea>().Direction;
        }
    }
    private void FixedUpdate()
    {
        if (InWindZone)
        {
            rigidbody.AddForce(WindZone.GetComponent<WindArea>().Direction * WindZone.GetComponent<WindArea>().Strenght);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "windArea")
        {
            WindZone = other.gameObject;
            InWindZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "windArea")
            InWindZone = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "destroyer")
        {
            Destruction();
        }
    }

    void Destruction()
    {
        Destroy(this.gameObject);
    }

    
}
