using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour {
    [SerializeField] GameObject PlayerPrefab; //Prefab for new character
    [SerializeField] GameObject Indicator; //Indicator for character placement
    InitiativeTracker InitTracker;    //Initiative tracker being used
    CharacterSheet CharToEdit = null; //Character being edited (if null, create from scratch)
    //Input field values
    string Model;
    string Name;
    int Speed;
    int HP;
    int Range;
    int Initiative;
    

    private void Start()
    {
        InitTracker = GameObject.FindObjectOfType<InitiativeTracker>();
    }

    //Identifies character being edited, and sets input fields
    public void Edit(CharacterSheet Selected)
    {
        CharToEdit = Selected;
        if (Selected == null)
            return;
        SetInput(1, Selected.Model);
        SetInput(2, Selected.Name);
        SetInput(3, Selected.HP.ToString());
        SetInput(4, Selected.Speed.ToString());
        SetInput(5, Selected.Range.ToString());
        SetInput(6, Selected.Initiative.ToString());
    }

    //Creates character based on input when user clicks add button
    public void Create()
    {       
        Model = ParseInput(1);
        if(Resources.Load<Mesh>("Models/" + Model) as Mesh == null)
        {
            SetInput(1, "Cleric");
            return;
        }
        Name = ParseInput(2);
        HP = ParseNumber(3);
        Speed = ParseNumber(4);
        Range = ParseNumber(5);
        Initiative = ParseNumber(6);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GameObject.FindObjectOfType<GetClicks>().DisableClick(false);
        if (CharToEdit == null) //Creatimg character from scratch
        {
            Indicator.SetActive(true);
            StartCoroutine(SelectLocation());
        }
        else //editing character
        {
            CharToEdit.Model = Model;
            CharToEdit.Name = Name;
            CharToEdit.HP = HP;
            CharToEdit.Speed = Speed;
            CharToEdit.Range = Range;
            CharToEdit.Initiative = Initiative;
            InitTracker.ListUpdate();
            CharToEdit.gameObject.GetComponent<CharacterBrain>().UpdateCharSheet();
            Mesh m = Resources.Load<Mesh>("Models/"+Model) as Mesh;
            CharToEdit.transform.GetChild(1).GetComponent<MeshFilter>().mesh = m;
            CharToEdit.transform.GetChild(2).localPosition = new Vector3(0, m.bounds.max.y + 0.3f, 0);
        }
    }

    //Get string value at index i
    string ParseInput(int i)
    {
        return transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<InputField>().text;
    }

    //Get int value at index i
    int ParseNumber(int i)
    {
        return int.Parse(transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<InputField>().text);
    }

    //Set input field at index i
    void SetInput(int i, string value)
    {
        transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<InputField>().text = value;
    }

    //Removes character from board
    public void Remove()
    {
        if (CharToEdit != null)
        {
            CharToEdit.GetComponent<CharacterBrain>().Deselect();
            InitTracker.ListRemove(CharToEdit);
            Destroy(CharToEdit.gameObject);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //Coroutine to select character position
    IEnumerator SelectLocation()
    {
        while (true)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.gameObject.tag == "Ground")
                {
                    Indicator.transform.position = hit.point;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.tag == "Ground")
                {
                    GameObject newPlayer = Instantiate(PlayerPrefab, hit.point, Quaternion.Euler(0,180,0));
                    Mesh m = Resources.Load<Mesh>("Models/"+Model) as Mesh;
                    newPlayer.transform.GetChild(1).GetComponent<MeshFilter>().mesh = m;
                    newPlayer.transform.GetChild(2).localPosition = new Vector3(0,m.bounds.max.y+0.3f,0);
                    CharacterSheet charSheet = newPlayer.GetComponent<CharacterSheet>();
                    charSheet.Model = Model;
                    charSheet.Name = Name;
                    charSheet.Speed = Speed;
                    charSheet.HP = HP;
                    charSheet.Range = Range;
                    charSheet.Initiative = Initiative;
                    InitTracker.ListAdd(charSheet);
                    Indicator.SetActive(false);
                    yield break;
                }
            }
            if (Input.GetMouseButton(1))
            {
                Indicator.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
}
