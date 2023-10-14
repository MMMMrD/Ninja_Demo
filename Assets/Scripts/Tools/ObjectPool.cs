using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance; //对象池唯一对象

    public static ObjectPool Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    //使用字典，根据键值寻找队列
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    //作为所有子对象池的父类物体
    private GameObject pool;


    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;

        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if(pool == null)
            {
                pool = new GameObject("ObjectPool");
            }

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if(!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }

            _object.transform.SetParent(childPool.transform);
        }

        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }

    public GameObject GetObjectButNoSetActive(GameObject prefab)    //获得物体但是不将物体打开
    {
        GameObject _object;

        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if(pool == null)
            {
                pool = new GameObject("ObjectPool");
            }

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if(!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }

            _object.transform.SetParent(childPool.transform);
        }

        _object = objectPool[prefab.name].Dequeue();
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);//更改子物体的名字

        if(!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }

        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }

    public void ClearObjectPool()   //外部调用清空字典
    {
        objectPool.Clear();
    }
}
