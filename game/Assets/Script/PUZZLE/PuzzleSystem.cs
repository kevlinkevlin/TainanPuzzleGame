using UnityEngine;
using System.Collections.Generic;
public class PuzzleSystem : MonoBehaviour
{
    public RectTransform BGRect;
    public int rowCount = 5;
    public int columnCount = 6;
    private Vector2 addPos = new Vector2(120, 120);
    public List<Orb> orbs = new List<Orb>();
    public List<List<Orb>> orbGroups = new List<List<Orb>>();
    public int removeCount = 3;
    [System.Serializable]
    public struct Combo
    {
        public Orb.OrbsType type;
        public int count;
    };
    public Combo [] orbCombo;
    public bool hasRemove = false;
    public float removeTime;
    void Start()
    {
        InitGrid();
        do {
            hasRemove = false;
            OrbInit();
            OrbGroup();
            OrbCombo();
            OrbRemove();
        } while (hasRemove);//if orb was removed
    }
    void InitGrid()
    {
        addPos.y = BGRect.sizeDelta.y / rowCount;
        addPos.x = BGRect.sizeDelta.x / columnCount;
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                //orb pos scale
                GameObject orbObj = Instantiate(Resources.Load("Prefabs/Orb")) as GameObject;
                RectTransform orbRect = orbObj.GetComponent<RectTransform>();
                orbRect.SetParent(BGRect);
                orbRect.localScale = Vector2.one;
                orbRect.anchoredPosition = new Vector2(c * addPos.x, r * addPos.y);
                //orb type number init
                Orb orb = orbObj.GetComponent<Orb>();
                orb.type = Orb.OrbsType.Null;
                orb.row = r;
                orb.column = c;
                orb.number = orb.row * columnCount + orb.column;
                orb.height = BGRect.sizeDelta.y / rowCount;
                orb.width = BGRect.sizeDelta.x / columnCount;
                orbs.Add(orb);
            }
        }
        //link orb (right top left bottom)
        for (int index = 0; index < orbs.Count; index++)
        {
            if (orbs[index].column != columnCount - 1)
                orbs[index].linkOrbs.Add(orbs[index + 1]);
            if(orbs[index].row != rowCount - 1)
                orbs[index].linkOrbs.Add(orbs[index + columnCount]);
            if (orbs[index].column != 0)
                orbs[index].linkOrbs.Add(orbs[index - 1]);
            if (orbs[index].row != 0)
                orbs[index].linkOrbs.Add(orbs[index - columnCount]);
        }
    }
    public void OrbInit()
    {
        foreach (Orb orb in orbs)//fall animation
        {
            if (orb.type == Orb.OrbsType.Null)
            {
                int count = 0;
                for (int index = orb.column; index <= columnCount * (rowCount - 1) + orb.column; index += columnCount)
                {
                    if (orbs[index].type == Orb.OrbsType.Null)
                    {
                        count++;
                    }
                }
                orb.SetAniPos(Vector2.up, count, Orb.OrbsState.Create);
            }
        }

        foreach (Orb orb in orbs)//clear orb's state(group removed) and random type
        {
            orb.group = false;
            orb.removed = false;
            if (orb.type == Orb.OrbsType.Null)
            {
                int typeNum = Random.Range(0, (int)Orb.OrbsType.Null);
                orb.type = (Orb.OrbsType)typeNum;
            }

        }
    }
    void FindMembers(Orb orb, int groupNum)
    {
        foreach (Orb linkOrb in orb.linkOrbs)
        {
            if (linkOrb.type == orb.type && linkOrb.group == false)//find members by type
            {
                orbGroups[groupNum].Add(linkOrb);
                linkOrb.group = true;
                FindMembers(linkOrb, groupNum);
            }
        }
    }
    public void OrbGroup()
    {
        orbGroups.Clear();
        foreach (Orb orb in orbs)
        {
            if (orb.group == false)
            {
                orbGroups.Add(new List<Orb>());
                int groupNum = orbGroups.Count - 1;
                orbGroups[groupNum].Add(orb);
                orb.group = true;
                FindMembers(orb, groupNum);
            }
        }
    }
    void FindRemoveOrb(Orb orb, int dir, int length, int comboIndex)
    {
        int orbIndex = orbs.IndexOf(orb);
        int orbCount = 0;
        for (int index = orbIndex; index <= length; index += dir) {
            if (orbs[index].type == orb.type)
                orbCount++;
            else
                break;
        }
        if (orbCount >= removeCount)
        {
            for (int index = orbIndex; index < orbIndex + (orbCount * dir); index += dir)
            {
                if (orbs[index].removed == false)
                    orbCombo[comboIndex].count += 1;
                orbs[index].removed = true;
                orbs[index].state = Orb.OrbsState.Remove;////
                orbs[index].removeTime = removeTime;
            }
        }
    }
    public void OrbCombo()
    {
        removeTime = 0;
        orbCombo = new Combo[orbGroups.Count];
        foreach (List<Orb> orbGroup in orbGroups)
        {
            int comboIndex = orbGroups.IndexOf(orbGroup);
            orbCombo[comboIndex].type = orbGroup[0].type;
            orbCombo[comboIndex].count = 0;
            foreach (Orb orb in orbGroup)
            {
                FindRemoveOrb(orb, 1, columnCount * (orb.row + 1) - 1, comboIndex);//find right combo
                FindRemoveOrb(orb, columnCount, columnCount * (rowCount - 1) + orb.column, comboIndex);//find top combo
            }
            if (orbCombo[comboIndex].count != 0)
                removeTime += 0.5f;// combo + 1 remove animation's time + 0.5
        }
    }
    public void OrbRemove()
    {
        foreach (Orb orb in orbs)
        {
            if (orb.removed == true)
            {
                orb.type = Orb.OrbsType.Null;
                hasRemove = true;
            }
        }
        foreach (Orb orb in orbs)
        {
            if (orb.type == Orb.OrbsType.Null)
            {
                int count = 0;
                for (int index = orbs.IndexOf(orb); index <= columnCount * (rowCount - 1) + orb.column; index += columnCount)
                {
                    count++;
                    if (orbs[index].type != Orb.OrbsType.Null)
                    {
                        orb.type = orbs[index].type;
                        orbs[index].type = Orb.OrbsType.Null;
                        orb.SetAniPos(Vector2.up, count - 1, Orb.OrbsState.Create);
                        break;
                    }
                }
            }
        }
    }
    void Update()
    {
        
       
    }
}
