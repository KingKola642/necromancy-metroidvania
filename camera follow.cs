using UnityEngine;

public class camerafollow : MonoBehaviour
{

    public Vector3 Offset;
    public float FixedY;
    public Transform Target;
    public float SmoothSpeed = 0.125f;
    public Vector3 SmoothedPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 DesiredPosition = new Vector3(Target.position.x, FixedY, 0) + Offset;
        SmoothedPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);
    }
    void LateUpdate()
    {
        transform.position = SmoothedPosition;
    }
}
