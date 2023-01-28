using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Animator animEnemy;
    public EnemyControl enemy;
    public GameObject particleEnemy;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animEnemy = GetComponent<Animator>();
        enemy = GetComponentInParent<EnemyControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.LookAt(player.transform);
        }
    }



    public void particleOpen()
    {
        Instantiate(particleEnemy, transform.position, Quaternion.identity);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            enemy.removeList(gameObject);
            particleOpen();
            Destroy(gameObject);

        }
    }


}
