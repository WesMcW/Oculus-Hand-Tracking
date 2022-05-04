using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject Rock, Paper, Scissors, Ready;

    public bool rockComplete, paperComplete, scissorComplete;

    private void Update()
    {
        if (rockComplete)
            Rock.SetActive(false);
        if (paperComplete)
            Paper.SetActive(false);
        if (scissorComplete)
            Scissors.SetActive(false);
        if (rockComplete && paperComplete && scissorComplete)
            Ready.SetActive(true);
    }
}
