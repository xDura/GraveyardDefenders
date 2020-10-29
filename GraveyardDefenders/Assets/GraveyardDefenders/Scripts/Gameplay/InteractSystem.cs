using System;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class ActionDictionary : Dictionary<PLAYER_ACTIONS, PlayerAction> { };
    public class InteractSystem : MonoBehaviour 
    {
        [Header("Assignable")]
        public PlayerCharacter playerCharacter;

        [Header("Config")]
        public LayerMask searchLayer;
        public float interactRadius;
        Collider[] overlap_collider_cache = new Collider[20];
        int foundCols;

        public IInteractable BestInteractable => bestInteractable;
        IInteractable bestInteractable;
        ActionDictionary actions;
        PlayerAction currentAction;
        public PLAYER_ACTIONS CurrentActionId => currentAction != null ? currentAction.Id : PLAYER_ACTIONS.NONE;

        [NonSerialized] public List<Upgradeable> nearbyUpgradeables = new List<Upgradeable>();
        public void Start()
        {
            actions = new ActionDictionary
            {
                { PLAYER_ACTIONS.GATHER, new Gather(playerCharacter) },
                { PLAYER_ACTIONS.ADD_RESOURCE, new AddResource(playerCharacter) },
                { PLAYER_ACTIONS.REPAIR, new Repair(playerCharacter) },
                { PLAYER_ACTIONS.UPGRADE, new Upgrade(playerCharacter) },
                { PLAYER_ACTIONS.TALK_NPC, new TalkNPC(playerCharacter) },
            };
        }

        public void Update()
        {
            if (currentAction == null)
            {
                SearchInteractables();
                if(playerCharacter.interactPressedThisFrame && bestInteractable != null) //start the action
                {
                    currentAction = actions[bestInteractable.GetAction()];
                    currentAction.Start(bestInteractable);
                } 
            }
            else
            {
                if (bestInteractable.CanBeInteracted(playerCharacter.inventory) && playerCharacter.moveVector == Vector2.zero)
                    currentAction.Update();
                else
                    StopCurrentAction();
            }


#if DEBUG_INTERACT_SYSTEM
            VisualDebug();
#endif
        }

        public void StopCurrentAction()
        {
            currentAction?.Stop();
            currentAction = null;
        }

        void SearchInteractables()
        {
            bestInteractable = null;

            if (bestInteractable == null) //TODO: once upgradeables are changed: remove this case (now upgradeables have super priority)
            {
                foundCols = Physics.OverlapSphereNonAlloc(transform.position, interactRadius, overlap_collider_cache, searchLayer, QueryTriggerInteraction.Collide);
                float bestDistance = float.PositiveInfinity;
                for (int i = 0; i < foundCols; i++)
                {
                    Collider current_collider = overlap_collider_cache[i];
                    IInteractable current_interactable = current_collider.GetComponent<IInteractable>();
                    if (current_interactable == null || !current_interactable.CanBeInteracted(playerCharacter.inventory)) continue;
                    float current_distance = current_interactable.GetDistance(transform.position);
                    if (current_distance > bestDistance) continue;

                    bestDistance = current_distance;
                    bestInteractable = current_interactable;
                }
            }
        }

#if DEBUG_INTERACT_SYSTEM
        void VisualDebug()
        {
            if(currentAction != null)
            Debug.Log($"currentAction -> {currentAction.GetType().ToString()}");
            for (int i = 0; i < foundCols; i++)
            {
                Collider current_collider = overlap_collider_cache[i];
                IInteractable current_interactable = current_collider.GetComponent<IInteractable>();
                if (current_interactable == null) continue;
                DebugExtension.DebugWireSphere(current_collider.transform.position, Color.yellow, 0.5f, 0.0f, false);
                DebugExtension.DebugWireSphere(current_collider.transform.position, Color.red, 1.0f, 0.0f, false);
            }

            if(bestInteractable != null)
            {
                GameObject go = bestInteractable.GetGO();
                DebugExtension.DebugWireSphere(go.transform.position, Color.green, 1.0f, 0.0f, false);
                DebugExtension.DebugWireSphere(go.transform.position, Color.yellow, 0.5f, 0.0f, false);
                Debug.Log($"BestInteractable {go?.name} : {bestInteractable.GetAction()}");
            }
            DebugExtension.DebugWireSphere(transform.position, interactRadius, 0.0f, false);
        }
#endif
    }
}
