using UnityEngine;

public class DynamicCreateMat
{
	public static Material GetMaterial(string path, string textureName, bool bCommonPath = true)
	{
		Shader shader = Shader.Find("Triniti/Sprite");
		Material material = null;
		Texture texture = null;
		material = ((!(shader == null)) ? new Material(shader) : new Material(textureName + "Mat"));
		texture = ((!bCommonPath) ? (Resources.Load(path + "/" + textureName) as Texture) : (Resources.Load("Zombie3D/UI/Textures/" + path + "/" + textureName) as Texture));
		if (texture == null)
		{
			Debug.LogWarning("texture == null|PATH|" + path + "/" + textureName);
		}
		material.mainTexture = texture;
		return material;
	}
}
