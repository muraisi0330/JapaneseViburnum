using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System .Serializable]
public class GameData 
{
    public int _score;
    public Vector3 _playerPosition;
    public SerializableDictionary<string,bool> itemCollected;

    public GameData()
    {
        this._score = 0;
        this._playerPosition = Vector3.zero;
        this.itemCollected = new SerializableDictionary<string, bool>();
    }
}
