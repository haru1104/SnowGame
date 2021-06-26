using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcScript : MonoBehaviour
{
    public List<GameObject> animalsPrefab;
    private Transform targetTR;
    public GameObject Col;
    private GameObject canvas;
    private Text tx;
    private Transform playerTr;
    public GameObject petSpawnButton;
    public int random;
    private bool isAnimals=false;
    private string textBox= "Do you want a pet?";
    private string textBox2 = "Greed is a bad thing";
    private void Start()
    {
        canvas = gameObject.transform.GetChild(1).gameObject;
        tx = canvas.GetComponentInChildren<Text>();
        //petSpawnButton = GameObject.Find("PetSpawnButton");
    }
    private void Update()
    {
        PlayerLook();
    }

    void PlayerLook()
    {
        if(Col.GetComponent<CamInOut>().isPlayer == true)
        {
            transform.LookAt(targetTR = GameObject.FindWithTag("Player").transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
            StartCoroutine("typing");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
        petSpawnButton.SetActive(false);
        StopCoroutine("typing");
    }

    IEnumerator typing()
    {
       
        for (int i = 0; i <= textBox.Length; i++)
        {
            tx.text = textBox.Substring(0,i);

            yield return new WaitForSeconds(0.10f);
        }
        if (petSpawnButton != null)
        {
            petSpawnButton.SetActive(true);
        }
        else
        {
            Debug.Log("null");
        }
        
    }
  
    public void YesClick()
    {
        if (isAnimals == false)
        {
            playerTr = GameObject.FindWithTag("Player").transform;
            random = Random.Range(0, 3);
            GameObject pet = Instantiate(animalsPrefab[random], new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z - 1), Quaternion.identity);
            isAnimals = true;
        }
        else if (isAnimals == true)
        {
            StartCoroutine("HaveAnimals");
        }
        
    }
    IEnumerable HaveAnimals()
    {
        for (int i = 0; i <= textBox2.Length; i++)
        {
            tx.text = textBox2.Substring(0, i);

            yield return new WaitForSeconds(0.10f);
        }
        if (petSpawnButton != null)
        {
            petSpawnButton.SetActive(true);
        }
        else
        {
            Debug.Log("null");
        }
    }
    public void NoClick()
    {
        petSpawnButton.SetActive(false);
        //동물 구현후 삭제하는 기능 추가
        //isAnimal 플레이어가 지금 동물을 스폰하고 있는지 상태 체크 

        //if (isAnimal == true)
        //{
        //    Destroy(GameObject.FindWithTag("Animals"));
        //}
    }
}
