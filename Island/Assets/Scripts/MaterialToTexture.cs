using UnityEngine;

public class MaterialToTexture : MonoBehaviour
{
    public Material materialToConvert; // материал, который нужно конвертировать
    public bool saveToFile; // флаг для сохранения текстуры в файл

    private void Start()
    {
        Texture2D texture = ConvertMaterialToTexture(materialToConvert); // конвертируем материал в текстуру
        if (saveToFile)
        {
            SaveTextureToFile(texture); // сохраняем текстуру в файл
        }
    }

    private Texture2D ConvertMaterialToTexture(Material material)
    {
        Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, false); // создаем новую текстуру
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear); // создаем временный рендер-текстур
        Graphics.Blit(null, renderTexture, material); // отрисовываем материал на рендер-текстуре
        RenderTexture.active = renderTexture; // устанавливаем текущую рендер-текстуру
        texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0); // считываем пиксели из рендер-текстуры в текстуру
        texture.Apply(); // применяем изменения
        RenderTexture.ReleaseTemporary(renderTexture); // освобождаем временный рендер-текстур
        return texture;
    }

    private void SaveTextureToFile(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG(); // кодируем текстуру в PNG-формат
        string fileName = "Sand.png"; // имя файла для сохранения
        System.IO.File.WriteAllBytes(Application.dataPath + "/" + fileName, bytes); // сохраняем файл в директории "Assets"
    }
}