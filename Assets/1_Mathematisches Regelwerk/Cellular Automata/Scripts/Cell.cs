using UnityEngine;

public class Cell : MonoBehaviour
{
    
    public States State { get => state; private set => state = value; }
    public float TimeWhenStartedBurning { get => timeWhenStartedBurning; private set => timeWhenStartedBurning = value; }
    
    // childs
    [SerializeField] private GameObject air;
    [SerializeField] private GameObject starting;
    [SerializeField] private GameObject burning;
    [SerializeField] private GameObject burningWithSmoke;
    [SerializeField] private GameObject burned;

    [SerializeField] private States state = States.Empty;
    
    float timeWhenStartedBurning;
    
    public enum States
    {
        Empty,
        Air,
        Starting,
        Burning,
        Burned
    }

    public void SetState(States state)
    {
        this.state = state;
        if(gameObject.activeInHierarchy==false)
        this.gameObject.SetActive(true);
        switch (state)
        {
            case States.Empty:
                
                break;
            case States.Air:
                break;
            case States.Starting:
                starting.SetActive(true);
                break;
            case States.Burning:
                timeWhenStartedBurning = Time.time;
                starting.SetActive(false);
                burning.SetActive(true);
                break;
            case States.Burned:
                burning.SetActive(false);
                burned.SetActive(true);
                break;
               
        }
          
    }
    public void DeleteCell()
    {
        Destroy(this.gameObject);
    }

}
