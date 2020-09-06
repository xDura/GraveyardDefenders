using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace XD
{
    public class UIResources : MonoBehaviour
    {
        public TextMeshProUGUI woodText;
        public TextMeshProUGUI rockText;
        public TextMeshProUGUI crystalText;
        public ResourceInventory inventory;

        private float lastWoodCount = 0;
        private float lastRockCount = 0;
        private float lastCrystalCount = 0;

        bool HasToUpdateWood => lastWoodCount != inventory.GetResourceCount(RESOURCE_TYPE.WOOD);
        bool HasToUpdateRock => lastRockCount != inventory.GetResourceCount(RESOURCE_TYPE.STONE);
        bool HasToUpdateCrystal => lastCrystalCount != inventory.GetResourceCount(RESOURCE_TYPE.CRYSTAL);

        void Update()
        {
            if (HasToUpdateWood)
            {
                float count = inventory.GetResourceCount(RESOURCE_TYPE.WOOD);
                woodText.text = count.ToString();
                lastWoodCount = count;

            }
            if (HasToUpdateRock)
            {
                float count = inventory.GetResourceCount(RESOURCE_TYPE.STONE);
                rockText.text = count.ToString();
                lastRockCount = count;
            }
            if (HasToUpdateCrystal)
            {
                float count = inventory.GetResourceCount(RESOURCE_TYPE.CRYSTAL);
                crystalText.text = count.ToString();
                lastCrystalCount = count;
            }
        }
    }
}
