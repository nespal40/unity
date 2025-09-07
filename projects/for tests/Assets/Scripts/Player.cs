using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float max_speed;
    [SerializeField] private float plus_speed;
    [SerializeField] private float jump_force;
    float speed;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject creating_obj;
    [SerializeField] private bool shoot_ray;

    [Header("Cursor")]
    [SerializeField] private GameObject[] cursors = new GameObject[3];
    [SerializeField] private float distance_interactable = 5f;
    [SerializeField] private GameObject in_hand;
    [SerializeField] private Vector3 angle_in_hand;
    private bool rotate_obj_in_hand;
    private float in_hand_offset;
    private Rigidbody in_hand_rb;
    [SerializeField] bool[] iho_d = new bool[2];

    [Header("Movement Angle")]
    [SerializeField] private MoveAroundObject cam;
    [SerializeField,Range(-180f,180f)] private int offset_angle;
    [SerializeField] private float angle;
    [SerializeField] private Transform cam_center;
    
    [Header("Debug")]
    [SerializeField] private LayerMask dont_ignore_layers;
    [SerializeField] private GameObject particle_destroy;
    [SerializeField] private GameObject particle_destroy_bullet;
    Rigidbody rb;
    bool is_grounded;
    float rad = 180/Mathf.PI;
    Camera main_cam;
    Vector2 last_angle = Vector2.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        main_cam = Camera.main;
        ChangeCursor(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        is_grounded = true;
    }

    Ray GetMouseRay()
    {
        return new Ray(cam_center.position, main_cam.ScreenPointToRay(Input.mousePosition).direction);
    }

    int CorrectN(int n)
    {
        if (n != 1)
            return n;
        return 0;
    }

    void ChangeCursor(int n)
    {
        cursors[Mathf.Abs(1-n)].SetActive(false);
        cursors[2-CorrectN(n)].SetActive(false);
        cursors[n].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        angle = (cam.GetAngleX()+offset_angle*rad)-Mathf.PI;

        //Input KeyBoard
        if (Input.GetKeyDown(KeyCode.Space) && is_grounded)
        {
            rb.AddForce(new Vector3(0,2,0) * jump_force, ForceMode.Impulse);
            is_grounded = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            shoot_ray = !shoot_ray;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (in_hand == null)
            {
                Ray ray = GetMouseRay();

                if (Physics.Raycast(ray, out RaycastHit hit, distance_interactable, dont_ignore_layers))
                {
                    GameObject obj = hit.collider.gameObject;
                    if (obj != null && (obj.tag == "Removing" || obj.tag == "Interactable"))
                    {
                        in_hand = obj;
                        in_hand_offset = Mathf.Sqrt(Mathf.Pow(cam_center.position.x - obj.transform.position.x, 2) + Mathf.Pow(cam_center.position.y - obj.transform.position.y, 2) + Mathf.Pow(cam_center.position.z - obj.transform.position.z, 2));
                        if (in_hand.GetComponent<Rigidbody>() != null)
                        {
                            iho_d[0] = in_hand.GetComponent<Rigidbody>().useGravity;
                            iho_d[1] = in_hand.GetComponent<Collider>().isTrigger;
                            //Debug.Log(in_hand.GetComponent<Collider>().isTrigger);
                            in_hand.GetComponent<Rigidbody>().useGravity = false;
                            in_hand.GetComponent<Collider>().isTrigger = true;
                            if (!in_hand.GetComponent<Rigidbody>().freezeRotation) in_hand_rb = in_hand.GetComponent<Rigidbody>();
                        }
                        cam.SetLock(false, true);
                    }
                }
            }
            else
            {
                if (in_hand.GetComponent<Rigidbody>() != null)
                {
                    in_hand.GetComponent<Rigidbody>().useGravity = iho_d[0];
                    in_hand.GetComponent<Collider>().isTrigger = iho_d[1];
                    in_hand_rb = null;
                }
                in_hand = null;
                rotate_obj_in_hand = false;
                cam.SetLock(false,false);
            }
        }

        //Change cursor
        if (true)//(last_angle == cam.GetAngle())
        {
            Ray ray = GetMouseRay();

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dont_ignore_layers))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj != null)
                {
                    if ((obj.tag == "Removing" || obj.tag == "Target") && in_hand == null)
                    {
                        ChangeCursor(2);
                    }
                    else if ((obj.tag == "Removing") && hit.distance <= distance_interactable && in_hand == null)
                    {
                        ChangeCursor(1);
                    }
                    else
                    {
                        ChangeCursor(0);
                    }
                }
                else
                {
                    ChangeCursor(0);
                }
            }
            else
            {
                ChangeCursor(0);
            }
        }

        //Mouse buttons
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = GetMouseRay();
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, distance_interactable, dont_ignore_layers))
            {
                GameObject obj = raycastHit.collider.gameObject;
                if (obj != null)
                {
                    if (obj.GetComponent<Button>() != null)
                    {
                        obj.GetComponent<Button>().ChangeButtonWork();
                    }
                    else
                    {
                        GameObject crt_obj = ObjectPool.instance.GetPooledObject(0);
                        //Debug.Log(crt_obj);
                        if (crt_obj != null)
                        {
                            crt_obj.transform.rotation = Quaternion.identity;
                            crt_obj.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + 0.5f, raycastHit.point.z);
                            crt_obj.SetActive(true);
                        }
                        //Instantiate(creating_obj, new Vector3(raycastHit.point.x, raycastHit.point.y + 0.5f, raycastHit.point.z), Quaternion.identity);
                    }
                }
            }
        }

        if (rotate_obj_in_hand)
        {
            Vector3 r = cam.GetMoveMouse()*2;

            in_hand.transform.Rotate(r.y * Mathf.Sin(last_angle.x), r.x*Mathf.Sin(last_angle.x),r.y*Mathf.Cos(last_angle.x));
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (in_hand != null)
            {
                rotate_obj_in_hand = true;
                cam.SetLock(true,true);
            }
            else
            {
                if (shoot_ray)
                {
                    Ray ray = GetMouseRay();

                    if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, dont_ignore_layers))
                    {
                        GameObject obj = raycastHit.collider.gameObject;
                        if (obj != null)
                        {
                            if (obj.tag == "Removing")
                            {
                                raycastHit.collider.gameObject.SetActive(false);
                                Instantiate(particle_destroy, raycastHit.collider.gameObject.transform.position, Quaternion.identity);
                            }
                            else
                            {
                                if (obj.GetComponent<Target>() != null)
                                    obj.GetComponent<Target>().Shoot();
                            }
                        }
                    }
                }
                else
                {
                    GameObject bul = ObjectPool.instance.GetPooledObject(1);

                    if (bul != null)
                    {
                        Ray ray = GetMouseRay();
                        bul.transform.rotation = Quaternion.identity;
                        bul.transform.position = ray.GetPoint(0.8f);
                        bul.SetActive(true);
                        bul.GetComponent<Bullet>().Shoot(ray.direction, 20, particle_destroy, particle_destroy_bullet);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (in_hand != null)
            {
                rotate_obj_in_hand = false;
                cam.SetLock(false,true);
            }
        }

        if (in_hand != null)
        {
            float mw = cam.GetDistance();
            if (Mathf.Abs(in_hand_offset - mw) <= distance_interactable && in_hand_offset - mw >0f)
                in_hand_offset -= mw;

            if (in_hand_rb != null)
            {
                in_hand_rb.MovePosition(cam_center.position + new Vector3(Mathf.Cos(last_angle.x) * Mathf.Cos(last_angle.y), Mathf.Sin(last_angle.y), Mathf.Sin(last_angle.x) * Mathf.Cos(last_angle.y)) * (-in_hand_offset));
            }
            else
            {
                in_hand.transform.position = cam_center.position + new Vector3(Mathf.Cos(last_angle.x) * Mathf.Cos(last_angle.y), Mathf.Sin(last_angle.y), Mathf.Sin(last_angle.x) * Mathf.Cos(last_angle.y)) * (-in_hand_offset);
            }
        }

        if (v != Vector2.zero)
        {
            speed += plus_speed*Time.deltaTime;
            if (speed > max_speed) speed = max_speed;
        }
        else if (speed > 0)
            speed -= plus_speed * 2*Time.deltaTime;
        else if (speed < 0)
            speed = 0;

        if (speed != 0)
        {
            Vector3 mv = Vector3.zero; mv+= new Vector3(v.x * Mathf.Cos(angle), 0, v.x*Mathf.Sin(angle));
            mv += new Vector3(v.y * Mathf.Sin(Mathf.PI-angle),0,v.y*Mathf.Cos(Mathf.PI-angle));mv = mv.normalized;
            //transform.position += mv * speed * Time.deltaTime;
            rb.MovePosition(transform.position+mv*speed*Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, -angle*rad, 0); last_angle = cam.GetAngle();
    }
}