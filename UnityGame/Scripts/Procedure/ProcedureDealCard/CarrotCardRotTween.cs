using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class CarrotCardRotTween
    {
        private const int xTurnFrame = 20;//x方向每次变换帧数
        private const int xTurnTimes = 2; //x方向变换次数
        private const float xTurnAngle = 120;//x方向变换角度
        private const int zRotRound = 2;//绕z轴旋转周数
        public static int CardRot(int beginFrame, List<CarrotCardBase> cardBaseList)
        {
            for (int i = 0; i < cardBaseList.Count; i++)
            {
                CarrotCardBase cardBase = cardBaseList[i];
                List<CardBase> topList = cardBase.topCardList;
                GameObject carrotCard = cardBase.card;
                Vector3 aimPos = cardBase.focusPos;
                Vector3 rot = carrotCard.transform.eulerAngles;
                Quaternion aimQua = carrotCard.transform.rotation;
                Quaternion beginQua = new Quaternion(0, 0, 0, 1);
                Vector3 beginV = new Vector3(0, 1, 0);
                Vector3 aimV = aimQua * beginV;
                aimV.z = 0;
                float angle = Vector3.Angle(beginV, aimV);
                Vector3 crossV = Vector3.Cross(beginV, aimV);
                angle += 360 * zRotRound;
                angle = crossV.z > 0 ? angle : -angle;
                float eachAngle = angle / (xTurnFrame * xTurnTimes);
                float eachXTurnAngle = xTurnAngle / xTurnFrame;
                float beginZ = aimPos.z;
                aimPos.z = CardCfg.cardTurnZ;
                Vector3 beginPos = carrotCard.transform.position;
                Vector3 cetPos = new Vector3(beginPos.x, aimPos.y + Random.Range(1f, 2f), CardCfg.cardTurnZ);
                Linc.Timer.FrameOnce(beginFrame, carrotCard, (object obj) =>
                {
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.float_1 = 0;
                    dic.float_2 = rot.z;
                    dic.Vector3_1 = carrotCard.transform.localScale;
                    float rotAdd = aimPos.x > 0 ? 360 * 2 : -360 * 2;
                    dic.Add("float_1", 1);
                    dic.Add("float_2", rot.z + rotAdd);
                    dic.Add("Vector3_1", Vector3.one);
                    int curTurnFrame = 0;
                    Linc.Tween.FrameTo(carrotCard, dic, dic, xTurnFrame * xTurnTimes, Linc.EaseFun.linearIn, null, () =>
                    {
                        carrotCard.transform.position = Linc.BezierUtil.Bezier(beginPos, cetPos, aimPos, dic.float_1);
                        carrotCard.transform.localScale = dic.Vector3_1;
                        if (CardBase.IsInterSectWithCardList(cardBase, topList))
                        {
                            Vector3 curPos = carrotCard.transform.position;
                            curPos.z = beginZ;
                            carrotCard.transform.position = curPos;
                            beginPos.z = cetPos.z = aimPos.z = beginZ;
                        }
                        carrotCard.transform.Rotate(new Vector3(0, 0, eachAngle), Space.World);
                        if (curTurnFrame < xTurnFrame)
                        {
                            curTurnFrame += 1;
                            carrotCard.transform.Rotate(new Vector3(0, eachXTurnAngle, 0), Space.Self);
                        }
                        else
                        {
                            curTurnFrame = 1;
                            eachXTurnAngle *= -1;
                            carrotCard.transform.Rotate(new Vector3(0, eachXTurnAngle, 0), Space.Self);
                        }
                    });
                });
            }
            int frameNum = cardBaseList.Count > 0 ? xTurnFrame * xTurnTimes : 0;
            return beginFrame + frameNum;
        }
    }
}
