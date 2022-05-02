using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    public GameState gameState;

    public bool isReady = false;

    public static TitleManager TM;

    private void Awake()
    {
        if (TM)
            Destroy(this);
        else
            TM = this;
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}
