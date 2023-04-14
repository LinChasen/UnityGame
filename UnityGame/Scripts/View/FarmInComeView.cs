using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmInComeView : Linc.SpriteBehaviour
{
    private Vector2 _coinFlyPos = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        LoadView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCoinFlyPos(Vector2 pos)
    {
        _coinFlyPos = pos;
    }

    private void LoadView()
    {
        //计算农场收益
        int farmCoinNum = 2000 + 150 * farmLevel;
        //zone
        Linc.Image2D zone = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("zone").gameObject) as Linc.Image2D;
        //金币数值
        Linc.Text2D coinNum = Linc.Sprite2D.CreateSpriteFromObj(zone.gameObject.transform.Find("coinZone").Find("coinNum").gameObject) as Linc.Text2D;
        coinNum.text = "x" + farmCoinNum;
        //光圈
        Linc.Image2D shine = Linc.Sprite2D.CreateSpriteFromObj(zone.gameObject.transform.Find("coinZone").Find("shine").gameObject) as Linc.Image2D;
        //领取按钮
        Linc.Image2D getBtn = Linc.Sprite2D.CreateSpriteFromObj(zone.gameObject.transform.Find("coinZone").Find("getBtn").gameObject) as Linc.Image2D;
        //退出按钮
        Linc.Image2D exitBtn = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("closeBtn").gameObject) as Linc.Image2D;
        //监听
        exitBtn.AddClickFun(() => { Linc.ViewMgr.CloseView(ViewType.FarmInComeView); });
        getBtn.AddClickFun(() => { OnGetBtnClick(farmCoinNum, getBtn); });
        //初始化
        //光圈旋转
        Linc.Timer.FrameLoop(1, gameObject, (object obj) => { shine.rotation -= 1; });
    }

    private void OnGetBtnClick(int coinNum, Linc.Image2D getBtn)
    {
        const int maxRange = 100;
        int rndCoinNum = Random.Range(10, 15);
        for (int i = 0; i < rndCoinNum; i++)
        {
            Linc.Image2D coinSp = new Linc.Image2D();
            coinSp.LoadImage("Assets/Resources/GameWinView/coin.png");
            coinSp.MiddlePivot();
            Linc.Stage.AddChild(coinSp);
            Vector2 pos = getBtn.globalPos;
            int rndX = Random.Range(-maxRange, maxRange);
            int rndY = Random.Range(-maxRange, maxRange);
            pos.x += rndX;
            pos.y += rndY;
            coinSp.posVec = pos;
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("posVec", _coinFlyPos);
            Linc.Tween.FrameTo(null, coinSp, dic, 30, Linc.EaseFun.linearIn,()=>{coinSp.Destroy();});
        }
        Linc.Timer.FrameOnce(30,null,(object obj)=>{AllGlobal.storage.coinNum+=coinNum;});
        AllGlobal.storage.harvestTime = Linc.Timer.currentTime + GameCfg.harvestTime * 60 * 1000;
        Linc.ViewMgr.CloseView(ViewType.FarmInComeView);
    }

    private int farmLevel
    {
        get
        {
            int level = 0;
            for (int i = 0; i < GameCfg.farmAreaUnlockLevel.Length; i++)
            {
                bool isUnlock = AllGlobal.storage.level > GameCfg.farmAreaUnlockLevel[i];
                if (isUnlock) { level += 1; };
            }
            return level;
        }
    }
}
