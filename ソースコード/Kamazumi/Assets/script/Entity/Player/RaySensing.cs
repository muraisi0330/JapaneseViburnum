using UnityEngine;

public class RaySensing : MonoBehaviour
{
    public GameObject player;
    public GameObject objectToShow;

    static KeyCode interactKey = KeyCode.Mouse0;

    int rayLength = 5;
    // Start is called before the first frame update
    void Start()
    {
        interactKey = OperationSettingsScript.OperationSettings.interactKey;
    }

    // Update is called once per frame
    void Update()
    {
        objectToShow.SetActive(false);

        if (player.GetComponent<Player>().isPaused) return;
        if (player.GetComponent<Player>().isSmartphoneOpen) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, rayLength)) return;
        if (!IsRayHit(hit)) return;

        InteractionEvents[] interactions =
            hit.collider.GetComponents<InteractionEvents>();

        if (interactions.Length == 0) return;

        objectToShow.SetActive(true);

        if (Input.GetKeyDown(interactKey))
        {
            foreach (InteractionEvents interaction in interactions)
            {
                interaction.interactionEvents();
            }
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
        return false;
    }
}
