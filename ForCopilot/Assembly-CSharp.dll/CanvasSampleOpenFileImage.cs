using System;
using System.Collections;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000031 RID: 49
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileImage : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000109 RID: 265 RVA: 0x000045B1 File Offset: 0x000027B1
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00006A93 File Offset: 0x00004C93
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006AB4 File Offset: 0x00004CB4
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Title", "", ".png", false);
		if (array.Length != 0)
		{
			base.StartCoroutine(this.OutputRoutine(new Uri(array[0]).AbsoluteUri));
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00006AF5 File Offset: 0x00004CF5
	private IEnumerator OutputRoutine(string url)
	{
		WWW loader = new WWW(url);
		yield return loader;
		this.output.texture = loader.texture;
		yield break;
	}

	// Token: 0x040000EC RID: 236
	public RawImage output;
}
