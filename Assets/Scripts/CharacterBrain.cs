using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBrain : MonoBehaviour {
    GameObject MoveIndicator; //indicator to show movement
    GameObject EnterDamage; //Gameobject of damage UI
    Transform moveRange; //Transform of move range indicator
    int mode = 0; //Sets mode of character (0) unselected (1) selected (2) dead TODO: Change to IEnumerable
    int oldSpeed = 0; //Detects if speed has been changed
    CharacterSheet charSheet; //Corresponding character sheet
    public bool Selected = false; //Determine if character is currently selected
    public float moveLeft = 0; //Amount of speed left
    public float hpLeft; //Amount of HP left
    SwitchView viewCheck; //Used to check if in top view or iso
    GameObject hitObject; //Character being targeted
    InitiativeTracker InitTracker; //Tracks initiative

    //Colors for players
    Color UnselectedC = Color.gray;
    Color DeadC = Color.black;
    Color SelectedC = Color.yellow;
    Color TargetedC = Color.red;

    // Use this for initialization
    void Start()
    {
        InitTracker = GameObject.FindObjectOfType<InitiativeTracker>();
        MoveIndicator = GameObject.Find("MoveIndicator");
        EnterDamage = GameObject.FindObjectOfType<SceneOptions>().EnterDamage;
        viewCheck = GameObject.FindObjectOfType<SwitchView>();
        charSheet = GetComponent<CharacterSheet>();
        moveRange = transform.GetChild(0);
        hpLeft = 0;
        ChangeHP(charSheet.HP);
        UpdateCharSheet();
        UpdateText();
        ResetTurn();
        SetMode(0);
    }


    //Updates model stats with new speed, name, and HP
    public void UpdateCharSheet()
    {
        moveLeft += charSheet.Speed - oldSpeed;
        if (moveLeft < 0)
            moveLeft = 0;
        oldSpeed = charSheet.Speed;
        moveRange.localScale = new Vector3(moveLeft, 0.01f, moveLeft);
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshPro>().text = charSheet.Name;
        ChangeHP(0);        
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 1)
        {
            Mode1();
        }
    }

    public void Run()
    {
            SetMode(1);
            Selected = true;
    }

    //Reset character for start of turn
    public void ResetTurn()
    {
        //StopAllCoroutines();
        moveLeft = charSheet.Speed;
        if (moveLeft < 0)
            moveLeft = 0;
        moveRange.localScale = new Vector3(moveLeft, 0.01f, moveLeft);
    }

    //Change between active and inactive
    public void SetMode(int num)
    {
        mode = num;
        if(mode == 1)
        {
            moveRange.gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<Renderer>().material.color = SelectedC;
            foreach(CharacterBrain brain in GameObject.FindObjectsOfType<CharacterBrain>())
            {
                if(brain.gameObject != gameObject)
                {
                    brain.Deselect();
                }
            }
            MoveIndicator.transform.GetChild(0).gameObject.SetActive(true);
            MoveIndicator.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(mode == 0)
        {
            //StopAllCoroutines();
            moveRange.gameObject.SetActive(false);
            transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = UnselectedC;
            if (Selected)
            {
                MoveIndicator.transform.GetChild(0).gameObject.SetActive(false);
                MoveIndicator.transform.GetChild(1).gameObject.SetActive(false);
            }
            if (hpLeft == 0)
                SetMode(2);
        }
        else if(mode == 2)
        {
            moveRange.gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<Renderer>().material.color = DeadC;
        }
    }

    //Actions in mode1
    void Mode1()
    {
        if(moveLeft < 0)
        {
            moveLeft = 0;
        }
        Vector3 point = Vector3.zero;
        moveRange.localScale = new Vector3(moveLeft, 0.01f, moveLeft); 
        RaycastHit hit;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (hit.transform.tag == "Interactable" && hit.transform.gameObject != gameObject && (dist - 2.5f <= moveLeft/2 || (Input.GetKey(KeyCode.LeftShift) && dist <= (charSheet.Range/2))))
                {
                    if(hitObject != hit.transform.gameObject && hitObject != null && hitObject.GetComponent<CharacterBrain>().mode != 2)
                    {
                        hitObject.transform.GetChild(1).GetComponent<Renderer>().material.color = UnselectedC;
                    }
                    hitObject = hit.transform.gameObject;
                    if(hitObject.GetComponent<CharacterBrain>().mode != 2)
                    {
                        hitObject.transform.GetChild(1).GetComponent<Renderer>().material.color = TargetedC;
                    }                    
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MoveIndicator.transform.position = transform.position;
                    }
                    else
                    {
                        point = hit.transform.position;
                        transform.LookAt(point);
                        point = transform.position + transform.forward * (dist-2.5f);
                        UpdateIndicatorText(dist - 2.5f);
                        MoveIndicator.transform.position = point;
                    }
                }
                else
                {
                    point = transform.position;
                    if (hitObject != null && hitObject.GetComponent<CharacterBrain>().mode != 2)
                    {
                        hitObject.transform.GetChild(1).GetComponent<Renderer>().material.color = UnselectedC;
                    }
                    hitObject = null;
                }  
                if (hit.transform.tag == "Ground")
                {
                    point = hit.point;
                    dist = Vector3.Distance(transform.position, point);                 
                    transform.LookAt(point);
                    UpdateText();
                    if (dist > moveLeft / 2f && !Input.GetKey(KeyCode.LeftShift))
                    {
                        point = transform.position + transform.forward * (moveLeft / 2);
                        dist = moveLeft / 2;
                    }
                    UpdateIndicatorText(dist);                      
                    MoveIndicator.transform.position = point;
                }
                           
            }
        }
        if (Input.GetMouseButtonDown(0) && GameObject.FindObjectOfType<GetClicks>().clickEnabled)
        {
            ClickedSomething(hit.transform.gameObject, point);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (hitObject != null)
            {
                hitObject.transform.GetChild(1).GetComponent<Renderer>().material.color = UnselectedC;
            }
            Deselect();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Deselect();
            CreateCharacter create = GameObject.FindObjectOfType<CreateCharacter>();
            create.Edit(GetComponent<CharacterSheet>());
            create.Remove();
        }       
    }

    void UpdateIndicatorText(float dist)
    {
        MoveIndicator.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshPro>().text = (Mathf.RoundToInt(dist * 2)).ToString();
        if (viewCheck.Top)
        {
            MoveIndicator.transform.GetChild(1).rotation = Quaternion.Euler(-90, 180, 0);
        }
        else
        {
            MoveIndicator.transform.GetChild(1).LookAt(Camera.main.transform.position);
        }
    }

    //Player clicked hit at point point
    void ClickedSomething(GameObject hit,Vector3 point)
    {
        
        if (hit.transform.tag == "Ground")
        {
            MoveToPos(point);
        }
        if (hit.transform.tag == "Interactable")
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Attack(hit);
            }
            else
            {
                MoveToPos(point);
                if(hit != gameObject && Vector3.Distance(point,hit.transform.position) <= 2.6f)
                {
                    Attack(hit);
                    Deselect();
                }
            }
        }
    }

    //Brings up damage dialogue
    void Attack(GameObject objectToHit)
    {
        EnterDamage.GetComponent<ConfirmDamage>().objectToHit = objectToHit.GetComponent<CharacterBrain>();
        EnterDamage.GetComponent<ConfirmDamage>().Attacker = this;
        EnterDamage.SetActive(true);
    }
    
    //Move character to given point, or closest point in range
    public void MoveToPos(Vector3 point)
    {
        StopAllCoroutines();
        float dist = Vector3.Distance(transform.position, point);
        StartCoroutine(SmoothLerp(dist/10f, transform.position,point, Input.GetKey(KeyCode.LeftShift)));
    }

    public void UpdateText()
    {
        if (!viewCheck.Top)
        {
            transform.GetChild(2).LookAt(Camera.main.transform.position); //+ Camera.main.transform.forward * 1000);
            transform.GetChild(2).localPosition = new Vector3(0, transform.GetChild(2).localPosition.y, 0);
        }
        else
        {
            transform.GetChild(2).rotation = Quaternion.Euler(-90, 180, 0);
            transform.GetChild(2).localPosition = new Vector3(0, transform.GetChild(2).localPosition.y, -1);
        }
    }

    //Change hpLeft by value
    public void ChangeHP(int value)
    {
        hpLeft += value;
        
        if(hpLeft <= 0)
        {
            hpLeft = 0;
            InitTracker.ListRemove(GetComponent<CharacterSheet>());
            Deselect();
        }
        else if(mode == 2)
        {
            InitTracker.ListAdd(GetComponent<CharacterSheet>());
            SetMode(0);
        }
        if(hpLeft > charSheet.HP)
        {
            hpLeft = charSheet.HP;
        }
        transform.GetChild(2).GetChild(0).GetChild(0).localScale = new Vector3(hpLeft / charSheet.HP, 1, 1);
    }

    public void Deselect()
    {
        SetMode(0);
        Selected = false;
    }

    //Smoothly move character via coroutine
    private IEnumerator SmoothLerp(float time, Vector3 startingPos, Vector3 finalPos,bool IgnoreMoveLeft)
    {
        float dist = Vector3.Distance(startingPos, finalPos);
        float elapsedTime = 0;
        float originalMoveLeft = moveLeft;
        while (elapsedTime < time)
        {
            if(!IgnoreMoveLeft)
                moveLeft = originalMoveLeft - dist * 2 * (elapsedTime / time);
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
