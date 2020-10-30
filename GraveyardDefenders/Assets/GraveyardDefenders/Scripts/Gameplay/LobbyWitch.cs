using UnityEngine;

namespace XD
{
    public class LobbyWitch : TalkableNPC
    {
        public Animator animator;
        public string triggerToSetOntalk;

        public override void Talk(PlayerCharacter pc)
        {
            base.Talk(pc);
            int currentSkinID = pc.CurrentSkinID;
            int skinCount = Constants.Instance.SkinCount;
            int nextSkin = (currentSkinID + 1) % skinCount;
            animator.SetTrigger(triggerToSetOntalk);
            pc.SetSkin(nextSkin);
            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.WITCH_LAUGH, gameObject);
        }
    }   
}
