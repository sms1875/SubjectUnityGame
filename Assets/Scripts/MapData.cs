using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class MapData : MonoBehaviour
{
    public static MapData instance;

    public TileType[,] _tile;

    public Dictionary<string, string> TileDic = new Dictionary<string, string>();
    public int EventSceneSize;
    public int MonsterSceneSize;
    public int[] EventSceneList;
    public int[] MonsterSceneList;

    public int playerStartingPoint_X, playerStartingPoint_Y;
    public int bossX, bossY;
    public bool mapInit = false;


    public int elite_Knight_Location_X, elite_Knight_Location_Y, elite_Rook_Location_X, elite_Rook_Location_Y, elite_Bishop_Location_X, elite_Bishop_Location_Y,
    elite_Queen_Location_X, elite_Queen_Location_Y;
    public int elite_Cnt = 2;

    public GameObject wall;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize(int size, GameObject[] Tile)
    {
        
        int player_X = Random.Range(0, 2);
        int player_Y = Random.Range(0, 2);
        if(player_X == 0 && player_Y == 0)
        {
            playerStartingPoint_X = 0;
            playerStartingPoint_Y = 10;
            Debug.Log("플레이어 시작 위치는 :" + playerStartingPoint_X + ", " + playerStartingPoint_Y);
        }
        else if( player_X == 0 && player_Y == 1)
        {
            playerStartingPoint_X = 10;
            playerStartingPoint_Y = 0;
            Debug.Log("플레이어 시작 위치는 :" + playerStartingPoint_X + ", " + playerStartingPoint_Y);
        }
        else if( player_X == 1 && player_Y == 0)
        {
            playerStartingPoint_X = 19;
            playerStartingPoint_Y = 10;
            Debug.Log("플레이어 시작 위치는 :" + playerStartingPoint_X + ", " + playerStartingPoint_Y);

        }
        else if(player_X == 1 && player_Y == 1)
        {
            playerStartingPoint_X = 10;
            playerStartingPoint_Y = 19;
            Debug.Log("플레이어 시작 위치는 :" + playerStartingPoint_X + ", " + playerStartingPoint_Y);
        }
        
        _tile = new TileType[size, size];

        int num = 0;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                _tile[y, x] = TileType.Empty;
                TileDic.Add(Tile[num++].name, string.Format("{0},{1}", x, y));
            }
        }

        //플레이어 위치 설정
        _tile[playerStartingPoint_X, playerStartingPoint_Y] = TileType.Player;

        elite_Knight_Location_X = 0;
        elite_Knight_Location_Y = 0;
        _tile[elite_Knight_Location_X, elite_Knight_Location_Y] = TileType.Enemy_Elite_Knight;

        elite_Rook_Location_X = 0;
        elite_Rook_Location_Y = 19;
        _tile[elite_Rook_Location_X, elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;

        elite_Bishop_Location_X = 19;
        elite_Bishop_Location_Y = 0;
        _tile[elite_Bishop_Location_X, elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;

        elite_Queen_Location_X = 19;
        elite_Queen_Location_Y = 19;
        _tile[elite_Queen_Location_X, elite_Queen_Location_Y] = TileType.Enemy_Elite_Queen;

        #region 보스

        int boss_RandomNum = Random.Range(0, 4);
        switch (boss_RandomNum)
        {
            case 0:
                _tile[5, 5] = TileType.Enemy_Boss;
                bossX = 5;
                bossY = 5;
                break;
            case 1:
                _tile[5, 14] = TileType.Enemy_Boss;
                bossX = 5;
                bossY = 14;
                break;
            case 2:
                _tile[14, 5] = TileType.Enemy_Boss;
                bossX = 14;
                bossY = 5;
                break;
            case 3:
                _tile[14, 14] = TileType.Enemy_Boss;
                bossX = 14;
                bossY = 14;
                break;

        }
        // 보스 게임 오브젝트 넣고 소환시켜줘야함
        #endregion

        #region 벽

        for (int i = 1; i < size; i+=2)
        {
            for (int j = 0; j < 4; j++)
            {
                int noEmpty = 0;
                int wallY = Random.Range(0, size);
                if (_tile[i, wallY] == TileType.Empty)
                {
                    _tile[i, wallY] = TileType.Wall;
                    Instantiate(wall, new Vector3(i*10, 0, wallY*10), Quaternion.identity);
                }
                else
                {
                    j--;
                }
                noEmpty++;
                if (noEmpty > 20) break;
            }
        }
        #endregion




        #region 이벤트
        EventSceneList = new int[EventSceneSize];

        for (int i = 0; i < EventSceneSize; i++)
        {
            int eventX = Random.Range(0, size);
            int eventY = Random.Range(0, size);
            if (_tile[eventX, eventY] == TileType.Empty)
       
            {
                _tile[eventX, eventY] = TileType.Event;
                //Debug.Log(string.Format("{0},{1}", eventX, eventY));
                EventSceneList[i] = Random.Range(3, 6);
            }
            else
            {
                i--;
            }
        }
        #endregion

        #region 일반몬스터

        MonsterSceneList = new int[MonsterSceneSize];

        Debug.Log("123");
        for (int i = 0; i < MonsterSceneSize; i++)
        {
            int monsterX = Random.Range(0, size);
            int monsterY = Random.Range(0, size);
            if (_tile[monsterX, monsterY] == TileType.Empty)               
            {
                _tile[monsterX, monsterY] = TileType.Enemy_Normal;
                Debug.Log(string.Format("{0},{1}", monsterX, monsterY));
                MonsterSceneList[i] = Random.Range(3, 9);
            }
            else
            {
                i--;
            }
        }
        #endregion


    }

}
