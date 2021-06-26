using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowBall : MonoBehaviour
{
    public float Damage = 10;
    private void Start()
    {
        Destroy(gameObject, 10);
    }


}
