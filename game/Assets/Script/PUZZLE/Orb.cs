using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class Orb : MonoBehaviour
{
    public enum OrbsType { Fire, Ice, Wood, Shine, Dark, Heart, Null };
    public OrbsType type;
    private Sprite[] typeImage;
    public Image image;
    public RectTransform imageRect;

    public float width;
    public float height;
    
    public int row;
    public int column;
    public int number;
    public List<Orb> linkOrbs = new List<Orb>();
    public bool group = false;
    public bool removed = false;
    
    public enum OrbsState { Create, Change, Remove, Stay };
    public OrbsState state;
    public float removeTime;
    private float timer = 0;
    public float removeAlpha;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finger")
        {
            image.color = new Color(1, 1, 1, 0);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Finger" && other.GetComponent<Finger>().moveState == false)
        {
            image.color = Color.white;
        }
    }
    public void RemoveAni()
    {
        timer += Time.deltaTime;
        if (timer > removeTime)
        {
            if (image.color.a > 0)
            {
                image.color = new Color(1, 1, 1, image.color.a - removeAlpha * Time.deltaTime);
            }
            else
            {
                state = OrbsState.Stay;
                timer = 0;
            }
        }
        
    }
    public void SetAniPos(Vector2 dir, int count, OrbsState state)
    {
        imageRect.anchoredPosition = new Vector2(width * dir.x, height * dir.y) * count;
        this.state = state;
    }
    public void MoveAni()
    {
        image.color = Color.white;
        imageRect.anchoredPosition = Vector2.MoveTowards(imageRect.anchoredPosition, Vector2.zero, 2000 * Time.deltaTime);
        if (imageRect.anchoredPosition == Vector2.zero)
            state = OrbsState.Stay;
    }
    void Start()
    {
        transform.name = "orb" + row + column;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        typeImage = new Sprite[(int)OrbsType.Null];
        for (int i = 0; i < typeImage.Length; i++)
        {
            typeImage[i] = Resources.Load<Sprite>("Image/" + (OrbsType)i);
        }

        GetComponent<BoxCollider2D>().offset = new Vector2(width / 2, height / 2);
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);

        imageRect.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        state = OrbsState.Create;
    }
    void Update()
    {
        image.sprite = typeImage[(int)type];
        switch (state)
        {
            case OrbsState.Create:
            case OrbsState.Change:
                MoveAni();
                break;
            case OrbsState.Remove:
                RemoveAni();
                break;
        }
    }
}
