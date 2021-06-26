using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] boxPrefab;

    
    public int count = 0;

    private int boxType;
    private float spawnX;
    private float spawnZ;
    private float spawnY;

    private Vector3 location = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        spawnX = Random.Range(-40,-2);
        spawnZ = Random.Range(-40, -3);
        boxType = Random.Range(0, 6);
        spawnY = 1;

        location = new Vector3(spawnX, spawnY, spawnZ);

        if (count < 4)
        {
            StartCoroutine("BoxSpawner");
            count++;
        }
      

    }

    IEnumerator BoxSpawner()
    {
        yield return new WaitForSeconds(6);
        GameObject WorldSpawnBox = Instantiate(boxPrefab[boxType], location, Quaternion.identity);
    }

}
