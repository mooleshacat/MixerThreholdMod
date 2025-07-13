using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000041 RID: 65
[ExecuteInEditMode]
public class ACpipeModular : MonoBehaviour
{
	// Token: 0x06000159 RID: 345 RVA: 0x000079C8 File Offset: 0x00005BC8
	private void Start()
	{
		this.largeACPipe = (Resources.Load("Models/AC_Pipe_Long") as GameObject);
		this.smallACpipe = (Resources.Load("Models/AC_Pipe_Medium") as GameObject);
		this.innerCorner = (Resources.Load("Models/AC_Pipe_Side_left") as GameObject);
		this.outerCorner = (Resources.Load("Models/AC_Pipe_Side_Right") as GameObject);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00007A2C File Offset: 0x00005C2C
	public void BuildNextItem(GameObject item)
	{
		if (this.itemsList.Count == 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, base.transform.position, item.transform.rotation);
			gameObject.transform.SetParent(base.transform);
			this.itemsList.Add(gameObject);
			return;
		}
		Transform child = this.itemsList.Last<GameObject>().transform.GetChild(0);
		MonoBehaviour.print(child.gameObject.transform.parent.gameObject.name);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(item, child.position, child.rotation);
		gameObject2.transform.SetParent(base.transform);
		this.itemsList.Add(gameObject2);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00007AE8 File Offset: 0x00005CE8
	public void DeleteLastItem()
	{
		GameObject gameObject = this.itemsList.Last<GameObject>();
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (Application.isEditor)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		this.itemsList.Remove(gameObject);
	}

	// Token: 0x04000121 RID: 289
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x04000122 RID: 290
	[HideInInspector]
	public GameObject largeACPipe;

	// Token: 0x04000123 RID: 291
	[HideInInspector]
	public GameObject smallACpipe;

	// Token: 0x04000124 RID: 292
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x04000125 RID: 293
	[HideInInspector]
	public GameObject outerCorner;
}
