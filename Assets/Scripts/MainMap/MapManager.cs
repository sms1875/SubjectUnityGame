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
                    Debug.Log("�̵� �غ� �Ϸ�");
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
                    Debug.Log("�̵� �غ� ����");
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
            //����Ʈ ����
            if ((x == playerNowPoint_X+1 && (y == playerNowPoint_Y +2|| y == playerNowPoint_Y - 2)) || (x == playerNowPoint_X - 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) || 
                (x == playerNowPoint_X + 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)) || (x == playerNowPoint_X - 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)))
            {
                tileCheck(x, y);
            }
            else
            {
                Debug.Log("�ش� ĭ�� �̵� ������ �ƴϹǷ� �̵��� �� �����ϴ�.");
            }

        }

        //ü�� ŷ
        else
        {
            if (playerNowPoint_X - 2 < x && x < playerNowPoint_X + 2 && y < playerNowPoint_Y + 2 && playerNowPoint_Y - 2 < y)
            {
                tileCheck(x, y);
            }
            else
            {
                Debug.Log("�ش� ĭ�� �̵� ������ �ƴϹǷ� �̵��� �� �����ϴ�.");
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
            Debug.Log("���� �ִ� Ÿ���� �� �� �����ϴ�.");
        }
        else if (MapData.instance._tile[x, y] == TileType.Event)
        {
            Debug.Log("�̺�Ʈ �߻�");
            StartCoroutine(MovingPlayer(x, y));
            EventSceneLoad();
        }
    }

    IEnumerator MovingPlayer(int x, int y)
    { 
        isMove = true;
        isPlayerWalk = true;
        playerMoving.GetComponent<Player>().checkPlayerWalk();
        //�÷��̾� �ִϸ��̼����� õõ�� �̵���Ű��

        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
        Player.position += new Vector3((x - playerNowPoint_X) * 10, 0, (y - playerNowPoint_Y) * 10);
        playerNowPoint_X = x;
        playerNowPoint_Y = y;
        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
        yield return new WaitForSeconds(2.3f);

        isPlayerWalk = false;
        playerMoving.GetComponent<Player>().checkPlayerWalk();

        //�÷��̾� �̵��� �ɸ��� �ð�
        yield return new WaitForSeconds(1f);

        //--���� �̵� �Լ�--


        //���� �̵��� �ɸ��� �ð�
        yield return new WaitForSeconds(1f);


        //�̵� Ȱ��ȭ
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
        //�ε� ȭ�� �߰�

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNum);

        while (!asyncLoad.isDone)
        {
            Debug.Log("loading : " + asyncLoad.progress * 100 + "%");
            yield return null;
        }
    }
}
