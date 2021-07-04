using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Spawn : MonoBehaviourPunCallbacks
{
    public GameObject[] boxPrefab;
    public int count = 0;
    private int boxType;
    private float spawnX;
    private float spawnZ;
    private float spawnY;
    private float delayTime = 4;
    private float timeSet = 0;
    private Vector3 location = Vector3.zero;

    private void Start()
    {
       // StartCoroutine("BoxSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == true)
        {
            spawnX = Random.Range(-40, -2);
            spawnZ = Random.Range(-40, -3);
            boxType = Random.Range(0, 6);
            spawnY = 1;

            location = new Vector3(spawnX, spawnY, spawnZ);

            timeSet += Time.deltaTime;

            if (timeSet >= delayTime)
            {
                GameObject WorldSpawnBox = PhotonNetwork.Instantiate("RandomBox", location, Quaternion.identity);
                timeSet = 0;
            }

           
        }
        else
        {
            return;
        }
    }

 

}
