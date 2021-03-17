using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData {
    public string Name;
    public Texture2D imageTexture;
    public MapData(string _Name)
    {
        Name = _Name;
        imageTexture = Resources.Load<Texture2D>("Maps/" + Name) as Texture2D;
    }
}
