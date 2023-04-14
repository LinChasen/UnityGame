using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private const int addCardTurnFrame = 10;
        private const int addNewCardTurnFrame=40;
        private void TurnOverTopAddCard(TableCardClickUndo rootUndo)
        {
            var addCardBaseList = _procedure_InGame._gameCardList.addCardBaseList;
            for (int i = 0; i < addCardBaseList.Count; i++)
            {
                var addCardBase = addCardBaseList[i];
                if (addCardBase.isTop)
                {
                    _procedure_InGame._inputLock.LockInput();//禁止输入
                    AddCardUndo addUndo=new AddCardUndo(addCardBase);
                    //位置计算
                    Vector3 pos = addCardBase.card.transform.position;
                    pos.z = CardCfg.cardTurnZ + CardCfg.cardPosOffsetZ;
                    addCardBase.card.transform.position = pos;
                    Vector3 aimScale = new Vector3(1.3f, 1.3f, 1.3f);
                    Vector3 aimPos = new Vector3(0, 0, pos.z);
                    Vector3 cetPos = new Vector3((pos.x + aimPos.x) * 0.5f, aimPos.y, pos.z);
                    Vector3 cetRot = Linc.Tween.CalcTweenEuAngles(addCardBase.card.transform.eulerAngles, new Vector3(-45, 45, 0));
                    Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(cetRot, new Vector3(0, 0, 0));
                    //缓动
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.Add("eulerAngles", cetRot);
                    Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, dic, addCardTurnFrame, Linc.EaseFun.linearIn, () =>
                    {
                        dic.Clear();
                        dic.Add("eulerAngles", aimRot);
                        Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, dic, addCardTurnFrame, Linc.EaseFun.linearIn);
                    });
                    Linc.EaseDic posScaleDic = new Linc.EaseDic();
                    posScaleDic.Add("position", aimPos);
                    posScaleDic.Add("localScale", aimScale);
                    Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, posScaleDic, addCardTurnFrame * 2, Linc.EaseFun.linearIn, () =>
                    {
                        Linc.EaseDic scaleDic = new Linc.EaseDic();
                        var handList = _procedure_InGame.handNormalCardBaseList;
                        Vector3 aimHandPos = handList[handList.Count - 1].focusPos;
                        AddCardDealCard(addCardBase, 0, addCardBase.addNum, scaleDic, aimHandPos, rootUndo,addUndo);
                    });
                }
            }
        }

        private void AddCardDealCard(CardBase addCardBase, int dealNum, int maxNum, Linc.EaseDic scaleDic, Vector3 aimHandPos, TableCardClickUndo rootUndo,AddCardUndo addUndo)
        {
            if (dealNum < maxNum)
            {
                dealNum += 1;
                scaleDic.Clear();
                scaleDic.Add("localScale", new Vector3(1.5f, 1.5f, 1.5f));
                Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, scaleDic, 7, Linc.EaseFun.linearIn, () =>
                {
                    //生成一张卡牌
                    aimHandPos.z -= CardCfg.cardPosOffsetZ;
                    CreateNewHandCard(aimHandPos,addUndo);
                    scaleDic.Clear();
                    scaleDic.Add("localScale", new Vector3(1.3f, 1.3f, 1.3f));
                    Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, scaleDic, 7, Linc.EaseFun.linearIn, () =>
                    {
                        AddCardDealCard(addCardBase, dealNum, maxNum, scaleDic, aimHandPos, rootUndo,addUndo);
                    });
                });
            }
            else
            {
                scaleDic.Clear();
                Vector3 curRot = addCardBase.card.transform.eulerAngles;
                curRot.z += 360;
                scaleDic.Add("localScale", new Vector3(0.5f, 0.5f, 0.5f));
                scaleDic.Add("eulerAngles", curRot);
                Linc.Tween.FrameTo(addCardBase.card, addCardBase.card.transform, scaleDic, 40, Linc.EaseFun.linearIn, () =>
                {
                    addCardBase.card.SetActive(false);
                    _procedure_InGame._inputLock.UnlockInput();
                    rootUndo.AddChUndo(addUndo);
                    TurnOverTopAddCard(rootUndo);
                });
            }
        }

        private void CreateNewHandCard(Vector3 aimHandPos,AddCardUndo addUndo)
        {
            GameObject newCard = GameObject.Instantiate(CardPool.rndCardPref, _procedure_InGame._levelObj.transform);
            NormalCardBase cardBase = new NormalCardBase(newCard);
            addUndo.AddNewCard(cardBase);
            cardBase.SetCardArea(CardArea.hand);
            newCard.transform.position = new Vector3(0, 0, CardCfg.cardTurnZ);
            //位置计算
            Vector3 beginPos = cardBase.card.transform.position;
            Vector3 aimPos = aimHandPos;
            Vector3 cetPos = new Vector3(aimPos.x, beginPos.y + Random.Range(4f, 5f), beginPos.z);
            Vector3 beginRot = cardBase.card.transform.eulerAngles;
            float turnRot = Random.Range(1, 1) * 360;//旋转周数后面可调
            turnRot = beginPos.x < aimPos.x ? -turnRot : turnRot;
            Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(beginRot, beginRot);
            aimRot.z += turnRot;
            cardBase.SetFocusPos(aimPos);
            cardBase.SetFocusRot(aimRot);
            //位置缓动
            beginPos.z = cetPos.z = aimPos.z = CardCfg.cardTurnZ;
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.float_1 = 0;
            dic.Add("float_1", 1);
            Linc.Tween.FrameTo(cardBase.card, dic, dic, addNewCardTurnFrame, Linc.EaseFun.linearIn, () =>
            {
                cardBase.card.transform.position = cardBase.focusPos;
                _procedure_InGame._gameCardList.normalCardBaseList.Add(cardBase);
            }, () =>
            {
                cardBase.card.transform.position = Linc.BezierUtil.Bezier(beginPos, cetPos, aimPos, dic.float_1);
            });
            Linc.Timer.FrameOnce(addNewCardTurnFrame - handCardMoveFrame - 1, cardBase.card, (object obj) =>
            {
                MoveHandCard(false);
            });
            //旋转缓动
            Linc.EaseDic dic2 = new Linc.EaseDic();
            dic2.Add("eulerAngles", aimRot);
            Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic2, addNewCardTurnFrame, Linc.EaseFun.linearIn);
        }
    }
}

