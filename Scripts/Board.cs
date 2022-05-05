using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board size")]
    [SerializeField] private int boardWidth;
    [SerializeField] private int boardHeight;

    public GameObject tilePrefab;
    public GameObject[] tiles;


    private BackgroundTile[,] allTiles;
    public GameObject[,] allBackgroundTiles;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[boardWidth, boardHeight];
        allBackgroundTiles = new GameObject[boardWidth, boardHeight];
        SetTilesUp();
    }

    private void SetTilesUp()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for(int j = 0; j < boardHeight; j++)
            {
                Vector2 tileStartPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tileStartPosition, Quaternion.identity);
                    //as GameObject;
                backgroundTile.transform.parent = this.transform;
                //name is position in space
                backgroundTile.name = "(" + i + ", " + j + ")";
                InitializeTiles(backgroundTile.name, tileStartPosition);

                allBackgroundTiles[i, j] = backgroundTile;
            }
        }
    }
    //should divide this onto 2 separate methods. DONT FORGET
    void InitializeTiles(string backgroundTileName, Vector2 tileStartPosition)
    {
        //choose a random tile for the segment
        int tileToUse = Random.Range(0, tiles.Length);
        //instantiate this tile
        GameObject tile = Instantiate(tiles[tileToUse], tileStartPosition, Quaternion.identity);
        //make dot a child of a tile section
        tile.transform.parent = this.transform;
        //give it a proper name
        tile.name = backgroundTileName;
    }

}
