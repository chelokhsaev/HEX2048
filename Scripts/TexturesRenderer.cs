using System.Collections;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Setup/TexturesRenderer")]
public class TexturesRenderer : PrefabObject
{

    public Material baseMaterial;

    public static Material[] materials = new Material[15];
    public static TexturesRenderer instance;

    public override void Awake()
    {
        instance = this;
        RenderTextures();
    }

    public void RenderTextures()
    {
        Camera camera = transform.GetComponentInChildren<Camera>();
        camera.enabled = true;
        TextMeshPro text = transform.GetComponentInChildren<TextMeshPro>();
        for (int i = 0; i < materials.Length; i++)
        {
            int num = (int)Mathf.Pow(2f, i);
            text.text = num.ToString();
            RenderTexture.active = camera.targetTexture;
            camera.Render();
            Texture2D offscreenTexture = new Texture2D(camera.targetTexture.width,
                camera.targetTexture.height, TextureFormat.RGBA32, false);
            offscreenTexture.ReadPixels(new Rect(0, 0, camera.targetTexture.width,
                camera.targetTexture.height), 0, 0, false);
            offscreenTexture.Apply();
            Material material = new Material(baseMaterial)
            {
                mainTexture = offscreenTexture,
                name = "Digits " + num
            };
            materials[i] = material;
        }
        camera.enabled = false;
        GameObject.Destroy(transform.gameObject);
    }

}
