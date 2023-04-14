using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private void HandCardClick(NormalCardBase cardBase)
        {
            new HandCardClickUndo(cardBase);
            Linc.Timer.ClearByObj(cardBase.card);
            //位置计算
            Vector3 pos = cardBase.card.transform.position;
            pos.z = CardCfg.cardTurnZ;
            cardBase.card.transform.position = pos;
            Vector3 topPos = _procedure_InGame.currentTopCardBase.focusPos;
            Vector3 aimPos = new Vector3(topPos.x, topPos.y, topPos.z - CardCfg.cardPosOffsetZ);
            Vector3 cetPos = new Vector3((pos.x + aimPos.x) * 0.5f, pos.y, pos.z);
            Vector3 cetRot = Linc.Tween.CalcTweenEuAngles(cardBase.card.transform.eulerAngles, new Vector3(-45, -135, 0));
            Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(cetRot, new Vector3(0, 180, 0));
            cetPos.y += CardCfg.handCardTurnOverUpOffsetY;
            //设置一个计划位置
            cardBase.SetFocusPos(aimPos);
            cardBase.SetFocusRot(aimRot);
            _procedure_InGame._gameCardList.normalCardBaseList.Remove(cardBase);
            _procedure_InGame._gameCardList.currentCardBaseList.Add(cardBase);
            //缓动
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("position", cetPos);
            dic.Add("eulerAngles", cetRot);
            Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic, CardCfg.handGameCardTurnOverFrame, Linc.EaseFun.linearIn, () =>
            {
                dic.Clear();
                dic.Add("position", aimPos);
                dic.Add("eulerAngles", aimRot);
                Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic, CardCfg.handGameCardTurnOverFrame, Linc.EaseFun.linearIn);
            });
            //全部手牌移动
            MoveHandCard();
        }
    }
}
