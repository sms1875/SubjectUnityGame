using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Empty,
    Wall,
    Player,
    Event,
    Enemy_Normal,
    Enemy_Elite_Knight,
    Enemy_Elite_Rook,
    Enemy_Elite_Bishop,
    Enemy_Elite_Queen,
    Enemy_Boss

}

public class MapManager : MonoBehaviour
{
    public TileType[,] _tile;

    
    public int _size;
    public int playerStartingPoint_X, playerStartingPoint_Y;
    public int playerNowPoint_X, playerNowPoint_Y;

    public int elite_Knight_Location_X, elite_Knight_Location_Y, elite_Rook_Location_X, elite_Rook_Location_Y, elite_Bishop_Location_X, elite_Bishop_Location_Y,
        elite_Queen_Location_X, elite_Queen_Location_Y;
    public int elite_Cnt = 2;
    public GameObject[] Tile;
    
    public Dictionary<string, string> TileDic = new Dictionary<string, string>();
    
    public Transform Player;
    public GameObject playerMoving;
    public GameObject[] enemy_Elite;

    EnemyOnBoard enemyOnBoard;
    bool isPlayerReady = false;

    public bool isMove = false;
    public bool isKight = false;

    public bool isPlayerWalk = false;
    public bool isEliteLose = true;

    void Start()
    {
        Initialize(_size);
        enemyOnBoard = GameObject.Find("Elite1").GetComponent<EnemyOnBoard>();

    }

    public Camera mainCamera;
    Vector3 mousePos = Vector3.zero;

    public void Initialize(int size)
    {
        _tile = new TileType[size, size];
        
        _size = size;
        string tilePos;

        // 엘리트 몬스터 배치 (고정자리)
        elite_Knight_Location_X = 0;
        elite_Knight_Location_Y = 0;
        _tile[elite_Knight_Location_X,elite_Knight_Location_Y] = TileType.Enemy_Elite_Knight;

        elite_Rook_Location_X = 0;
        elite_Rook_Location_Y = 7;
        _tile[elite_Rook_Location_X,elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;

        elite_Bishop_Location_X = 7;
        elite_Bishop_Location_Y = 0;
        _tile[elite_Bishop_Location_X,elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;

        elite_Queen_Location_X = 7;
        elite_Queen_Location_Y = 7;
        _tile[elite_Queen_Location_X, elite_Queen_Location_Y] = TileType.Enemy_Elite_Queen;


        int aa = 0;
        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                _tile[y, x] = TileType.Empty;
                tilePos = string.Format("{0},{1}", x, y);
                
                TileDic.Add(Tile[aa++].name, tilePos);
                
            }


        }

        playerNowPoint_X = playerStartingPoint_X;
        playerNowPoint_Y = playerStartingPoint_Y;

        _tile[playerStartingPoint_X, playerStartingPoint_Y] = TileType.Player;
        


        for (int z = 0; z < 10; z++)
        {
            int a = Random.Range(0, _size);
            int b = Random.Range(0, _size);
            int c = Random.Range(0, _size);
            int d = Random.Range(0, _size);

            if (_tile[a, b] == TileType.Empty && _tile[c,d] == TileType.Empty)
            {
                _tile[a, b] = TileType.Event;
                _tile[c, d] = TileType.Enemy_Normal;

            }
            else
            {
                if (z <= 0) { }
                else z--;
                 
                
            }

        }

        
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

                else if (isPlayerReady == true)
                {
                    string xy;
                    TileDic.TryGetValue(raycastHit.transform.name, out xy);

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
            for(int i=0; i<_size; i++)
            {

            }
        }
    }

    public void PlayerMove(int x, int y)
    {
        int b = playerNowPoint_X + 2;
        int a = playerNowPoint_X - 2;
        int d = playerNowPoint_Y + 2;
        int c = playerNowPoint_Y - 2;


        if (isKight)
        {
            //나이트 조건
            if ((x == playerNowPoint_X + 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) || (x == playerNowPoint_X - 1 && (y == playerNowPoint_Y + 2 || y == playerNowPoint_Y - 2)) ||
                (x == playerNowPoint_X + 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)) || (x == playerNowPoint_X - 2 && (y == playerNowPoint_Y + 1 || y == playerNowPoint_Y - 1)))
            {
                if (_tile[x, y] == TileType.Empty)
                {
                    StartCoroutine(MovingPlayer(x, y));
                    isKight = false;
                }
                else if (_tile[x, y] == TileType.Wall)
                {
                    Debug.Log("벽이 있는 타일은 갈 수 없습니다.");
                }
                else if (_tile[x, y] == TileType.Event)
                {
                    Debug.Log("이벤트 발생");
                    StartCoroutine(MovingPlayer(x, y));
                    //추후 이벤트 발생 함수 호출 필요함
                }

            }
            else
            {
                Debug.Log("해당 칸은 이동 범위가 아니므로 이동할 수 없습니다.");
            }

        }

        //플레이어 이동이 체스 킹 기준일 때만 작성되어있음
        else
        {
            if (a < x && x < b && y < d && c < y)
            {
                if (_tile[x, y] == TileType.Empty)
                {
                    StartCoroutine(MovingPlayer(x, y));
                }
                else if (_tile[x, y] == TileType.Wall)
                {
                    Debug.Log("벽이 있는 타일은 갈 수 없습니다.");
                }
                else if (_tile[x, y] == TileType.Event)
                {
                    Debug.Log("이벤트 발생");
                    StartCoroutine(MovingPlayer(x, y));
                    //추후 이벤트 발생 함수 호출 필요함
                }

            }
            else
            {
                Debug.Log("해당 칸은 이동 범위가 아니므로 이동할 수 없습니다.");
            }
        }


        {
            //string tilePos = TileDic.FirstOrDefault(x => x.Value.name == tileName).Key;
            //1. x,y와 플레이어 사이가 이동할 수 있는 거리 체크
            //2. 이동할 수 있으면 이동하는 x,y 타일 이벤트 체크
            //3. 타일 종류 따라서 이동


            /*
            if (playerNowPoint_X >= 0 && playerNowPoint_Y >= 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {

                    if (_tile[playerNowPoint_X - 1, playerNowPoint_Y] == TileType.Event)
                    {
                        Debug.Log(playerNowPoint_X - 1 + "," + playerNowPoint_Y + "는 이벤트 타일입니다");

                    }
                    else if (_tile[playerNowPoint_X - 1, playerNowPoint_Y] == TileType.Wall)
                    {
                        Debug.Log(playerNowPoint_X - 1 + "," + playerNowPoint_Y + "는 벽이므로 이동할 수 없습니다");
                    }
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
                    playerNowPoint_X--;
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (_tile[playerNowPoint_X, playerNowPoint_Y - 1] == TileType.Event)
                    {
                        Debug.Log(playerNowPoint_X + "," + playerNowPoint_Y + " -1 는 이벤트 타일입니다");

                    }
                    else if (_tile[playerNowPoint_X - 1, playerNowPoint_Y - 1] == TileType.Wall)
                    {
                        Debug.Log(playerNowPoint_X - 1 + "," + playerNowPoint_Y + " -1 는 벽이므로 이동할 수 없습니다");
                    }
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
                    playerNowPoint_Y--;
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (_tile[playerNowPoint_X + 1, playerNowPoint_Y] == TileType.Event)
                    {
                        Debug.Log(playerNowPoint_X + 1 + "," + playerNowPoint_Y + "는 이벤트 타일입니다");

                    }
                    else if (_tile[playerNowPoint_X + 1, playerNowPoint_Y] == TileType.Wall)
                    {
                        Debug.Log(playerNowPoint_X + 1 + "," + playerNowPoint_Y + "는 벽이므로 이동할 수 없습니다");
                    }
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
                    playerNowPoint_X++;
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (_tile[playerNowPoint_X, playerNowPoint_Y + 1] == TileType.Event)
                    {
                        Debug.Log(playerNowPoint_X + "," + playerNowPoint_Y + 1 + "는 이벤트 타일입니다");

                    }
                    else if (_tile[playerNowPoint_X, playerNowPoint_Y + 1] == TileType.Wall)
                    {
                        Debug.Log(playerNowPoint_X + "," + playerNowPoint_Y + 1 + "는 벽이므로 이동할 수 없습니다");
                    }
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
                    playerNowPoint_Y++;
                    _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    Debug.Log("지금 플레이어 위치는 " + playerNowPoint_X + "," + playerNowPoint_Y + "입니다");
                }
            }
            else Debug.Log("범위 밖입니다.");*/
        }
    }

    IEnumerator MovingPlayer(int x, int y)
    {
        isMove = true;
        isPlayerWalk = true;
        playerMoving.GetComponent<Player>().checkPlayerWalk();
        //플레이어 애니메이션으로 천천히 이동시키기

        _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Empty;
        
        Player.position += new Vector3((x - playerNowPoint_X) * 10, 0, (y - playerNowPoint_Y) * 10);
        playerNowPoint_X = x;
        playerNowPoint_Y = y;
        _tile[playerNowPoint_X, playerNowPoint_Y] = TileType.Player;

        yield return new WaitForSeconds(2.3f);

        isPlayerWalk = false;
        playerMoving.GetComponent<Player>().checkPlayerWalk();




        //플레이어 이동에 걸리는 시간
        yield return new WaitForSeconds(1f);

        //--몬스터 이동 함수--

        EliteMoving();
        //몬스터 이동에 걸리는 시간
        Debug.Log("플레이어 위치" + playerNowPoint_X + "," + playerNowPoint_Y);
        Debug.Log("나이트 위치" + elite_Knight_Location_X + "," + elite_Knight_Location_Y);
        yield return new WaitForSeconds(1f);



        //이동 활성화
        isMove = false;

    }

    bool Knight_Check, Bishop_Check, Rook_Check, Queen_Check;

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
                        int xx = elite_Knight_Location_X;
                        int yy = elite_Knight_Location_Y;

                        var player_Knight = new int[,] { { x - 2, y + 1}, { x - 2, y - 1},{ x - 1, y + 2 }, { x - 1, y - 2 },{ x + 1, y + 2 },{ x + 1, y - 2 },{ x + 2, y - 1 },{ x + 2, y + 1 } };
                        var knight_Player = new int[,] { { xx - 2, yy + 1 }, { xx - 2, yy - 1 }, { xx - 1, yy + 2 }, { xx - 1, yy - 2 }, { xx + 1, yy + 2 }, { xx + 1, yy - 2 }, { xx + 2, yy - 1 }, { xx + 2, yy + 1 } };


                        for (int k = 0; k < 8; k++)
                        {
                            if (_tile[knight_Player[k, 0], knight_Player[k, 1]] == TileType.Player)
                            {
                                elite_Knight_Location_X = knight_Player[k, 0];
                                elite_Knight_Location_Y = knight_Player[k, 1];
                                //전투 처리
                            }
                            else if (player_Knight[k, 0] == knight_Player[k,0] && player_Knight[k,1] == knight_Player[k,1])
                            {
                                elite_Knight_Location_X = knight_Player[k, 0];
                                elite_Knight_Location_Y = knight_Player[k, 1];
                                //맵에서 실제로 이동하는거 구현
                            }
                        }
                        //주석처리된 코드
                        {/*
                        if (_tile[elite_Knight_Location_X + 2, elite_Knight_Location_Y + 1] == TileType.Player || // 당장 잡을 수 있는 칸에 플레이어가 있는지?
                            _tile[elite_Knight_Location_X + 2, elite_Knight_Location_Y - 1] == TileType.Player ||
                            _tile[elite_Knight_Location_X - 2, elite_Knight_Location_Y + 1] == TileType.Player ||
                            _tile[elite_Knight_Location_X - 2, elite_Knight_Location_Y - 1] == TileType.Player ||
                            _tile[elite_Knight_Location_X + 1, elite_Knight_Location_Y + 2] == TileType.Player ||
                           _tile[elite_Knight_Location_X + 1, elite_Knight_Location_Y - 2] == TileType.Player ||
                           _tile[elite_Knight_Location_X - 1, elite_Knight_Location_Y + 2] == TileType.Player ||
                           _tile[elite_Knight_Location_X - 1, elite_Knight_Location_Y - 2] == TileType.Player
                           )
                        {
                            //나이트 전투 
                        }
                        else if()*/
                        }
                    }
                    break;
                case 2:
                    Elite_CheckPlayer("Rook");
                    if (Rook_Check)
                    {
                        //주석처리된 코드

                        {
                            /*if (elite_Rook_Location_X > playerNowPoint_X)
                            {
                                elite_Rook_Location_X -= 2;
                            }
                            else if (elite_Rook_Location_X < playerNowPoint_X)
                            {
                                elite_Rook_Location_X += 2;
                            }
                            else if (elite_Rook_Location_Y < playerNowPoint_Y)
                            {
                                elite_Rook_Location_Y += 2;
                            }
                            else if (elite_Rook_Location_Y > playerNowPoint_Y)
                            {
                                elite_Rook_Location_Y -= 2;
                            }
                            else if (Mathf.Abs(elite_Rook_Location_X - playerNowPoint_X)<2 && Mathf.Abs(elite_Rook_Location_X - playerNowPoint_X)>0)
                            {
                                elite_Rook_Location_X = playerNowPoint_X;
                            }
                            else if (Mathf.Abs(elite_Rook_Location_Y - playerNowPoint_Y) < 2 && Mathf.Abs(elite_Rook_Location_Y - playerNowPoint_Y) > 0)
                            {
                                elite_Rook_Location_Y = playerNowPoint_Y;
                            }*/
                        }

                        if (_tile[elite_Rook_Location_X, elite_Rook_Location_Y] != TileType.Player)
                        {
                            _tile[elite_Rook_Location_X, elite_Rook_Location_Y] = TileType.Empty;
                        }
                        
                        if(elite_Rook_Location_X != playerNowPoint_X)
                        {
                            elite_Rook_Location_X = playerNowPoint_X;
                        }
                        else if(elite_Rook_Location_Y != playerNowPoint_Y)
                        {
                            elite_Rook_Location_Y = playerNowPoint_Y;
                        }
                        
                        if(elite_Rook_Location_X == playerNowPoint_X && elite_Rook_Location_Y == playerNowPoint_Y)
                        {
                            Debug.Log("룩 플레이어와 전투");
                            //겹쳐서 전투일 때 플레이어를 지우지 않도록 해야함
                            isEliteLose = false;
                        }
                        // 실제 맵에서 움직이는 코드 필요함
                        if (isEliteLose)
                        {
                            _tile[elite_Rook_Location_X, elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;
                        }
                    }
                    break;
                case 3:
                    Elite_CheckPlayer("Bishop");
                    if (Bishop_Check)
                    {
                        _tile[elite_Bishop_Location_X, elite_Bishop_Location_Y] = TileType.Empty;

                        int x = elite_Bishop_Location_X - playerNowPoint_X;
                        elite_Bishop_Location_X -= x;
                        if (elite_Bishop_Location_Y > playerNowPoint_Y)
                        {
                            elite_Bishop_Location_Y -= Mathf.Abs(x);
                        } else
                        {
                            elite_Bishop_Location_Y += Mathf.Abs(x);
                        }
                        _tile[elite_Bishop_Location_X, elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;

                        //플레이어와 겹쳐졌는지 확인 필요
                    }
                    break;
                case 4:
                    Elite_CheckPlayer("Queen");
                    if (Queen_Check)
                    {
                        int aaa = Random.Range(0, 2);
                        if(aaa== 1)
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
                for(int i=-2; i<3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        if (elite_Knight_Location_X + i < _size && elite_Knight_Location_Y + j < _size && elite_Knight_Location_X + i >= 0 && elite_Knight_Location_Y + j >= 0)
                        {
                            if (_tile[elite_Knight_Location_X + i, elite_Knight_Location_Y + j] == TileType.Player)
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
                    if (elite_Rook_Location_X + i < _size && elite_Rook_Location_Y + i < _size && elite_Rook_Location_X +i >=0 && elite_Rook_Location_Y +i >=0)
                    {
                        if (_tile[elite_Rook_Location_X + i, elite_Rook_Location_Y + i] == TileType.Player)
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
                    if (elite_Bishop_Location_X + i < _size && elite_Bishop_Location_Y + i < _size && elite_Bishop_Location_X+i >=0 && elite_Bishop_Location_Y+i >=0)
                    {
                        if (_tile[elite_Bishop_Location_X + i, elite_Bishop_Location_Y + i] == TileType.Player)
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
                    if (elite_Queen_Location_X + i < _size && elite_Queen_Location_Y + i < _size && elite_Queen_Location_X+i>=0 && elite_Queen_Location_Y +i >=0)
                    {
                        if (_tile[elite_Queen_Location_X + i, elite_Queen_Location_Y + i] == TileType.Player)
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

