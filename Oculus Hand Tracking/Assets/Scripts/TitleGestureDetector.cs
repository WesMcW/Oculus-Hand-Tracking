using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TitleGestureDetector : MonoBehaviour
{
    public static TitleGestureDetector tgd;

    public float threshold = 0.1f;

    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool waitForBones = true;

    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    public Gesture currentGesture;

    public bool usedRock = false;
    public bool usedPaper = false;
    public bool usedScissors = false;

    private void Awake()
    {
        if (tgd == null)
            tgd = this;
        else
            Destroy(this);
    }

    void Start()
    {
        StartCoroutine("SetBones");
        previousGesture = new Gesture();
        //waitForBones = true;
    }

    public IEnumerator SetBones()
    {
        while (!skeleton.IsInitialized)
            yield return null;
        fingerBones = new List<OVRBone>(skeleton.Bones);
        waitForBones = false;
        //Debug.LogError(fingerBones.Count);
    }

    void Update()
    {
        if (!waitForBones)
        {
            currentGesture = Recognize();

            bool hasRecognized = !currentGesture.Equals(new Gesture());

            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                if (currentGesture.name == "Ready" && TitleManager.TM.isReady)
                    TitleManager.TM.GameStart();
            }

            if (!TitleManager.TM.isReady && hasRecognized)
            {
                if (currentGesture.name == gestures[0].name && !usedRock)
                {
                    usedRock = true;
                    currentGesture.onRecognized.Invoke();
                }

                else if (currentGesture.name == gestures[1].name && !usedPaper)
                {
                    usedPaper = true;
                    currentGesture.onRecognized.Invoke();
                }

                else if (currentGesture.name == gestures[2].name && !usedScissors)
                {
                    usedScissors = true;
                    currentGesture.onRecognized.Invoke();
                }
            }

            if (usedRock && usedPaper && usedScissors)
                TitleManager.TM.isReady = true;

            previousGesture = currentGesture;
        }
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var g in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, g.fingerData[i]);

                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = g;
            }
        }
        //debugText.text = currentGesture.name;
        return currentGesture;
    }
}
