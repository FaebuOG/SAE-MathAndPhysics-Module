using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    [SerializeField] private Transform[] floaters;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    public float floatingPower = 15f;
    private Rigidbody rigidbody;

    public float waterHeight = 0;
    private int floatersUnderWater;
    private bool underWater;

    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - waterHeight;
            
            if (difference < 0)
            {
                rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderWater += 1;
                if (!underWater)
                {
                    underWater = true;
                    SwitchState(true);
                }
            }
            
            if(underWater && floatersUnderWater == 0)
            {
                underWater = false;
                SwitchState(false);
            }
        }
        
        void SwitchState(bool isUnderwater)
        {
            if (isUnderwater)
            {
                rigidbody.drag = underWaterDrag;
                rigidbody.angularDrag = underWaterAngularDrag;
            }
            else
            {
                rigidbody.drag = airDrag;
                rigidbody.angularDrag = airAngularDrag;
            }
            
        }
        
    }
}
