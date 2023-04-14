using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRotTweenModel
{
    private const int turnTime = 1;//旋转数

    public static void Tween(CardBase cardBase, int tweenFrame)
    {
        Linc.Timer.ClearByObj(cardBase.card);
        //位置
        float upperY = Random.Range(4f, 6f);
        Vector3 beginPos = cardBase.card.transform.position;
        Vector3 aimPos = cardBase.focusPos;
        Vector3 cetPos = new Vector3(beginPos.y > aimPos.y ? aimPos.x : beginPos.x, beginPos.y > aimPos.y ? beginPos.y + upperY : aimPos.y + upperY, beginPos.z);
        beginPos.z = cetPos.z = aimPos.z = CardCfg.cardTurnZ;
        cardBase.card.transform.position = beginPos;
        //旋转
        Vector3 beginRot = cardBase.card.transform.eulerAngles;
        int rotAdd = turnTime * 360;
        rotAdd = beginPos.x < aimPos.x ? rotAdd : -rotAdd;
        Vector3 aimRot = Linc.Tween.CalcTweenEuAngles(beginRot, cardBase.focusRot);
        aimRot.z += rotAdd;
        //缓动
        Linc.EaseDic dic = new Linc.EaseDic();
        dic.float_1 = 0;
        dic.Add("float_1", 1);
        Linc.Tween.FrameTo(cardBase.card, dic, dic, tweenFrame, Linc.EaseFun.linearIn, () =>
        {
            cardBase.card.transform.position = cardBase.focusPos;
        }, () =>
        {
            cardBase.card.transform.position = Linc.BezierUtil.Bezier(beginPos, cetPos, aimPos, dic.float_1);
        });
        //旋转缓动
        Linc.EaseDic dic2 = new Linc.EaseDic();
        dic2.Add("eulerAngles", aimRot);
        Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic2, tweenFrame, Linc.EaseFun.linearIn);
    }
}
