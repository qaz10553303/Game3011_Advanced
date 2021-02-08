using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class ColorType
    {
        public Color noneColor = new Color(0.4f,1,0.4f,1);
        public Color maxColor = new Color(1,1,0,1);
        public Color halfColor = new Color(1,0.75f,0,1);
        public Color quaterColor = new Color(1,0.5f,0,1);
    }

    

    public enum ResourceAmountScale
    {
        NONE = 0,
        QUATER = 1,
        HALF = 2,
        MAX = 4,
    }


    public enum ResourceType
    {
        NONE,
        MAX,
        HALF,
        QUATER,
    }

    public bool isScanned;
    public ColorType color;
    public ResourceType type;
    public ResourceAmountScale amountScale;
    public int x;
    public int y;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isScanned)
        {
            UpdateTileInfo();
            GetComponentInChildren<Text>().text = "";
        }
    }

    void UpdateTileInfo()
    {
        switch (type)
        {
            case ResourceType.NONE:
                GetComponent<Image>().color = color.noneColor;
                amountScale = ResourceAmountScale.NONE;
                break;
            case ResourceType.MAX:
                GetComponent<Image>().color = color.maxColor;
                amountScale = ResourceAmountScale.MAX;
                break;
            case ResourceType.HALF:
                GetComponent<Image>().color = color.halfColor;
                amountScale = ResourceAmountScale.HALF;
                break;
            case ResourceType.QUATER:
                GetComponent<Image>().color = color.quaterColor;
                amountScale = ResourceAmountScale.QUATER;
                break;
            default:
                break;
        }
    }

    public void OnButtonDown()
    {
        switch (GameManager.instance.mode)
        {
            case GameManager.Mode.SCAN_MODE:
                if (GameManager.instance.scanChance > 0)
                {
                    GameManager.instance.scanChance -= 1;
                    Grid.instance.Scan(this);
                }
                else
                {
                    GameManager.instance.PrintMessage("You cannot scan anymore!");
                }
                break;
            case GameManager.Mode.EXTRACT_MODE:
                if (GameManager.instance.extractChance > 0)
                {
                    GameManager.instance.extractChance -= 1;
                    Grid.instance.Extract(this);
                }
                else
                {
                    GameManager.instance.PrintMessage("You cannot extract anymore!");
                }
                break;
            default:
                break;
        }
    }


}
