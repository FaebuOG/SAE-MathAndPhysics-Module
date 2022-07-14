using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        camera.transform.position = followTarget.transform.position + offset;
    }
}