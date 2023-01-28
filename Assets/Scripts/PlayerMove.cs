using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody playerRb;
    Animator playerAnim;
    PlayerControl _playerControl;
    [SerializeField] private GameObject particlePrefabs;
    public GameObject enemy;
    private bool firstTrig;

    // Start is called before the first frame update
    void Start()
    {

        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        _playerControl = GetComponentInParent<PlayerControl>();


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.Run)
        {
            playerAnim.SetFloat("Blend", 1f);

            if (_playerControl.PlayerState == PlayerState.attack)
            {
                if (enemy != null)
                {
                    transform.LookAt(enemy.transform);
                }

            }
            else
            {
                transform.localRotation = Quaternion.Euler(Vector3.zero);

            }
        }

        else if (GameManager.gameState == GameState.Win)
        {
            playerAnim.SetFloat("Win", 1);
            playerAnim.SetFloat("Blend", 0f);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        else
        {
            playerAnim.SetFloat("Blend", 0f);
        }

    }


    private void FixedUpdate()
    {

        
       

       
    }


    private void LateUpdate()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gate"))
        {

            Gate newgate = other.gameObject.GetComponent<Gate>();

            if(newgate.GateState == GateState.add)
            {
                _playerControl.SpawnPlayer(newgate.GateCount);
                
            }

            if (newgate.GateState == GateState.multi )
            {
                _playerControl.SpawnPlayer((newgate.GateCount - 1) * _playerControl.PlayerListCount());
                
            }

            if(newgate.GateState == GateState.subtrac)
            {
                _playerControl.deletePlayer(newgate.GateCount);
            }


            newgate.gateClose();



        }


        if (other.gameObject.CompareTag("Win"))
        {
            GameManager.gameState = GameState.Win;

        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(particlePrefabs, transform.position, Quaternion.identity);

            _playerControl.removeOtherList(gameObject);
            
            Destroy(gameObject);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Blade"))
        {
                 Instantiate(particlePrefabs, transform.position, Quaternion.identity);
             _playerControl.removeOtherList(gameObject);
            Destroy(gameObject);
        }

    }

    
}
