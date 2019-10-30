using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace XD
{
    public class UIResources : MonoBehaviour
    {
        public TextMeshProUGUI woodText;
        public ResourceInventory inventory;

        void Update()
        {
            woodText.text = inventory.GetResourceCount(RESOURCE_TYPE.WOOD).ToString();
        }
    }
}
