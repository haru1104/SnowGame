using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public InputField input;
    public Image img;
    public bool touch = false;
    private void Start()
    {
       // Screen.SetResolution(1920, 1080, true);
        input.gameObject.SetActive(false);
        img.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (touch == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                img.gameObject.SetActive(true);
                input.gameObject.SetActive(true);
                touch = true;
                
            }
        }
    }


}
