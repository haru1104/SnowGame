using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector3 movedir;
   
    Rigidbody playerRigid;
    Animator playerAni;
    Button jumpButton;
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float TurnSpeed = 2;

    private bool isJump=false;
    private bool isRun = false;
    private bool isGrounded = false;
    public bool isDead = false;
    public bool isTouchPad = false;
    private bool jumpFind = false;
    private float groundCheckDist = 1.0f;
    private float h,v,j;
    private float jump_pos = 480;
    private float touchH, touchV;


    [SerializeField]
    private LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
        jumpButton = GameObject.FindWithTag("jumpButton").GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)//자기자신이 아닐경우에는 움직이지 못하게 리턴 전송
        {
            return;
        }
        if (isDead == false)
        {


            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            movedir = new Vector3(0, 0, v);

            if (isTouchPad == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CheckGroundStatus();
                    if (isGrounded == true)
                    {
                        Debug.Log(isGrounded);
                        isJump = true;
                        Jump();
                    }
                }
            }
            else if (isTouchPad == true && jumpFind == false)
            {
                jumpButton.onClick.AddListener(Jump);
                jumpFind = true;
            }
          
        }
        if (( h != 0 || v != 0 ) || ( touchH !=0 || touchV !=0 ))
        {
            isRun = true;
            playerAni.SetBool("Run", true);
        }
        else
        {
            isRun = false;
            playerAni.SetBool("Run", false);
        }
        Move();
        turnPostion();
    }

   private void Move()
    {
        if (isTouchPad == false)
        {
            Vector3 moveDistance = v * transform.forward * moveSpeed * Time.deltaTime;
            playerRigid.MovePosition(playerRigid.position + moveDistance);
        }
        else if (isTouchPad == true)
        {
            Vector3 moveDistance = touchV * transform.forward * moveSpeed * Time.deltaTime;
            playerRigid.MovePosition(playerRigid.position + moveDistance);
        }
    }  
    private void turnPostion()
    {
        if (isTouchPad == false)
        {
            playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(0, h * TurnSpeed, 0);
        }
        else if (isTouchPad == true)
        {
            playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(0, touchH * TurnSpeed, 0);
        }

    }
    public void OnStickChanged(Vector3 stickpos)
    {
        touchH = stickpos.x;
        touchV = stickpos.y;
    }
    public void Jump()
    {
    
        if (isRun ==true)
        {
            playerAni.SetTrigger("RunJump");
        }
        else
        {
            playerAni.SetTrigger("Jump");
        }

        if (playerRigid.velocity.y <= 0)
        {
            playerRigid.AddForce(Vector3.up * jump_pos);
          
        }
        
    }
    void CheckGroundStatus()
    {
        RaycastHit rayHit;

#if UNITY_EDITOR
        Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDist));
#endif

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out rayHit, groundCheckDist, layer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
