using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum PlayerState { idle,run,attack}

public class PlayerControl : MonoBehaviour
{
    [SerializeField]private PlayerState _playerState;
    public PlayerState PlayerState { get => _playerState; set => _playerState = value; }

    public GameObject targetEnemy;
    public Vector3 directionEnemy;

    [SerializeField] private GameObject playerPrefabs;

    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    [Range(0f, 1f)] [SerializeField] private float distanceFactor;

    [Range(0f, 1f)] [SerializeField] private float radius;

    [SerializeField] private Camera mainCam;

    //kamera
    Vector3 offset;
    Transform target;
    // sayac
    [SerializeField] GameObject canvasPlayer;
    [SerializeField] Text counterText;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 playerStartPos;
    Vector3 newPos;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float directionSpeed;


    float time = 0;
    float timer = 0.5f;

    bool isCutting;
    bool moveTouch;



    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        target = transform;
        offset = mainCam.transform.position - target.position;
        isCutting = false;
        moveTouch = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerList.Add(transform.GetChild(0).gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = PlayerListCount().ToString();


        if (GameManager.gameState == GameState.Run)
        {



            if (_playerState == PlayerState.run)
            {
                move();

            }
            else if (_playerState == PlayerState.attack)
            {
                if (targetEnemy != null)
                {

                    transform.position += directionEnemy * Time.deltaTime;

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        playerList[i].GetComponent<PlayerMove>().enemy = targetEnemy;
                    }

                }


            }

            if (!isCutting)
            {
                if (_playerState == PlayerState.run)
                {
                    ShortList();
                }

            }



            if (PlayerListCount() <= 0)
            {
                GameManager.gameState = GameState.Lose;
            }


        }
        else if (GameManager.gameState == GameState.Lose)
        {
            canvasPlayer.gameObject.SetActive(false);
        }

    }


   

    private void LateUpdate()
    {
        
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, offset + target.position, 2 * Time.deltaTime);

    }


    private void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveTouch = true;

            var ground = new Plane(Vector3.up, 0f);

            var ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (ground.Raycast(ray, out var distance))
            {
                startPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }
        }

       

        else if (Input.GetMouseButtonUp(0))
        {
            moveTouch = false;
        }

        if (moveTouch)
        {
            // kontrol icin playercontrol konumunda bir zemin oluþturalým

            var ground = new Plane(Vector3.up, 0f);
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (ground.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);

                var move = mousePos - startPos;

                var control = playerStartPos + move;


                if (PlayerListCount() > 150)
                {
                    control.x = Mathf.Clamp(control.x, -0.7f, 0.7f);
                }

                else if(PlayerListCount() > 100)
                {
                    control.x = Mathf.Clamp(control.x, -1f, 1f);
                }
                else if(PlayerListCount() > 50)
                {
                    control.x = Mathf.Clamp(control.x, -1.5f, 1.5f);
                }
                else
                {
                    control.x = Mathf.Clamp(control.x, -2f, 2f);
                }
                    

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * directionSpeed)
                    , transform.position.y, transform.position.z);

            }

        }


        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }


    internal void playerNotLookAt()
    {
        for(int i = 0; i< playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerMove>().enemy = null;
        }
    }


    protected void spawnPlayer(int neededFellowNum)
    {
        for (int i = 0; i < neededFellowNum; i++)
        {
            var newObj =  Instantiate(playerPrefabs, playerList[0].transform.position, Quaternion.identity, transform);
            playerList.Add(newObj);

        }

        // spawn edildikten sonra tekrardan kontrol edelim
        ShortList();
    }


    internal void SpawnPlayer(int newInt)
    {
        spawnPlayer(newInt);
    }


    internal void deletePlayer(int newInt)
    {
        for (int i = 0; i < newInt; i++)
        {
            GameObject obj = playerList[i];
            playerList.Remove(obj);
            Destroy(obj);
        }
    }

    internal void ShortList()
    {

        playerList[0].transform.DOLocalMove(Vector3.zero, 0.5f);
        
        for (int i = 1; i < playerList.Count; i++)
        {
            float x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            float z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);
            Vector3 newPosition = new Vector3(x, 0f, z);

            playerList[i].transform.DOLocalMove(newPosition,0.5f);
        }
    }

   


    internal int PlayerListCount()
    {
        return playerList.Count;
    }


   
    // düþman karakterler dýþýndaki objelerle temas ettiðinde sýralamayý durduralým daha sonra ise yeniden sýralýyalým
    public void removeOtherList(GameObject obj)
    {
        isCutting = true;
        Invoke("notCutting", 3f);
        playerList.Remove(obj);

    }

     void notCutting()
     {
        isCutting = false;
     }
    

}
