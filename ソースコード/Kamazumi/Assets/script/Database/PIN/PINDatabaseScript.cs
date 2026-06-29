using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PIN")]
public class PINDatabaseScript : ScriptableObject
{
   public List<PINData> PINList = new List<PINData>();
}

[System.Serializable]
public class PINData
{
    public string PIN;
}
