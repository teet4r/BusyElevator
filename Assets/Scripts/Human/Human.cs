using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour, ActInterface
{
    public static class Mask
    {
        public const int Up = 9;
        public const int Down = 10;
        public const int After = 11;  // 하차 후 
    }

    public int color { get; set; } // 자신의 색깔을 가진 층에만 갈 수 있음
    public float dirty_size; // 이 사람이 층에 더러움을 기여하는 정도
    public int destination_floor; // 목적지
    public int exp;
    public float speed;
    public int gold;

    // 상태변수

    public int state { get; set; }
    public bool is_first { get; set; } // 이 사람이 현재 맨 앞인지
    public bool is_INF_human { get; set; } = false; // 이 사람이 무한정 기다릴 수 있는 사람인지 (청소부, 가드, 정비공, 위생관, 회장)

    // 줄을 몇초간 설 수 있는지
    [SerializeField]
    protected float time_who_can_wait;

    protected int layermask;
    public int population { get; protected set; } // 이 변수는 버프&디버프 절대 없음 껄껄
    public Floor currentfloor_script { get; protected set; }
    public Floor destinationfloor_script { get; protected set; }

    [SerializeField]
    protected SpriteRenderer speech_bubble;    //상태 말풍선
    [SerializeField]
    protected TextMesh des_bubble_text;    //목적지 글자

    [SerializeField]
    protected GameObject gold_image; // 사람이 돈 벌었을때 보여줄것 (x축은 불변 -0.3y축 0.45에서 생성 0.9까지 올라갔다가 0.8까지 떨어짐 그리고 서서히 사라짐)
    [SerializeField]
    protected TextMesh gold_text; // 사람이 돈 벌었을때 글자 변경

    public Rigidbody2D rigid { get; private set; }
    public BoxCollider2D coll { get; private set; }

    // 코루틴
    protected Coroutine move_to_end_line = null;
    protected Coroutine move_coroutine = null;
    protected Coroutine move_right = null;
    protected Coroutine angry_timer = null;

    protected SpriteRenderer spriteRenderer;

    public Animator animator { get; private set; }

    protected float r;
    protected float g;
    protected float b;

    protected List<Floor> destinations { get; private set; } // 목적지 설정을 위한 배열, Awake()에서 할당

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinations = new List<Floor>();
        points = new Queue<EventObject>();

        r = spriteRenderer.color.r;
        g = spriteRenderer.color.g;
        b = spriteRenderer.color.b;

        gold_image.GetComponent<SpriteRenderer>().sortingLayerName = "HumanAfter";

        MeshRenderer gt_mesh = gold_text.GetComponent<MeshRenderer>();
        gt_mesh.sortingLayerName = "HumanAfter";
        gt_mesh.sortingOrder = 1;
    }
    protected void OnEnable()
    {
        state = HumanState.WAITING;
        spriteRenderer.color = new Color(r, g, b, 1);
        is_first = false;
        gold_image.SetActive(false);
    }

    public virtual void ReturnHuman() { } // 자식 클래스에서 overriding
    public virtual void Set(Floor currentfloor_script) { }
    public virtual void StartMove(Vector2 spawn_point, bool going_left)
    {
        layermask = 1 << gameObject.layer; // 현재 오브젝트와 같은 레이어와만 충돌
        rigid.position = new Vector2(Random.Range(4f, 8f), spawn_point.y);

        if (gameObject.activeSelf)
            move_to_end_line = StartCoroutine(MoveToEndLine());
    }
    public virtual void ChangeSpriteLayer(string layername)
    {
        spriteRenderer.sortingLayerName = layername;
        speech_bubble.sortingLayerName = layername;
        MeshRenderer mesh = des_bubble_text.GetComponent<MeshRenderer>();
        mesh.sortingLayerName = layername;
        mesh.sortingOrder = 1;
    }
    protected IEnumerator MoveToEndLine()
    {
        spriteRenderer.flipX = true;

        Debug.DrawRay(rigid.position + Vector2.down * 0.3f, Vector2.left * 0.3f, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rigid.position + Vector2.down * 0.3f, new Vector2(-1, 0), 1, layermask);

        while (hit.Length > 1) // 본인 포함 충돌 = 다른 객체와 충돌했다는 뜻 = 뒷줄로 꺼져버렷
        {
            Debug.DrawRay(rigid.position + Vector2.down * 0.3f, Vector2.left * 0.3f, Color.red);
            hit = Physics2D.RaycastAll(rigid.position + Vector2.down * 0.3f, Vector2.left, 1f, layermask);
            // 현재 게임 오브젝트와 같은 레이어만 검색하게 함

            rigid.velocity = Vector2.right * speed; // move

            yield return null;
        }

        rigid.velocity = Vector2.zero;

        spriteRenderer.flipX = false;

        // 정상적으로 줄을 설때까지 대기
        if (destination_floor > currentfloor_script.floor_level) // 위층으로 이동
        {
            ChangeSpriteLayer("HumanUp");
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(rigid.position.x, currentfloor_script.spawn_point_up.y)));
        }
        else
        {
            ChangeSpriteLayer("HumanDown");
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(rigid.position.x, currentfloor_script.spawn_point_down.y)));
        }


        move_coroutine = StartCoroutine(Move());
        angry_timer = StartCoroutine(AngryTimer(time_who_can_wait));
    }
    protected virtual IEnumerator Move()
    {
        while (true)
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left * 0.3f, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left, 0.3f, layermask);
            // 현재 게임 오브젝트와 같은 레이어만 검색하게 함

            if (hit.collider != null)
            { // 사람 혹은 벽과 충돌함 멈춰야함
                rigid.velocity = Vector2.zero;

                if (hit.collider.tag == "Endline") // 벽과 충돌
                {
                    animator.SetBool("isWaiting", true);
                    currentfloor_script.ReadyToGetElevator(this);
                    is_first = true;
                    yield break;
                }
                else
                    animator.SetBool("isWaiting", rigid.velocity == Vector2.zero);
            }
            else // 충돌한 것이 없음 
            {
                rigid.velocity = Vector2.left * speed; // move
                animator.SetBool("isWaiting", false);
            }

            yield return null;
        }
    }

    protected Queue<EventObject> points;
    protected virtual IEnumerator MoveRight(Vector2 start) // 배달부 스크립트에서 override
    {
        animator.SetBool("isWaiting", false);
        points.Clear();
        rigid.position = start;
        // 방문할 지점 받아올 때까지 기다림
        yield return destinationfloor_script.StartCoroutine(destinationfloor_script.GetEventPoints(points));

        // 지점 방문
        while (points.Count != 0)
        {
            EventObject point = points.Dequeue();
            Vector2 point_pos = point.gameObject.transform.position;
            spriteRenderer.flipX = rigid.position.x < point_pos.x;  // 이동하려는 방향으로 돌아섬
            while (rigid.position != point_pos)
            {
                rigid.position = Vector2.MoveTowards(rigid.position, point_pos, speed * Time.deltaTime);
                yield return null;
            }
            //yield return StartCoroutine(StopMoveRightWhile()); // 해당 지점의 동작 수행

            //StopCoroutine(move_right);
            yield return StartCoroutine(point.EventOn(this));
            //move_right = StartCoroutine(MoveRight(rigid.position));
        }
    }

    protected virtual IEnumerator MoveAtoB(Vector2 start, Vector2 end)
    {
        rigid.position = start;
        animator.SetBool("isWaiting", false);

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime);
            yield return null;
        }
    }

    // elevator에 타기 위해 이동하는 로직
    public virtual IEnumerator MovetoElevator(Vector2 start, Vector2 end)
    {
        rigid.position = start;
        animator.SetBool("isWaiting", false);

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_on_speed_rate));
            yield return null;
        }
    }

    public IEnumerator Fadeout()
    {
        for (float a = 1; a >= 0; a -= 0.05f)
        {
            spriteRenderer.color = new Color(r, g, b, a);
            yield return null;
        }
    }

    /// <summary>
    /// 특정시간 기다렸다가 그때까지 탑승 못하면 개빡쳐서 사라짐
    /// </summary>
    /// <param name="patience_guage"></param>
    /// <returns></returns>
    protected IEnumerator AngryTimer(float patience_sec)
    {
        if (is_INF_human)
            yield break;

        yield return new WaitForSeconds(patience_sec);

        // 여전히 기다리는 중이면
        if (state == HumanState.WAITING)
        {
            if (gameObject.layer == Mask.Up && is_first) // 만약 위로가는 줄에서 제일 앞자리에서 대기 중이였다면
                currentfloor_script.up_first = null;
            else if (gameObject.layer == Mask.Down && is_first)
                currentfloor_script.down_first = null;
            spriteRenderer.flipX = false;
            state = HumanState.WALKING;
            gameObject.layer = LayerMask.GetMask("HumanAfter");
            if (move_to_end_line != null)
                StopCoroutine(move_to_end_line);
            if (move_coroutine != null)
                StopCoroutine(move_coroutine);
            rigid.velocity = Vector2.zero;
            Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(-population);
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.ANGRY];
            des_bubble_text.text = "";
            currentfloor_script.SetDirtyRate(2 * dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(-5, 2 * (currentfloor_script.floor_level - 1))));
            yield return StartCoroutine(Fadeout());
            ReturnHuman();
        }
    }

    /// <summary>
    /// Default Functions of ActInterface
    /// </summary>
    /// <param name="elevator"></param>
    public virtual IEnumerator ActAtATM(ATM atm)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, atm.earn_things);
        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtVM(VendingMachine vm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, vm.earn_things);
        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtHD(HospitalDoor hd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, hd.earn_things);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtExit(Exit exit)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtInfo(Infomation info)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, info.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtWP(WaterPurifier wp)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, wp.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtBS(BookShelf bs)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, bs.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtND(NormalDoor nd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, nd.earn_things);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtFFExit(FirstFloorExit ffe)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.go_to_home_pay_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtRestExit(RestExit re)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtCounter(Counter c)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, c.earn_things);

        yield return new WaitForSeconds(3f);
    }





    /// <summary>
    /// Default Functions of reacting with other humans
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator ActWithGuard(Human other)
    {
        yield return null;
    }
    public virtual IEnumerator ActWithThief(Human other)
    {
        yield return null;
    }





    /// <summary>
    /// ActOff/ActIn
    /// </summary>
    /// <param name="elevator"></param>
    public virtual void ActOffElevator(ElevatorClass elevator)
    {
        // 층 더럽힘
        destinationfloor_script.SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        // 골드 추가 층이 너무 더러우면 돈을 안줘요
        if (destinationfloor_script.dirty_index < 90)
            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, gold);
        else // 너무 더러우면 돈을 오히려 뺏기는 디버프 용( 곰팡이 벽지 )
            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.mold_pay_size);

        speech_bubble.gameObject.SetActive(false); // 말풍선 삭제
        des_bubble_text.text = "";
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // 오른쪽으로 움직이기 시작
        move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }

    public virtual void ActInElevator(ElevatorClass elevator)
    {
        //대기 인구수 제거
        Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(-population);
        StopCoroutine(move_coroutine);
    }

    public IEnumerator CoinEffect(int gold) // 금액
    {
        gold_text.text = (gold >= 0 ? "+" : "") + gold.ToString();
        gold_image.transform.position = gameObject.transform.position + new Vector3(-0.2f, 0.45f);
        gold_image.SetActive(true);

        while (gold_image.transform.position.y < gameObject.transform.position.y + 0.9)
        {
            gold_image.transform.position += Vector3.up * 0.01f;
            yield return null;
        }
        while (gold_image.transform.position.y > gameObject.transform.position.y + 0.8)
        {
            gold_image.transform.position += Vector3.down * 0.01f;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        gold_image.SetActive(false);
    }
}