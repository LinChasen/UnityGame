using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : Linc.SpriteBehaviour
{
    private Linc.Text2D _farmCountDownTxt;
    private Linc.Text2D _coinNum;
    private Linc.Image2D _farmShine;
    private Linc.Image2D _farmShineRect;
    private Linc.Image2D _farmIcon;
    private bool _canHarvest = true;

    // Start is called before the first frame update
    void Start()
    {
        LoadView();
    }

    // Update is called once per frame
    void Update()
    {
        FreshCountDownTxt();
    }

    private void LoadView()
    {
        //金币数
        _coinNum = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("coinBg").Find("coinNum").gameObject) as Linc.Text2D;
        //农场收益按钮光圈
        _farmShine = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("shine").gameObject) as Linc.Image2D;
        //图标
        _farmIcon = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("farmIcon").gameObject) as Linc.Image2D;
        //农场收益按钮光圈四边形
        _farmShineRect = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("shineRect").gameObject) as Linc.Image2D;
        //农场收益倒计时
        _farmCountDownTxt = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("countDownTxt").gameObject) as Linc.Text2D;
        //监听
        _farmIcon.AddClickFun(OnHarvestBtnClick);
        //初始化
        _canHarvest=Linc.Timer.currentTime>=AllGlobal.storage.harvestTime;
        if(isCropUnlock){if(_canHarvest)CanHarvestFun();else CannotHarvestFun();};
        _farmIcon.visible=isCropUnlock;
    }

    private bool isCropUnlock{
        get{
            return AllGlobal.storage.level>GameCfg.farmAreaUnlockLevel[0];
        }
    }

    private void OnHarvestBtnClick()
    {
        Linc.ViewMgr.OpenView(ViewType.FarmInComeView).AddComponent<FarmInComeView>().SetCoinFlyPos(_coinNum.globalPos);
    }

    private void FreshCountDownTxt()
    {
        if(!isCropUnlock)return;
        string str = Linc.Timer.CountDownMinSecGetStr(AllGlobal.storage.harvestTime);
        _farmCountDownTxt.text = str;
        bool timeOk=Linc.Timer.currentTime>=AllGlobal.storage.harvestTime;
        if (timeOk && !_canHarvest)//时间到了
        {
            CanHarvestFun();
        }
        else if (!timeOk && _canHarvest)//时间没到
        {
            CannotHarvestFun();
        }
    }

    private void CannotHarvestFun()
    {
        _farmIcon.visible=true;
        _farmCountDownTxt.visible = true;
        _farmShine.visible = _farmShineRect.visible = false;
        Linc.Timer.ClearByObj(gameObject);
        _farmIcon.scale(1, 1);
        _farmShineRect.scale(1, 1);
        _farmIcon.mouseEnable=false;
        _canHarvest=false;
    }

    private void CanHarvestFun()
    {
        _farmIcon.visible=true;
        _farmCountDownTxt.visible = false;
        _farmShine.visible = _farmShineRect.visible = true;
        //呼吸效果和光圈旋转效果
        Linc.Timer.ClearByObj(gameObject);
        Linc.Timer.FrameLoop(1, gameObject, (object obj) =>
        {
            _farmShine.rotation -= 1;
        });
        BtnTween(_farmIcon,new Linc.EaseDic());
        BtnTween(_farmShineRect,new Linc.EaseDic());
        _farmIcon.mouseEnable=true;
        _canHarvest=true;
    }

    private void BtnTween(Linc.Image2D btn,Linc.EaseDic tweenDic)
    {
        tweenDic.Clear();
        tweenDic.Add("scaleVec", new Vector2(1.2f, 1.2f));
        Linc.Tween.FrameTo(gameObject, btn, tweenDic, 20, Linc.EaseFun.linearIn, () =>
        {
            tweenDic.Clear();
            tweenDic.Add("scaleVec", new Vector2(1, 1));
            Linc.Tween.FrameTo(gameObject, btn, tweenDic, 20, Linc.EaseFun.linearIn, () =>
            {
                BtnTween(btn,tweenDic);
            });
        });
    }
}
