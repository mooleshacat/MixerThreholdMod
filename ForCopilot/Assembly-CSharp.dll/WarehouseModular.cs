using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000049 RID: 73
[ExecuteInEditMode]
public class WarehouseModular : MonoBehaviour
{
	// Token: 0x0600017B RID: 379 RVA: 0x0000807C File Offset: 0x0000627C
	private void Start()
	{
		this.myMeshFilter = base.GetComponent<MeshFilter>();
		this.largeWall = (Resources.Load("Models/LargeWall") as GameObject);
		this.mediumWall = (Resources.Load("Models/MediumWall") as GameObject);
		this.smallWall = (Resources.Load("Models/SmallWall") as GameObject);
		this.miniWall = (Resources.Load("Models/Extra_SmallWall") as GameObject);
		this.tinyWall = (Resources.Load("Models/Extra_SmallWall1") as GameObject);
		this.windowWall = (Resources.Load("Models/WindowWall") as GameObject);
		this.smallWindowWall = (Resources.Load("Models/SmallWindowWall") as GameObject);
		this.innerCorner = (Resources.Load("Models/LeftCorner") as GameObject);
		this.outerCorner = (Resources.Load("Models/RightCorner") as GameObject);
		this.garageFrame = (Resources.Load("Models/GarageDoorFrame") as GameObject);
		this.doorFrame = (Resources.Load("Models/DoorWall") as GameObject);
		this.doubleDoorFrame = (Resources.Load("Models/DoubleDoorWall") as GameObject);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00008194 File Offset: 0x00006394
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
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(item, child.position, child.rotation);
		gameObject2.transform.SetParent(base.transform);
		this.itemsList.Add(gameObject2);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00008230 File Offset: 0x00006430
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

	// Token: 0x04000139 RID: 313
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x0400013A RID: 314
	[HideInInspector]
	public GameObject largeWall;

	// Token: 0x0400013B RID: 315
	[HideInInspector]
	public GameObject mediumWall;

	// Token: 0x0400013C RID: 316
	[HideInInspector]
	public GameObject smallWall;

	// Token: 0x0400013D RID: 317
	[HideInInspector]
	public GameObject miniWall;

	// Token: 0x0400013E RID: 318
	[HideInInspector]
	public GameObject tinyWall;

	// Token: 0x0400013F RID: 319
	[HideInInspector]
	public GameObject windowWall;

	// Token: 0x04000140 RID: 320
	[HideInInspector]
	public GameObject smallWindowWall;

	// Token: 0x04000141 RID: 321
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x04000142 RID: 322
	[HideInInspector]
	public GameObject outerCorner;

	// Token: 0x04000143 RID: 323
	[HideInInspector]
	public GameObject garageFrame;

	// Token: 0x04000144 RID: 324
	[HideInInspector]
	public GameObject doorFrame;

	// Token: 0x04000145 RID: 325
	[HideInInspector]
	public GameObject doubleDoorFrame;

	// Token: 0x04000146 RID: 326
	private MeshFilter myMeshFilter;
}
