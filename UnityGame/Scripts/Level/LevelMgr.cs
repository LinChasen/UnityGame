using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelMgr
{
    private const float sceneObjBeginZ = -8.4f;
    private readonly Vector3 alpacaIdleRot = new Vector3(0, 120, 0);
    private readonly Vector3 alpacaLocalPos = new Vector3(0, 0.6f, 0);
    private GameObject _sceneObj;
    private GameObject _alpaca;//羊驼
    private Animator _alpacaAni;
    private List<LevelData> _levelDataList = new List<LevelData>();
    private LevelData _curSelectLevelData;
    private List<GameObject> _farmAreaList = new List<GameObject>();
    private int _curHarvestStep = 3;

    public LevelMgr(GameObject inSceneObj)
    {
        _sceneObj = inSceneObj;
        //生成关卡基座类
        InitBase();
        //羊驼
        InitAlpaca();
        //农场
        InitFarmArea();
        //刷新庄稼
        FreshCrops();
    }

    private void InitBase()
    {
        int index = 1;
        for (int i = 0; i < _sceneObj.transform.childCount; i++)
        {
            GameObject pointsObj = _sceneObj.transform.GetChild(i).Find("Point").gameObject;
            for (int j = 0; j < pointsObj.transform.childCount; j++)
            {
                GameObject baseObj = pointsObj.transform.GetChild(j).gameObject;
                LevelData data = new LevelData(baseObj, index);
                _levelDataList.Add(data);
                index += 1;
            }
        }
        //调整地块位置
        _curSelectLevelData = _levelDataList[AllGlobal.storage.level - 1];
        float subZ = _curSelectLevelData.baseObj.transform.position.z - sceneObjBeginZ - 2;
        float subX = _curSelectLevelData.baseObj.transform.position.x - 7;
        Vector3 pos = _sceneObj.transform.position;
        pos.z -= subZ;
        pos.x -= subX;
        _sceneObj.transform.position = pos;
    }

    private void InitAlpaca()
    {
        GameObject curBaseObj = _curSelectLevelData.baseObj;
        GameObject alpacaPref = Resources.Load<GameObject>("Prefabs/Alpaca");
        _alpaca = GameObject.Instantiate(alpacaPref, curBaseObj.transform);
        _alpacaAni = _alpaca.GetComponent<Animator>();
        _alpaca.transform.localPosition = alpacaLocalPos;
        _alpaca.transform.localEulerAngles = alpacaIdleRot;
    }

    private void InitFarmArea()
    {
        for (int i = 0; i < _sceneObj.transform.childCount; i++)
        {
            GameObject vegetablePr = _sceneObj.transform.GetChild(i).Find("Vegetable").gameObject;
            for (int j = 0; j < vegetablePr.transform.childCount; j++)
            {
                _farmAreaList.Add(vegetablePr.transform.GetChild(j).gameObject);
            }
        }
    }

    public void FreshCrops()
    {
        long subTime = AllGlobal.storage.harvestTime - Linc.Timer.currentTime;
        int harvestStep = subTime < 0 ? 3 : subTime < GameCfg.harvestTime * 60 * 1000 / 2 ? 2 : 1;
        for (int i = 0; i < _farmAreaList.Count; i++)
        {
            bool isUnlock = AllGlobal.storage.level > GameCfg.farmAreaUnlockLevel[i];
            GameObject vPr = _farmAreaList[i];
            if (!isUnlock)
            {
                Linc.GameObjectUtil.DestroyChildren(vPr);
            }
            else
            {
                if (_curHarvestStep != harvestStep)
                {
                    for (int j = 0; j < vPr.transform.childCount; j++)
                    {
                        GameObject crop = vPr.transform.GetChild(j).gameObject;
                        string cropName = crop.name;
                        cropName = Regex.Replace(cropName, @"\(.*?\)", "");
                        cropName = cropName.Replace(" ", "");
                        if (cropName.IndexOf("Tree") == -1)//普通作物
                        {
                            string baseName = Regex.Replace(cropName, @"(\d)", "");
                            Vector3 pos = crop.transform.position;
                            Quaternion qua = crop.transform.rotation;
                            int index = crop.transform.GetSiblingIndex();
                            crop.transform.SetParent(null);
                            Linc.Game.DestroyObject(crop);
                            //新作物
                            GameObject cropPref = Resources.Load<GameObject>("Prefabs/Crops/" + baseName + harvestStep);
                            GameObject newCrop = GameObject.Instantiate(cropPref, vPr.transform);
                            newCrop.transform.position = pos;
                            newCrop.transform.rotation = qua;
                            newCrop.transform.SetSiblingIndex(index);
                        }
                        else
                        {//苹果树
                            GameObject fruitPr = crop.transform.Find("Fruits").gameObject;
                            fruitPr.SetActive(harvestStep == 3);
                        }
                    }
                }
            }
        }
        _curHarvestStep = harvestStep;
    }

    public LevelData curSelectLevelData
    {
        get
        {
            return _curSelectLevelData;
        }
    }

    public void MapMove(Vector3 moveVec)
    {
        Vector3 pos = _sceneObj.transform.position;
        pos += new Vector3(moveVec.x, 0, -moveVec.y);
        if (pos.z < -97) { pos.z = -97; };
        if (pos.z > sceneObjBeginZ) { pos.z = sceneObjBeginZ; };
        if (pos.x < -4) { pos.x = -4; };
        if (pos.x > 4) { pos.x = 4; };
        _sceneObj.transform.position = pos;
    }

    public void OnBaseSelect(GameObject baseObj, System.Action onFinish)
    {
        //地块移动 
        float subZ = baseObj.transform.position.z - sceneObjBeginZ - 2;
        float subX = baseObj.transform.position.x - 7;
        Vector3 pos = _sceneObj.transform.position;
        pos.z -= subZ;
        pos.x -= subX;
        Linc.EaseDic dic = new Linc.EaseDic();
        dic.Add("position", pos);
        Linc.Tween.FrameTo(_sceneObj, _sceneObj.transform, dic, 20, Linc.EaseFun.linearIn, () =>
        {
            AlpacaJumpMove(baseObj, onFinish);
        });
    }

    public bool IsBaseCanClick(GameObject baseObj)
    {
        int index = _levelDataList.FindIndex((v) => { return v.baseObj == baseObj; });
        return AllGlobal.storage.level > index;
    }

    private void AlpacaJumpMove(GameObject baseObj, System.Action onFinish)
    {
        System.Action moveEndFun = () =>
        {
            int index = _levelDataList.FindIndex((v) => { return v.baseObj == baseObj; });
            _curSelectLevelData = _levelDataList[index];
            _alpaca.transform.SetParent(baseObj.transform);
            _alpaca.transform.localPosition = alpacaLocalPos;
            Linc.ViewMgr.OpenView(ViewType.LevelView).AddComponent<LevelView>().SetLevel(index + 1);
            onFinish();
        };
        if(baseObj==_curSelectLevelData.baseObj){moveEndFun();return;}//点的当前地块，则羊驼不用跳了
        //羊驼移动旋转
        Vector3 beginPos = curSelectLevelData.baseObj.transform.position;
        Vector3 aimPos = baseObj.transform.position;
        float alpacaY = _alpaca.transform.position.y;
        beginPos.y = aimPos.y = alpacaY;
        Vector3 cetPos = (beginPos + aimPos) / 2;
        cetPos.y += 2;//羊驼跳跃高度
        Vector3 vec = aimPos - beginPos;
        Quaternion qua = Quaternion.FromToRotation(_alpaca.transform.forward, vec);
        Quaternion aimQua = qua * _alpaca.transform.rotation;
        Vector3 aimEu = aimQua.eulerAngles;
        aimEu = Linc.Tween.CalcTweenEuAngles(_alpaca.transform.eulerAngles, aimEu);
        Linc.EaseDic dic = new Linc.EaseDic();
        dic.float_1 = 0;
        dic.Add("float_1", 1);
        Linc.Tween.FrameTo(_alpaca, dic, dic, 30, Linc.EaseFun.linearIn, () =>
        {
            aimEu = Linc.Tween.CalcTweenEuAngles(_alpaca.transform.eulerAngles, alpacaIdleRot);
            dic.Clear();
            dic.Add("eulerAngles", aimEu);
            Linc.Tween.FrameTo(_alpaca, _alpaca.transform, dic, 10, Linc.EaseFun.linearIn, () =>
            {
                moveEndFun();
            });
        }, () =>
        {
            _alpaca.transform.position = Linc.BezierUtil.Bezier(beginPos, cetPos, aimPos, dic.float_1);
        });
        Linc.EaseDic dic2 = new Linc.EaseDic();
        dic2.Add("eulerAngles", aimEu);
        Linc.Tween.FrameTo(_alpaca, _alpaca.transform, dic2, 10, Linc.EaseFun.linearIn);
    }
}
