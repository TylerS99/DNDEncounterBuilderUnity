using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabNext : MonoBehaviour {
    int i = 0;
    int numFields = 5;
    EventSystem system;
    List<InputField> inputs;

    private void OnEnable()
    {
        i = 0;
    }

    void Start()
    {
        system = FindObjectOfType<EventSystem>();
        inputs = new List<InputField>();
        for(int j = 1; j <= numFields+1; j++)
        {
            inputs.Add(transform.GetChild(j).GetChild(0).GetComponent<InputField>());
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputs.Contains(system.currentSelectedGameObject.GetComponent<InputField>())) {
                i = inputs.IndexOf(system.currentSelectedGameObject.GetComponent<InputField>()) + 1;
            }
            i++;
            if (i > numFields + 1)
            {
                i = 1;
            }
            InputField inputfield = transform.GetChild(i).GetChild(0).GetComponent<InputField>();
            if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
            system.SetSelectedGameObject(inputfield.gameObject, new BaseEventData(system));           
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            transform.parent.GetComponent<CreateCharacter>().Create();
        }
	}
}
