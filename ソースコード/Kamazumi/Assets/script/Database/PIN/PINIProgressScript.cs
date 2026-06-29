using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PINprogress")]

public class PINIProgressScript : ScriptableObject
{
    // Start is called before the first frame update
    public List<PINProgress> PINList = new List<PINProgress>();
}

[System.Serializable]
public class PINProgress
{
    public string taskName;
}
