using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private const int tableCardClickFlyFrame=40;
        private void TableCardClick(NormalCardBase cardBase)
        {
            TableCardClickUndo rootUndo = new TableCardClickUndo(cardBase);//创建撤回类
            Linc.Timer.ClearByObj(cardBase.card);
            //位置计算
            cardBase.SetFocusPos(_procedure_InGame.nextCurrentTopPos);
            cardBase.SetFocusRot(new Vector3(0, 180, 0));
            _procedure_InGame._gameCardList.currentCardBaseList.Add(cardBase);
            _procedure_InGame._gameCardList.normalCardBaseList.Remove(cardBase);
            //缓动
            BezierRotTweenModel.Tween(cardBase,tableCardClickFlyFrame);
            //翻起最上层卡牌
            TurnOverTopTableCard(rootUndo);
        }

    }
}
