using UnityEngine;

namespace XD
{
    public interface IInteractable
    {
        GameObject GetGO();
        bool CanBeInteracted(ResourceInventory inventory);
        float GetDistance(Vector3 pos);
        PLAYER_ACTIONS GetAction();
        Vector3 GetInteractLookAt();
    }   
}
