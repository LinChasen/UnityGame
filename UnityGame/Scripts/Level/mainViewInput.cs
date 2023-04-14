using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainViewInput : Linc.ScBase2D
{
    private LevelMgr _levelMgr;
    private GameObject _curClickBaseObj;
    private const float mouseMoveSpeed = 0.01f;
    private Linc.Image2D _panel;
    private Vector3 _lastPoint = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        CreateView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevelMgr(LevelMgr inLevelMgr)
    {
        _levelMgr = inLevelMgr;
    }

    private void CreateView()
    {
        //panel
        _panel = new Linc.Image2D();
        _panel.width = Linc.Stage.width;
        _panel.height = Linc.Stage.height;
        pr.AddChild(_panel);
        _panel.alpha = 0;
        _panel.gameObject.transform.SetAsFirstSibling();
        //监听
        _panel.on(Linc.Event.mouseDown, OnPanelDown);
    }

    private void OnPanelDown()
    {
        _lastPoint = Linc.Stage.mousePos;
        _panel.on(Linc.Event.mouseMove, OnPanelMove);
        _panel.on(Linc.Event.mouseUp, OnPanelUp);
        _panel.on(Linc.Event.mouseOut, OnPanelUp);
        BaseObjClickDown();
    }

    private void OnPanelMove()
    {
        Vector3 curPoint = Linc.Stage.mousePos;
        Vector3 moveVec = (curPoint - _lastPoint) * mouseMoveSpeed;
        _levelMgr.MapMove(moveVec);
        _lastPoint = curPoint;
    }

    private void OnPanelUp()
    {
        _panel.off(Linc.Event.mouseMove);
        _panel.off(Linc.Event.mouseUp);
        _panel.off(Linc.Event.mouseOut);
        BaseObjClickUp();
    }

    private void BaseObjClickDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(ray, out hitInfo);
        if (hitInfo.collider != null && _levelMgr.IsBaseCanClick(hitInfo.collider.gameObject))
        {
            _curClickBaseObj = hitInfo.collider.gameObject;
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("localScale", new Vector3(1.2f, 1.2f, 1.2f));
            Linc.Tween.FrameTo(_curClickBaseObj, _curClickBaseObj.transform, dic, 2, Linc.EaseFun.linearIn);
        }
    }

    private void BaseObjClickUp()
    {
        if (_curClickBaseObj != null)
        {
            GameObject clickBaseObj = _curClickBaseObj;
            _curClickBaseObj = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider != null && _levelMgr.IsBaseCanClick(hitInfo.collider.gameObject))
            {
                if (hitInfo.collider.gameObject == clickBaseObj)
                {
                    LockInput();
                    _levelMgr.OnBaseSelect(clickBaseObj,UnlockInput);
                }
            }
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("localScale", new Vector3(1, 1, 1));
            Linc.Tween.FrameTo(clickBaseObj, clickBaseObj.transform, dic, 2, Linc.EaseFun.linearIn);
        }
    }

    private void LockInput()
    {
        _panel.mouseEnable = false;
    }

    private void UnlockInput()
    {
        _panel.mouseEnable = true;
    }
}
