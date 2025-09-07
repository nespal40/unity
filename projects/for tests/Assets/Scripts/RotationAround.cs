using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAround : MonoBehaviour
{
    [SerializeField] private Vector2 distance;
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform center_object_point;
    [SerializeField] Vector2 angle;
    [SerializeField] Vector2 speed_rotation;
    [SerializeField] private Transform rotation_object;
    [SerializeField] private bool look_at_center;
    float rad = 180 / Mathf.PI;

    void Start()
    {
        position = new Vector3(Mathf.Cos(angle.x), 0, Mathf.Sin(angle.x));
    }

    public float GetAngleX() { return angle.x; }

    void Update()
    {
        angle.y += speed_rotation.y*Time.deltaTime; angle.y %= Mathf.PI * 2;
        angle.x += speed_rotation.x*Time.deltaTime; angle.x %= Mathf.PI * 2;
        position = new Vector3(Mathf.Cos(angle.x)*distance.x, Mathf.Sin(angle.y)*distance.y, Mathf.Sin(angle.x) * distance.x);

        rotation_object.position = center_object_point.position + position;

        if (look_at_center)
        {
            rotation_object.rotation = Quaternion.Euler(angle.y * rad, -angle.x * rad - 90, 0);
            //rotation_object.localRotation = Quaternion.identity(-angley,-anglex,0,0);
        }
    }
}
