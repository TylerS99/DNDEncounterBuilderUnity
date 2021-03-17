using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour {
    [SerializeField] GameObject Panel;
    public void Toggle()
    {
        Panel.SetActive(!Panel.activeSelf);
    }
}
