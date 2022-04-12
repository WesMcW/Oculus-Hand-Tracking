using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject rock, paper, scissors;
    public Transform spawnLocation;

    public GameObject chosenObj;

    public void SpawnRock()
    {
        chosenObj = Instantiate(rock, spawnLocation);
    }

    public void SpawnPaper()
    {
        chosenObj = Instantiate(paper, spawnLocation);
    }

    public void SpawnScissors()
    {
        chosenObj = Instantiate(scissors, spawnLocation);
    }
}
