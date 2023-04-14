using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardBase
{
    public CardBase(GameObject inCard)
    {
        _card = inCard;
        _cardList.Add(this);
    }

    protected GameObject _card;
    private CardArea _cardArea = CardArea.table;
    private Vector3 _focusPos = new Vector3();
    private Vector3 _focusRot = new Vector3();

    public void SetCardArea(CardArea area)
    {
        _cardArea = area;
    }

    public CardArea cardArea
    {
        get
        {
            return _cardArea;
        }
    }

    public void SetCard(GameObject inCard)
    {
        _card = inCard;
    }

    public GameObject card
    {
        get
        {
            return _card;
        }
    }

    public void InitFocusValue()
    {
        _focusPos = card.transform.position;
        _focusRot = card.transform.eulerAngles;
    }

    public void SetFocusPos(Vector3 pos)
    {
        _focusPos = pos;
    }

    public void SetFocusRot(Vector3 rot)
    {
        _focusRot = rot;
    }

    public Vector3 focusPos
    {
        get
        {
            return _focusPos;
        }
    }

    public Vector3 focusRot
    {
        get
        {
            return _focusRot;
        }
    }

    public bool IsPosInCardRange(Vector3 pos)
    {
        Quaternion qua = Quaternion.Euler(focusRot.x, focusRot.y, focusRot.z);
        Vector3 right = qua * (new Vector3(1, 0, 0));
        Vector3 up = qua * (new Vector3(0, 1, 0));
        right.Normalize();
        up.Normalize();
        right *= (CardCfg.cardWidth * 0.5f);
        up *= (CardCfg.cardHeight * 0.5f);
        Vector3 point1 = focusPos - right + up;
        Vector3 point2 = focusPos + right + up;
        Vector3 point3 = focusPos + right - up;
        Vector3 point4 = focusPos - right - up;
        return Linc.MathUtil.IsPosInRectRange(pos, point1, point2, point3, point4);
    }

    public bool isTop
    {
        get
        {
            if (!_card.activeSelf) return false;//隐藏的牌都不是最上方的牌
            for (int i = 0; i < _cardList.Count; i++)
            {
                CardBase curCard = _cardList[i];
                bool zSmaller = curCard.focusPos.z < focusPos.z;
                bool isInterSect = IntersectFocusCheck(curCard, this);
                bool isSameCard = curCard.card.GetInstanceID() == card.GetInstanceID();
                bool isActive = curCard.card.activeSelf;//必须是显示的牌才可以压着下方卡牌
                if (!isSameCard && zSmaller && isInterSect && isActive) return false;
            }
            return true;
        }
    }

    public List<CardBase> topCardList
    {
        get
        {
            List<CardBase> curList = new List<CardBase>();
            for (int i = 0; i < _cardList.Count; i++)
            {
                CardBase curCard = _cardList[i];
                if (curCard != this)
                {
                    bool zSmaller = curCard.focusPos.z < focusPos.z;
                    bool isInterSect = IntersectFocusCheck(curCard, this);
                    if (zSmaller && isInterSect) { curList.Add(curCard); };
                }
            }
            return curList;
        }
    }

    public Vector3 turnEulerAngle
    {
        get
        {
            Quaternion focusRot=Quaternion.Euler(_focusRot.x,_focusRot.y,_focusRot.z);
            Vector3 focusUp=focusRot*Vector3.up;
            Quaternion qua = Quaternion.AngleAxis(180, focusUp);
            Quaternion aimQua = qua * focusRot;
            return aimQua.eulerAngles;
        }
    }

    public bool cardIsFront{
        get{
            Quaternion focusQua = Quaternion.Euler(focusRot.x, focusRot.y, focusRot.z);
            Vector3 focusForward = focusQua * (new Vector3(0, 0, 1));
            return Vector3.Dot(focusForward, Vector3.forward) < 0;
        }
    }

    public void RemoveFromList()
    {
        _cardList.Remove(this);
    }
}
