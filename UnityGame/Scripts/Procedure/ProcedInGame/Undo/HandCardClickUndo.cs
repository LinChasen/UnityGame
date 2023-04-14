using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private class HandCardClickUndo : UndoBase
    {
        private Vector3 _beginPos = new Vector3();
        private NormalCardBase _cardBase;

        public HandCardClickUndo(NormalCardBase cardBase) : base(true)
        {
            _cardBase = cardBase;
            _beginPos = cardBase.focusPos;
        }

        protected override void UndoStep()
        {
            Vector3 cetRot = new Vector3(-45, 225, 0);
            Vector3 aimRot = new Vector3(360, 360, 0);
            Vector3 pos = _cardBase.card.transform.position;
            pos.z = CardCfg.cardTurnZ;
            _cardBase.card.transform.position = pos;
            Vector3 aimPos = _beginPos;
            Vector3 cetPos = new Vector3((pos.x + aimPos.x) * 0.5f, pos.y, pos.z);
            cetPos.y += CardCfg.handCardTurnOverUpOffsetY;
            _cardBase.SetFocusPos(_beginPos);
            _cardBase.SetFocusRot(aimRot);
            Linc.Timer.ClearByObj(_cardBase.card);
            //全部手牌移动
            MoveHandCard();
            _procedure_InGame._gameCardList.normalCardBaseList.Add(_cardBase);
            _procedure_InGame._gameCardList.currentCardBaseList.Remove(_cardBase);
            //缓动
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("position", cetPos);
            dic.Add("eulerAngles", cetRot);
            Linc.Tween.FrameTo(_cardBase.card, _cardBase.card.transform, dic, CardCfg.handGameCardTurnOverFrame, Linc.EaseFun.linearIn, () =>
            {
                dic.Clear();
                dic.Add("position", aimPos);
                dic.Add("eulerAngles", aimRot);
                Linc.Tween.FrameTo(_cardBase.card, _cardBase.card.transform, dic, CardCfg.handGameCardTurnOverFrame, Linc.EaseFun.linearIn);
            });
        }

        private void MoveHandCard()
        {
            var handList = _procedure_InGame.handNormalCardBaseList;
            for (int i = 0; i < handList.Count; i++)
            {
                var handBase = handList[i];
                Linc.Timer.ClearByObj(handBase.card);
                Vector3 handBeginPos = handBase.focusPos;
                Vector3 handAimPos = new Vector3(handBeginPos.x - CardCfg.handCardOffsetX, handBeginPos.y, handBeginPos.z);
                Vector3 curRot = handBase.card.transform.eulerAngles;
                Vector3 aimRot = handBase.focusRot;
                aimRot = Linc.Tween.CalcTweenEuAngles(curRot, aimRot);
                handBase.SetFocusPos(handAimPos);
                Linc.EaseDic handDic = new Linc.EaseDic();
                handDic.Add("position", handAimPos);
                handDic.Add("eulerAngles", aimRot);
                Linc.Tween.FrameTo(handBase.card, handBase.card.transform, handDic, CardCfg.handGameCardTurnOverFrame, Linc.EaseFun.linearIn);
            }
        }
    }
}
