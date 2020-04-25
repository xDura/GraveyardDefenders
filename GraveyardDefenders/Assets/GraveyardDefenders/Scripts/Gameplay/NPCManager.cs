using UnityEngine;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD
{
    public class PlayerGhostDictionary : SerializableDictionaryBase<PlayerCharacter, GhostController> { };

    public class NPCManager : Singleton<NPCManager>
    {
        public Pool skeletonPool;
        public SkeletonSet skeletons;

        public Pool ghostPool;
        public GhostSet ghosts;
        PlayerGhostDictionary ghostForPlayer = new PlayerGhostDictionary();

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Clear();
        }

        public void OnEnable() 
        {
            Clear();
            GlobalEvents.newDayStarted.AddListener(FadeGhosts);
        }

        public void OnDisable() 
        { 
            Clear();
            GlobalEvents.newDayStarted.RemoveListener(FadeGhosts);
        }

        public void Update()
        {
            PlayerInput pi = PlayerInput.Instance;
            for (int i = 0; i < pi.CurrentPlayerCount; i++)
            {
                PlayerCharacter pc = pi.local_players[i];
                if (!ghostForPlayer.ContainsKey(pc) && (pc.TImeOutsideSafeArea >= 2.0f) && DayNightCycle.currentPhase_s == DAY_NIGHT_PHASE.NIGHT) InstantiateGhost(pc.lastSafeAreaExitPosition, pc.transform.rotation, pc);
            }
        }

        public void Clear()
        {
            ghostForPlayer.Clear();
            skeletons.Clear();
            ghosts.Clear();
        }

        public void FadeGhosts()
        {
            for (int i = 0; i < ghosts.items.Count; i++) ghosts.items[i].DisappearAndDespawn();
        }

        public bool InstantiateSkeleton(Vector3 position, Quaternion rotation)
        {
            if(Constants.Instance.noEnemiesMode) return false;

            GameObject newSkeleton = skeletonPool.Spawn(position, rotation);
            if (!newSkeleton) return false;

            SkeletonController controller = newSkeleton.GetComponent<SkeletonController>();
            controller.Init();
            skeletons.Add(controller);
            return true;
        }

        public void RemoveSkeleton(SkeletonController controller)
        {
            skeletons.Remove(controller);
            skeletonPool.Despawn(controller.gameObject);
        }

        public bool InstantiateGhost(Vector3 position, Quaternion rotation, PlayerCharacter characterToFollow)
        {
            if (Constants.Instance.noEnemiesMode) return false;

            GameObject newGhost = ghostPool.Spawn(position, rotation);
            if (!newGhost) return false;

            GhostController controller = newGhost.GetComponent<GhostController>();
            ghosts.Add(controller);
            controller.Init(characterToFollow);
            ghostForPlayer.Add(characterToFollow, controller);
            return true;
        }

        public void RemoveGhost(GhostController controller)
        {
            ghostForPlayer.Remove(controller.target);
            ghosts.Remove(controller);
            ghostPool.Despawn(controller.gameObject);
        }
    }   
}
