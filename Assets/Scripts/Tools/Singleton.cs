using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :Singleton<T> //where确定继承该父类的是什么类型的子类
{
    private static T instance;

    //用于获取单例的属性
    public static T Instance
    {
        get { return instance;}
    }

    //程序启动时调用，给单例唯一变量赋值
    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    //判断单例是否已经被创建的属性
    public static bool IsInitialized
    {
        get { return instance != null;}
    }
    
    //销毁函数
    protected void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }    
    }
}