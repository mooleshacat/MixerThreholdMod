using System;
using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x0600011F RID: 287 RVA: 0x000045B1 File Offset: 0x000027B1
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00006C93 File Offset: 0x00004E93
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00006CB4 File Offset: 0x00004EB4
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
		if (array.Length != 0)
		{
			List<string> list = new List<string>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(new Uri(array[i]).AbsoluteUri);
			}
			base.StartCoroutine(this.OutputRoutine(list.ToArray()));
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00006D18 File Offset: 0x00004F18
	private IEnumerator OutputRoutine(string[] urlArr)
	{
		string outputText = "";
		int num;
		for (int i = 0; i < urlArr.Length; i = num + 1)
		{
			WWW loader = new WWW(urlArr[i]);
			yield return loader;
			outputText += loader.text;
			loader = null;
			num = i;
		}
		this.output.text = outputText;
		yield break;
	}

	// Token: 0x040000F8 RID: 248
	public Text output;
}
