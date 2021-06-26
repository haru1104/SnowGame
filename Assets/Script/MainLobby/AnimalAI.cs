using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AnimalAI : MonoBehaviour
{

    private bool isWalk;
    private NavMeshAgent nav;
    private GameObject player;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > 5)
        {
            nav.isStopped = false;
            nav.SetDestination(player.transform.position);
            ani.SetBool("walk", true);
        }
        else
        {
            nav.isStopped = true;
            transform.LookAt(player.transform.position);
            ani.SetBool("walk", false);

        }
    }
}
