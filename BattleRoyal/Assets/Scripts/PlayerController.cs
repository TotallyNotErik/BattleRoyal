using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPun
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Components")]
    public Rigidbody rig;

    [HideInInspector]
    public int id;
    public Player photonPlayer;

    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;
        dir.y = rig.velocity.y;

        rig.velocity = dir;
    }

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, 1.5f)) 
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;
        if(!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);   
            rig.isKinematic = true;
        }
    }
}
