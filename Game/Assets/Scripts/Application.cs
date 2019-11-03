using System.IO;
using UnityEngine;

public class Application : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.orthographicSize = Screen.height / 2f;
    }
}
