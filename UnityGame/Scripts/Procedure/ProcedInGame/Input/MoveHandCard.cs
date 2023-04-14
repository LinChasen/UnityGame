using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private int handCardMoveFrame=10;

        public void MoveHandCard(bool isMoveRight = true,int cardNum=1)
        {
            var handList = _procedure_InGame.handNormalCardBaseList;
            for (int i = 0; i < handList.Count; i++)
            {
                var handBase = handList[i];
                Linc.Timer.ClearByObj(handBase.card);
                Vector3 handBeginPos = handBase.focusPos;
                float offsetValue = isMoveRight ? CardCfg.handCardOffsetX*cardNum : -CardCfg.handCardOffsetX*cardNum;
                Vector3 handAimPos = new Vector3(handBeginPos.x + offsetValue, handBeginPos.y, handBeginPos.z);
                Vector3 curRot = handBase.card.transform.eulerAngles;
                Vector3 aimRot = handBase.focusRot;
                aimRot = Linc.Tween.CalcTweenEuAngles(curRot, aimRot);
                handBase.SetFocusPos(handAimPos);
                Linc.EaseDic handDic = new Linc.EaseDic();
                handDic.Add("position", handAimPos);
                handDic.Add("eulerAngles", aimRot);
                Linc.Tween.FrameTo(handBase.card, handBase.card.transform, handDic, handCardMoveFrame, Linc.EaseFun.linearIn);
            }
        }
    }
}
