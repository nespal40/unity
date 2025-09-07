using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private List<List<GameObject>> pooled_objects = new List<List<GameObject>>();
    [SerializeField] private int[] amount_to_pool;

    [SerializeField] private GameObject[] object_prefab;

    void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        for (int j = 0; j < amount_to_pool.Length; j++)
        {
            pooled_objects.Add(new List<GameObject>());
            GameObject parent_obj = new GameObject(object_prefab[j].name + "s parent");Destroy(parent_obj);
            parent_obj = Instantiate(parent_obj, Vector3.zero,Quaternion.identity,transform);

            for (int i = 0; i < amount_to_pool[j]; i++)
            {
                GameObject obj = Instantiate(object_prefab[j],parent_obj.transform);
                pooled_objects[j].Add(obj);
                obj.SetActive(false);
            }
        }
    }

    public GameObject GetPooledObject(string s)
    {
        int n = 0;
        for (int i = 0; i < pooled_objects[n].Count; i++)
        {
            if (!pooled_objects[n][i].activeInHierarchy)
                return pooled_objects[n][i];
        }
        return null;
    }
    public GameObject GetPooledObject(int n)
    {
        if (n > pooled_objects.Count) return null;
        for (int i = 0; i < pooled_objects[n].Count; i++)
        {
            if (!pooled_objects[n][i].activeInHierarchy)
                return pooled_objects[n][i];
        }
        return null;
    }
}
