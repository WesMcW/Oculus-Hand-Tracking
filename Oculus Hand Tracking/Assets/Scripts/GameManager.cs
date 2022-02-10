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

    public static GameManager GM;

    private void Awake()
    {
        if (GM)
            Destroy(this);
        else
            GM = this;
    }

    public void RoundStart()
    {
        StartCoroutine("SetRoundState", roundTimer);
    }

    public IEnumerator SetRoundState(float time)
    {
        gameState = GameState.Ready;

        yield return new WaitForSeconds(time);
        gameState = GameState.Active;
        StopCoroutine("RoundStart");
    }
}