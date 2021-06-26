using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class spawnPoint : MonoBehaviourPunCallbacks
{
    // GameObject pla;
    Slider _playerSlider;
    GameObject pla;
    public bool sliderDes = true;
    // Start is called before the first frame update
    private void Start()
    {
        PlayerSpawn();
    }
    void PlayerSpawn()
    {
        pla = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);

        pla.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        _playerSlider = pla.GetComponentInChildren<Slider>();
    }

}
