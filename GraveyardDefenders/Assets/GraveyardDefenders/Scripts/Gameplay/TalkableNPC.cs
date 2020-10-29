using UnityEngine;

namespace XD
{
    public class TalkableNPC : MonoBehaviour, IInteractable
    {
        public Vector3 Position => transform.position;

        #region INTERACTABLE
        public bool CanBeInteracted(ResourceInventory inventory){ return true; }
        public PLAYER_ACTIONS GetAction() { return PLAYER_ACTIONS.TALK_NPC; }
        public float GetDistance(Vector3 pos) { return Vector3.Distance(pos, Position); }
        public GameObject GetGO() { return gameObject; }
        public Vector3 GetInteractLookAt() { return Position; }
        #endregion

        public virtual void Talk(PlayerCharacter pc) {}
    }   
}
