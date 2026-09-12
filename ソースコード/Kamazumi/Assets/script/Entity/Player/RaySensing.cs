using UnityEngine;

public class RaySensing : MonoBehaviour, IDataPersistence
{
    public GameObject player;
    public GameObject objectToShow;

    static KeyCode interactKey = KeyCode.Mouse0;

    int rayLength = 5;
    int NumberOfClicks;
    // Start is called before the first frame update
    void Start()
    {
        interactKey = OperationSettingsScript.OperationSettings.interactKey;
    }

    public void LoadData(GameData data)
    {
        NumberOfClicks = data._score;
    }

    public void SaveData(ref GameData data) {
        data._score = NumberOfClicks;
    }

    // Update is called once per frame
    void Update()
    {
        objectToShow.SetActive(false);

        if (player.GetComponent<Player>().isPaused) return;
        if (player.GetComponent<Player>().isSmartphoneOpen) return;

        Ray ray = Camera.main.ViewportPointToRay(
      new Vector3(0.5f, 0.5f, 0f)
  );
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, rayLength)) return;
        if (!IsRayHit(hit)) return;
        NumberOfClicks++;
        InteractionEvents[] interactions =
            hit.collider.GetComponents<InteractionEvents>();
        ItemOutline itemOutline =
            hit.collider.GetComponent<ItemOutline>();

        if (interactions.Length == 0) return;

        objectToShow.SetActive(true);
        if (Input.GetKeyDown(interactKey))
        {
            foreach (InteractionEvents interaction in interactions)
            {
                if(interaction == null) continue;
                if (!interaction.enabled) continue;
                interaction.interactionEvents();
            }
        }
        if (itemOutline != null)
        {
            itemOutline.isOutlineON = true;
        }

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, rayLength);
    }
   

    //レイが当たったオブジェクトのタグを確認し
    bool IsRayHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("DoorWithLock")) return true;
        if (hit.collider.CompareTag("key")) return true;
        if (hit.collider.CompareTag("Image")) return true;
        if (hit.collider.CompareTag("Tips")) return true;
        if (hit.collider.CompareTag("Locker")) return true;
        if (hit.collider.CompareTag("InteractionObject")) return true;
        return false;
    }

    
}
