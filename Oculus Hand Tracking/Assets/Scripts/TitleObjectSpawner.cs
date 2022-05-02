using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleObjectSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject rock, paper, scissors;

    public Transform rSpawnLocation;
    public Transform pSpawnLocation;
    public Transform sSpawnLocation;

    public void SpawnRock()
    {
        Instantiate(rock, rSpawnLocation);
    }

    public void SpawnPaper()
    {
        Instantiate(paper, pSpawnLocation);
    }

    public void SpawnScissors()
    {
        Instantiate(scissors, sSpawnLocation);
    }
}
