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
    public bool isKight = false;

    public bool isPlayerWalk = false;
    public bool isEvent = false;
    public bool isMonster = false;

    public int _size;
    public GameObject[] Tile;

    public Camera mainCamera;
    Vector3 mousePos = Vector3.zero;

    public int movecount = 0;
    public int move = 0;

    public bool isKnightDie = false;
    public bool isRookDie = false;
    public bool isBishopDie = false;
    public bool isQueenDie = false;


    public GameObject[] enemy_Elite;

    public bool isEliteLose = true;
    public GameObject bossTile;


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
        Instantiate(bossTile, new Vector3(MapData.instance.bossX * 10, 0, MapData.instance.bossY * 10), Quaternion.identity);

    }

    public void SetPlayerPos()
    {
        playerNowPoint_X = MapData.instance.playerStartingPoint_X;
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
                Debug.Log(raycastHit.transform.name);
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
            if ((x == playerNowPoint_X + 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) || (x == playerNowPoint_X - 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) ||
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
        else if (MapData.instance._tile[x, y] == TileType.Enemy_Normal)
        {
            Debug.Log("몬스터 전투 발생");
            StartCoroutine(MovingPlayer(x, y));
            isMonster = true;
        }
        else if(MapData.instance._tile[x, y] == TileType.Enemy_Boss)
        {
            Debug.Log("보스 전투 발생");
            StartCoroutine(MovingPlayer(x, y));
            LoadingSceneManager.LoadScene("TempleBoss");
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
        if (isEvent) EventSceneLoad();
        if (isMonster) MonsterSceneLoad();
        //--몬스터 이동 함수--
        EliteMoving();
        //몬스터 이동에 걸리는 시간
        Debug.Log("플레이어 위치" + playerNowPoint_X + "," + playerNowPoint_Y);
        Debug.Log("나이트 위치" + MapData.instance.elite_Knight_Location_X + "," + MapData.instance.elite_Knight_Location_Y);
        Debug.Log("룩 위치" + MapData.instance.elite_Rook_Location_X + "," + MapData.instance.elite_Rook_Location_Y);
        Debug.Log("비숍 위치" + MapData.instance.elite_Bishop_Location_X + "," + MapData.instance.elite_Bishop_Location_Y);
        Debug.Log("퀸 위치" + MapData.instance.elite_Queen_Location_X + "," + MapData.instance.elite_Queen_Location_Y);
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
            /*
            switch (lastSceneNum)
            {
                case 3:
                    LoadingSceneManager.LoadScene("Forest");
                    break;
                case 4:
                    LoadingSceneManager.LoadScene("Forest");
                    break;
                case 5:
                    LoadingSceneManager.LoadScene("Forest");
                    break;
            }
            */
        }
    }


    public void MonsterSceneLoad()
    {
        if (MapData.instance.MonsterSceneList != null)
        {
            int lastSceneNum = MapData.instance.MonsterSceneList.Last();
            MapData.instance.MonsterSceneList = MapData.instance.MonsterSceneList.SkipLast(1).ToArray();

            MapData.instance.playerStartingPoint_X = playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = playerNowPoint_Y;
            
            switch (lastSceneNum)
            {
                case 3:
                    LoadingSceneManager.LoadScene("Forest");
                    break;
                case 4:
                    LoadingSceneManager.LoadScene("Forest2");
                    break;
                case 5:
                    LoadingSceneManager.LoadScene("Temple1");
                    break;
                case 6:
                    LoadingSceneManager.LoadScene("Temple2");
                    break;
                case 7:
                    LoadingSceneManager.LoadScene("Wasteland");
                    break;
                case 8:
                    LoadingSceneManager.LoadScene("Wasteland2");
                    break;
                
            }
            

        }
    }


    public void EliteMoving()
    {
        for (int i = 1; i < enemy_Elite.Length + 1; i++)
        {

            switch (i)
            {
                case 1:

                    if (isKnightDie) break;
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
                            if (knight_Player[k, 0] >= 0 && knight_Player[k, 0] < _size && knight_Player[k, 1] >= 0 && knight_Player[k, 1] < _size)
                            {
                                if (MapData.instance._tile[knight_Player[k, 0], knight_Player[k, 1]] == TileType.Player)
                                {
                                    MapData.instance.elite_Knight_Location_X = -1;
                                    MapData.instance.elite_Knight_Location_Y = -1;
                                    isKnightDie = true;
                                    Debug.Log("나이트와 전투");
                                    LoadingSceneManager.LoadScene("MutantBoss");

                                    break;
                                    // 나이트가 플레이어를 당장 잡을 수 있을 때
                                    //전투 처리
                                }
                                else if (player_Knight[k, 0] == knight_Player[k, 0] && player_Knight[k, 1] == knight_Player[k, 1]
                                    && MapData.instance._tile[knight_Player[k, 0], knight_Player[k, 1]] != TileType.Wall)
                                {
                                    MapData.instance.elite_Knight_Location_X = knight_Player[k, 0];
                                    MapData.instance.elite_Knight_Location_Y = knight_Player[k, 1];
                                    MapData.instance._tile[MapData.instance.elite_Knight_Location_X, MapData.instance.elite_Knight_Location_Y] = TileType.Enemy_Elite_Knight;
                                    enemy_Elite[0].transform.localPosition = new Vector3(MapData.instance.elite_Knight_Location_X * 10, 0, MapData.instance.elite_Knight_Location_Y * 10);
                                    break;
                                    // 나이트가 한턴 뒤에 플레이어를 잡을 수 있을 때 
                                    //맵에서 실제로 이동하는거 구현
                                }
                                else if (k == 7)
                                {
                                    int rCnt = 0;
                                    while (rCnt<30)
                                    {
                                        

                                        int randomA = Random.Range(0, 7);
                                        if (knight_Player[randomA, 0] >= 0 && knight_Player[randomA, 0] < _size && knight_Player[randomA, 1] >= 0 && knight_Player[randomA, 1] < _size
                                            && MapData.instance._tile[knight_Player[randomA, 0], knight_Player[randomA, 1]] != TileType.Wall)
                                            
                                        {
                                            MapData.instance.elite_Knight_Location_X = knight_Player[randomA, 0];
                                            MapData.instance.elite_Knight_Location_Y = knight_Player[randomA, 1];
                                            MapData.instance._tile[MapData.instance.elite_Knight_Location_X, MapData.instance.elite_Knight_Location_Y] = TileType.Enemy_Elite_Knight;
                                            enemy_Elite[0].transform.localPosition = new Vector3(MapData.instance.elite_Knight_Location_X * 10, 0, MapData.instance.elite_Knight_Location_Y * 10);
                                            break;
                                        }
                                        rCnt++;

                                    }
                                }
                                /*else if (k == 7
                                    && MapData.instance._tile[knight_Player[randomA, 0], knight_Player[randomA, 1]] != TileType.Wall)
                                {
                                    
                                    MapData.instance.elite_Knight_Location_X = knight_Player[randomA, 0];
                                    MapData.instance.elite_Knight_Location_Y = knight_Player[randomA, 1];
                                    
                                    MapData.instance._tile[MapData.instance.elite_Knight_Location_X, MapData.instance.elite_Knight_Location_Y] = TileType.Enemy_Elite_Knight;
                                    enemy_Elite[0].transform.position= new Vector3(MapData.instance.elite_Knight_Location_X * 10, 0, MapData.instance.elite_Knight_Location_Y * 10);
                                    break;
                                    // 한턴 뒤에도 플레이어를 잡지 못할 때 
                                }
                                else if( k==7 && MapData.instance._tile[knight_Player[randomA, 0], knight_Player[randomA, 1]] != TileType.Wall)
                                {
                                    randomA = Random.Range(0, 4);
                                    k = 6;
                                }*/

                            }
                        }

                    }
                    break;
                case 2:

                    if (isRookDie) break;
                    Elite_CheckPlayer("Rook");
                    if (Rook_Check)
                    {
                        MapData.instance._tile[MapData.instance.elite_Rook_Location_X, MapData.instance.elite_Rook_Location_Y] = TileType.Empty;
                        {
                            Rook_zone[,] Zone = new Rook_zone[7, 7];
                            for (int ll = 0; ll < 7; ll++)
                            {
                                for (int oo = 0; oo < 7; oo++)
                                {
                                    Zone[oo, ll] = new Rook_zone();

                                }
                            }


                            int xx = MapData.instance.elite_Rook_Location_X - 3;
                            int yy = MapData.instance.elite_Rook_Location_Y - 3;

                            Queue<Rook_zone> z = new Queue<Rook_zone>();

                            int pp = 0;
                            int kk = 0;

                            for (int k = 0; k < 7; k++)
                            {
                                xx = MapData.instance.elite_Rook_Location_X - 3;
                                for (int p = 0; p < 7; p++)
                                {
                                    if (xx < 0 || xx >= _size)
                                    {

                                    }
                                    else if (yy < 0 || yy >= _size)
                                    {

                                    }
                                    else
                                    {
                                        Zone[p, k] = new Rook_zone();
                                        Zone[p, k].XY_setting(xx, yy);

                                        if (MapData.instance._tile[xx, yy] == TileType.Player)
                                        {
                                            z.Enqueue(Zone[p, k]);
                                            Zone[p, k].Num_setting(10);
                                            pp = p;
                                            kk = k;
                                        }
                                    }
                                    xx++;
                                }
                                yy++;
                            }

                            int qq = 1;
                            int rr = 0;
                            int queueCnt = 0;
                            int qCnt = 0;

                            Rook_zone[] zz = new Rook_zone[5];

                            zz[rr] = z.Dequeue();
                            while (true)
                            {
                                for (int qwer = 0; qwer < 7; qwer++)
                                {
                                    for (int rewq = 0; rewq < 7; rewq++)
                                    {
                                        if (Zone[rewq, qwer] == zz[rr])
                                        {
                                            pp = rewq;
                                            kk = qwer;
                                        }
                                    }
                                }


                                if (pp + 1 >= 0 && pp + 1 < 7 && kk >= 0 && kk < 7)
                                {
                                    if (zz[rr].x + 1 >= 0 && zz[rr].x + 1 < _size &&
                                        zz[rr].y >= 0 && zz[rr].y < _size)
                                    {
                                        if (MapData.instance._tile[zz[rr].x + 1, zz[rr].y] == TileType.Enemy_Elite_Rook)
                                        {
                                            Zone[pp + 1, kk].Num_setting(qq);
                                            Debug.Log("찾았다!");
                                            break;
                                        }
                                        else if (MapData.instance._tile[zz[rr].x + 1, zz[rr].y] == TileType.Wall)
                                        {
                                            Zone[pp + 1, kk].Num_setting(0);
                                            //벽인지 판정

                                        }
                                        else if (Zone[pp + 1, kk].num < 0)
                                        {
                                            Zone[pp + 1, kk].Num_setting(qq);
                                            z.Enqueue(Zone[pp + 1, kk]);
                                            Debug.Log(Zone[pp + 1, kk].num);
                                            queueCnt++;

                                        }
                                    }
                                }
                                if (pp >= 0 && pp < 7 && kk + 1 >= 0 && kk + 1 < 7)
                                {
                                    if (zz[rr].x >= 0 && zz[rr].x < _size &&
                                        zz[rr].y + 1 >= 0 && zz[rr].y + 1 < _size)
                                    {
                                        if (MapData.instance._tile[zz[rr].x, zz[rr].y + 1] == TileType.Enemy_Elite_Rook)
                                        {

                                            Zone[pp, kk + 1].Num_setting(qq);
                                            Debug.Log("찾았다!");
                                            break;
                                        }
                                        else if (MapData.instance._tile[zz[rr].x, zz[rr].y + 1] == TileType.Wall)
                                        {
                                            Zone[pp, kk + 1].Num_setting(0);
                                            //벽인지 판정

                                        }
                                        else if (Zone[pp, kk + 1].num < 0)
                                        {
                                            Zone[pp, kk + 1].Num_setting(qq);
                                            z.Enqueue(Zone[pp, kk + 1]);
                                            Debug.Log(Zone[pp, kk + 1].num);
                                            queueCnt++;


                                        }
                                    }
                                }
                                if (pp - 1 >= 0 && pp - 1 < 7 && kk >= 0 && kk < 7)
                                {
                                    if (zz[rr].x - 1 >= 0 && zz[rr].x - 1 < _size &&
                                        zz[rr].y >= 0 && zz[rr].y < _size)
                                    {

                                        if (MapData.instance._tile[zz[rr].x - 1, zz[rr].y] == TileType.Enemy_Elite_Rook)
                                        {
                                            Zone[pp - 1, kk].Num_setting(qq);
                                            Debug.Log("찾았다!");
                                            break;
                                        }
                                        else if (MapData.instance._tile[zz[rr].x - 1, zz[rr].y] == TileType.Wall)
                                        {
                                            Zone[pp - 1, kk].Num_setting(0);
                                            //벽인지 판정

                                        }
                                        else if (Zone[pp - 1, kk].num < 0)
                                        {
                                            Zone[pp - 1, kk].Num_setting(qq);
                                            z.Enqueue(Zone[pp - 1, kk]);
                                            Debug.Log(Zone[pp - 1, kk].num);
                                            queueCnt++;


                                        }
                                    }
                                }
                                if (pp >= 0 && pp < 7 && kk - 1 >= 0 && kk - 1 < 7)
                                {
                                    if (zz[rr].x >= 0 && zz[rr].x < _size &&
                                        zz[rr].y - 1 >= 0 && zz[rr].y - 1 < _size)
                                    {
                                        if (MapData.instance._tile[zz[rr].x, zz[rr].y - 1] == TileType.Enemy_Elite_Rook)
                                        {
                                            Zone[pp, kk - 1].Num_setting(qq);
                                            Debug.Log("찾았다!");
                                            break;
                                        }
                                        else if (MapData.instance._tile[zz[rr].x, zz[rr].y - 1] == TileType.Wall)
                                        {
                                            Zone[pp, kk - 1].Num_setting(0);
                                            //벽인지 판정
                                        }
                                        else if (Zone[pp, kk - 1].num < 0)
                                        {
                                            Zone[pp, kk - 1].Num_setting(qq);
                                            z.Enqueue(Zone[pp, kk - 1]);
                                            Debug.Log(Zone[pp, kk - 1].num);
                                            queueCnt++;


                                        }
                                    }
                                }
                                qCnt--;
                                if (qCnt <= 0)
                                {
                                    qCnt = queueCnt;
                                    queueCnt = 0;
                                    qq++;
                                }
                                if (z.Count() == 0)
                                {
                                    Debug.Log("룩이 벽에 막혀있음!");
                                    break;
                                }
                                zz[rr] = z.Dequeue();
                            }


                            int[] goal_X = new int[12];
                            int[] goal_Y = new int[12];

                            int[] array_Nums = new int[20];
                            int array_cnt = 0;
                            for (int v = 1; v < 4; v++)
                            {
                                if (Zone[3, 3 + v].x < _size && Zone[3, 3 + v].x >= 0 && Zone[3, 3 + v].y < _size && Zone[3, 3 + v].y >= 0
                                    && Zone[3, 3 +v ].num > 0)
                                {
                                    goal_X[array_cnt] = Zone[3, 3 + v].x;
                                    goal_Y[array_cnt] = Zone[3, 3 + v].y;
                                    array_Nums[array_cnt] = Zone[3, 3 + v].num;
                                    Debug.Log("1 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                    array_cnt++;
                                }
                                else break;
                            }
                            for (int vv = 1; vv < 4; vv++)
                            {
                                if (Zone[3, 3 - vv].x < _size && Zone[3, 3 - vv].x >= 0 && Zone[3, 3 - vv].y < _size && Zone[3, 3 - vv].y >= 0
                                    && Zone[3, 3 - vv].num > 0)
                                {
                                    goal_X[array_cnt] = Zone[3, 3 - vv].x;
                                    goal_Y[array_cnt] = Zone[3, 3 - vv].y;
                                    array_Nums[array_cnt] = Zone[3, 3 - vv].num;
                                    Debug.Log("2 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                    array_cnt++;
                                }
                                else break;
                            }
                            for (int m = 1; m < 4; m++)
                            {
                                if (Zone[3 - m, 3].x < _size && Zone[3 - m, 3].x >= 0 && Zone[3 - m, 3].y < _size && Zone[3 - m, 3].y >= 0
                                    && Zone[3 - m, 3].num > 0)
                                {

                                    goal_X[array_cnt] = Zone[3 - m, 3].x;
                                    goal_Y[array_cnt] = Zone[3 - m, 3].y;
                                    array_Nums[array_cnt] = Zone[3 - m, 3].num;
                                    Debug.Log("3 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                    array_cnt++;
                                }
                                else break;
                            }
                            for (int mm = 1; mm < 4; mm++)
                            {
                                if (Zone[3 + mm, 3].x < _size && Zone[3 + mm, 3].x >= 0 && Zone[3 + mm, 3].y < _size && Zone[3 + mm, 3].y >= 0
                                    && Zone[3 + mm, 3].num > 0)
                                {
                                    goal_X[array_cnt] = Zone[3 + mm, 3].x;
                                    goal_Y[array_cnt] = Zone[3 + mm, 3].y;
                                    array_Nums[array_cnt] = Zone[3 + mm, 3].num;
                                    array_cnt++;
                                    Debug.Log("4 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                }
                                else break;
                            }
                            int rookMoveTo_X = goal_X[0];
                            int rookMoveTo_Y = goal_Y[0];
                            for (int iop = 0; iop < 11; iop++)
                            {
                                if ((array_Nums[iop] > 0 && array_Nums[iop + 1] > 0 && array_Nums[iop] > array_Nums[iop + 1]) || array_Nums[iop + 1] == 10)
                                {
                                    rookMoveTo_X = goal_X[iop + 1];
                                    rookMoveTo_Y = goal_Y[iop + 1];
                                    if (array_Nums[iop + 1] == 10) break;
                                }

                            }
                            /*
                            for (int qw = 0; qw < 7; qw++)
                            {
                                for (int wq = 0; wq < 7; wq++)
                                {
                                    Debug.Log("룩존 : (" + wq + "," + qw + ") = " + Zone[wq, qw].num);
                                }
                            }
                            */

                            MapData.instance.elite_Rook_Location_X = rookMoveTo_X;
                            MapData.instance.elite_Rook_Location_Y = rookMoveTo_Y;
                            if (MapData.instance._tile[rookMoveTo_X, rookMoveTo_Y] == TileType.Player)
                            {
                                Debug.Log("룩과 전투합니다");
                                //전투씬
                                MapData.instance.elite_Rook_Location_X = -1;
                                MapData.instance.elite_Rook_Location_Y = -1;
                                isRookDie = true;
                                LoadingSceneManager.LoadScene("RobotBoss");
                            }
                            else
                            {
                                MapData.instance._tile[MapData.instance.elite_Rook_Location_X, MapData.instance.elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;
                                enemy_Elite[1].transform.localPosition = new Vector3(MapData.instance.elite_Rook_Location_X * 10, 0, MapData.instance.elite_Rook_Location_Y * 10);
                                /*for (int popp = 0; popp < array_Nums.Length; popp++)
                                {
                                    Debug.Log("배열 " + popp + "번째 원소 : " + array_Nums[popp]);

                                }*/
                            }
                        }


                        // 0보다 커야함
                        // 저장
                        // 비교
                        // 비교 결과 저장 (변동 o / x)




                        {
                            /*
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
                            */
                        }
                    }
                    break;
                case 3:


                    if (isBishopDie) break;
                    Elite_CheckPlayer("Bishop");
                    if (Bishop_Check)
                    {
                        MapData.instance._tile[MapData.instance.elite_Bishop_Location_X, MapData.instance.elite_Bishop_Location_Y] = TileType.Empty;

                        int x = MapData.instance.elite_Bishop_Location_X - playerNowPoint_X;
                        Debug.Log("비숍 x 값은 : " + x);
                        int goal_x = MapData.instance.elite_Bishop_Location_X;
                        int goal_y = MapData.instance.elite_Bishop_Location_Y;
                        int move_x = goal_x;
                        int move_y = goal_y;
                        bool isXnagative = false;
                        bool isYnagative = false;
                        bool isMove = false;

                        int[] moveBishop_X = new int[2]
                            { MapData.instance.elite_Bishop_Location_X+1, MapData.instance.elite_Bishop_Location_X-1};
                        int[] moveBishop_Y = new int[2]
                            {MapData.instance.elite_Bishop_Location_Y+1, MapData.instance.elite_Bishop_Location_Y-1 };

                        Debug.Log("비숍 x 값은 : " + x);
                        if (x == 0)
                        {

                        }
                        else
                        {
                            Debug.Log("하나둘셋 야");
                            if (x < 0) isXnagative = true;
                            else isXnagative = false;

                            goal_x -= x;
                            if (MapData.instance.elite_Bishop_Location_Y > playerNowPoint_Y)
                            {
                                isYnagative = false;
                                goal_y -= Mathf.Abs(x);
                            }
                            else
                            {
                                isYnagative = true;
                                goal_y += Mathf.Abs(x);
                            }

                            for (int bb = 1; bb < Mathf.Abs(x) + 1; bb++)
                            {
                                if (isXnagative && isYnagative)
                                {
                                    if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X + bb,
                                        MapData.instance.elite_Bishop_Location_Y + bb] == TileType.Wall)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        move_x = MapData.instance.elite_Bishop_Location_X + bb;
                                        move_y = MapData.instance.elite_Bishop_Location_Y + bb;
                                        isMove = true;
                                    }
                                    if (move_x == goal_x && move_y == goal_y) break;
                                }
                                else if (!isXnagative && isYnagative)
                                {
                                    if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X - bb,
                                        MapData.instance.elite_Bishop_Location_Y + bb] == TileType.Wall)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        move_x = MapData.instance.elite_Bishop_Location_X - bb;
                                        move_y = MapData.instance.elite_Bishop_Location_Y + bb;
                                        isMove = true;
                                    }
                                    if (move_x == goal_x && move_y == goal_y) break;
                                }
                                else if (isXnagative && !isYnagative)
                                {
                                    if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X + bb,
                                        MapData.instance.elite_Bishop_Location_Y - bb] == TileType.Wall)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        move_x = MapData.instance.elite_Bishop_Location_X + bb;
                                        move_y = MapData.instance.elite_Bishop_Location_Y - bb;
                                        isMove = true;
                                    }
                                    if (move_x == goal_x && move_y == goal_y) break;
                                }
                                else if (!isXnagative && !isYnagative)
                                {
                                    if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X - bb,
                                        MapData.instance.elite_Bishop_Location_Y - bb] == TileType.Wall)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        move_x = MapData.instance.elite_Bishop_Location_X - bb;
                                        move_y = MapData.instance.elite_Bishop_Location_Y - bb;
                                        isMove = true;
                                    }
                                    if (move_x == goal_x && move_y == goal_y) break;
                                }

                            }
                            if (isMove == false)
                            {
                                int random_x = Random.Range(0, 2);
                                int random_y = Random.Range(0, 2);
                                goal_x = moveBishop_X[random_x];
                                goal_y = moveBishop_Y[random_y];
                            }
                            else
                            {
                                goal_x = move_x;
                                goal_y = move_y;
                            }

                            if (MapData.instance._tile[goal_x, goal_y] == TileType.Player)
                            {
                                Debug.Log("비숍과 전투");
                                isBishopDie = true;
                                MapData.instance.elite_Bishop_Location_X = -1;
                                MapData.instance.elite_Bishop_Location_Y = -1;
                                LoadingSceneManager.LoadScene("SnowBoss");

                            }
                            else
                            {
                                MapData.instance.elite_Bishop_Location_X = goal_x;
                                MapData.instance.elite_Bishop_Location_Y = goal_y;


                                MapData.instance._tile[MapData.instance.elite_Bishop_Location_X, MapData.instance.elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;
                                enemy_Elite[1].transform.localPosition  =new Vector3(MapData.instance.elite_Bishop_Location_X * 10, 0, MapData.instance.elite_Bishop_Location_Y * 10);

                            }
                        }
                    }
                    break;
                case 4:

                    if (isQueenDie) break;
                    Elite_CheckPlayer("Queen");
                    if (Queen_Check)
                    {
                        int rookOrBishop = Random.Range(0, 2);
                        if (rookOrBishop == 1)

                        {
                            MapData.instance._tile[MapData.instance.elite_Queen_Location_X, MapData.instance.elite_Queen_Location_Y] = TileType.Empty;
                            {
                                Rook_zone[,] Zone = new Rook_zone[7, 7];
                                for (int ll = 0; ll < 7; ll++)
                                {
                                    for (int oo = 0; oo < 7; oo++)
                                    {
                                        Zone[oo, ll] = new Rook_zone();

                                    }
                                }


                                int xx = MapData.instance.elite_Queen_Location_X - 3;
                                int yy = MapData.instance.elite_Queen_Location_Y - 3;

                                Queue<Rook_zone> z = new Queue<Rook_zone>();

                                int pp = 0;
                                int kk = 0;

                                for (int k = 0; k < 7; k++)
                                {
                                    xx = MapData.instance.elite_Queen_Location_X - 3;
                                    for (int p = 0; p < 7; p++)
                                    {
                                        if (xx < 0 || xx >= _size)
                                        {

                                        }
                                        else if (yy < 0 || yy >= _size)
                                        {

                                        }
                                        else
                                        {
                                            Zone[p, k] = new Rook_zone();
                                            Zone[p, k].XY_setting(xx, yy);

                                            if (MapData.instance._tile[xx, yy] == TileType.Player)
                                            {

                                                z.Enqueue(Zone[p, k]);
                                                Zone[p, k].Num_setting(10);
                                                pp = p;
                                                kk = k;
                                            }
                                        }
                                        xx++;
                                    }
                                    yy++;
                                }

                                int qq = 1;
                                int rr = 0;
                                int queueCnt = 0;
                                int qCnt = 0;

                                Rook_zone[] zz = new Rook_zone[5];

                                zz[rr] = z.Dequeue();
                                while (true)
                                {
                                    for (int qwer = 0; qwer < 7; qwer++)
                                    {
                                        for (int rewq = 0; rewq < 7; rewq++)
                                        {
                                            if (Zone[rewq, qwer] == zz[rr])
                                            {
                                                pp = rewq;
                                                kk = qwer;
                                            }
                                        }
                                    }


                                    if (pp + 1 >= 0 && pp + 1 < 7 && kk >= 0 && kk < 7)
                                    {
                                        if (zz[rr].x + 1 >= 0 && zz[rr].x + 1 < _size &&
                                            zz[rr].y >= 0 && zz[rr].y < _size)
                                        {
                                            if (MapData.instance._tile[zz[rr].x + 1, zz[rr].y] == TileType.Enemy_Elite_Queen)
                                            {
                                                Zone[pp + 1, kk].Num_setting(qq);
                                                Debug.Log("찾았다!");
                                                break;
                                            }
                                            else if (MapData.instance._tile[zz[rr].x + 1, zz[rr].y] == TileType.Wall)
                                            {
                                                Zone[pp + 1, kk].Num_setting(0);
                                                //벽인지 판정

                                            }
                                            else if (Zone[pp + 1, kk].num < 0)
                                            {
                                                Zone[pp + 1, kk].Num_setting(qq);
                                                z.Enqueue(Zone[pp + 1, kk]);
                                                queueCnt++;

                                            }
                                        }
                                    }
                                    if (pp >= 0 && pp < 7 && kk + 1 >= 0 && kk + 1 < 7)
                                    {
                                        if (zz[rr].x >= 0 && zz[rr].x < _size &&
                                            zz[rr].y + 1 >= 0 && zz[rr].y + 1 < _size)
                                        {
                                            if (MapData.instance._tile[zz[rr].x, zz[rr].y + 1] == TileType.Enemy_Elite_Queen)
                                            {

                                                Zone[pp, kk + 1].Num_setting(qq);
                                                Debug.Log("찾았다!");
                                                break;
                                            }
                                            else if (MapData.instance._tile[zz[rr].x, zz[rr].y + 1] == TileType.Wall)
                                            {
                                                Zone[pp, kk + 1].Num_setting(0);
                                                //벽인지 판정

                                            }
                                            else if (Zone[pp, kk + 1].num < 0)
                                            {
                                                Zone[pp, kk + 1].Num_setting(qq);
                                                z.Enqueue(Zone[pp, kk + 1]);
                                                queueCnt++;


                                            }
                                        }
                                    }
                                    if (pp - 1 >= 0 && pp - 1 < 7 && kk >= 0 && kk < 7)
                                    {
                                        if (zz[rr].x - 1 >= 0 && zz[rr].x - 1 < _size &&
                                            zz[rr].y >= 0 && zz[rr].y < _size)
                                        {

                                            if (MapData.instance._tile[zz[rr].x - 1, zz[rr].y] == TileType.Enemy_Elite_Queen)
                                            {
                                                Zone[pp - 1, kk].Num_setting(qq);
                                                Debug.Log("찾았다!");
                                                break;
                                            }
                                            else if (MapData.instance._tile[zz[rr].x - 1, zz[rr].y] == TileType.Wall)
                                            {
                                                Zone[pp - 1, kk].Num_setting(0);
                                                //벽인지 판정

                                            }
                                            else if (Zone[pp - 1, kk].num < 0)
                                            {
                                                Zone[pp - 1, kk].Num_setting(qq);
                                                z.Enqueue(Zone[pp - 1, kk]);
                                                queueCnt++;


                                            }
                                        }
                                    }
                                    if (pp >= 0 && pp < 7 && kk - 1 >= 0 && kk - 1 < 7)
                                    {
                                        if (zz[rr].x >= 0 && zz[rr].x < _size &&
                                            zz[rr].y - 1 >= 0 && zz[rr].y - 1 < _size)
                                        {
                                            if (MapData.instance._tile[zz[rr].x, zz[rr].y - 1] == TileType.Enemy_Elite_Queen)
                                            {
                                                Zone[pp, kk - 1].Num_setting(qq);
                                                Debug.Log("찾았다!");
                                                break;
                                            }
                                            else if (MapData.instance._tile[zz[rr].x, zz[rr].y - 1] == TileType.Wall)
                                            {
                                                Zone[pp, kk - 1].Num_setting(0);
                                                //벽인지 판정
                                            }
                                            else if (Zone[pp, kk - 1].num < 0)
                                            {
                                                Zone[pp, kk - 1].Num_setting(qq);
                                                z.Enqueue(Zone[pp, kk - 1]);
                                                queueCnt++;


                                            }
                                        }
                                    }
                                    qCnt--;
                                    if (qCnt <= 0)
                                    {
                                        qCnt = queueCnt;
                                        queueCnt = 0;
                                        qq++;
                                    }
                                    if (z.Count() == 0)
                                    {
                                        Debug.Log("퀸이 벽에 막혀있음!");
                                        break;
                                    }
                                    zz[rr] = z.Dequeue();
                                }

                                for (int qw = 0; qw < 7; qw++)
                                {
                                    for (int wq = 0; wq < 7; wq++)
                                    {
                                        Debug.Log("퀸존 : (" + wq + "," + qw + ") = " + Zone[wq, qw].num);
                                    }
                                }

                                int[] goal_X = new int[12];
                                int[] goal_Y = new int[12];

                                int[] array_Nums = new int[20];
                                int array_cnt = 0;

                                for (int v = 1; v < 4; v++)
                                {
                                    Debug.Log(Zone[3, 3 + v].num + ", " + Zone[3, 3 + v].x + ", " + Zone[3, 3 + v].y);
                                    if (Zone[3, 3 + v].num > 0 && Zone[3, 3 + v].x < _size && Zone[3, 3 + v].x >= 0 && Zone[3, 3 + v].y < _size && Zone[3, 3 + v].y >= 0)

                                    {
                                        goal_X[array_cnt] = Zone[3, 3 + v].x;
                                        goal_Y[array_cnt] = Zone[3, 3 + v].y;
                                        array_Nums[array_cnt] = Zone[3, 3 + v].num;
                                        Debug.Log("1 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                        array_cnt++;
                                    }
                                    else break;
                                }
                                for (int vv = 1; vv < 4; vv++)
                                {
                                    if (Zone[3, 3 - vv].x < _size && Zone[3, 3 - vv].x >= 0 && Zone[3, 3 - vv].y < _size && Zone[3, 3 - vv].y >= 0
                                        && Zone[3, 3 - vv].num > 0)
                                    {
                                        goal_X[array_cnt] = Zone[3, 3 - vv].x;
                                        goal_Y[array_cnt] = Zone[3, 3 - vv].y;
                                        array_Nums[array_cnt] = Zone[3, 3 - vv].num;
                                        Debug.Log("2 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                        array_cnt++;
                                    }
                                    else break;
                                }
                                for (int m = 1; m < 4; m++)
                                {
                                    if (Zone[3 - m, 3].x < _size && Zone[3 - m, 3].x >= 0 && Zone[3 - m, 3].y < _size && Zone[3 - m, 3].y >= 0
                                        && Zone[3 - m, 3].num > 0)
                                    {

                                        goal_X[array_cnt] = Zone[3 - m, 3].x;
                                        goal_Y[array_cnt] = Zone[3 - m, 3].y;
                                        array_Nums[array_cnt] = Zone[3 - m, 3].num;
                                        Debug.Log("3 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                        array_cnt++;
                                    }
                                    else break;
                                }
                                for (int mm = 1; mm < 4; mm++)
                                {
                                    if (Zone[3 + mm, 3].x < _size && Zone[3 + mm, 3].x >= 0 && Zone[3 + mm, 3].y < _size && Zone[3 + mm, 3].y >= 0
                                        && Zone[3 + mm, 3].num > 0)
                                    {
                                        goal_X[array_cnt] = Zone[3 + mm, 3].x;
                                        goal_Y[array_cnt] = Zone[3 + mm, 3].y;
                                        array_Nums[array_cnt] = Zone[3 + mm, 3].num;
                                        array_cnt++;
                                        Debug.Log("4 / 배열 " + array_cnt + "번째 원소: " + array_Nums[array_cnt]);
                                    }
                                    else break;
                                }
                                int rookQueenTo_X = goal_X[0];
                                int rookQueenTo_Y = goal_Y[0];
                                for (int iop = 0; iop < 11; iop++)
                                {
                                    if ((array_Nums[iop] > 0 && array_Nums[iop + 1] > 0 && array_Nums[iop] > array_Nums[iop + 1]) || array_Nums[iop + 1] == 10)
                                    {
                                        rookQueenTo_X = goal_X[iop + 1];
                                        rookQueenTo_Y = goal_Y[iop + 1];
                                        if (array_Nums[iop + 1] == 10) break;
                                    }

                                }




                                MapData.instance.elite_Queen_Location_X = rookQueenTo_X;
                                MapData.instance.elite_Queen_Location_Y = rookQueenTo_Y;
                                if (MapData.instance._tile[rookQueenTo_X, rookQueenTo_Y] == TileType.Player)
                                {
                                    Debug.Log("퀸과 전투합니다");
                                    //전투씬
                                    MapData.instance.elite_Queen_Location_X = -1;
                                    MapData.instance.elite_Queen_Location_Y = -1;
                                    isQueenDie = true;
                                    LoadingSceneManager.LoadScene("ForestBoss");
                                }
                                else
                                {
                                    MapData.instance._tile[MapData.instance.elite_Queen_Location_X, MapData.instance.elite_Queen_Location_Y] = TileType.Enemy_Elite_Rook;
                                    enemy_Elite[3].transform.localPosition  = new Vector3(MapData.instance.elite_Queen_Location_X * 10, 0, MapData.instance.elite_Queen_Location_Y * 10);
                                }



                            }
                        }
                        else if (rookOrBishop == 0)
                        {
                            Debug.Log("퀸: 비숍처럼 움직임");
                            MapData.instance._tile[MapData.instance.elite_Queen_Location_X, MapData.instance.elite_Queen_Location_Y] = TileType.Empty;

                            int x = MapData.instance.elite_Queen_Location_X - playerNowPoint_X;

                            int goal_x = MapData.instance.elite_Queen_Location_X;
                            int goal_y = MapData.instance.elite_Queen_Location_Y;
                            int move_x = goal_x;
                            int move_y = goal_y;
                            bool isXnagative = false;
                            bool isYnagative = false;
                            bool isMove = false;

                            int[] moveQueen_X = new int[2]
                                { MapData.instance.elite_Queen_Location_X+1, MapData.instance.elite_Queen_Location_X-1};
                            int[] moveQueen_Y = new int[2]
                                {MapData.instance.elite_Queen_Location_Y+1, MapData.instance.elite_Queen_Location_Y-1 };


                            if (x == 0)
                            {

                            }
                            else
                            {

                                if (x < 0) isXnagative = true;
                                else isXnagative = false;

                                goal_x -= x;
                                if (MapData.instance.elite_Queen_Location_Y > playerNowPoint_Y)
                                {
                                    isYnagative = false;
                                    goal_y -= Mathf.Abs(x);
                                }
                                else
                                {
                                    isYnagative = true;
                                    goal_y += Mathf.Abs(x);
                                }

                                for (int bb = 1; bb < Mathf.Abs(x) + 1; bb++)
                                {
                                    if (isXnagative && isYnagative)
                                    {
                                        if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X + bb,
                                            MapData.instance.elite_Queen_Location_Y + bb] == TileType.Wall)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            move_x = MapData.instance.elite_Queen_Location_X + bb;
                                            move_y = MapData.instance.elite_Queen_Location_Y + bb;
                                            isMove = true;
                                        }
                                        if (move_x == goal_x && move_y == goal_y) break;
                                    }
                                    else if (!isXnagative && isYnagative)
                                    {
                                        if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X - bb,
                                            MapData.instance.elite_Queen_Location_Y + bb] == TileType.Wall)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            move_x = MapData.instance.elite_Queen_Location_X - bb;
                                            move_y = MapData.instance.elite_Queen_Location_Y + bb;
                                            isMove = true;
                                        }
                                        if (move_x == goal_x && move_y == goal_y) break;
                                    }
                                    else if (isXnagative && !isYnagative)
                                    {
                                        if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X + bb,
                                            MapData.instance.elite_Queen_Location_Y - bb] == TileType.Wall)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            move_x = MapData.instance.elite_Queen_Location_X + bb;
                                            move_y = MapData.instance.elite_Queen_Location_Y - bb;
                                            isMove = true;
                                        }
                                        if (move_x == goal_x && move_y == goal_y) break;
                                    }
                                    else if (!isXnagative && !isYnagative)
                                    {
                                        if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X - bb,
                                            MapData.instance.elite_Queen_Location_Y - bb] == TileType.Wall)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            move_x = MapData.instance.elite_Queen_Location_X - bb;
                                            move_y = MapData.instance.elite_Queen_Location_Y - bb;
                                            isMove = true;
                                        }
                                        if (move_x == goal_x && move_y == goal_y) break;
                                    }

                                }
                                if (isMove == false)
                                {
                                    int random_x = Random.Range(0, 2);
                                    int random_y = Random.Range(0, 2);
                                    goal_x = moveQueen_X[random_x];
                                    goal_y = moveQueen_Y[random_y];
                                }
                                else
                                {
                                    goal_x = move_x;
                                    goal_y = move_y;
                                }

                                if (MapData.instance._tile[goal_x, goal_y] == TileType.Player)
                                {
                                    Debug.Log("퀸과 전투");
                                    isQueenDie = true;
                                    MapData.instance.elite_Queen_Location_X = -1;
                                    MapData.instance.elite_Queen_Location_Y = -1;
                                    LoadingSceneManager.LoadScene("ForestBoss");

                                }
                                else
                                {
                                    MapData.instance.elite_Queen_Location_X = goal_x;
                                    MapData.instance.elite_Queen_Location_Y = goal_y;


                                    MapData.instance._tile[MapData.instance.elite_Queen_Location_X, MapData.instance.elite_Queen_Location_Y] = TileType.Enemy_Elite_Queen;
                                    enemy_Elite[3].transform.localPosition = new Vector3(MapData.instance.elite_Queen_Location_X * 10, 0, MapData.instance.elite_Queen_Location_Y * 10);
                                }
                            }
                        }
                    }
                    break;

            }
        }


    }

    public class Rook_zone
    {
        public int x;
        public int y;
        public int num = -1;

        public void XY_setting(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public void Num_setting(int _num)
        {
            this.num = _num;
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
                                break;
                            }

                            else
                            {
                                
                                Knight_Check = false;
                            }
                        }
                    }
                    if (Knight_Check) break;
                }
                break;
            case "Rook":
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        if (MapData.instance.elite_Rook_Location_X + i < _size && MapData.instance.elite_Rook_Location_Y + j < _size && MapData.instance.elite_Rook_Location_X + i >= 0 && MapData.instance.elite_Rook_Location_Y + j >= 0)
                        {

                            if (MapData.instance._tile[MapData.instance.elite_Rook_Location_X + i, MapData.instance.elite_Rook_Location_Y + j] == TileType.Player)
                            {
                                Rook_Check = true;
                                Debug.Log("룩 : 플레이어 인식");
                                break;
                            }
                            else Rook_Check = false;
                        }
                    }
                    if (Rook_Check) break;
                }

                break;
            case "Bishop":
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        if (MapData.instance.elite_Bishop_Location_X + i < _size && MapData.instance.elite_Bishop_Location_Y + j < _size
                            && MapData.instance.elite_Bishop_Location_X + i >= 0 && MapData.instance.elite_Bishop_Location_Y + j >= 0)
                        {
                            if (MapData.instance._tile[MapData.instance.elite_Bishop_Location_X + i, MapData.instance.elite_Bishop_Location_Y + j] == TileType.Player)
                            {
                                Bishop_Check = true;
                                Debug.Log("비숍 : 플레이어 인식");

                                break;
                            }

                            else Bishop_Check = false;
                        }
                    }
                    if (Bishop_Check) break;
                }
                break;
            case "Queen":
                
                for (int i = -3; i < 4; i++)
                {
                    for (int j = -3; j < 4; j++)
                    {
                        if (MapData.instance.elite_Queen_Location_X + i < _size && MapData.instance.elite_Queen_Location_Y + j < _size
                            && MapData.instance.elite_Queen_Location_X + i >= 0 && MapData.instance.elite_Queen_Location_Y + j >= 0)
                        {
                            if (MapData.instance._tile[MapData.instance.elite_Queen_Location_X + i, MapData.instance.elite_Queen_Location_Y + j] == TileType.Player)
                            {
                                Queen_Check = true;
                                Debug.Log("퀸 : 플레이어 인식");
                                break;

                            }
                            else Queen_Check = false;
                        }
                    }
                    if (Queen_Check) break;
                }
                
                break;
        }
    }


}
