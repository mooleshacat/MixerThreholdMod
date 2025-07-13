using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001F9 RID: 505
	[CreateAssetMenu(fileName = "ScriptableObjectExample.asset", menuName = "EasyButtons/ScriptableObjectExample")]
	public class ScriptableObjectExample : ScriptableObject
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x00030F47 File Offset: 0x0002F147
		[Button]
		public void SayHello()
		{
			Debug.Log("Hello");
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00030EC7 File Offset: 0x0002F0C7
		[Button(Mode = 2)]
		public void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00030ED3 File Offset: 0x0002F0D3
		[Button(Mode = 1)]
		public void SayHelloPlayMode()
		{
			Debug.Log("Hello from play mode");
		}
	}
}
