using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DieCamSet : MonoBehaviour
{
    private GameObject Player;
    private CinemachineVirtualCamera CamSet;
    private Transform DieCam;

    private bool reSet=false;
    // Update is called once per frame
    void Update()
    {
        ReSet();
    
    }
    private void ReSet()
    {
        if (reSet == false||Player ==null)
        {
            Player = GameObject.FindWithTag("Player");
            CamSet = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            DieCam = GameObject.Find("DieCam").GetComponent<Transform>();
            if (Player != null && CamSet != null)
            {
                reSet = true;
            }
        }
    }
    private void DieCamChack()
    {
        if (Player.GetComponent<PlayerController>().isDead == false)
        {
            CamSet.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
            CamSet.GetComponent<CinemachineVirtualCamera>().LookAt = Player.transform;
        }
        else if (Player.GetComponent<PlayerController>().isDead == true)
        {
            CamSet.GetComponent<CinemachineVirtualCamera>().Follow =DieCam;
            CamSet.GetComponent<CinemachineVirtualCamera>().LookAt =DieCam;
            StartCoroutine("ReSpawnSet");
        }
    }
}
