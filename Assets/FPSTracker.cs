using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateFPS());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateFPS() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            int fps = (int) (1f / Time.unscaledDeltaTime);
            TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
            text.text = "FPS " + fps;
        }
    }
}
