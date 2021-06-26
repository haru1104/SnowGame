using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotHp : MonoBehaviour
{
    public Slider Botsli;


    private void OnTriggerEnter(Collider other)
    {    
        float max = Botsli.value;
        if (other.tag == "SnowBall")
        {
            Botsli.value = max - 0.10f;
            Destroy(other.gameObject);

        }
        if (other.tag == "RedSnowBall")
        {
            Botsli.value = max - 0.12f;
            Destroy(other.gameObject);
        }
        if (other.tag == "BlueSnowBall")
        {
            Botsli.value = max - 0.15f;
            Destroy(other.gameObject);
        }    


    }

}
