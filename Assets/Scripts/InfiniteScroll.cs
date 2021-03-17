using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InfiniteScroll : MonoBehaviour {
    int i = 0; //index for first text
    List<MapData> Maps; //List of map structures
    List<Text> textList; //List of texts
    public MapData Selected;
    int startingIndex = 1;
    int endingIndex = 5;


    private void Start()
    {
        //Generate list of maps from file
        Maps = new List<MapData>();
        TextAsset txt = (TextAsset)Resources.Load("maps", typeof(TextAsset));
        foreach(var line in txt.text.Split('\n'))
        {
            string newLine = line;
            if (line[line.Length - 1] == '\n' || line[line.Length-1] == '\r')
                newLine = line.Substring(0, line.Length - 1);
            Maps.Add(new MapData(newLine));
        }
        Maps.Sort(SortByName);
        if (Maps.Count < startingIndex - endingIndex + 1)
            return;

        //Get list of text fields
        textList = new List<Text>();
        for (int i = startingIndex; i <= endingIndex; i++)
        {
            textList.Add(transform.GetChild(i).GetComponent<Text>());
        }

        //Initialize text fields
        UpdateText();
    }

    //Helper to sort list by Name of map
    static int SortByName(MapData p1, MapData p2)
    {
        return string.Compare(p1.Name, p2.Name);
    }

    //Scrolls upwards in list
    public void ScrollUp()
    {
        i--;
        if (i < 0)
        {
            i = Maps.Count - 1;
        }
        for (int j = 0; j < textList.Count; j++)
        {
            Vector3 currPos = new Vector3(0, 40 - 20 * j, 0);
            Vector3 finalPos = new Vector3(currPos.x, currPos.y - 20, currPos.z);
            StartCoroutine(MoveAndUpdate(0.1f, textList[j].gameObject, currPos, finalPos));
        }
    }

    //Scrolls downwards in list
    public void ScrollDown()
    {
        i++;
        if (i >= Maps.Count)
        {
            i = 0;
        }
        for(int j = 0; j < textList.Count; j++)
        {
            Vector3 currPos = new Vector3(0, 40 - 20 * j, 0);
            Vector3 finalPos = new Vector3(currPos.x, currPos.y + 20, currPos.z);
            StartCoroutine(MoveAndUpdate(0.1f, textList[j].gameObject, currPos, finalPos));
        }
    }

    //Jump to closest letter in list
    public void JumpToLetter(string letterStr)
    {
        char letter = letterStr[0];
        int j = 0;
        while(j<Maps.Count && Maps[j].Name[0].CompareTo(letter) < 0)
        {
            j++;
        }
        i = j - 2;
        if(i < 0)
        {
            i = Maps.Count + i;
        }
        UpdateText();
    }


    //Updates text after scrolling or jumping
    void UpdateText()
    {
        int currI = i;
        for (int j = 0; j < textList.Count; j++)
        {
            if (currI + j >= Maps.Count)
            {
                currI = -j;
            }
            textList[j].text = Maps[currI + j].Name;
            textList[j].transform.GetChild(0).GetComponent<RawImage>().texture = Maps[currI + j].imageTexture;
            if(j == 2)
            {
                Selected = Maps[currI + 2];
            }
        }
    }

    //Moves each text to given place, then updates it
    private IEnumerator MoveAndUpdate(float time, GameObject toMove, Vector3 startingPos, Vector3 finalPos)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            toMove.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UpdateText();
        toMove.transform.localPosition = startingPos;
    }
}
