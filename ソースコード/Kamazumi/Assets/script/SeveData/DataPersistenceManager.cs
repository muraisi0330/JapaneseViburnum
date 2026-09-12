using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;

    [SerializeField] private bool _useEncryption;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;
    public static DataPersistenceManager instance { get; private set; }

private void Awake()
  {
    if (instance != null)
    {
      Debug.LogError("Found more than one DataPersistenceManager in the scene. Destroying the newest one.");
      Destroy(this.gameObject);
      return;
    }
    instance = this;
    DontDestroyOnLoad(this.gameObject);
   }
    //このメソッドは、ゲームの開始時に呼び出され、データの読み込みを行います。
    private void Start()
    {
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    //このメソッドは、新しいゲームを開始するために呼び出されます。
    public void NewGame()
    {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        _gameData = new GameData();
    }
    //このメソッドは、ゲームの進行状況を読み込むために呼び出されます。
    public void LoadGame()
    {
        this._gameData = _fileDataHandler.Load();

        if (_gameData == null)
        {
           // Debug.LogWarning("No game data found. Starting a new game.");
            NewGame();
        }
       foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
      // Debug.Log("Loading game data from file: " + _gameData._score);
    }
    //このメソッドは、ゲームの進行状況を保存するために呼び出されます。
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }
        Debug.Log("Saving deth data to file: " + _gameData._score);

        _fileDataHandler.Save(_gameData);
    }
    //このメソッドは、アプリケーションが終了する際に呼び出され、データの保存を行います。
    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}
    
    public void SaveGameButton()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
