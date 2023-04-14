using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private const int topCardTurnOverFrame = 10;

        private void TurnOverTopNormalCard(TableCardClickUndo rootUndo)
        {
            var tableList = _procedure_InGame.tableNormalCardBaseList;
            for (int i = 0; i < tableList.Count; i++)
            {
                var tableCardBase = tableList[i];
                if (tableCardBase.isTop && !tableCardBase.cardIsFront)
                {
                    Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(tableCardBase.card.transform.eulerAngles, tableCardBase.turnEulerAngle);
                    tableCardBase.SetFocusRot(aimRot);
                    Linc.Timer.ClearByObj(tableCardBase.card);
                    Linc.EaseDic tableDic = new Linc.EaseDic();
                    tableDic.Add("position",tableCardBase.focusPos);
                    tableDic.Add("eulerAngles", aimRot);
                    //卡牌翻转
                    Linc.Tween.FrameTo(tableCardBase.card, tableCardBase.card.transform, tableDic, topCardTurnOverFrame, Linc.EaseFun.linearIn);
                }
            }
        }
    }
}
