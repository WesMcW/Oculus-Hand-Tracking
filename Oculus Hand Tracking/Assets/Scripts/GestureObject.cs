using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureObject : MonoBehaviour
{
    public bool lost;
    public GameObject unbroken;
    public GameObject broken;

    public void BreakObj()
    {
        broken.SetActive(true);
        unbroken.SetActive(false);
    }
}
