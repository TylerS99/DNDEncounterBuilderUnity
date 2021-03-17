using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEdit : MonoBehaviour {
    [SerializeField] GameObject CreateCharacterObj;
    public void FindSelected()
    {
        GameObject selected = null;
        foreach(var brain in GameObject.FindObjectsOfType<CharacterBrain>())
        {
            if (brain.Selected)
            {
                selected = brain.gameObject;
                break;
            }
        }
        if(selected == null)
        {
            AddCharacter();
        }
        else
        {
            EditCharacter(selected);
        }
        CreateCharacterObj.SetActive(true);
    }

    void AddCharacter()
    {
        CreateCharacterObj.transform.parent.GetComponent<CreateCharacter>().Edit(null);
    }


    void EditCharacter(GameObject selected)
    {
        CreateCharacterObj.transform.parent.GetComponent<CreateCharacter>().Edit(selected.GetComponent<CharacterSheet>());
    }
}
