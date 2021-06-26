using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamInOut : MonoBehaviour
{
    private CinemachineTransposer camSet;
    private Vector3 inPos;
    private Vector3 outPos;
    private float zoomSpeed = 2.0f;
    public bool isPlayer=false;

    private void Start()
    {
        camSet = GameObject.FindWithTag("Cam").GetComponent<CinemachineVirtualCamera>().
                 GetCinemachineComponent<CinemachineTransposer>();
        inPos = new Vector3(0, 2, -4);
        outPos = new Vector3(0, 5, -8);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camSet.m_FollowOffset =inPos;
            isPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            camSet.m_FollowOffset = outPos;
            isPlayer = false;
        }
    }
}
