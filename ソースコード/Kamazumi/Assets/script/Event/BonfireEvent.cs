using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireEvent : MonoBehaviour
{
    [System.Serializable]
    public struct Bonfire
    {
        public LightingTheBonfireBrazier lightingTheBonfireBraziers;
        public bool isOnOff;
    }

    [SerializeField] Bonfire[] bonfires;
    [SerializeField] DissolveSummonController[] dissolveSummonController;
    bool EventTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventTriggered) return;
        if (bonfires == null) return;
        // すべてのかがり火が点灯しているかどうかをチェック
        foreach (var brazier in bonfires)
        { 
            if (brazier.isOnOff != brazier.lightingTheBonfireBraziers.isOnOff) return;
        }
        AllBonfiresLit();
    }
    //すべてのかがり火が点灯しているときに呼び出されるイベント
    void AllBonfiresLit()
    {
        Debug.Log("すべてのかがり火が点灯しました！");
        foreach (var controller in dissolveSummonController)
        {
            controller.StartDissolve();
        }
        EventTriggered = true;
        //ここに、すべてのかがり火が点灯したときに実行したい処理を追加します。
    }
}
