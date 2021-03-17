using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickClose : MonoBehaviour {
    private void Update()
    {
        ConfirmDamage cd;
        if (Input.GetMouseButton(1))
        {
            FindObjectOfType<GetClicks>().DisableClick(false);
            gameObject.SetActive(false);
            if ((cd = GetComponent<ConfirmDamage>()) != null)
            {
                cd.objectToHit.GetComponent<CharacterBrain>().SetMode(0);
            }
        }       
    }
}
