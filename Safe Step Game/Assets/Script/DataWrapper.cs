using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class DataWrapper : MonoBehaviour
{

    DataWrapper instance;
#if UNITY_WEBGL && !UNITY_EDITOR
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
#endif


    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        HelloWorld();
#endif
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
