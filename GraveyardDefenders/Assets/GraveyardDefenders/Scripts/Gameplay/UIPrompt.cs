using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    [System.Serializable]
    public class RequirementPromptUI
    {
        public Image resourceImage;
        public TextMeshProUGUI ammountText;
        public GameObject go;
    }

    public class UIPrompt : MonoBehaviour
    {
        public ResourceIconsDatabase icons_db;
        public PlayerCharacter targetCharacter;
        public Upgradeable upgradeable;
        public RectTransform rect;
        public List<RequirementPromptUI> requirementRows;

        Camera cam;
        Camera Cam { get { if (cam == null) cam = Camera.main; return cam; } }

        void ClearRequirements()
        {
            for (int i = 0; i < requirementRows.Count; i++) requirementRows[i].go.SetActive(false);
        }

        public void SetUpgradeable(Upgradeable a_upgradeable)
        {
            upgradeable = a_upgradeable;
            ClearRequirements();
            ResourceRequirements requirements = upgradeable.GetCurrentRequirements();
            for (int i = 0; i < requirements.requirements.Length; i++)
            {
                ResourceRequirements.Requirement requirement = requirements.requirements[i];
                RequirementPromptUI row = requirementRows[i];
                row.go.SetActive(true);
                row.resourceImage.sprite = icons_db.GetSprite(requirement.resource);
                row.ammountText.text = requirement.ammount.ToString();
            }
        }

        void Update()
        {
            if (!upgradeable) return;
            Vector3 desiredPosInScreen = Cam.WorldToScreenPoint(upgradeable.UIPos);
            desiredPosInScreen.z = 0.0f;
            rect.position = desiredPosInScreen;
        }

        public void Clear()
        {
            ClearRequirements();
            upgradeable = null;
            targetCharacter = null;
        }
    }   
}
