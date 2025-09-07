using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool when_open = true;
    [SerializeField] private GameObject door;
    public void SetOpenDoor(bool open)
    {
        door.SetActive(!(open==when_open));
    }
}
