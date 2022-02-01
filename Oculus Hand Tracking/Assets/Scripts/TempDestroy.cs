using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDestroy : MonoBehaviour
{
    public float timeTilDestroy = 2f;

    void Start()
    {
        StartCoroutine("DestroyMe", timeTilDestroy);
    }

    IEnumerator DestroyMe(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }

}
