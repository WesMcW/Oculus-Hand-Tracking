using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerData;
    public UnityEvent onRecognized;
}

public class GestureDetection : MonoBehaviour
{
    public float threshold = 0.1f;

    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool waitForBones = true;

    public GameObject rock, paper, scissors;
    public GameObject objSpawnLoc;

    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    void Start()
    {
        //fingerBones = new List<OVRBone>(skeleton.Bones);
        //Debug.LogError(fingerBones.Count);
        StartCoroutine("SetBones");
        previousGesture = new Gesture();
        waitForBones = true;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveGesture();
        }

        if (!waitForBones)
        {
            Gesture currentGesture = Recognize();

            bool hasRecognized = !currentGesture.Equals(new Gesture());

            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                Debug.LogWarning("Gesture Found: " + currentGesture.name);

                if (currentGesture.Equals(gestures[0]))
                    Instantiate(rock, objSpawnLoc.transform);

                else if (currentGesture.Equals(gestures[1]))
                    Instantiate(paper, objSpawnLoc.transform);

                else if (currentGesture.Equals(gestures[2]))
                    Instantiate(scissors, objSpawnLoc.transform);

                previousGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
        }
    }

    void SaveGesture()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        foreach (var bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerData = data;
        gestures.Add(g);
        //waitForBones = false;
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var g in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for(int i = 0; i < fingerBones.Count; i++)
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
            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = g;
            }
        }
        return currentGesture;
    }
}