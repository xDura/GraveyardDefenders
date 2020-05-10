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

    public class Upgradeable : MonoBehaviour
    {
        public int currentLevel;
        public int MaxLevel => levels.Count - 1;
        public bool IsMaxLevel => currentLevel >= MaxLevel;
        public List<GameObject> levels = new List<GameObject>();
        public List<ResourceRequirements> requirementsForLevel = new List<ResourceRequirements>();


        public GameObject prompt;

        public void Awake()
        {
            SetLevel(currentLevel);
            HidePrompt();
        }

        public void ShowPrompt()
        {
            prompt.SetActive(true);
        }

        public void HidePrompt()
        {
            prompt.SetActive(false);
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
            HidePrompt();
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc) pc.nearbyUpgradeables.Add(this);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc) pc.nearbyUpgradeables.Remove(this);
        }
    }   
}
