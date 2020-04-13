using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    public class NPCManager : Singleton<NPCManager>
    {
        public Pool skeletonPool;
        public SkeletonSet skeletons;

        public Pool ghostPool;
        public GhostSet ghosts;

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

        public void Clear()
        {
            skeletons.Clear();
            ghosts.Clear();
        }

        public void FadeGhosts()
        {
            for (int i = 0; i < ghosts.items.Count; i++) ghosts.items[i].DisappearAndDespawn();
        }

        public bool InstantiateSkeleton(Vector3 position, Quaternion rotation)
        {
            GameObject newSkeleton = skeletonPool.Spawn(position, rotation);
            if (!newSkeleton) return false;

            SkeletonController controller = newSkeleton.GetComponent<SkeletonController>();
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
            GameObject newGhost = ghostPool.Spawn(position, rotation);
            if (!newGhost) return false;

            GhostController controller = newGhost.GetComponent<GhostController>();
            ghosts.Add(controller);
            controller.Init(characterToFollow);
            return true;
        }

        public void RemoveGhost(GhostController controller)
        {
            ghosts.Remove(controller);
            ghostPool.Despawn(controller.gameObject);
        }
    }   
}
