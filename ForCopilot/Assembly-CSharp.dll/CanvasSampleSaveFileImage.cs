using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileImage : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x0600012A RID: 298 RVA: 0x00006E10 File Offset: 0x00005010
	private void Awake()
	{
		int num = 100;
		int num2 = 100;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				texture2D.SetPixel(i, j, Color.red);
			}
		}
		texture2D.Apply();
		this._textureBytes = ImageConversion.EncodeToPNG(texture2D);
		UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000045B1 File Offset: 0x000027B1
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00006E6F File Offset: 0x0000506F
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00006E90 File Offset: 0x00005090
	public void OnClick()
	{
		string text = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "png");
		if (!string.IsNullOrEmpty(text))
		{
			File.WriteAllBytes(text, this._textureBytes);
		}
	}

	// Token: 0x04000100 RID: 256
	public Text output;

	// Token: 0x04000101 RID: 257
	private byte[] _textureBytes;
}
