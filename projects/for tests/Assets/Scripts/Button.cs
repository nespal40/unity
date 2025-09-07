using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private bool is_work;
    [SerializeField] private GameObject[] buttons_model = new GameObject[2];
    [SerializeField] private List<GameObject> obj;
    [SerializeField] private GameObject button_obj;
    [SerializeField] private Transform spwn;
    [SerializeField] private Door door;
    void Start()
    {
        for (int i =0;i<buttons_model.Length;i++)
        {
            obj.Add(Instantiate(buttons_model[i], transform.position, transform.rotation, spwn));
        }
        if (button_obj != null)
        {
            Destroy(button_obj);
        }
        ChangeButtonModel(); 
        if (door != null)
        {
            door.SetOpenDoor(is_work);
        }
    }
    public void ChangeButtonWork()
    {
        is_work = !is_work;
        if (door != null)
        {
            door.SetOpenDoor(is_work);
        }

        ChangeButtonModel();
    }
    void ChangeButtonModel()
    {
        obj[1].SetActive(is_work);
        obj[0].SetActive(!is_work);
    }
}
