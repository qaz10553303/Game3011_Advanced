using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singletonbase<GameManager>
{
    public float resourceAmount;
    public Sprite extractIcon;
    public Sprite scanIcon;
    public Button modeToggleBtn;
    public int extractChance=3;
    public int scanChance=6;
    public Text msgText;
    public Text resourceText;
    public Text resultText;

    public int totalResourceAmount;
    public int oneScale;

    public GameObject canvas;
    public GameObject resultScreen;

    bool isGameOver;

    public enum Mode
    {
        SCAN_MODE,
        EXTRACT_MODE
    }

    public Mode mode;

    void Start()
    {
        mode = Mode.EXTRACT_MODE;
        oneScale = Random.Range(100, 200);
        totalResourceAmount = oneScale * 36;// 3600-7200
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        if (extractChance<=0&& !isGameOver)
        {
            GameOver();
        }
        resourceText.text = "Resource: " + resourceAmount;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!canvas.activeSelf)
            {
                canvas.SetActive(true);
                msgText.text = "";
            }
            else
            {
                canvas.SetActive(false);
                msgText.text = "Press [Tab] To Start";
            }
        }

    }

    public void ToggleMode()
    {
        mode= mode==Mode.SCAN_MODE?Mode.EXTRACT_MODE:Mode.SCAN_MODE;
        modeToggleBtn.image.sprite = mode == Mode.SCAN_MODE ? scanIcon : extractIcon;
        PrintMessage("Now is " + mode);
    }

    public void PrintMessage(string msg)
    {
        msgText.text = msg;
        msgText.CrossFadeAlpha(1, 0f, true);
        StopAllCoroutines();
        StartCoroutine("HideMessage");
    }


    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2f);
        msgText.CrossFadeAlpha(0, 3f, false);
    }

    void GameOver()
    {
        isGameOver = true;
        canvas.SetActive(false);
        msgText.gameObject.SetActive(false);
        resultText.text = "You've gathered " + resourceAmount + " resource. Congrats!";
        resultScreen.SetActive(true);
    }

    public void CloseResult()
    {

        resultScreen.SetActive(false);
    }
}
