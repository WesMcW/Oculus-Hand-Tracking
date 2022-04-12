using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentLogic : MonoBehaviour
{
    public Animator anim;
    public List<GameObject> objects;
    public Transform objectSpawn;
    public GameObject gestureBlocker;

    public GameObject chosenObj;
    public int pickedGesture;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PickMove();
        }
    }

    public void PickMove()
    {
        anim.SetTrigger("Roll");
        int moveNum = Random.Range(0,3);

        switch (moveNum)
        {
            case 0:
                anim.SetTrigger("Rock");
                break;
            case 1:
                anim.SetTrigger("Paper");
                break;
            case 2:
                anim.SetTrigger("Scissors");
                break;
        }
        pickedGesture = moveNum;
        //Debug.LogError(moveNum);
    }

    public void SpawnObject(int num)
    {
        chosenObj = Instantiate(objects[num], objectSpawn);
    }

    public void HideGesture()
    {
        gestureBlocker.SetActive(true);
    }

    public void ShowGesture()
    {
        gestureBlocker.SetActive(false);
        SpawnObject(pickedGesture);
    }
}
