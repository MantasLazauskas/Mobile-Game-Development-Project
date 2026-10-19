using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 18f;
    public float turnSpeed = 90f;

    bool canDrive = true;
    float groundY;

    void Start()
    {
        groundY = transform.position.y;
        // Make sure the car starts flat
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

    void Update()
    {
        if (!canDrive) return;

        // Keep car flat and on the ground
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        // Always move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Touch steering only
        if (Input.touchCount > 0)
        {
            float x = Input.GetTouch(0).position.x;
            float steer = (x < Screen.width * 0.5f) ? -1f : 1f;
            transform.Rotate(0f, steer * turnSpeed * Time.deltaTime, 0f);
        }
    }

    public void Stop()
    {
        canDrive = false;
    }

    public void ResetCar(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
        groundY = pos.y;
        canDrive = true;
    }
}