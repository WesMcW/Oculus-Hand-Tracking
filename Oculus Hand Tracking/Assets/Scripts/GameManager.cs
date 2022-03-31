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

    public void CheckOutcome()
    {
        //opponent.pickedGesture;
    }

}
