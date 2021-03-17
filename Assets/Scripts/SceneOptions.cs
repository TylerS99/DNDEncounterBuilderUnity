using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOptions : MonoBehaviour {
    [SerializeField] public GameObject EnterDamage;

    public void LoadFirst()
    {
        SceneManager.LoadScene(0);
    }
}
