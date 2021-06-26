using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    private Text timer;
    private int count = 120;
    public bool zeroTime = false;

    private GameManeger gm;
    // Start is called before the first frame update

    private void Start()
    {
        timer = GetComponentInChildren<Text>();
        StartCoroutine("SetTimer");
        gm = GameObject.Find("GameManeger").GetComponent<GameManeger>() ;
      
    }

    void PrintTime()
    {
        timer.text = "Time : " + count;
    }
    IEnumerator SetTimer()
    {
        Debug.Log("count -- ");
        yield return new WaitForSeconds(1f);
        count--;
        PrintTime();
        StartCoroutine("SetTimer");
        if (count <= 0)
        {
            zeroTime = true;
            GameoverDis();
            gm.GameOverScreen.SetActive(true);
            StopCoroutine("SetCount");
        }
    }

    private void GameoverDis()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("bot");

        for (int i = 0; i < go.Length ; i++)
        {
            Destroy(go[i]);
        }
    }
}
