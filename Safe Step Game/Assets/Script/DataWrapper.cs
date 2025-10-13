using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class DataWrapper : MonoBehaviour
{

    DataWrapper instance;

    [DllImport("__Internal")]
    public static extern void HelloWorld();

    [DllImport("__Internal")]
    public static extern string AccessLocalStorage(string key, string value);

    [DllImport("__Internal")]
    public static extern string ReadLocalStorage(string key);

    [DllImport("__Internal")]
    public static extern void WriteLocalStorage(string key, string value);

    [DllImport("__Internal")]
    public static extern void ClearLocalStorage();


    void Start()
    {
        HelloWorld();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


}
