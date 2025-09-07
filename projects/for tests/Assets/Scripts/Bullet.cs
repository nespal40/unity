using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 mvdir;
    [SerializeField] private ParticleSystem p;
    Rigidbody rb;
    GameObject particle_destroy;
    GameObject particle_destroy_bullet;
    Vector3 speed_rot;
    float k = 28;
    float k_start= 180;
    float time_death = 8;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed_rot = new Vector3(Random.Range(-k, k), Random.Range(-k, k), Random.Range(-k, k));
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + mvdir * speed * Time.deltaTime);
    }

    private void Update()
    {
        transform.Rotate(speed_rot * Time.deltaTime);
    }

    Vector3 GetV(float n)
    {
        return new Vector3(Random.Range(-n, n), Random.Range(-n, n), Random.Range(-n, n));
    }

    public void Shoot(Vector3 dir, float speed, GameObject p, GameObject p2)
    {
        if (speed != 0) this.speed = speed;
        transform.rotation = Quaternion.identity;
        mvdir = dir;
        particle_destroy = p;
        particle_destroy_bullet = p2;
        speed_rot = GetV(k);
        transform.Rotate(GetV(k_start));
        StartCoroutine(timer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Removing")
        {
            Instantiate(particle_destroy, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.SetActive(false);
        }
        
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<Target>().Shoot();
        }
        Instantiate(particle_destroy_bullet, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(time_death);
        gameObject.SetActive(false);
    }
}
