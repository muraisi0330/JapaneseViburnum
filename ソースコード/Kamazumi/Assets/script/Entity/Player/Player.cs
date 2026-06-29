using UnityEngine;
using UnityEngine.UI;//UIを使うときに書きます。
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float acceleration = 12f;

    [Header("Look")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform flashlight;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Stamina")]
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float staminaUseInterval = 0.08f;
    [SerializeField] private float staminaRecoverInterval = 0.12f;
    [SerializeField] private int fatigueRecoverBorder = 30;

    [Header("UI")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject userManualUI;

    private Rigidbody rb;

    private Vector3 moveVelocity;
    private float pitch;

    private int stamina;
    private float useTimer;
    private float recoverTimer;
    private bool canSprint = true;//スタミナが0のときは走れない

    [Header("Control Flags")]
    public bool canControl = true;//外部からプレイヤーの操作を制御するためのフラグ
    public bool isPaused = false;//ゲームが一時停止しているかどうかを示すフラグ
    public bool isSmartphoneOpen = false;
    public bool isHiding = true;

    //操作キー
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    //走るキー
    KeyCode sprintKey = KeyCode.LeftShift;
    //視点移動のマウス
    string MouseX = "Mouse X";
    string MouseY = "Mouse Y";
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        stamina = maxStamina;
    }

    private void Start()
    {

        LoadOperationSettings();
        ApplyCursorLock(true);
        UpdateUI();
    }
    // 操作設定をOperationSettingsScriptから読み込む
    void LoadOperationSettings()
    {
        horizontal = OperationSettingsScript.OperationSettings.horizontal;
        vertical = OperationSettingsScript.OperationSettings.vertical;
        sprintKey = OperationSettingsScript.OperationSettings.sprintKey;
        MouseX = OperationSettingsScript.OperationSettings.mouseX;
        MouseY = OperationSettingsScript.OperationSettings.mouseY;
        mouseSensitivity = SaveDataScript.LoadMouseSensitivity();
    }

    private void Update()
    {
        HandlePause();
        LoadMouseSensitivityDuringPause();
        if (isPaused) return;
        HandleStamina();
        if (!canControl) return;
        if (isSmartphoneOpen) return;
        HandleLook();
       
    }
    //スマホUIの切り替え
   

    private void FixedUpdate()
    {
        if (!canControl) return;
        if (isPaused) return;
        if (isSmartphoneOpen) return;
        if(isHiding) return;
        HandleMove();
    }


    //移動処理を行う関数 
    private void HandleMove()
    {
        float x = Input.GetAxisRaw(horizontal);
        float z = Input.GetAxisRaw(vertical);

        Vector3 inputDir =
            (transform.right * x + transform.forward * z).normalized;

        bool sprintInput =
            Input.GetKey(sprintKey) && z > 0f;

        float targetSpeed = walkSpeed;

        if (inputDir.sqrMagnitude > 0 &&
            sprintInput &&
            stamina > 0 &&
            canSprint)
        {
            targetSpeed = sprintSpeed;
        }

        Vector3 targetVelocity = inputDir * targetSpeed;

        moveVelocity = Vector3.Lerp(
            moveVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime);

        float moveDistance =
            moveVelocity.magnitude * Time.fixedDeltaTime;

        RaycastHit hit;

        if (!Physics.CapsuleCast(
            transform.position + Vector3.up * 0.5f,
            transform.position + Vector3.up * 1.8f,
            0.3f,
            moveVelocity.normalized,
            out hit,
            moveDistance))
        {
            rb.MovePosition(
                rb.position +
                moveVelocity * Time.fixedDeltaTime);
        }
    }

    //カメラと懐中電灯の視点移動を処理する
    private void HandleLook()
    {
        float mouseX = Input.GetAxis(MouseX) * mouseSensitivity;
        float mouseY = Input.GetAxis(MouseY) * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 lookRot = new Vector3(pitch, 0f, 0f);

        if (cameraRoot != null)
            cameraRoot.localEulerAngles = lookRot;

        if (flashlight != null)
            flashlight.localEulerAngles = lookRot;
    }

   //スタミナの消費と回復を処理する
    private void HandleStamina()
    {
        bool usingSprint =
            Input.GetKey(sprintKey) &&
            Input.GetAxisRaw("Vertical") > 0f &&
            moveVelocity.sqrMagnitude > 0.1f &&
            canSprint;

        if (usingSprint)
        {
            ConsumeStamina();
        }
        else
        {
            RecoverStamina();
        }
    }
    //スタミナの消費
    void ConsumeStamina()
    {
        if (!canControl) return;
        if (isSmartphoneOpen) return;
        useTimer += Time.deltaTime;
        recoverTimer = 0f;

        if (useTimer >= staminaUseInterval)
        {
            stamina = Mathf.Max(0, stamina - 1);
            useTimer = 0f;

            if (stamina <= 0)
                canSprint = false;

            UpdateUI();
        }
    }
    //スタミナの回復
    void RecoverStamina()
    {
        recoverTimer += Time.deltaTime;
        useTimer = 0f;
        if (recoverTimer >= staminaRecoverInterval)
        {
            stamina = Mathf.Min(maxStamina, stamina + 1);
            recoverTimer = 0f;

            if (stamina >= fatigueRecoverBorder)
                canSprint = true;

            UpdateUI();
        }
    }


    //一時停止
    private void HandlePause()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseUI != null)
            pauseUI.SetActive(isPaused);

        ApplyCursorLock(!isPaused);
       
    }
    //一時停止中なら、マウス感度を保存データから読み込む
    void LoadMouseSensitivityDuringPause()
    {
        if (!isPaused) return;
            mouseSensitivity = SaveDataScript.LoadMouseSensitivity();
    }
    //カーソルのロックと表示の切り替え
    private void ApplyCursorLock(bool lockCursor)
    {
        Cursor.visible = !lockCursor;
        Cursor.lockState =
            lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }

  
    // UI
    private void UpdateUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
         bool IsMoving = moveVelocity.sqrMagnitude > 0.0f;
        if (userManualUI.activeSelf == !IsMoving)
            return;

        userManualUI.SetActive(!IsMoving);
    }

   // Public API
    public void SetControl(bool enable)
    {
        canControl = enable;

        if (!enable)
            moveVelocity = Vector3.zero;
    }

    public int GetStamina()
    {
        return stamina;
    }
}