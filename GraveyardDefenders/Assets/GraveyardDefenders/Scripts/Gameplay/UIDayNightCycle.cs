using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

        private int lastDaysSurvived = 0;

        [Header("DayCounter")]
        public CanvasGroup dayCountCanvasGroup;
        public float showDayCounterDuration = 3.0f;
        public float showDayCounterFadeDuration = 0.2f;

        public void OnEnable()
        {
            GlobalEvents.newDayStarted.AddListener(ShowDayCount);
        }

        public void OnDisable()
        {
            GlobalEvents.newDayStarted.RemoveListener(ShowDayCount);
        }

        public bool HasToUpdateDaysSurvived { get { return lastDaysSurvived != cycle.daysSurvived; } }

        void Update()
        {
            if (cycle == null) FindDayNightCycle();
            if (cycle == null) return;

            currentCycleTimeImage.fillAmount = 1.0f - cycle.CycleRemainingTimeNormalized;
            if (HasToUpdateDaysSurvived)
            {
                currentDayText.text = (cycle.daysSurvived + 1).ToString();
                lastDaysSurvived = cycle.daysSurvived + 1;
            }

            if (cycle.currentPhase == DAY_NIGHT_PHASE.DAY)
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

        public void ShowDayCount() { dayCountCanvasGroup.DOFade(1.0f, showDayCounterFadeDuration).OnComplete(HideDayCountDelayed); }
        public void HideDayCountDelayed() { dayCountCanvasGroup.DOFade(0.0f, showDayCounterFadeDuration).SetDelay(showDayCounterDuration); }

        private void FindDayNightCycle() { cycle = FindObjectOfType<DayNightCycle>(); }
    }
}
