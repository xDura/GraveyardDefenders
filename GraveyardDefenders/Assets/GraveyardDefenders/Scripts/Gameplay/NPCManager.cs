using UnityEngine;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.SceneManagement;
using XD.Utils;

namespace XD
{
    public class PlayerGhostDictionary : SerializableDictionaryBase<PlayerCharacter, GhostController> { };

    public class NPCManager : Singleton<NPCManager>
    {
        public Pool skeletonPool;
        public SkeletonSet skeletons;
        public int skeletonsLeft = 0;

        public Pool ghostPool;
        public GhostSet ghosts;
        PlayerGhostDictionary ghostForPlayer = new PlayerGhostDictionary();
        public bool inMenus = false;


        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Clear();
            SceneManager.sceneLoaded += OnSceneLoaded;
            GlobalEvents.newDayStarted.AddListener(OnNewDayStarted);
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GlobalEvents.newDayStarted.RemoveListener(OnNewDayStarted);
        }

        void OnNewDayStarted()
        {
            skeletonsLeft = ProgressionUtils.DayToProgression(DayNightCycle.daysSurvived_s);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            inMenus = (scene.name == "MainMenu");
            if (inMenus) Clear();
        }

        void OnEnable() 
        {
            Clear();
            GlobalEvents.newDayStarted.AddListener(FadeGhosts);
        }

        void OnDisable() 
        { 
            Clear();
            GlobalEvents.newDayStarted.RemoveListener(FadeGhosts);
        }

        void Update()
        {
            if (inMenus) return;

            PlayerInput pi = PlayerInput.Instance;
            for (int i = 0; i < pi.CurrentPlayerCount; i++)
            {
                PlayerCharacter pc = pi.local_players[i];
                if (!ghostForPlayer.ContainsKey(pc) && (pc.TImeOutsideSafeArea >= 2.0f) && DayNightCycle.currentPhase_s == DAY_NIGHT_PHASE.NIGHT) InstantiateGhost(pc.lastSafeAreaExitPosition, pc.transform.rotation, pc);
            }
        }

        public void Clear()
        {
            for (int i = skeletons.items.Count - 1; i >= 0; i--) RemoveSkeleton(skeletons.items[i]);
            for (int i = ghosts.items.Count - 1; i >= 0; i--) RemoveGhost(ghosts.items[i]);

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
            if (skeletonsLeft <= 0) return false;

            skeletonsLeft--;
            GameObject newSkeleton = skeletonPool.Spawn(position, rotation);
            if (!newSkeleton) return false;

            SkeletonController controller = newSkeleton.GetComponent<SkeletonController>();
            controller.Init();
            skeletons.Add(controller);
            return true;
        }

        public void RemoveSkeleton(SkeletonController controller)
        {
            if (controller == null) return;
            skeletons.Remove(controller);
            skeletonPool.Despawn(controller.gameObject);
            GlobalEvents.skeletonDespawned.Invoke(controller);
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
            if (controller == null) return;
            ghostForPlayer.Remove(controller.target);
            ghosts.Remove(controller);
            ghostPool.Despawn(controller.gameObject);
        }
    }   
}
