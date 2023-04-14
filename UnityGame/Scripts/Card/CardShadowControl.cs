using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShadowControl : MonoBehaviour
{
    GameObject _shadow;
    // Start is called before the first frame update
    void Start()
    {
        var tt=gameObject.transform.Find("cardShadow");
        if(tt==null)
        {
            Debug.Log(gameObject.name);
        }
        _shadow=gameObject.transform.Find("cardShadow").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        FreshShadowShow();
    }

    private void FreshShadowShow()
    {
        bool isShowShadow=Vector3.Dot(gameObject.transform.forward,new Vector3(0,0,1))>0;
        _shadow.SetActive(isShowShadow);
    }
}
