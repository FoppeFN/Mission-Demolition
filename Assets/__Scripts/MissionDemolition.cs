using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public TMP_Text uitLevel;
    public TMP_Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject gameOverPanel;
    public GameObject levelFailedPanel;
    public int shotLimit = 5;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (levelFailedPanel != null) levelFailedPanel.SetActive(false);

        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null) Destroy(castle);

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        if (uitLevel == null)
        {
            Debug.LogError("uitLevel is NULL");
            return;
        }

        if (uitShots == null)
        {
            Debug.LogError("uitShots is NULL");
            return;
        }

        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken + " / " + shotLimit;
    }

    void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }

        if ((mode == GameMode.playing) && shotsTaken >= shotLimit && !Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            Invoke("ShowLevelFailed", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            ShowGameOver();
            return;
        }
        StartLevel();
    }

    void ShowGameOver()
    {
        mode = GameMode.idle;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    void ShowLevelFailed()
    {
        mode = GameMode.idle;
        if (levelFailedPanel != null) levelFailedPanel.SetActive(true);
    }

    public void RetryLevel()
    {
        shotsTaken = 0;
        if (levelFailedPanel != null) levelFailedPanel.SetActive(false);
        StartLevel();
    }

    public void PlayAgain()
    {
        level = 0;
        shotsTaken = 0;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        StartLevel();
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
