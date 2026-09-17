using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checktexture : MonoBehaviour
{
	public Texture2D sourceTexture;
	public Texture2D subtractTexture;
	public GameObject gg;
	void Start()
	{
		SubtractTextures();
	}

	void SubtractTextures()
	{
		// Check if textures are provided
		if (sourceTexture == null || subtractTexture == null)
		{
			Debug.LogError("Source texture or subtract texture is not assigned!");
			return;
		}

		// Ensure both textures have the same dimensions
		if (sourceTexture.width != subtractTexture.width || sourceTexture.height != subtractTexture.height)
		{
			Debug.LogError("Textures have different dimensions!");
			return;
		}

		// Create a new texture to store the result
		Texture2D resultTexture = new Texture2D(sourceTexture.width, sourceTexture.height);

		// Iterate through each pixel
		for (int y = 0; y < sourceTexture.height; y++)
		{
			for (int x = 0; x < sourceTexture.width; x++)
			{
				// Get the color of the pixels from both textures
				Color sourceColor = sourceTexture.GetPixel(x, y);
				Color subtractColor = subtractTexture.GetPixel(x, y);

				// Subtract the color values
				Color resultColor = new Color(
					Mathf.Max(0, sourceColor.r - subtractColor.r),
					Mathf.Max(0, sourceColor.g - subtractColor.g),
					Mathf.Max(0, sourceColor.b - subtractColor.b),
					Mathf.Max(0, sourceColor.a - subtractColor.a)
				);

				// Set the resulting color to the new texture
				resultTexture.SetPixel(x, y, resultColor);
			}
		}

		// Apply changes and update the texture
		resultTexture.Apply();
		// Assign the resulting texture to a material or UI element
		GetComponent<RawImage>().texture = resultTexture;
	}
}
