using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
    Ready,
    Active,
    Complete
};

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameState gameState;
    public float roundTimer;
    public bool started = false;
    public OpponentLogic opponent;
    public int p1Score = 0;
    public int p2Score = 0;

    public static GameManager GM;

    private void Awake()
    {
        if (GM)
            Destroy(this);
        else
            GM = this;

        SaveSystem.Initialize();
    }

    private void Update()
    {
        /*
        if (gameState == GameState.Complete)
        {
            opponent.ShowGesture();
            gameState = GameState.Ready;
        }
        */
    }

    public void RoundStart()
    {
        started = true;
        StartCoroutine("SetRoundState", roundTimer);
    }

    public void Reavel()
    {
        opponent.ShowGesture();
        gameState = GameState.Ready;
    }

    public IEnumerator SetRoundState(float time)
    {
        gameState = GameState.Ready;
        opponent.PickMove();

        yield return new WaitForSeconds(time);
        gameState = GameState.Active;
        GestureDetection.gd.currentGesture = new Gesture();

        StopCoroutine("RoundStart");
    }

    public void CheckOutcome(string playerGesture)
    {
        int playerChoice = -1;
        int opponentChoice = opponent.pickedGesture;

        switch (playerGesture)
        {
            case "Rock":
                playerChoice = 0;
                break;
            case "Paper":
                playerChoice = 1;
                break;
            case "Scissors":
                playerChoice = 2;
                break;
        }

        // Player chooses rock
        if (playerChoice == 0 && opponentChoice == 0)
            Debug.LogError("DRAW");
        else if (playerChoice == 0 && opponentChoice == 1)
            p2Score++;
        else if (playerChoice == 0 && opponentChoice == 2)
            p1Score++;

        // Player chooses paper
        if (playerChoice == 1 && opponentChoice == 1)
            Debug.LogError("DRAW");
        else if (playerChoice == 1 && opponentChoice == 2)
            p2Score++;
        else if (playerChoice == 1 && opponentChoice == 0)
            p1Score++;

        // Player chooses scissors
        if (playerChoice == 2 && opponentChoice == 2)
            Debug.LogError("DRAW");
        else if (playerChoice == 2 && opponentChoice == 0)
            p2Score++;
        else if (playerChoice == 2 && opponentChoice == 1)
            p1Score++;

        Debug.LogError("P1: " + p1Score + "P2: " + p2Score);
    }

}
