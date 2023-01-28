using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum EnemyState { idle,attack}

public class EnemyControl : MonoBehaviour
{

    [SerializeField] private EnemyState enemyState;

    [SerializeField][Range(1,200)] private int enemyCount;

    [SerializeField] private GameObject enemyPrefabs;
    
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();


    [Range(0f, 1f)] [SerializeField] private float distanceFactor;

    [Range(0f, 1f)] [SerializeField] private float radius;

    private GameObject playerControlObj;
    private PlayerControl _playerControl;


    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private Text enemyCounter;

    bool directionAttack;
    Vector3 direction;

    // Start is called before the first frame update
    private void Start()
    {
        directionAttack = false;
        playerControlObj = GameObject.FindGameObjectWithTag("PlayerControl");
        _playerControl = playerControlObj.GetComponent<PlayerControl>();

        enemyList.Add(transform.GetChild(0).gameObject);

        spawnenemy(enemyCount);


    }

    // Update is called once per frame
    private void Update()
    {

        enemyCounter.text = enemyListCount().ToString();

        if (GameManager.gameState == GameState.Run)
        {


            if (Vector3.Distance(transform.position, playerControlObj.transform.position) < 5)
            {
                _playerControl.PlayerState = PlayerState.attack;
                _playerControl.targetEnemy = gameObject;

                enemyState = EnemyState.attack;
                if (!directionAttack)
                {
                    direction = Vector3.Normalize(playerControlObj.transform.position - transform.position);
                    _playerControl.directionEnemy = -direction;
                    directionAttack = true;
                }
            }


            if (enemyState == EnemyState.attack)
            {

                transform.position += direction * Time.deltaTime;

                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].gameObject.GetComponent<EnemyMove>().player = playerControlObj;
                    enemyList[i].GetComponent<Animator>().SetFloat("run", 1f);
                }


                if (enemyList.Count <= 0)
                {
                    _playerControl.PlayerState = PlayerState.run;
                    _playerControl.playerNotLookAt();
                    _playerControl.targetEnemy = null;
                    enemyCanvas.gameObject.SetActive(false);
                    Destroy(gameObject, 2f);
                }

                

            }

        }
        
        if(GameManager.gameState == GameState.Lose)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {


                enemyList[i].GetComponent<Animator>().SetFloat("win", 1f);
                enemyList[i].GetComponent<Animator>().SetFloat("run", 0f);
            }
        }

    }



    protected void spawnenemy(int neededFellowNum)
    {
        for (int i = 0; i < neededFellowNum; i++)
        {
            var newObj = Instantiate(enemyPrefabs, enemyList[0].transform.position, Quaternion.Euler(0,180,0), transform);
            
            enemyList.Add(newObj);

        }

        // spawn edildikten sonra tekrardan kontrol edelim
        ShortList();
    }
    protected void ShortList()
    {

        for (int i = 0; i < enemyList.Count; i++)
        {
            float x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            float z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);
            Vector3 newPosition = new Vector3(x, 0f, z);

            enemyList[i].transform.DOLocalMove(newPosition, 1f);

        }
    }






    internal void removeList(GameObject obj)
    {
        enemyList.Remove(obj);
    }

    private int enemyListCount()
    {
        return enemyList.Count;
    }





}
