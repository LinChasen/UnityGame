using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Linc.StorageMgr.Clear();
        Linc.StorageMgr.SetStorageData(AllGlobal.storage);
        Linc.ProcedureMgr.ChangeToProcedure<Procedure_Loading>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
