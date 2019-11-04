using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace XD
{
    public class UIDayNightCycle : MonoBehaviour
    {
        [Header("Assignable")]
        public GameObject moonImage;
        public GameObject sunImage;
        public Image currentCycleTimeImage;
        public TextMeshProUGUI currentDayText;

        public DayNightCycle cycle;

        void Update()
        {
            if (cycle == null) FindDayNightCycle();
            if (cycle == null) return;

            currentCycleTimeImage.fillAmount = 1.0f - cycle.CycleRemainingTimeNormalized;
            currentDayText.text = (cycle.daysSurvived + 1).ToString();
            if(cycle.currentPhase == DAY_NIGHT_PHASE.DAY)
            {
                moonImage.SetActive(false);
                sunImage.SetActive(true);
            }
            else
            {
                moonImage.SetActive(true);
                sunImage.SetActive(false);
            }
        }

        private void FindDayNightCycle()
        {
            cycle = FindObjectOfType<DayNightCycle>();
        }
    }
}
