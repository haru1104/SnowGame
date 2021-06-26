using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    private Text bluePoint;
    private Text blueScore;
    private Text redPoint;
    private Text redScore;
    private GameManagerMap1 gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerMap1>();
        blueScore = GameObject.Find("BlueKill").GetComponent<Text>();
        bluePoint = GameObject.Find("BlueGetPoint").GetComponent<Text>();
        redScore = GameObject.Find("RedKill").GetComponent<Text>();
        redPoint = GameObject.Find("RedGetPoint").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        GameFinsh();
        Debug.Log(gm.botKillCount);
    }
    void GameFinsh()
    {
        blueScore.text = "Player Kills : " + gm.botKillCount;
        bluePoint.text = "Get Point : " + gm.botKillCount * 5;
        redScore.text = "Player Kills : " + gm.playerKillCount;
        redPoint.text = "Get Point :" + gm.playerKillCount * 5;
    }
}
