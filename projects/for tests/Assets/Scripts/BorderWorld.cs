using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderWorld : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = new Vector3 (0, 10, 0);

    }
}
