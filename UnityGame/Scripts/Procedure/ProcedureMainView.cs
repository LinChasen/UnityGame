using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureMainView : Linc.ProcedureBase
{
    private readonly Vector3 camPos=new Vector3(7,4,-10);
    private readonly Vector3 camEu=new Vector3(25,0,0);
    private readonly float camField=75;
    private LevelMgr _levelMgr;
    protected override void OnInit()
    {
        Camera.main.transform.position=camPos;
        Camera.main.transform.eulerAngles=camEu;
        Camera.main.fieldOfView=camField;
    }

    protected override void OnEnter(object data)
    {
        //加载场景
        GameObject scenePref=Resources.Load<GameObject>("ScenePrefab/MainLevelScene");
        GameObject sceneObj=Linc.Game.InstantiateObject(scenePref).transform.Find("root").gameObject;
        _levelMgr=new LevelMgr(sceneObj);
        Linc.Stage.AddViewCom<MainViewInput>().SetLevelMgr(_levelMgr);
        Linc.ViewMgr.OpenView(ViewType.MainView).AddComponent<MainView>();
    }

    protected override void OnUpdate()
    {
        _levelMgr.FreshCrops();
    }

    protected override void OnExit()
    {

    }
}
