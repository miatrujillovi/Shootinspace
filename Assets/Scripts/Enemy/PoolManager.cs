using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pool.ContainsKey(prefab) || pool[prefab].Count == 0)
        {
            return Instantiate(prefab, pos, rot);
        }

        GameObject obj = pool[prefab].Dequeue();
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);

        GameObject prefab = obj.GetComponent<Poolable>()?.originalPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Este objeto no tiene Poolable, no puede volver al pool.");
            Destroy(obj);
            return;
        }

        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        pool[prefab].Enqueue(obj);
    }

    public T Get<T>(T prefabComponent, Vector3 pos, Quaternion rot)
            where T : Component
    {
        GameObject go = Get(prefabComponent.gameObject, pos, rot);
        return go.GetComponent<T>();
    }
}
