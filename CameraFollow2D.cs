using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 shakeOffset = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow2D: No target assigned.");
            return;
        }

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothed + shakeOffset;

    }

    public void SetShakeOffset(Vector3 shake)
    {
        shakeOffset = shake; 
    }

}

