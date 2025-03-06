using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager Instance;
        
        public Image hurtVignette;
        public Image bossInitVignette;
        public TextMeshProUGUI bossNameText;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        
        public void FadeHurtVignette(float intensity)
        {
            StartCoroutine(FadeVignette(hurtVignette, intensity));
        }

        private IEnumerator FadeVignette(Image vignette, float intensity)
        {
            vignette.gameObject.SetActive(true);
            yield return new WaitForSeconds(intensity);
            vignette.gameObject.SetActive(false);
        }

        public void ShowBossName(string bossName)
        {
            bossNameText.gameObject.SetActive(true);
            bossNameText.text = bossName;
            bossNameText.color = new Color(1, 1, 1, 0);
            DOTween.Sequence()
                .Append(bossNameText.DOFade(1.0f, 0.5f))
                .AppendInterval(3.0f)
                .Append(bossNameText.DOFade(0.0f, 0.5f));
        }

        public void FadeBossInitVignette(float intensity)
        {
            StartCoroutine(FadeVignette(bossInitVignette, intensity));
        }

    }
}