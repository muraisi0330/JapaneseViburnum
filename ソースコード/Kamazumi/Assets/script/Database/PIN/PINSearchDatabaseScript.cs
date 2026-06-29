
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PINSearch")]
public class PINSearchDatabaseScript : ScriptableObject
{
    public List<PINSearchData> PINList = new List<PINSearchData>();
}

[System.Serializable]
public class PINSearchData
{
    public string PIN;
    public Sprite searchImage;
}
