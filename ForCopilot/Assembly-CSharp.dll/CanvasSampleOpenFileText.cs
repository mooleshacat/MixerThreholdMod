using System;
using System.Collections;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000033 RID: 51
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileText : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000114 RID: 276 RVA: 0x000045B1 File Offset: 0x000027B1
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00006B93 File Offset: 0x00004D93
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00006BB4 File Offset: 0x00004DB4
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", false);
		if (array.Length != 0)
		{
			base.StartCoroutine(this.OutputRoutine(new Uri(array[0]).AbsoluteUri));
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00006BF5 File Offset: 0x00004DF5
	private IEnumerator OutputRoutine(string url)
	{
		WWW loader = new WWW(url);
		yield return loader;
		this.output.text = loader.text;
		yield break;
	}

	// Token: 0x040000F2 RID: 242
	public Text output;
}
