using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerData;
    public UnityEvent onRecognized;
}

public class GestureDetection : MonoBehaviour
{
    public static GestureDetection gd;

    public float threshold = 0.1f;

    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool waitForBones = true;

    public TextMeshPro debugText;

    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    public Gesture currentGesture;

    private void Awake()
    {
        if (gd == null)
            gd = this;
        else
            Destroy(this);
    }

    void Start()
    {
        //fingerBones = new List<OVRBone>(skeleton.Bones);
        //Debug.LogError(fingerBones.Count);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveGesture();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Load();
        }

        if (!waitForBones)
        {
            currentGesture = Recognize();

            bool hasRecognized = !currentGesture.Equals(new Gesture());

            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                if (currentGesture.name == "Ready" && !GameManager.GM.started && GameManager.GM.gameState != GameState.Finished)
                    GameManager.GM.RoundStart();

                if (currentGesture.name == "Ready" && GameManager.GM.gameState == GameState.Finished)
                    SceneManager.LoadScene(0);
            }

            if (GameManager.GM.gameState == GameState.Active && hasRecognized)
            {
                if (currentGesture.name == gestures[0].name || currentGesture.name == gestures[1].name || currentGesture.name == gestures[2].name) {

                    currentGesture.onRecognized.Invoke();
                    GameManager.GM.Reavel();
                    GameManager.GM.CheckOutcome(currentGesture.name);
                    GameManager.GM.gameState = GameState.Complete;
                    GameManager.GM.started = false;
                    if(GameManager.GM.p1Win || GameManager.GM.p2Win)
                        GameManager.GM.gameState = GameState.Finished;
                }
            }
            previousGesture = currentGesture;
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
        //debugText.text = currentGesture.name;
        return currentGesture;
    }

    private void Save()
    {
        List<Gesture> g = gestures;

        SaveObject saveObject = new SaveObject
        {
            savedGestures = g
        };

        string json = JsonUtility.ToJson(saveObject);

        SaveSystem.Save(json);
        Debug.LogWarning("Data saved.");
    }

    private void Load()
    {
        string saveString = SaveSystem.Load();

        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            gestures = saveObject.savedGestures;
            Debug.LogWarning("Data loaded.");

        } else {
            Debug.LogError("No Save!");
        }
    }

    private class SaveObject
    {
        public List<Gesture> savedGestures;
    }
}
