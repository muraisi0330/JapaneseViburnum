using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using DG.Tweening;
using DC.Scanner;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    enum State
    {
        Patrol,//パトロール
        Chase,//追跡
        Investigate,//調査
        Return,//帰還
        Search,//捜索
        GameOver//ゲームオーバー
    }

    [Header("References")]
    public Transform[] explorationPoints;
    public Transform stopPoint;
    public Transform heardPoint;
    public GameObject audibleObject;//音の発生源
    public GameObject playerObject;
    public GameObject enemyLight;
    public GameObject UI;
    public PlayableDirector timeline;
    public TargetScanner scanner;

    //敵の速度  
    public float patrolSpeed = 2.5f;
    // 追跡速度
    public float chaseSpeed = 5f;

    // 目的地までの距離のが近すぎる場合は巡回しない
    public int minPatrolDistance = 10;

    NavMeshAgent agent;
    Animator animator;

    State currentState = State.Patrol;
    Transform target;
    Sequence seq;

    Vector3 lastKnownPosition;
    bool isSearching = false;
    bool hasSetSearchDestination = false;
    bool reachedPatrol = false;

    int patrolIndex;
    float lostTimer;
    const float LOST_TIME = 5f;

    readonly int animRun = Animator.StringToHash("walking");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        seq = DOTween.Sequence();
        agent.speed = patrolSpeed;

        if (scanner == null)
            Debug.LogError("Scanner未設定");
    }
    private void OnDrawGizmos()
    {
        if (scanner != null)
            scanner.ShowGizmos();
    }

    void Update()
    {
        if (currentState == State.GameOver) return;
        UpdateTarget();
        UpdateState();
        UpdateAction();
    }

    //========================
    // 状態更新
    //========================
    void UpdateState()
    {
        // 1. 最優先：プレイヤー発見時
        if (target != null)
        {
            currentState = State.Chase;
            lostTimer = LOST_TIME;
            lastKnownPosition = target.position;
            seq.Kill();
            agent.speed = chaseSpeed;
            return;
        }
        // 2. 追跡猶予時間
        if (lostTimer > 0)
        {
            lostTimer -= Time.deltaTime;
            currentState = State.Chase;
            return;
        }

        // 3. 音の調査
        if (audibleObject != null && audibleObject.activeSelf)
        {
            currentState = State.Investigate;
            agent.speed = chaseSpeed;
            return;
        }
        // 4. 捜索中は他の状態に遷移しない
        if (currentState == State.Search)
        {
            agent.speed = patrolSpeed;
            return;
        }
        // 5. その他はパトロール
        if (currentState == State.Chase)
        {
            agent.speed = patrolSpeed;
            EnterSearch(lastKnownPosition);
            return;
        }
        // 6. 調査からパトロールに戻る際は捜索へ
        if (currentState == State.Investigate)
        {
            agent.speed = patrolSpeed;
            currentState = State.Search;
            return;
        }

        currentState = State.Patrol;
    }

    //========================
    // 行動処理
    //========================
    void UpdateAction()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Investigate:
                Investigate();
                break;

            case State.Return:
                Return();
                break;

            case State.Search:
                Search();
                break;
        }
    }

    //========================
    // 各行動
    //========================

    void Patrol()
    {
        animator.SetBool(animRun, true);

        if (explorationPoints.Length == 0) return;

        if (agent.hasPath && IsArrived() && !reachedPatrol)
        {
            reachedPatrol = true;
            EnterSearch(transform.position);
            return;
        }

        // ★追加：まだ移動してないなら巡回開始
        if (!agent.hasPath)
        {
            patrolIndex = Random.Range(0, explorationPoints.Length);
            float dis = Vector3.Distance(transform.position, explorationPoints[patrolIndex].position);
            if (dis < minPatrolDistance) return;
            SetDestination(explorationPoints[patrolIndex]);
        }
    }
    // 追跡
    void Chase()
    {
        if (target == null) return;

        animator.SetBool(animRun, true);
        SetDestination(target);
    }
    // 調査
    void Investigate()
    {
        if (heardPoint == null) return;

        animator.SetBool(animRun, true);
        SetDestination(heardPoint);
        
        if (IsArrived())
        {
            audibleObject.SetActive(false);
            currentState = State.Search; 
        }
    }

    // 帰還
    void Return()
    {
        if (stopPoint == null) return;

        animator.SetBool(animRun, true);
        SetDestination(stopPoint);

        if (IsArrived())
        {
            currentState = State.Patrol;
        }
    }
    // 捜索
    void Search()
    {
        animator.SetBool(animRun, true);

        if (!hasSetSearchDestination)
        {
            SetDestinationPosition(lastKnownPosition);
            hasSetSearchDestination = true;
        }

        if (IsArrived() && !isSearching)
        {
            isSearching = true;
            agent.isStopped = true;
            animator.SetBool(animRun, false);

            seq = DOTween.Sequence();

            seq.Append(transform.DORotate(new Vector3(0, -60, 0), 0.8f).SetRelative());
            seq.Append(transform.DORotate(new Vector3(0, 120, 0), 1.6f).SetRelative());
            seq.Append(transform.DORotate(new Vector3(0, -60, 0), 0.8f).SetRelative());

            seq.OnComplete(() =>
            {
                isSearching = false;
                hasSetSearchDestination = false;
                reachedPatrol = false;
                agent.isStopped = false;
                currentState = State.Return;
            });
        }
    }

    //========================
    // 共通処理
    //========================

    // ターゲットの更新
    void UpdateTarget()
    {
        if (scanner == null) return;

        target = scanner.GetTarget();
    }
    
    void SetDestination(Transform t)
    {
        if (t == null) return;

        agent.isStopped = false;
        agent.SetDestination(t.position);
    }

    void SetDestinationPosition(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
    }
    void EnterSearch(Vector3 pos)
    {
        lastKnownPosition = pos;
        hasSetSearchDestination = false;
        isSearching = false;
        currentState = State.Search;

        reachedPatrol = false; 
    }

    //到達判定
    bool IsArrived()
    {
        if (agent.pathPending) return false;

        // 目的地までの距離が停止距離以下か？
        if (agent.remainingDistance > agent.stoppingDistance) return false;
            // 経路を持っていない（=まだSetDestinationしてない、または計算失敗）か、
            // 速度がほぼゼロになったかを確認すると、より確実に「停止」を捉えられます
         if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) return true;

        return false;
    }


    //========================
    // 衝突
    //========================
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        GameOver();
    }
    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        
        GameOver();
    }



    void GameOver()
    {
        currentState = State.GameOver;

        agent.isStopped = true;
        animator.SetBool(animRun, false);
        //プレイヤーの方を向く
        LookAtPlayer();
        

        if (timeline != null) timeline.Play();
        if (enemyLight != null) enemyLight.SetActive(true);
        if (playerObject != null) playerObject.SetActive(false);
        if (UI != null) UI.SetActive(false);
    }
    //プレイヤーの方を向く
    void LookAtPlayer()
    {
        Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        direction.y = 0; // 水平方向のみにする
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }
}