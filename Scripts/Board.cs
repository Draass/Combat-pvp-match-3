using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board size")]
    public int boardWidth;
    public private int boardHeight;

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
                backgroundTile.transform.parent = this.transform;
                //name is position in space
                backgroundTile.name = "(" + i + ", " + j + ")";
                
                //choose a random tile for the segment
                int tileToUse = Random.Range(0, tiles.Length);
                
                backgroundTile.tag = tiles[tileToUse].tag;
                
                int maxIterations = 0;
                while (isMatchOnStart(i, j, tiles[tileToUse]) && maxIterations < 100)
                {
                    tileToUse = Random.Range(0, tiles.Length);
                    maxIterations++;
                }
                //Debug.Log(maxIterations);
                maxIterations = 0;
                InitializeTiles(backgroundTile.name, tileStartPosition, tileToUse);
                
                allBackgroundTiles[i, j] = tiles[tileToUse];
            }
        }
    }
    
    void InitializeTiles(string backgroundTileName, Vector2 tileStartPosition, int tileToUse)
    {
        //instantiate tile
        GameObject tile = Instantiate(tiles[tileToUse], tileStartPosition, Quaternion.identity);
        //make dot a child of a tile section
        tile.transform.parent = this.transform;
        //give it a proper name
        tile.name = backgroundTileName;
    }
//Check if there are matches on game beginning
private bool isMatchOnStart(int column, int row, GameObject piece)
    {
        if (column > 1)         
        {
            if (allBackgroundTiles[column - 1, row].tag == piece.tag &&
                allBackgroundTiles[column - 2, row].tag == piece.tag)
            {
                return true;
            }
        }

        if (row > 1)
        {
            if (allBackgroundTiles[column, row - 1].tag == piece.tag) 
            {
                if (allBackgroundTiles[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
            return false;
    }
}
