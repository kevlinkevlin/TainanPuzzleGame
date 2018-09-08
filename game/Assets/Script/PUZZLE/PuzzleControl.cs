using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PuzzleControl : MonoBehaviour {
    public float puzzleTime = 4;
    public PuzzleSystem puzzleSystem;
    public enum PuzzleState { Standby, Start, Move, End, Confirm };
    private string[] stateString = {"待機中...", "準備開始", "移動中...","轉珠結束", "Combo計算" };
    public PuzzleState state;
    public RectTransform BGRect;
    public RectTransform fingerRect;
    public Finger finger;
    public float moveTimer = 0;
    public Text typeText;
    public Text timerText;
    private Vector3 inputPos;
    private Vector3 fingerPos;
    private bool touchState = false;
    Vector2 WorldToRect()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, inputPos);
        Vector2 Pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(BGRect, screenPoint, Camera.main, out Pos);
        return Pos;
    }
    void OrbReset()
    {
        puzzleSystem.OrbRemove();
        puzzleSystem.OrbInit();
        if (puzzleSystem.hasRemove)
        {
            Invoke("FindCombo", 0.5f);
            puzzleSystem.hasRemove = false;
        }
        else
            state = PuzzleState.Standby;

    }
    void FindCombo()
    {
        puzzleSystem.OrbInit();//clear clear orb's state(group removed)
        puzzleSystem.OrbGroup();//find group
        puzzleSystem.OrbCombo();//find combo
        Invoke("OrbReset", puzzleSystem.removeTime);//remove and init orb when animation finish
        state = PuzzleState.Confirm;
    }
    void MoveControl()
    {
        moveTimer += Time.deltaTime;
        if (touchState)
        {
            Vector2 pos = Vector2.MoveTowards(fingerRect.anchoredPosition, 
                new Vector2(Mathf.Clamp(fingerPos.x, 5, BGRect.sizeDelta.x - 5), Mathf.Clamp(fingerPos.y, 5, BGRect.sizeDelta.y - 5)), 
                100);
            fingerRect.anchoredPosition = pos;
        }
        else
        {
            GameObject.Find("PuzzleSystem").GetComponent<PuzzleSystem>().orbs[finger.nowOrb.number].GetComponent<Orb>().image.color = Color.white;
            finger.FingerInit();
            state = PuzzleState.End;
            moveTimer = 0;
        }
        
        if (moveTimer >= puzzleTime)
        {
            finger.FingerInit();
            state = PuzzleState.End;
            moveTimer = 0;
        }
    }
    void StartControl()
    {
        if (touchState)
        {
            fingerRect.anchoredPosition = new Vector2(Mathf.Clamp(fingerPos.x, 5, BGRect.sizeDelta.x - 5),
                          Mathf.Clamp(fingerPos.y, 5, BGRect.sizeDelta.y - 5));
            if (finger.moveState)
                state = PuzzleState.Move;
        }
        else
        {
            finger.FingerInit();
            state = PuzzleState.Standby;
        }

    }
    void StandbyControl()
    {
        if (touchState)
        {
            if (fingerPos.x < BGRect.sizeDelta.x && fingerPos.x > 0
                && fingerPos.y < BGRect.sizeDelta.y && fingerPos.y > 0)
                state = PuzzleState.Start;
        }
    }
    void Start ()
    {
        state = PuzzleState.Standby;
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case PuzzleState.Standby:
                StandbyControl();
                break;
            case PuzzleState.Start:
                StartControl();
                break;
            case PuzzleState.Move:
                MoveControl();
                break;
            case PuzzleState.End:
                FindCombo();
                break;
        }
    }
    void Update()
    {
        typeText.text = "狀態 " + stateString[(int)state];
        timerText.text = "剩餘時間:" + (puzzleTime - moveTimer).ToString("00.00") + "秒";
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        if (Input.GetMouseButton(0))
        {
            touchState = true;
            inputPos = Input.mousePosition;
            fingerPos = WorldToRect();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchState = false;
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touchState = true;
            inputPos = Input.GetTouch(0).position;
            fingerPos = WorldToRect();
        }
        else if (Input.touchCount <= 0)
        {
            touchState = false;
        }
#endif
    }
}
