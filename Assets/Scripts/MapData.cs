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

    public int playerStartingPoint_X, playerStartingPoint_Y;
    public bool mapInit = false;


    public int elite_Knight_Location_X, elite_Knight_Location_Y, elite_Rook_Location_X, elite_Rook_Location_Y, elite_Bishop_Location_X, elite_Bishop_Location_Y,
    elite_Queen_Location_X, elite_Queen_Location_Y;
    public int elite_Cnt = 2;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize(int size, GameObject[] Tile)
    {
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
        elite_Rook_Location_Y = 7;
        _tile[elite_Rook_Location_X, elite_Rook_Location_Y] = TileType.Enemy_Elite_Rook;

        elite_Bishop_Location_X = 7;
        elite_Bishop_Location_Y = 0;
        _tile[elite_Bishop_Location_X, elite_Bishop_Location_Y] = TileType.Enemy_Elite_Bishop;

        elite_Queen_Location_X = 7;
        elite_Queen_Location_Y = 7;
        _tile[elite_Queen_Location_X, elite_Queen_Location_Y] = TileType.Enemy_Elite_Queen;

        #region 이벤트
        EventSceneList = new int[EventSceneSize];

        for (int i = 0; i < EventSceneSize; i++)
        {
            int eventX = Random.Range(0, size);
            int eventY = Random.Range(0, size);
            if (_tile[eventX, eventY] == TileType.Empty)
            {
                _tile[eventX, eventY] = TileType.Event;
                Debug.Log(string.Format("{0},{1}", eventX, eventY));
                EventSceneList[i] = Random.Range(3, 6);
            }
            else
            {
                i--;
            }
        }
        #endregion

        #region 일반몬스터

        for (int i = 0; i < MonsterSceneSize; i++)
        {
            int monsterX = Random.Range(0, size);
            int monsterY = Random.Range(0, size);
            if (_tile[monsterX, monsterY] == TileType.Empty)
            {
                _tile[monsterX, monsterY] = TileType.Enemy_Normal;
            }
            else
            {
                i--;
            }
        }
        #endregion
    }


}
