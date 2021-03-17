using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalAppManager : MonoBehaviour {
    public void Run()
    {
        GameObject.FindObjectOfType<SceneController>().LoadNextScene();
    }
}
