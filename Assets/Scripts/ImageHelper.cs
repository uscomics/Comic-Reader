using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageHelper {

    public IEnumerator SetImageFromURL(RawImage image, string url) {
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        Object.Destroy(image.texture);
        image.texture = www.texture;
        ScaleImage(image);
    }

    public void SetImageFromResource(RawImage image, string path) {
        Object.Destroy(image.texture);
        image.texture = Resources.Load(path) as Texture2D;
        ScaleImage(image);
    }

    public float GetImageWidth(RawImage image) {
        return image.texture.width;
    }

    public float GetImageHeight(RawImage image) {
        return image.texture.height;
    }

    void ScaleImage(RawImage image) {
        if (null == image.GetComponent<AspectRatioFitter>()) return;
        float imageRatio = GetImageWidth(image) / GetImageHeight(image);
        image.GetComponent<AspectRatioFitter>().aspectRatio = imageRatio;
    }
}
