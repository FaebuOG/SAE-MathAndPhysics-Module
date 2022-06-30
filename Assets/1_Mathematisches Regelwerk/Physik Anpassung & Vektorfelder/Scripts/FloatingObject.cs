using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Transform[] floaters;

    // Air Forces
    [SerializeField] private float airDrag = 0f;
    [SerializeField] private float airAngularDrag = 0.05f;
    // Underwater Forces
    [SerializeField] private float underWaterDrag = 3f;
    [SerializeField] private float underWaterAngularDrag = 1f;
    [SerializeField] private float floatingPower = 15f;
    
    // Water levels
    [SerializeField] private float waterHeight = 0;
    [SerializeField] private int floatersUnderWater;
    [SerializeField] private bool underWater;

    // Rigidibody
    private Rigidbody rigidbody;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // We make the logic in FixedUpdate bc it is Physic based.
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
