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
        public ResourceInventory inventory;

        private int lastWoodCount = 0;
        private int lastRockCount = 0;

        bool HasToUpdateWood { get { return lastWoodCount != inventory.GetResourceCount(RESOURCE_TYPE.WOOD); } }
        bool HasToUpdateRock { get { return lastRockCount != inventory.GetResourceCount(RESOURCE_TYPE.STONE); } }

        void Update()
        {
            if(HasToUpdateWood)
                woodText.text = inventory.GetResourceCount(RESOURCE_TYPE.WOOD).ToString();
            if(HasToUpdateRock)
                rockText.text = inventory.GetResourceCount(RESOURCE_TYPE.STONE).ToString();
        }
    }
}
