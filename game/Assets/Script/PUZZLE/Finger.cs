using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Finger : MonoBehaviour {
    public PuzzleSystem puzzleSystem;
    private RectTransform rect;
    public Vector2 initPos;
    private Image image;
    private Color display = new Color(1, 1, 1, 0.5f);
    private Color hide = new Color(1, 1, 1, 0);
    public Orb nowOrb;
    public bool moveState;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Orb")
        {
            if (nowOrb == null || nowOrb == other.GetComponent<Orb>())
            {
                nowOrb = other.GetComponent<Orb>();
            }
            else
            {
                moveState = true;
                Orb.OrbsType temp = other.GetComponent<Orb>().type;
                other.GetComponent<Orb>().type = puzzleSystem.orbs[nowOrb.number].type;
                puzzleSystem.orbs[nowOrb.number].type = temp;

                nowOrb.SetAniPos(new Vector2(other.GetComponent<Orb>().column - nowOrb.column, other.GetComponent<Orb>().row - nowOrb.row), 1, Orb.OrbsState.Change);
                
                nowOrb = other.GetComponent<Orb>();
                
            }
            image.sprite = Resources.Load<Sprite>("Image/" + other.GetComponent<Orb>().type);
            image.color = display;
        }
    }
    public void FingerInit()
    {
        rect.anchoredPosition = initPos;
        image.color = hide;
        nowOrb = null;
        moveState = false;
    }
    void Start() {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(puzzleSystem.BGRect.sizeDelta.x / puzzleSystem.columnCount,
                                    puzzleSystem.BGRect.sizeDelta.y / puzzleSystem.rowCount);
        image = GetComponent<Image>();
        FingerInit();
    }
}
