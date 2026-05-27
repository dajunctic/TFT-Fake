using TMPro;
using UnityEngine;

namespace Dajunctic
{
    public class ShopStreakUI : BaseView
    {
        [SerializeField] private GameObject winStreakIcon;
        [SerializeField] private GameObject loseStreakIcon;
        [SerializeField] private GameObject normalStreakIcon;
        [SerializeField] private GameObject winStreakLight;
        [SerializeField] private GameObject loseStreakLight;
        [SerializeField] private TMP_Text streakCountText;

        private int loseStreakLimit = -3;
        private int winStreakLimit = 3;

        public void UpdateStreak(int streakCount)
        {
            winStreakIcon.SetActive(streakCount > winStreakLimit);
            loseStreakIcon.SetActive(streakCount < loseStreakLimit);
            normalStreakIcon.SetActive(streakCount == 0);
            winStreakLight.SetActive(streakCount > 0 && streakCount <= winStreakLimit);
            loseStreakLight.SetActive(streakCount < 0 && streakCount >= loseStreakLimit);

            streakCountText.text = Mathf.Abs(streakCount).ToString();
        }
    }
}
