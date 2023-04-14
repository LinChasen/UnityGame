using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class ProcedureDealCard : Linc.ProcedureBase
{
    private readonly Vector3 camPos = new Vector3(0, 0, -20);
    private readonly Vector3 camEu = new Vector3();
    private readonly float camField = 60;

    private List<NormalCardBase> _normalCardBaseList = new List<NormalCardBase>();
    private List<RabbitCardBase> _rabbitCardBaseList = new List<RabbitCardBase>();
    private List<CarrotCardBase> _carrotCardBaseList = new List<CarrotCardBase>();
    private List<AddCardBase> _addCardBaseList = new List<AddCardBase>();
    private GameObject _levelObj;
    private int _levelNum = 0;

    protected override void OnInit()
    {
        Camera.main.transform.position = camPos;
        Camera.main.transform.eulerAngles = camEu;
        Camera.main.fieldOfView = camField;
        Camera.main.orthographic = true;
    }

    protected override void OnEnter(object data)
    {
        _levelNum = (int)data;
        Linc.ViewMgr.OpenView(ViewType.InGameView);
        LoadLevel();
        TableTopCardNoReply();//保证最上层的数字牌不重复
        CreateCarrotCard();//生成萝卜牌
        AdjustCardParent();//全部统一到level节点下
        DealCard();
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {

    }

    private GameObject LoadCard(GameObject cardObj, GameObject pref)
    {
        Vector3 pos = cardObj.transform.position;
        Quaternion rot = cardObj.transform.rotation;
        int index = cardObj.transform.GetSiblingIndex();
        GameObject newCard = Linc.Game.InstantiateObject(pref);
        newCard.transform.SetParent(cardObj.transform.parent);
        cardObj.transform.SetParent(null);
        Linc.Game.DestroyObject(cardObj);
        newCard.transform.position = pos;
        newCard.transform.rotation = rot;
        newCard.transform.SetSiblingIndex(index);
        return newCard;
    }

    private void LoadLevel()
    {
        GameObject levelPref = Resources.Load<GameObject>("Prefabs/Level/Level" + _levelNum);
        _levelObj = Linc.Game.InstantiateObject(levelPref);
        //生成卡牌
        for (int i = 0; i < _levelObj.transform.childCount; i++)
        {
            GameObject ch = _levelObj.transform.GetChild(i).gameObject;
            for (int j = 0; j < ch.transform.childCount; j++)
            {
                GameObject cardObj = ch.transform.GetChild(j).gameObject;
                if (ch.name == "Table")
                {
                    if (cardObj.name.IndexOf("ani_rabbit") != -1)//兔子牌
                    {
                        GameObject cardPref = Resources.Load<GameObject>("Prefabs/Cards/ani_rabbit");
                        GameObject newCard = LoadCard(cardObj, cardPref);
                        RabbitCardBase cardBase = new RabbitCardBase(newCard);
                        _rabbitCardBaseList.Add(cardBase);
                    }
                    else if (cardObj.name.IndexOf("prop_add") != -1)//增加牌
                    {
                        Match match = Regex.Match(cardObj.name, @"[\d]");
                        int addNum = int.Parse(match.Value);
                        GameObject cardPref = Resources.Load<GameObject>("Prefabs/Cards/prop_add" + addNum);
                        GameObject newCard = LoadCard(cardObj, cardPref);
                        newCard.AddComponent<CardShadowControl>();
                        AddCardBase cardBase = new AddCardBase(newCard, addNum);
                        _addCardBaseList.Add(cardBase);
                    }
                    else
                    {//普通桌面牌
                        GameObject newCard = LoadCard(cardObj, CardPool.rndCardPref);
                        newCard.AddComponent<CardShadowControl>();
                        NormalCardBase cardBase = new NormalCardBase(newCard);
                        cardBase.SetCardArea(CardArea.table);
                        _normalCardBaseList.Add(cardBase);
                    }
                }
                else//手牌
                {
                    GameObject newCard = LoadCard(cardObj, CardPool.rndCardPref);
                    newCard.AddComponent<CardShadowControl>();
                    NormalCardBase cardBase = new NormalCardBase(newCard);
                    cardBase.SetCardArea(CardArea.hand);
                    _normalCardBaseList.Add(cardBase);
                }
            }
        }
    }

    private void TableTopCardNoReply()
    {
        CardBase.Init();
        List<string> poolCopy = new List<string>(CardPool.poolList);
        foreach (var item in _normalCardBaseList)
        {
            if (item.cardArea == CardArea.table && item.isTop)
            {
                int rndIndex = Random.Range(0, poolCopy.Count);
                GameObject pref = Resources.Load<GameObject>("Prefabs/Cards/" + poolCopy[rndIndex]);
                poolCopy.RemoveAt(rndIndex);
                GameObject newCard = LoadCard(item.card, CardPool.rndCardPref);
                newCard.AddComponent<CardShadowControl>();
                item.SetCard(newCard);
            }
        }
    }

    private void CreateCarrotCard()
    {
        CardBase.Init();
        if (_rabbitCardBaseList.Count == 0) return;
        //找出所有没被兔子牌关联压着的桌面牌
        List<NormalCardBase> noPressList = new List<NormalCardBase>();
        CheckNoPress(noPressList);
        //不能是最上面的牌
        for (int i = 0; i < noPressList.Count; i++)
        {
            if (noPressList[i].isTop)
            {
                noPressList.RemoveAt(i);
                i--;
            }
        }
        //根据兔子牌的数量生成萝卜牌
        for (int i = 0; i < _rabbitCardBaseList.Count; i++)
        {
            int rndIndex = Random.Range(0, noPressList.Count);
            NormalCardBase originBase = noPressList[rndIndex];
            noPressList.RemoveAt(rndIndex);
            //萝卜牌
            GameObject cardObj = originBase.card;
            GameObject pref = Resources.Load<GameObject>("Prefabs/Cards/prop_carrot");
            GameObject newCard = LoadCard(cardObj, pref);
            _normalCardBaseList.Remove(originBase);
            originBase.RemoveFromList();
            newCard.AddComponent<CardShadowControl>();
            newCard.SetActive(false);
            CarrotCardBase carrotCardBase = new CarrotCardBase(newCard);
            _carrotCardBaseList.Add(carrotCardBase);
        }
    }

    private void CheckNoPress(List<NormalCardBase> noPressList)
    {
        Dictionary<CardBase, bool> cardPressStatus = new Dictionary<CardBase, bool>();
        foreach (var item in _normalCardBaseList)
        {
            if (item.cardArea == CardArea.table)
            {
                if (!IsPressByRabbit(item, cardPressStatus))
                {
                    noPressList.Add(item);
                }
            }
        }
    }

    private bool IsPressByRabbit(CardBase cardBase, Dictionary<CardBase, bool> cardPressStatus)
    {
        if (cardPressStatus.TryGetValue(cardBase, out bool isPressed))
        {
            return isPressed;
        }

        List<CardBase> topList = cardBase.topCardList;
        if (topList.Count == 0)
        {
            cardPressStatus[cardBase] = false;
            return false;
        }
        else
        {
            for (int i = 0; i < topList.Count; i++)
            {
                CardBase topCardBase = topList[i];
                if (topCardBase.GetType() == typeof(RabbitCardBase))
                {
                    cardPressStatus[cardBase] = true;
                    return true;
                }
                else
                {
                    if (IsPressByRabbit(topCardBase, cardPressStatus))
                    {
                        cardPressStatus[cardBase] = true;
                        return true;
                    }
                }
            }
            cardPressStatus[cardBase] = false;
            return false;
        }
    }

    private void AdjustCardParent()
    {
        GameObject _tableObj = _levelObj.transform.Find("Table").gameObject;
        GameObject _handObj = _levelObj.transform.Find("Hand").gameObject;
        CardBase.SetParent(_levelObj);
        Linc.Game.DestroyObject(_tableObj);
        Linc.Game.DestroyObject(_handObj);
    }

    private void DealCard()
    {
        CardBase.Init();
        int tableCardDropFrame = TableCardDrop();
        //桌面落下结束后手牌升起
        int handCardUpperFrame = HandCardUpper(tableCardDropFrame - 30);
        //手牌升起后动物牌出现
        int aniCardAppearFrame = RabbitCardAppear(handCardUpperFrame);
        //手牌升起后同时翻开最上层桌面牌
        int turnOverTableTopCardFrame = TurnOverTableTopCard(handCardUpperFrame);
        //翻起最上层桌面牌后萝卜牌翻开
        int carrotCardAppearFrame = CarrotCardAppear(aniCardAppearFrame);
        //萝卜牌旋转效果
        int carrotCardRotFrame = CarrotCardRot(carrotCardAppearFrame);
        //萝卜牌翻开后翻开一张手牌
        int turnOverOneHandCardFrame = TurnOverOneHandCard(carrotCardRotFrame);
    }

    private int TableCardDrop()//桌面牌落下
    {
        List<CardBase> combineList = new List<CardBase>();
        foreach (var item in _normalCardBaseList) { if (item.cardArea == CardArea.table) combineList.Add(item); };
        combineList = combineList.Union(_addCardBaseList).ToList<CardBase>();
        return TableCardDropTween.CardDrop(combineList);
    }

    private int RabbitCardAppear(int beginFrame)
    {
        return RabbitCardAppearTween.CardAppear(beginFrame, _rabbitCardBaseList);
    }

    private int HandCardUpper(int beginFrame)
    {
        List<NormalCardBase> list = new List<NormalCardBase>();
        foreach (var item in _normalCardBaseList) { if (item.cardArea == CardArea.hand) list.Add(item); };
        return HandCardAppearTween.CardAppear(beginFrame, list);
    }

    private int CarrotCardRot(int beginFrame)
    {
        return CarrotCardRotTween.CardRot(beginFrame, _carrotCardBaseList);
    }

    private int CarrotCardAppear(int beginFrame)
    {
        return CarrotCardAppearTween.CardAppear(beginFrame, _carrotCardBaseList);
    }

    private int TurnOverTableTopCard(int beginFrame)
    {
        List<NormalCardBase> topCardList = new List<NormalCardBase>();
        foreach (var item in _normalCardBaseList) { if (item.cardArea == CardArea.table && item.isTop) topCardList.Add(item); };
        return TurnOverTopCardTween.TurnOverCard(beginFrame, topCardList);
    }

    private int TurnOverOneHandCard(int beginFrame)
    {
        List<NormalCardBase> list = new List<NormalCardBase>();
        foreach (var item in _normalCardBaseList) { if (item.cardArea == CardArea.hand) list.Add(item); };
        NormalCardBase firstCard = list[list.Count - 1];
        return TurnOverOneHandCardTween.TurnOverCard(beginFrame + 20, firstCard, () =>
        {
            CardBase.Init();
            GameCardList gameCardList = new GameCardList();
            gameCardList.normalCardBaseList = _normalCardBaseList;
            gameCardList.rabbitCardBaseList = _rabbitCardBaseList;
            gameCardList.carrotCardBaseList = _carrotCardBaseList;
            gameCardList.addCardBaseList = _addCardBaseList;
            gameCardList.currentCardBaseList.Add(firstCard);
            gameCardList.normalCardBaseList.Remove(firstCard);
            Linc.ProcedureMgr.ChangeToProcedure<Procedure_InGame>(gameCardList);
        });
    }
}
