using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAni : MonoBehaviour
{
    public const int ANI_IDLE = 0;
    public const int ANI_RUN = 1;
    public const int ANI_ATTACK = 2;
    public const int ANI_DEAD = 3;

    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

     public void ChangeAni(int aniNum)
    {
        ani.SetInteger("AniName", aniNum);
    }

}
