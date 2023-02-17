using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAlt : MonoBehaviour
{
    //Public 
    public UI_ManagerAlt uiManager;
    public ProgressionManager progressionManager;
    public UI_SCTManager sctManager;
    public int maxScore;
    public Transform spawnTransform;

    //Protected
    protected int score;
    protected PlayerController player;

    public void Awake()
    {
        if (uiManager == null)
        {
            uiManager = GetComponent<UI_ManagerAlt>();
        }
        if (sctManager == null)
        {
            sctManager = GetComponent<UI_SCTManager>();
        }

        PlayerController pC = FindObjectOfType<PlayerController>();

        if (pC != null)
        {
            player = pC;

            if (spawnTransform == null)
            {
                spawnTransform.position = player.transform.position;
                spawnTransform.rotation = player.transform.rotation;
            }
        }
    }

    public void GenerateSCT(string scrollingText, Transform trans)
    {
        sctManager.CreateSCT(scrollingText, trans);
    }

    public void RespawnPlayer()
    {
        if (spawnTransform != null)
        {
            player.transform.position = spawnTransform.position;
            player.transform.rotation = spawnTransform.rotation;
        }
    }

    public PlayerController GetPlayer()
    {
        return player;
    }
    public ProgressionManager GetProgression()
    {
        return progressionManager;
    }

    public void IncreaseScore(int amount)
    {
        int newScore = score + amount;

        if (newScore <= maxScore)
        {
            score = newScore;

            //Score UI
            //uIManager.UpdateScoreText(score);
        }
    }
}
