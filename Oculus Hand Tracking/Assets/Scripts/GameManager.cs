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
    public ObjectSpawner playerObj;
    public int p1Score = 0;
    public int p2Score = 0;
    public TextMeshPro p1Text;
    public TextMeshPro p2Text;

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
        //p1Text.text = p1Score.ToString();
        //p2Text.text = p2Score.ToString();
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
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            //Debug.LogWarning("DRAW");
        }
        else if (playerChoice == 0 && opponentChoice == 1)
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            p2Score++;
            p2Text.text = p2Score.ToString();
        }
        else if (playerChoice == 0 && opponentChoice == 2)
        {
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            p1Score++;
            p1Text.text = p1Score.ToString();
        }

        // Player chooses paper
        if (playerChoice == 1 && opponentChoice == 1)
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            //Debug.LogWarning("DRAW");
        }
        else if (playerChoice == 1 && opponentChoice == 2)
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            p2Score++;
            p2Text.text = p2Score.ToString();
        }
        else if (playerChoice == 1 && opponentChoice == 0)
        {
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            p1Score++;
            p1Text.text = p1Score.ToString();
        }

        // Player chooses scissors
        if (playerChoice == 2 && opponentChoice == 2)
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            //Debug.LogWarning("DRAW");
        }
        else if (playerChoice == 2 && opponentChoice == 0)
        {
            playerObj.chosenObj.GetComponent<GestureObject>().BreakObj();
            p2Score++;
            p2Text.text = p2Score.ToString();
        }
        else if (playerChoice == 2 && opponentChoice == 1)
        {
            opponent.chosenObj.GetComponent<GestureObject>().BreakObj();
            p1Score++;
            p1Text.text = p1Score.ToString();
        }

        //Debug.LogWarning("P1: " + p1Score + "P2: " + p2Score);
    }
}
