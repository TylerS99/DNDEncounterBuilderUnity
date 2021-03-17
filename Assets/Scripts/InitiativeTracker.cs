using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeTracker : MonoBehaviour {
    List<CharacterSheet> InitiativeOrder;
    int TurnIndex;
    
    // Use this for initialization
    void Start () {
        TurnIndex = 0;
        InitiativeOrder = new List<CharacterSheet>();
	}
	
    public void ListAdd(CharacterSheet newChar)
    {
        InitiativeOrder.Add(newChar);
        ListUpdate();
    }

    public void ListRemove(CharacterSheet removeChar)
    {
        InitiativeOrder.Remove(removeChar);
        ListUpdate();
    }

    public void ListUpdate()
    {
        InitiativeOrder.Sort(SortByInitiative);
    }

    static int SortByInitiative(CharacterSheet p1, CharacterSheet p2)
    {
        return p2.Initiative.CompareTo(p1.Initiative);
    }

    public void NextTurn()
    {
        if (InitiativeOrder.Count == 0)
            return;
        if (TurnIndex >= InitiativeOrder.Count)
        {
            TurnIndex = 0;
        }
        InitiativeOrder[TurnIndex].gameObject.GetComponent<CharacterBrain>().ResetTurn();
        InitiativeOrder[TurnIndex].gameObject.GetComponent<CharacterBrain>().Run();
        TurnIndex++;      
    }
}
