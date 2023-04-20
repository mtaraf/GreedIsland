using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TakeScreenshot());
    }

    Sprite GetScreenShot()
    {

        var image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);

        image.Apply();
        Rect rec = new Rect(0, 0, image.width, image.height);

        var sprite = Sprite.Create(image, rec, new Vector2(0.5f, 0.5f), 100);
        return sprite;
    }

    IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        Sprite screenshot = GetScreenShot();
        SaveScreenShot(screenshot, "myscreenshot");
    }

    void SaveScreenShot(Sprite sprite, string outputfilename)
    {
        Texture2D itemBGTex = sprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes($"/Users/Marco/OneDrive/Pictures/{outputfilename}.png", itemBGBytes);
    }

}
