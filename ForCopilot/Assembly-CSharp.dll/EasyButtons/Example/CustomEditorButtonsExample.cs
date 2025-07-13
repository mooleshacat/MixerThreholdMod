using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001F8 RID: 504
	public class CustomEditorButtonsExample : MonoBehaviour
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x00030F2F File Offset: 0x0002F12F
		[Button("Custom Editor Example")]
		private void SayHello()
		{
			Debug.Log("Hello from custom editor");
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00030F3B File Offset: 0x0002F13B
		[Button]
		private void SecondButton()
		{
			Debug.Log("Second button of the custom editor.");
		}
	}
}
