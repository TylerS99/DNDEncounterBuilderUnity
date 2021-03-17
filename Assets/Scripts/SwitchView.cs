using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchView : MonoBehaviour {
    [SerializeField] GameObject TopDownTex;
    [SerializeField] GameObject TopDownCam;
    [SerializeField] GameObject IsoCam;
    [SerializeField] GameObject IsoTex;
    public bool Top = false;

    private void Start()
    {
        RotateText();
    }

    //Change between camera views
    public void FullScreen()
    {
        Top = !Top;
        TopDownTex.GetComponent<Camera>().orthographicSize = TopDownCam.GetComponent<Camera>().orthographicSize;
        IsoTex.transform.position = IsoCam.transform.position;
        TopDownCam.SetActive(Top);
        TopDownTex.SetActive(!Top);
        IsoCam.SetActive(!Top);
        IsoTex.SetActive(Top);
        RotateText();
    }

    //Rotate all text to face camera
    public void RotateText()
    {
        foreach (var Player in GameObject.FindObjectsOfType<CharacterBrain>())
        {
            Player.UpdateText();
        }
    }
}
