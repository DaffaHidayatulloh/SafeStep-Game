using UnityEngine;
using System.IO;

public class DataImageDelet : MonoBehaviour
{
    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/character.json";
    }

    public void DeleteData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Character data deleted!");
        }
        else
        {
            Debug.Log("No data to delete.");
        }
    }
}
