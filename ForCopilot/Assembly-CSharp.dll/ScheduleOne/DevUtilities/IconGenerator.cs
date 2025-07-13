using System;
using System.Collections.Generic;
using System.IO;
using EasyButtons;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200071B RID: 1819
	public class IconGenerator : Singleton<IconGenerator>
	{
		// Token: 0x06003152 RID: 12626 RVA: 0x000CDED0 File Offset: 0x000CC0D0
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.gameObject.SetActive(false);
			this.CameraPosition.gameObject.SetActive(false);
			this.CameraPosition.clearFlags = CameraClearFlags.Color;
			if (this.Registry == null)
			{
				this.Registry = Singleton<Registry>.Instance;
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000CDF2C File Offset: 0x000CC12C
		[Button]
		public void GenerateIcon()
		{
			LayerUtility.SetLayerRecursively(this.ItemContainer.gameObject, LayerMask.NameToLayer("IconGeneration"));
			Transform transform = null;
			for (int i = 0; i < this.ItemContainer.transform.childCount; i++)
			{
				if (this.ItemContainer.transform.GetChild(i).gameObject.activeSelf)
				{
					transform = this.ItemContainer.transform.GetChild(i);
				}
			}
			string text = this.OutputPath + "/" + transform.name + "_Icon.png";
			Texture2D texture = this.GetTexture(transform.transform);
			Debug.Log("Writing to: " + text);
			byte[] bytes = ImageConversion.EncodeToPNG(texture);
			File.WriteAllBytes(text, bytes);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000CDFE4 File Offset: 0x000CC1E4
		public Texture2D GeneratePackagingIcon(string packagingID, string productID)
		{
			if (Singleton<Registry>.Instance != null)
			{
				this.Registry = Singleton<Registry>.Instance;
			}
			IconGenerator.PackagingVisuals packagingVisuals = this.Visuals.Find((IconGenerator.PackagingVisuals x) => packagingID == x.PackagingID);
			if (packagingVisuals == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Failed to find visuals for packaging (",
					packagingID,
					") containing product (",
					productID,
					")"
				}));
				return null;
			}
			ItemDefinition itemDefinition = this.Registry._GetItem(productID, true);
			if (Application.isPlaying)
			{
				itemDefinition = Singleton<Registry>.Instance._GetItem(productID, true);
			}
			ProductDefinition productDefinition = itemDefinition as ProductDefinition;
			if (productDefinition == null)
			{
				Debug.LogError("Failed to find product definition for product (" + productID + ")");
				return null;
			}
			(productDefinition.GetDefaultInstance(1) as ProductItemInstance).SetupPackagingVisuals(packagingVisuals.Visuals);
			packagingVisuals.Visuals.gameObject.SetActive(true);
			Texture2D texture = this.GetTexture(packagingVisuals.Visuals.transform.parent);
			packagingVisuals.Visuals.gameObject.SetActive(false);
			return texture;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000CE100 File Offset: 0x000CC300
		public Texture2D GetTexture(Transform model)
		{
			this.MainContainer.gameObject.SetActive(true);
			bool activeSelf = this.ItemContainer.gameObject.activeSelf;
			this.ItemContainer.gameObject.SetActive(true);
			if (this.ModifyLighting)
			{
				RenderSettings.ambientMode = AmbientMode.Flat;
				RenderSettings.ambientLight = Color.white;
			}
			RuntimePreviewGenerator.CamPos = this.CameraPosition.transform.position;
			RuntimePreviewGenerator.CamRot = this.CameraPosition.transform.rotation;
			RuntimePreviewGenerator.Padding = 0f;
			RuntimePreviewGenerator.UseLocalBounds = true;
			RuntimePreviewGenerator.BackgroundColor = new Color32(0, 0, 0, 0);
			Texture2D result = RuntimePreviewGenerator.GenerateModelPreview(model, this.IconSize, this.IconSize, false, true);
			RenderSettings.ambientMode = AmbientMode.Trilight;
			this.MainContainer.gameObject.SetActive(false);
			this.ItemContainer.gameObject.SetActive(activeSelf);
			return result;
		}

		// Token: 0x040022B8 RID: 8888
		public int IconSize = 512;

		// Token: 0x040022B9 RID: 8889
		public string OutputPath;

		// Token: 0x040022BA RID: 8890
		public bool ModifyLighting = true;

		// Token: 0x040022BB RID: 8891
		[Header("References")]
		public Registry Registry;

		// Token: 0x040022BC RID: 8892
		public Camera CameraPosition;

		// Token: 0x040022BD RID: 8893
		public Transform MainContainer;

		// Token: 0x040022BE RID: 8894
		public Transform ItemContainer;

		// Token: 0x040022BF RID: 8895
		public GameObject Canvas;

		// Token: 0x040022C0 RID: 8896
		public List<IconGenerator.PackagingVisuals> Visuals;

		// Token: 0x0200071C RID: 1820
		[Serializable]
		public class PackagingVisuals
		{
			// Token: 0x040022C1 RID: 8897
			public string PackagingID;

			// Token: 0x040022C2 RID: 8898
			public FilledPackagingVisuals Visuals;
		}
	}
}
