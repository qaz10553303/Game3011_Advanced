using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : Singletonbase<Grid>
{

    public int gridSize=32;
    public Dictionary<int,Tile> tilesDict;
    public GameObject tilePrefab;

    void Start()
    {
        Init_();
        SetSpecialTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init_()
    {
        tilesDict = new Dictionary<int, Tile>();

        float tileSideSize = GetComponent<RectTransform>().rect.width / gridSize;
        Debug.Log(tileSideSize);
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Tile newTile = Instantiate(tilePrefab, this.transform, false).GetComponent<Tile>();
                tilesDict.Add(y*gridSize+x,newTile);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * tileSideSize, y * tileSideSize);
                newTile.x = x;
                newTile.y = y;
                newTile.type = Tile.ResourceType.NONE;
            }
        }
    }

    void SetSpecialTiles()
    {
        int specialTileX = Random.Range(2, gridSize - 2);
        int specialTileY = Random.Range(2, gridSize - 2);
        tilesDict[specialTileY * gridSize+ specialTileX].type = Tile.ResourceType.MAX;
        tilesDict[specialTileY * gridSize + specialTileX].amountScale = Tile.ResourceAmountScale.MAX;
        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (x==2||x==-2||y==2||y==-2)
                {
                    tilesDict[(specialTileY + y) * gridSize + specialTileX + x].type = Tile.ResourceType.QUATER;
                    tilesDict[(specialTileY + y) * gridSize + specialTileX + x].amountScale = Tile.ResourceAmountScale.QUATER;
                }
                else if (x == 1 || x == -1 || y == 1 || y == -1)
                {
                    tilesDict[(specialTileY + y) * gridSize + specialTileX + x].type = Tile.ResourceType.HALF;
                    tilesDict[(specialTileY + y) * gridSize + specialTileX + x].amountScale = Tile.ResourceAmountScale.HALF;
                }
            }
        }
    }

    public void Scan(Tile center)
    {
        GameManager.instance.PrintMessage("Scan at " + "["+center.x+"," + center.y+"]\n"+GameManager.instance.scanChance+" times left!");
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if(tilesDict.ContainsKey((center.y + y) * gridSize + center.x + x))
                {
                    tilesDict[(center.y + y) * gridSize + center.x + x].isScanned = true;
                }
            }
        }
    }

    public void Extract(Tile center)
    {
        GameManager.instance.resourceAmount += (int)center.amountScale * GameManager.instance.oneScale;
        GameManager.instance.PrintMessage("You've gained " + (int)center.amountScale * GameManager.instance.oneScale+" resource\n" + GameManager.instance.extractChance + " times left!");
        center.type = Tile.ResourceType.NONE;
        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (tilesDict.ContainsKey((center.y + y) * gridSize + center.x + x))
                {
                    DownGradeResource(tilesDict[(center.y + y) * gridSize + center.x + x]);
                }
            }
        }
    }

    public void DownGradeResource(Tile tile)
    {
        switch (tile.type)
        {
            case Tile.ResourceType.NONE:
                break;
            case Tile.ResourceType.MAX:
                tile.type = Tile.ResourceType.HALF;
                break;
            case Tile.ResourceType.HALF:
                tile.type = Tile.ResourceType.QUATER;
                break;
            case Tile.ResourceType.QUATER:
                tile.type = Tile.ResourceType.NONE;
                break;
            default:
                break;
        }
    }

}
