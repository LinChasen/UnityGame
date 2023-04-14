using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinView : Linc.SpriteBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        // Linc.Image2D nextBtn=Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("nextLevel").gameObject)as Linc.Image2D;
        // nextBtn.AddClickFun(()=>{
        //     Linc.ViewMgr.CloseView(ViewType.GameWinView);
        //     Linc.ViewMgr.OpenView(ViewType.LoadingView);
        // });
        // Linc.EaseDic dic=new Linc.EaseDic();
        // dic.Add("posVec",new Vector2(Linc.Stage.width-20-nextBtn.width/2,Linc.Stage.height-20-nextBtn.height/2));
        // Linc.Tween.To(gameObject,nextBtn,dic,2000,Linc.EaseFun.linearIn);
    }
}
