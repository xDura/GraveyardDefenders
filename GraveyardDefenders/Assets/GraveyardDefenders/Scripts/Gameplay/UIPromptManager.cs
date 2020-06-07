using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    public class UIPromptManager : MonoBehaviour
    {
        public Pool promptsPool;
        public List<UIPrompt> current_prompts;

        void OnEnable()
        {
            UIEvents.showUpgradeableEvnt.AddListener(OnPromptRequest);
            UIEvents.hideUpgradeableEvnt.AddListener(OnPromptFree);
            GlobalEvents.upgradeableUpgraded.AddListener(OnUpgradeableUpgraded);
        }

        void OnDestroy()
        {
            UIEvents.showUpgradeableEvnt.RemoveListener(OnPromptRequest);
            UIEvents.hideUpgradeableEvnt.RemoveListener(OnPromptFree);
            GlobalEvents.upgradeableUpgraded.RemoveListener(OnUpgradeableUpgraded);
            ClearAllPrompts();
        }

        void OnUpgradeableUpgraded(Upgradeable upgradeable)
        {
            for (int i = 0; i < current_prompts.Count; i++)
            {
                UIPrompt p = current_prompts[i];
                if (p.upgradeable == upgradeable)
                {
                    if (upgradeable.IsMaxLevel) FreePrompt(p);
                    else p.SetUpgradeable(upgradeable);
                }
            }
        }

        void OnPromptRequest(PlayerCharacter pc, Upgradeable upgradeable)
        {
            GameObject promptGO = promptsPool.Spawn(Vector3.zero, Quaternion.identity);
            UIPrompt prompt = promptGO.GetComponent<UIPrompt>();
            prompt.targetCharacter = pc;
            prompt.SetUpgradeable(upgradeable);
            current_prompts.Add(prompt);
        }

        void OnPromptFree(PlayerCharacter pc, Upgradeable upgradeable)
        {
            UIPrompt p = FindPrompt(pc, upgradeable);
            FreePrompt(p);
        }

        UIPrompt FindPrompt(PlayerCharacter pc, Upgradeable upgradeable)
        {
            for (int i = 0; i < current_prompts.Count; i++)
            {
                UIPrompt p = current_prompts[i];
                if (p.targetCharacter == pc && p.upgradeable == upgradeable) return p;
            }
            return null;
        }

        void FreePrompt(UIPrompt promptToFree)
        {
            if (promptToFree == null) return;
            promptToFree.Clear();
            promptsPool.Despawn(promptToFree.gameObject);
        }

        public void ClearAllPrompts()
        {
            for (int i = 0; i < current_prompts.Count; i++) FreePrompt(current_prompts[i]);
            current_prompts.Clear();
        }
    }   
}
