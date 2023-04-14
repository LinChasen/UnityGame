using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class TurnOverOneHandCardTween
    {
        private const int turnOverFrame = 10;
        public static int TurnOverCard(int beginFrame, NormalCardBase cardBase, System.Action onComplete)
        {
            Linc.Timer.FrameOnce(beginFrame, cardBase.card, (object obj) =>
               {
                   Vector3 pos = cardBase.card.transform.position;
                   pos.z = CardCfg.cardTurnZ;
                   cardBase.card.transform.position = pos;
                   Vector3 aimPos = new Vector3(CardCfg.firstHandCardPosX, pos.y, CardCfg.cardBasePosZ);
                   Vector3 cetPos = new Vector3((pos.x + aimPos.x) * 0.5f, pos.y, pos.z);
                   Vector3 cetRot = Linc.Tween.CalcTweenEuAngles(cardBase.card.transform.eulerAngles, new Vector3(-45, -135, 0));
                   Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(cetRot, new Vector3(0, 180, 0));
                   cetPos.y += CardCfg.handCardTurnOverUpOffsetY;
                   Linc.EaseDic dic = new Linc.EaseDic();
                   dic.Add("position", cetPos);
                   dic.Add("eulerAngles", cetRot);
                   Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic, turnOverFrame, Linc.EaseFun.linearIn, () =>
                   {
                       dic.Clear();
                       dic.Add("position", aimPos);
                       dic.Add("eulerAngles", aimRot);
                       Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic, turnOverFrame, Linc.EaseFun.linearIn, () =>
                       {
                           onComplete();
                       });
                   });
               });
            return beginFrame + turnOverFrame * 2;
        }
    }
}
