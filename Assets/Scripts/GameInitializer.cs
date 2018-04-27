using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public Image splashImage;
    public GameObject loadingOrb;
    public string level;

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }
    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, 2.5f, false);
    }

    IEnumerator Start ()
    {

        splashImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        loadingOrb.SetActive(true);
        while (!operation.isDone)
        {
            //Debug.Log(operation.progress);
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingOrb.GetComponent<LightOrb>().SetOrbCharge(progress * 10);
            loadingOrb.transform.GetChild(1).gameObject.transform.Rotate(1,1,0, Space.World);

            yield return null;
        }
    }
}
