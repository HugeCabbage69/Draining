using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 10, -10);

    public float positionSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lastTargetPosition;

    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionSmoothTime);

        Vector3 targetMovement = (target.position - lastTargetPosition) / Time.deltaTime;
        Vector3 lookTarget = target.position + targetMovement * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime);

        lastTargetPosition = target.position;
    }
}
