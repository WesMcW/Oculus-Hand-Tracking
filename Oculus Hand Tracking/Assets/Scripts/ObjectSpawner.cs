using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject rock, paper, scissors;
    public Transform spawnLocation;

    public void SpawnRock()
    {
        Instantiate(rock, spawnLocation);
    }

    public void SpawnPaper()
    {
        Instantiate(paper, spawnLocation);
    }

    public void SpawnScissors()
    {
        Instantiate(scissors, spawnLocation);
    }
}
