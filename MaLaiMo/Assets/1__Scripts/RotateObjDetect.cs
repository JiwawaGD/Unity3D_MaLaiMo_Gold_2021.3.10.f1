using UnityEngine;

public class RotateObjDetect : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad;

            if (Vector3.Dot(transform.up, Vector3.up) > 0)
                transform.RotateAround(transform.position, transform.up, rotX);
            else
                transform.RotateAround(transform.position, transform.up, -rotX);
        }
    }
}