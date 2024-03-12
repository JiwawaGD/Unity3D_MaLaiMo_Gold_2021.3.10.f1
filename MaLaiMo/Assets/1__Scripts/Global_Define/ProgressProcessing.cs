using UnityEngine;
using UnityEngine.UI;

namespace ProgressP {
    public class ProgressProcessing : MonoBehaviour
    {
        [SerializeField] Text[] progressPrompt;
        private int currentProgressIndex = 0;

        private void Awake()
        {
            foreach (Text text in progressPrompt)
            {
                Color textColor = text.color;
                textColor.a = .01f;
                text.color = textColor;
            }
            UpdateProgress(currentProgressIndex);
        }
        public void UpdateProgress(int index)
        {
            if (index >= 0 && index < progressPrompt.Length)
            {              
                Color textColor = progressPrompt[index].color;
                textColor.a = 1f;
                progressPrompt[index].color = textColor;
            }
        }
    }
}
