using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PINITem")]
public class PINITemDatabaseDcript : ScriptableObject
{
    public List<PINITemDatabase> PINList = new List<PINITemDatabase>();
}

[System.Serializable]
public class PINITemDatabase
{
    public Sprite searchImage;
    public string itemName;
    public string itemDescription;
}

