using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public int playerNowPoint_X, playerNowPoint_Y;
    public Transform Player;
    public GameObject playerMoving;
    bool isPlayerReady = false;

    public bool isMove = false;
    public bool isKight=false;

    public bool isPlayerWalk = false;

    public int size;
    public GameObject[] Tile;

    public Camera mainCamera;
    Vector3 mousePos = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!MapData.instance.mapInit)
        {
            MapData.instance.mapInit = true;
            MapData.instance.Initialize(size, Tile);
        }
        SetPlayerPos();
    }

    public void SetPlayerPos()
    {
        playerNowPoint_X =  MapData.instance.playerStartingPoint_X;
        playerNowPoint_Y = MapData.instance.playerStartingPoint_Y;
        Player.position = new Vector3((playerNowPoint_X) * 10, 0, (playerNowPoint_Y) * 10);
    }
    void Update()
    {
        if (isMove) return;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Water");
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
            {
                if (raycastHit.transform.name == "Player")
                {
                    Debug.Log("이동 준비 완료");
                    isPlayerReady = true;
                    ViewWay(isKight);
                }

                else if (isPlayerReady == true)
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());
                    PlayerMove(x, y);
                    isPlayerReady = false;
                    Debug.Log("이동 준비 해제");
                }
            }
        }

    }
    public void ViewWay(bool moveLikeKnight)
    {


        if (moveLikeKnight)
        {

        }
        else
        {
            for (int i = 0; i < size; i++)
            {

            }
        }
    }

    public void PlayerMove(int x, int y)
    {
        if (isKight)
        {
            //나이트 조건
            if ((x == playerNowPoint_X+1 && (y == playerNowPoint_Y +2|| y == playerNowPoint_Y - 2)) || (x == playerNowPoint_X - 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) || 
                (x == playerNowPoint_X + 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)) || (x == playerNowPoint_X - 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)))
            {
                tileCheck(x, y);
            }
            else
            {
                Debug.Log("해당 칸은 이동 범위가 아니므로 이동할 수 없습니다.");
            }

        }

        //체스 킹
        else
        {
            if (playerNowPoint_X - 2 < x && x < playerNowPoint_X + 2 && y < playerNowPoint_Y + 2 && playerNowPoint_Y - 2 < y)
            {
                tileCheck(x, y);
            }
            else
            {
                Debug.Log("해당 칸은 이동 범위가 아니므로 이동할 수 없습니다.");
            }
        }
    }

    void tileCheck(int x, int y)
    {
        if (MapData.instance._tile[x, y] == TileType.Empty)
        {
            StartCoroutine(MovingPlayer(x, y));
            isKight = false;
        }
        else if (MapData.instance._tile[x, y] == TileType.Wall)
        {
            Debug.Log("벽이 있는 타일은 갈 수 없습니다.");
        }
        else if (MapData.instance._tile[x, y] == TileType.Event)
        {
            Debug.Log("이벤트 발생");
            StartCoroutine(MovingPlayer(x, y));
            EventSceneLoad();
        }
    }

    IEnumerator MovingPlayer(int x, int y)
    { 
        isMove = true;
        isPlayerWalk = true;
        playerMoving.GetComponent<Player>().checkPlayerWalk();
        //플레이어 애니메이션으로 천천히 이동시키기

        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
        Player.position += new Vector3((x - playerNowPoint_X) * 10, 0, (y - playerNowPoint_Y) * 10);
        playerNowPoint_X = x;
        playerNowPoint_Y = y;
        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
        yield return new WaitForSeconds(2.3f);

        isPlayerWalk = false;
        playerMoving.GetComponent<Player>().checkPlayerWalk();

        //플레이어 이동에 걸리는 시간
        yield return new WaitForSeconds(1f);

        //--몬스터 이동 함수--


        //몬스터 이동에 걸리는 시간
        yield return new WaitForSeconds(1f);


        //이동 활성화
        isMove = false;

    }
    public void EventSceneLoad()
    {
        if (MapData.instance.EventSceneList != null)
        {
            int lastSceneNum = MapData.instance.EventSceneList.Last();            
            MapData.instance.EventSceneList = MapData.instance.EventSceneList.SkipLast(1).ToArray();

            MapData.instance.playerStartingPoint_X = playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = playerNowPoint_Y;

            switch (lastSceneNum)
            {
                case 3:
                    LoadingSceneManager.LoadScene("Test");
                    //StartCoroutine(LoadEventScene(2));
                    break;
                case 4:
                    LoadingSceneManager.LoadScene("Test");
                    //StartCoroutine(LoadEventScene(2));
                    break;
                case 5:
                    LoadingSceneManager.LoadScene("Test");
                    //StartCoroutine(LoadEventScene(2));
                    break;
            }

        }
    }

    IEnumerator LoadEventScene(int sceneNum)
    {
        //로딩 화면 추가

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNum);

        while (!asyncLoad.isDone)
        {
            Debug.Log("loading : " + asyncLoad.progress * 100 + "%");
            yield return null;
        }
    }
}
