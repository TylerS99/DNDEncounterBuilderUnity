using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour {
    [SerializeField] public string Model = "Cleric";
    [SerializeField] public string Name = "Player";
    [SerializeField] public int HP = 20;
    [SerializeField] public int Speed = 30;
    [SerializeField] public int Range = 30;
    [SerializeField] public int Initiative = 10;
}
