using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RabbitCardBase : CardBase
{
    private PlayableDirector _playableDirector;
    private PlayableAsset _endAsset=Resources.Load<PlayableAsset>("Timeline/RabbitTimeline_end");
    private PlayableAsset _idleAsset = Resources.Load<PlayableAsset>("Timeline/RabbitTimeline_idle");
    public RabbitCardBase(GameObject inCard) : base(inCard)
    {
        SetCardArea(CardArea.table);
        _playableDirector=card.GetComponent<PlayableDirector>();
    }

    public void EatCarrot()
    {
        _playableDirector.Play(_endAsset, DirectorWrapMode.None);
    }

    public void Idle()
    {
        _playableDirector.Play(_idleAsset, DirectorWrapMode.Loop);
    }
}
