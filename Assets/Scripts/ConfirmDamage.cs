using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDamage : MonoBehaviour {
    public CharacterBrain objectToHit;
    public CharacterBrain Attacker;
    public InputField input;
    private void Start()
    {
        input = transform.GetChild(0).GetComponent<InputField>();
    }

    public void ChangeDamage(int num)
    {
        input.text = (int.Parse(input.text) + num).ToString();
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (int.Parse(input.text) > 0)
        {
            input.image.color = Color.green;
        }
        else if(int.Parse(input.text) < 0)
        {
            input.image.color = Color.red;
        }
        else
        {
            input.image.color = Color.white;
        }
    }

    public void EnterDamage()
    {
        int value = int.Parse(input.text);
        objectToHit.ChangeHP(value);
        objectToHit.GetComponent<CharacterBrain>().Deselect();
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<GetClicks>().DisableClick(false);
        //Attacker.Deselect();
    }
}
