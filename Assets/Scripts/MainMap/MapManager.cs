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
    bool isPlayerReady = false;

    public bool isMove = false;
    public bool isKight=false;

    public bool isPlayerWalk = false;
    public bool isEvent = false;

    public int _size;
    public GameObject[] Tile;

    public Camera mainCamera;
    Vector3 mousePos = Vector3.zero;

    public int movecount = 0;
    public int move = 0;



    public GameObject[] enemy_Elite;

    public bool isEliteLose = true;


    bool Knight_Check, Bishop_Check, Rook_Check, Queen_Check;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!MapData.instance.mapInit)
        {
            MapData.instance.mapInit = true;
            MapData.instance.Initialize(_size, Tile);
        }
        SetPlayerPos();
    }

    public void SetPlayerPos()
    {
        playerNowPoint_X =  MapData.instance.playerStartingPoint_X;
        playerNowPoint_Y = MapData.instance.playerStartingPoint_Y;
        Player.position = new Vector3((playerNowPoint_X) * 10, 0, (playerNowPoint_Y) * 10);
        Debug.Log(Player.position);
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

                else if (isPlayerReady == true && move < movecount)
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());
                    PlayerMove(x, y);
                    move++;

                    Debug.Log("이동");
                }

                else if (isPlayerReady == true && move == movecount)
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());
                    PlayerMove(x, y);
                    move = 0;
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
            for (int i = 0; i < _size; i++)
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
            else if (ItemData.instance.Jump == true)
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
            isEvent = true;
        }
    }

    IEnumerator MovingPlayer(int x, int y)
    { 
        isMove = true;
        isPlayerWalk = true;
        MainMapCharacter.instance.checkPlayerWalk();
        //플레이어 애니메이션으로 천천히 이동시키기

        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
        Player.position += new Vector3((x - playerNowPoint_X) * 10, 0, (y - playerNowPoint_Y) * 10);
        playerNowPoint_X = x;
        playerNowPoint_Y = y;
        MapData.instance._tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;


        if (ItemData.instance.Jump == true)
        {
            yield return new WaitForSeconds(3.5f);
        }
        yield return new WaitForSeconds(2.3f);


        isPlayerWalk = false;
        MainMapCharacter.instance.checkPlayerWalk();

        //플레이어 이동에 걸리는 시간
        
        yield return new WaitForSeconds(1f);
        if(isEvent) EventSceneLoad();
        //--몬스터 이동 함수--
        EliteMoving();
        //몬스터 이동에 걸리는 시간
        Debug.Log("플레이어 위치" + playerNowPoint_X + "," + playerNowPoint_Y);
        Debug.Log("나이트 위치" + MapData.instance.elite_Knight_Location_X + "," + MapData.instance.elite_Knight_Location_Y);

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
                    break;
                case 4:
                    LoadingSceneManager.LoadScene("Test");
                    break;
                case 5:
                    LoadingSceneManager.LoadScene("Test");
                    break;
            }

        }
    }

    public void EliteMoving()
    {
        for (int i = 0; i < enemy_Elite.Length; i++)
        {
            switch (i)
            {
                case 1:
                    Elite_CheckPlayer("Knight");
                    if (Knight_Check)
                    {
                        int x = playerNowPoint_X;
                        int y = playerNowPoint_Y;
                        int xx = MapData.instance.elite_Knight_Location_X;
                        int yy = MapData.instance.elite_Knight_Location_Y;

                        var player_Knight = new int[,] { { x - 2, y + 1 }, { x - 2, y - 1 }, { x - 1, y + 2 }, { x - 1, y - 2 }, { x + 1, y + 2 }, { x + 1, y - 2 }, { x + 2, y - 1 }, { x + 2, y + 1 } };
                        var knight_Player = new int[,] { { xx - 2, yy + 1 }, { xx - 2, yy - 1 }, { xx - 1, yy + 2 }, { xx - 1, yy - 2 }, { xx + 1, yy + 2 }, { xx + 1, yy - 2 }, { xx + 2, yy - 1 }, { xx + 2, yy + 1 } };


                        for (int k = 0; k < 8; k++)
                        {
                            if ( MapData.instance._tile[knight_Player[k, 0], knight_Player[k, 1]] == TileType.Player)
                            {
                                MapData.instance.elite_Knight_Location_X = knight_Player[k, 0];
                                MapData.instance.elite_Knight_Location_Y = knight_Player[k, 1];
                                //전투 처리
                            }
                            else if (player_Knight[k, 0] == knight_Player[k, 0] && player_Knight[k, 1] == knight_Player[k, 1])
                            {
                                MapData.instance.elite_Knight_Location_X = knight_Player[k, 0];
                                MapData.instance.elite_Knight_Location_Y = knight_Player[k, 1];
                                //맵에서 실제로 이동하는거 구현
                            }
                        }
                       
                    }
                    break;
                case 2:
                    Elite_CheckPlayer("Rook");
                    if (Rook_Check)
                    {
                   

                       

                        if (MapData.instance._tile[MapData.instance.elite_Rook_Location_X, MapData.instance.elite_Rook_Location_Y] != TileType.Player)
                        {
                            MapData.instance._tile[MapData.instance.elite_Rook_Location_X, MapData.instance.elite_Rook_Location_Y] = TileType.Empty;
                        }

                        if (MapData.instance.elite_Rook_Location_X != playerNowPoint_X)
                        {
                            MapData.instance.elite_Rook_Location_X = playerNowPoint_X;
                        }
                        else if (MapData.instance.elite_Rook_Location_Y != playerNowPoint_Y)
                        {
                            MapData.instance.elite_Rook_Location_Y = playerNowPoint_Y;
                        }

                        if (MapData.instance.elite_Rook_Location_X == playerNowPoint_X && MapData.instance.elite_Rook_Location_Y == playerNowPoint_Y)
                        {
                            Debug.Log("룩 플레이어와 전투");
                            //겹쳐서 전투일 때 플레이어를 지우지 않도록 해야함
                            isEliteLose = false;
                        }
                        // 실제 맵에서 움직이는 코드 필요함
                        if (isEliteLose)
                        {
                            MapData.instance._tile[MapData.instance.elite_Rook_Location_X, MapData.instance.elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;
                        }
                    }
                    break;
                case 3:
                    Elite_CheckPlayer("Bishop");
                    if (Bishop_Check)
                    {
                        MapData.instance._tile[MapData.instance.elite_Bishop_Location_X, MapData.instance.elite_Bishop_Location_Y] = TileType.Empty;

                        int x = MapData.instance.elite_Bishop_Location_X - playerNowPoint_X;
                        MapData.instance.elite_Bishop_Location_X -= x;
                        if (MapData.instance.elite_Bishop_Location_Y > playerNowPoint_Y)
                        {
                            MapData.instance.elite_Bishop_Location_Y -= Mathf.Abs(x);
                        }
                        else
                        {
                            MapData.instance.elite_Bishop_Location_Y += Mathf.Abs(x);
                        }
                        MapData.instance._tile[MapData.instance.elite_Bishop_Location_X, MapData.instance.elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;

                        //플레이어와 겹쳐졌는지 확인 필요
                    }
                    break;
                case 4:
                    Elite_CheckPlayer("Queen");
                    if (Queen_Check)
                    {
                        int aaa = Random.Range(0, 2);
                        if (aaa == 1)
                        {
                            Debug.Log("퀸: 룩처럼 움직임");
                            //룩처럼 움직임
                        }
                        else if (aaa == 0)
                        {
                            Debug.Log("퀸: 비숍처럼 움직임");
                            //비숍처럼 움직임
                        }
                    }
                    break;

            }
        }


    }

    public void Elite_CheckPlayer(string elite)
    {
        switch (elite)
        {
            case "Knight":
                for (int i = -2; i < 3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        if (MapData.instance.elite_Knight_Location_X + i < _size && MapData.instance.elite_Knight_Location_Y + j < _size && MapData.instance.elite_Knight_Location_X + i >= 0 && MapData.instance.elite_Knight_Location_Y + j >= 0)
                        {
                            if (MapData.instance._tile[MapData.instance.elite_Knight_Location_X + i, MapData.instance.elite_Knight_Location_Y + j] == TileType.Player)
                            {
                                Knight_Check = true;
                                Debug.Log("나이트 : 플레이어 인식");
                            }
                            else Knight_Check = false;
                        }
                    }
                }
                break;
            case "Rook":
                for (int i = -3; i < 4; i++)
                {
                    if (MapData.instance.elite_Rook_Location_X + i < _size && MapData.instance.elite_Rook_Location_Y + i < _size && MapData.instance.elite_Rook_Location_X + i >= 0 && MapData.instance.elite_Rook_Location_Y + i >= 0)
                    {
                        if (MapData.instance._tile[MapData.instance.elite_Rook_Location_X + i, MapData.instance.elite_Rook_Location_Y + i] == TileType.Player)
                        {
                            Rook_Check = true;
                            Debug.Log("룩 : 플레이어 인식");
                        }
                        else Rook_Check = false;
                    }
                }
                break;
            case "Bishop":
                for (int i = -3; i < 4; i++)
                {
                    if (MapData.instance.elite_Bishop_Location_X + i < _size && MapData.instance.elite_Bishop_Location_Y + i < _size && MapData.instance.elite_Bishop_Location_X + i >= 0 && MapData.instance.elite_Bishop_Location_Y + i >= 0)
                    {
                        if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X + i, MapData.instance.elite_Bishop_Location_Y + i] == TileType.Player)
                        {
                            Bishop_Check = true;
                            Debug.Log("비숍 : 플레이어 인식");
                        }

                        else Bishop_Check = false;
                    }
                }
                break;
            case "Queen":
                for (int i = -4; i < 5; i++)
                {
                    if (MapData.instance.elite_Queen_Location_X + i < _size && MapData.instance.elite_Queen_Location_Y + i < _size && MapData.instance.elite_Queen_Location_X + i >= 0 && MapData.instance.elite_Queen_Location_Y + i >= 0)
                    {
                        if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X + i, MapData.instance.elite_Queen_Location_Y + i] == TileType.Player)
                        {
                            Queen_Check = true;
                            Debug.Log("퀸 : 플레이어 인식");

                        }
                        else Queen_Check = false;
                    }
                }
                break;
        }
    }


}
