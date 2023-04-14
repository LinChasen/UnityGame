using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelView : Linc.SpriteBehaviour
{
    private int _selectLevel;
    private LevelDifficulity _difficulity = LevelDifficulity.simple;

    // Start is called before the first frame update
    void Start()
    {
        LoadView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevel(int level)
    {
        _selectLevel = level;
    }

    private void LoadView()
    {
        string[] difficulityDetail = new string[] { "简单", "奖金x2", "奖金x4", "奖金x8" };
        GameObject zoneObj = gameObject.transform.Find("zone").gameObject;
        //zone入场效果
        Linc.Image2D zone = Linc.Sprite2D.CreateSpriteFromObj(zoneObj) as Linc.Image2D;
        zone.x = Linc.Stage.width + zone.width / 2;
        Linc.EaseDic dic = new Linc.EaseDic();
        dic.Add("x", Linc.Stage.width / 2);
        Linc.Tween.FrameTo(zone.gameObject, zone, dic, 30, Linc.EaseFun.backOut);
        //关卡
        Linc.Text2D levelTxt = Linc.Sprite2D.CreateSpriteFromObj(zoneObj.transform.Find("levelTxt").gameObject) as Linc.Text2D;
        levelTxt.text = _selectLevel.ToString();
        //难度
        Linc.Text2D difficulityTxt = Linc.Sprite2D.CreateSpriteFromObj(zoneObj.transform.Find("difficulityTag").GetChild(0).gameObject) as Linc.Text2D;
        //退出按钮
        Linc.Image2D exitBtn = Linc.Sprite2D.CreateSpriteFromObj(zoneObj.transform.Find("exitBtn").gameObject) as Linc.Image2D;
        //开始按钮
        Linc.Image2D beginBtn = Linc.Sprite2D.CreateSpriteFromObj(zoneObj.transform.Find("beginBtn").gameObject) as Linc.Image2D;
        //难度+按钮
        Linc.Image2D difficulityAddBtn = Linc.Image2D.CreateSpriteFromObj(zoneObj.transform.Find("add").gameObject) as Linc.Image2D;
        //难度-按钮
        Linc.Image2D difficulitySubBtn = Linc.Image2D.CreateSpriteFromObj(zoneObj.transform.Find("sub").gameObject) as Linc.Image2D;
        //添加监听
        exitBtn.AddClickFun(() =>
        {
            GameObject maskObj=gameObject.transform.Find("mask").gameObject;
            maskObj.SetActive(false);
            dic.Clear();
            dic.Add("x", Linc.Stage.width + zone.width / 2);
            Linc.Tween.FrameTo(zone.gameObject, zone, dic, 30, Linc.EaseFun.backInOut, () =>
            {
                Linc.ViewMgr.CloseView(ViewType.LevelView);
            });
        });
        difficulityAddBtn.AddClickFun(() =>
        {
            difficulitySubBtn.mouseEnable = true;
            difficulitySubBtn.LoadImage("Assets/Resources/LevelView/sub.png");
            _difficulity = (LevelDifficulity)((int)_difficulity + 1);
            difficulityTxt.text = difficulityDetail[(int)_difficulity];
            if ((int)_difficulity >= (int)LevelDifficulity.rewardx8) { difficulityAddBtn.LoadImage("Assets/Resources/LevelView/add0.png"); difficulityAddBtn.mouseEnable = false; };
        });
        difficulitySubBtn.AddClickFun(() =>
        {
            difficulityAddBtn.mouseEnable = true;
            difficulityAddBtn.LoadImage("Assets/Resources/LevelView/add.png");
            _difficulity = (LevelDifficulity)((int)_difficulity - 1);
            difficulityTxt.text = difficulityDetail[(int)_difficulity];
            if ((int)_difficulity <= (int)LevelDifficulity.simple) { difficulitySubBtn.LoadImage("Assets/Resources/LevelView/sub0.png"); difficulitySubBtn.mouseEnable = false; };
        });
        beginBtn.AddClickFun(()=>{
            Linc.Main.ClearGame();
            Linc.ProcedureMgr.ChangeToProcedure<ProcedureDealCard>(_selectLevel);
        });
        //初始化
        difficulitySubBtn.mouseEnable = false;
    }
}
