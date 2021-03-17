using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurn : MonoBehaviour {

    InitiativeTracker InitTracker;

	// Use this for initialization
	void Start () {
        InitTracker = GameObject.FindObjectOfType<InitiativeTracker>();
	}

    public void NextTurn()
    {
        InitTracker.NextTurn();
    }

    public void ResetAll()
    {
        foreach(var charBrain in GameObject.FindObjectsOfType<CharacterBrain>())
        {
            charBrain.ResetTurn();
        }
    }
}
