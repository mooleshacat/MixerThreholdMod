using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000038 RID: 56
[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileText : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x0600012F RID: 303 RVA: 0x000045B1 File Offset: 0x000027B1
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00006ECB File Offset: 0x000050CB
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00006EEC File Offset: 0x000050EC
	public void OnClick()
	{
		string text = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "txt");
		if (!string.IsNullOrEmpty(text))
		{
			File.WriteAllText(text, this._data);
		}
	}

	// Token: 0x04000102 RID: 258
	public Text output;

	// Token: 0x04000103 RID: 259
	private string _data = "Example text created by StandaloneFileBrowser";
}
