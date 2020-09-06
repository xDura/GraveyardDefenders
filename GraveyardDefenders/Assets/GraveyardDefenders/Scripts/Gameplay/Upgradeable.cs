using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    [System.Serializable]
    public class ResourceRequirements
    {
        [System.Serializable]
        public class Requirement
        {
            public RESOURCE_TYPE resource;
            public float ammount;
        }
        public Requirement[] requirements;
    }

    public class Upgradeable : MonoBehaviour, IInteractable
    {
        public int currentLevel;
        public int MaxLevel => levels.Count - 1;
        public bool IsMaxLevel => currentLevel >= MaxLevel;
        public List<GameObject> levels = new List<GameObject>();
        public List<ResourceRequirements> requirementsForLevel = new List<ResourceRequirements>();
        public Vector3 uiOffset;
        public Vector3 InteractPos => transform.position;
        public Vector3 UIPos => transform.position + uiOffset;

        public void Awake()
        {
            SetLevel(currentLevel); 
        }

        public ResourceRequirements GetCurrentRequirements()
        {
            return requirementsForLevel[currentLevel];
        }

        public void SpendCurrentRequirements(ResourceInventory inventory)
        {
            ResourceRequirements requirements = requirementsForLevel[currentLevel];
            for (int i = 0; i < requirements.requirements.Length; i++)
            {
                ResourceRequirements.Requirement requ = requirements.requirements[i];
                inventory.SubstractResource(requ.resource, requ.ammount);
            }
        }

        public void SetLevel(int level)
        {
            for (int i = 0; i < levels.Count; i++)
                levels[i].SetActive(false);

            levels[level].SetActive(true);
            currentLevel = level;
        }

        public bool CanBeUpgraded(ResourceInventory inventory)
        {
            return !IsMaxLevel && MeetsCurrentRequirements(inventory);
        }

        public bool MeetsCurrentRequirements(ResourceInventory inventory)
        {
            ResourceRequirements requirements = requirementsForLevel[currentLevel];
            for (int i = 0; i < requirements.requirements.Length; i++)
            {
                if (!inventory.HasResource(requirements.requirements[i].resource, requirements.requirements[i].ammount)) return false;
            }

            return true;
        }

        public void Upgrade()
        {
            SetLevel(currentLevel + 1);
            GlobalEvents.upgradeableUpgraded.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc && !IsMaxLevel)
            {
                UIEvents.showUpgradeableEvnt.Invoke(pc, this);
                //pc.interactSystem.UpgradeableNear(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc)
            {
                UIEvents.hideUpgradeableEvnt.Invoke(pc, this);
                //pc.interactSystem.UpgradeableFar(this);
            }
        }

        #region IInteractable
        public GameObject GetGO() { return gameObject; }
        public bool CanBeInteracted(ResourceInventory inventory) { return CanBeUpgraded(inventory); }
        public float GetDistance(Vector3 pos) { return Vector3.Distance(pos, transform.position); }
        public PLAYER_ACTIONS GetAction() { return PLAYER_ACTIONS.UPGRADE; }
        public Vector3 GetInteractLookAt() { return InteractPos; }
        #endregion
    }
}
