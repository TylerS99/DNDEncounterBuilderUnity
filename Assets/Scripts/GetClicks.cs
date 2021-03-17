using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClicks : MonoBehaviour
{
    GameObject hitObject;
    GameObject selectedObject;
    public bool clickEnabled = true;
    public bool somethingSelected = false;
    SwitchView viewCheck;

    // Use this for initialization
    void Start()
    {
        hitObject = null;
        viewCheck = GameObject.FindObjectOfType<SwitchView>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Interactable")
            {
                if(hitObject != hit.transform.gameObject && hitObject != null)
                {
                    hitObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                hitObject = hit.transform.gameObject;
                hitObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if(hitObject != null && !hitObject.GetComponent<CharacterBrain>().Selected)
                {
                    hitObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                hitObject = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            somethingSelected = false;
            foreach (var brain in GameObject.FindObjectsOfType<CharacterBrain>())
            {
                if (brain.Selected)
                {
                    if(brain.gameObject != hitObject)
                        somethingSelected = true;
                    break;
                }
            }
            if (clickEnabled && !somethingSelected) {
                if (hitObject != null)
                {
                    hitObject.GetComponent<CharacterBrain>().Run();

                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (viewCheck.Top)
            {
                Camera.main.orthographicSize -= 0.5f;
                if (Camera.main.orthographicSize < 1)
                    Camera.main.orthographicSize = 1;
            }
            else
            {
                Camera.main.transform.position += Camera.main.transform.forward;
                if(Camera.main.transform.position.y < 1)
                {
                    Camera.main.transform.position -= Camera.main.transform.forward;
                }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (viewCheck.Top)
                Camera.main.orthographicSize += 0.5f;
            else
                Camera.main.transform.position -= Camera.main.transform.forward;
        }
    }

    //Enable or disable clicking when using UI
    public void DisableClick(bool change)
    {
        clickEnabled = !change;
    }
}
