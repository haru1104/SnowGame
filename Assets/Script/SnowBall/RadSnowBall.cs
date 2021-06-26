using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadSnowBall : MonoBehaviour
{
    public float Damage = 15;
    private void Start()
    {
        Destroy(gameObject, 10);
    }


}
