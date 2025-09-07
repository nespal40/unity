using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float speed_distance;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 distance_border;
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform center_object_point;
    [SerializeField] Vector2 angle;
    [SerializeField] private Transform rotation_object;
    [SerializeField] private bool look_at_center;
    [SerializeField] private MeshRenderer player_model;
    [SerializeField] private int[] fov = { 60, 10 };
    [SerializeField] private bool locked_move = false;
    [SerializeField] private bool locked_distance = false;
    Camera main_cam;
    float rad = 180/Mathf.PI;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        position = new Vector3(Mathf.Cos(angle.x), 0, Mathf.Sin(angle.x));
        main_cam = Camera.main;
        main_cam.fieldOfView = fov[0];
    }

    public void SetLock(bool lockedmv,bool lockedd)
    {
        locked_move = lockedmv;
        locked_distance = lockedd;
    }

    public Vector2 GetMoveMouse()
    {
        Vector2 v = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

        //Debug.Log(v.x);Debug.Log(v.y);

        return v;
    }

    public float GetSensitivity() { return sensitivity; }

    public float GetAngleX() { return angle.x; }
    public Vector2 GetAngle() { return angle; }
    public float GetDistance() { float mw = Input.GetAxis("Mouse ScrollWheel"); return distance + mw * speed_distance; }

    void Update()
    {
        float v = GetMoveMouse().x,h = GetMoveMouse().y;
        float mw = Input.GetAxis("Mouse ScrollWheel");

        GetMoveMouse();

        if (Input.GetKeyDown(KeyCode.C))
        {
            main_cam.fieldOfView = fov[1];
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            main_cam.fieldOfView = fov[0];
        }

        if (!locked_distance)
        {
            float _dist = distance + mw * speed_distance;
            if (_dist > distance_border.x && _dist < distance_border.y)
            {
                distance += mw * speed_distance;
                if (distance < 0.4f)
                {
                    if (player_model.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.On)
                        player_model.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
                else
                {
                    if (player_model.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly)
                        player_model.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }

        if ((v != 0 || h != 0) && !locked_move)
        {
            float s = sensitivity;
            if (main_cam.fieldOfView == fov[1])
                s /= 2.5f;

            angle.y += (h * s); if (Mathf.Abs(angle.y) >= 90 / rad) angle.y -= (h * s);
            angle.x += (v * s); angle.x %= Mathf.PI * 2;
            position = new Vector3(Mathf.Cos(angle.x) * Mathf.Cos(angle.y), Mathf.Sin(angle.y), Mathf.Sin(angle.x) * Mathf.Cos(angle.y));
        }

        rotation_object.position = center_object_point.position + position*distance;
        
        if (look_at_center)
        {
            rotation_object.rotation = Quaternion.Euler(angle.y * rad, -angle.x * rad-90,0);
            //rotation_object.localRotation = Quaternion.identity(-angley,-anglex,0,0);
        }
    }
}
