using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    MapData Selected;
    [SerializeField] Material Ground;
    Texture2D groundTex;
    static SceneController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += LoadScene;
        }
        else if (instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene()
    {
        Selected = GameObject.FindObjectOfType<InfiniteScroll>().Selected;
        SceneManager.LoadScene(1);
        groundTex = Resources.Load<Texture2D>("Maps/" + Selected.Name) as Texture2D;
        Ground.SetTexture("_MainTex", groundTex);

    }

    private void LoadScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            if (groundTex.width > groundTex.height)
            {
                GameObject.FindGameObjectWithTag("Ground").transform.localScale = new Vector3(5 * groundTex.width / groundTex.height, 1, 5);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Ground").transform.localScale = new Vector3(5, 1, 5 * groundTex.height / groundTex.width);
            }
            GameObject Scenery = GameObject.Find(Selected.Name);
            if(Scenery == null)
            {
                Scenery = GameObject.Find("Default");
            }
            Scenery.transform.GetChild(0).gameObject.SetActive(true);
            Scenery.transform.GetChild(1).gameObject.SetActive(true);
        }      
    }
}
