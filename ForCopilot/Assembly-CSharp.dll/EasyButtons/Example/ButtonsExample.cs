using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001F7 RID: 503
	public class ButtonsExample : MonoBehaviour
	{
		// Token: 0x06000B23 RID: 2851 RVA: 0x00030EBA File Offset: 0x0002F0BA
		[Button]
		public void SayMyName()
		{
			Debug.Log(base.name);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00030EC7 File Offset: 0x0002F0C7
		[Button(Mode = 2)]
		protected void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00030ED3 File Offset: 0x0002F0D3
		[Button(Mode = 1)]
		private void SayHelloInRuntime()
		{
			Debug.Log("Hello from play mode");
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00030EDF File Offset: 0x0002F0DF
		[Button("Special Name", Spacing = 1)]
		private void TestButtonName()
		{
			Debug.Log("Hello from special name button");
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00030EEB File Offset: 0x0002F0EB
		[Button("Special Name Editor Only", Mode = 2)]
		private void TestButtonNameEditorOnly()
		{
			Debug.Log("Hello from special name button for editor only");
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00030EF7 File Offset: 0x0002F0F7
		[Button]
		private static void TestStaticMethod()
		{
			Debug.Log("Hello from static method");
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00030F03 File Offset: 0x0002F103
		[Button("Space Before and After", Spacing = 3)]
		private void TestButtonSpaceBoth()
		{
			Debug.Log("Hello from a button surround by spaces");
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00030F0F File Offset: 0x0002F10F
		[Button("Button With Parameters")]
		private void TestButtonWithParams(string message, int number)
		{
			Debug.Log(string.Format("Your message #{0}: \"{1}\"", number, message));
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00030F27 File Offset: 0x0002F127
		[Button("Expanded Button Example", Expanded = true)]
		private void TestExpandedButton(string message)
		{
			Debug.Log(message);
		}
	}
}
