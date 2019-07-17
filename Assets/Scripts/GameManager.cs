using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool GameStart = false;
    public FishManager fishManager;

    public float roundLimit;

    public float bestTime;
    public float currentTime;

    private float timer;

    public Text timerText;
    public Text bestTimeText;
    public Text gameoverText;
    public float purpleTime;
    public float orangeTime;
    public float RedTime;
    public float greenTime;
    public float blueTime;

    public ParticleSystem ps;

    public Text popupText;

    public GameObject startText;

    // Use this for initialization
    void Start () {

        gameoverText.gameObject.SetActive(false);
        startText.SetActive(true);
        timerText.gameObject.SetActive(false);

    }

    void Awake()
    {
        LoadGame();
    }

    // Update is called once per frame
	void Update () {

	    if (Input.GetMouseButtonDown(0))
	    {
            playCircleParticle();

	        if (!GameStart)
	        {
                StartGame();
	        }
	    }

	    if (GameStart)
	    {
	        timer -= Time.deltaTime;
	        currentTime += Time.deltaTime;
            UpdateTimer();

	        if (timer <= 0)
	        {
                GameOver();
	        }
	    }

	    ps.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

	}

    public void StartGame()
    {
        fishManager.ClearPools();

        fishManager.CanSpawnFish = true;
        fishManager.CanSpawnBobbers = true;

        currentTime = 0;

        GameStart = true;

        gameoverText.gameObject.SetActive(false);
        startText.SetActive(false);
        timerText.gameObject.SetActive(true);

        timer = roundLimit;
        UpdateTimer();
    }

    public void playCircleParticle()
    {
        ps.Emit(1);
    }

    public void GameOver()
    {
        popupText.gameObject.SetActive(false);
        fishManager.CanSpawnFish = false;
        fishManager.CanSpawnBobbers = false;
        GameStart = false;

        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            SaveGame();
        }

        timerText.gameObject.SetActive(false);

        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        var timer = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        gameoverText.text = "Game Over \n" +
                            "Your Time: " + timer + "\n" +
                            "Click to play again!";

        gameoverText.gameObject.SetActive(true);

    }

    public void UpdateTimer()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        TimeSpan bestTimeSpan = TimeSpan.FromSeconds(bestTime);
        bestTimeText.text = "Best \n" + string.Format("{0:D2}:{1:D2}", bestTimeSpan.Minutes, bestTimeSpan.Seconds);
    }

    public void AddTimer(string color)
    {
        var newTime = 0f;

        switch (color)
        {
            case "Blue":
                newTime += blueTime;
                break;
            case "Green":
                newTime += greenTime;
                break;
            case "Orange":
                newTime += orangeTime;
                break;
            case "Purple":
                newTime += purpleTime;
                break;
            case "Red":
                newTime += RedTime;
                break;

        }

        timer += newTime;
        
        UpdateTimer();
    }

    public void ShowText(Vector3 position, string color)
    {
        var newTime = 0f;

        switch (color)
        {
            case "Blue":
                newTime += blueTime;
                break;
            case "Green":
                newTime += greenTime;
                break;
            case "Orange":
                newTime += orangeTime;
                break;
            case "Purple":
                newTime += purpleTime;
                break;
            case "Red":
                newTime += RedTime;
                break;

        }

        popupText.transform.position = position;
        TimeSpan ts = TimeSpan.FromSeconds(newTime);
        popupText.text = string.Format("+ {0:D1}secs",ts.Seconds);
        popupText.gameObject.SetActive(true);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("BestTime", bestTime);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", 0);
    }
}
