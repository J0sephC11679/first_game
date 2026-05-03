using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Update()
    {
        float targetX = target.position.x;
        float targetY = target.position.y + yOffset;

        float clampedX = Mathf.Clamp(targetX, minX, maxX);
        float clampedY = Mathf.Clamp(targetY, minY, maxY);

        Vector3 newPos = new Vector3(clampedX, clampedY, -10f);

        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}