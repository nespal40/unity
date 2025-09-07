using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Material default_material;
    [SerializeField] private Material shooting_material;
    [SerializeField] private float kd_time;
    bool is_shooting = false;
    public void Shoot()
    {
        if (!is_shooting)
            StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        is_shooting = true;
        GetComponent<Renderer>().material = shooting_material;
        yield return new WaitForSeconds(kd_time);
        is_shooting = false;
        GetComponent<Renderer>().material = default_material;
    }
}