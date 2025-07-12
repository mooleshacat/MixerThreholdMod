using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
[ExecuteAlways]
public class MaterialChanger : MonoBehaviour
{
	// Token: 0x06000251 RID: 593 RVA: 0x0000D77E File Offset: 0x0000B97E
	private void OnEnable()
	{
		this.FindAllMaterialInChild();
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000D786 File Offset: 0x0000B986
	private void Update()
	{
		this._propBlock = new MaterialPropertyBlock();
		this.SetNewValueForAllMaterial(this._value);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000D79F File Offset: 0x0000B99F
	private void FindAllMaterialInChild()
	{
		this._renderers = base.transform.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
	private void SetNewValueForAllMaterial(float value)
	{
		this.FindAllMaterialInChild();
		for (int i = 0; i < this._renderers.Length; i++)
		{
			this._renderers[i].GetPropertyBlock(this._propBlock);
			this._propBlock.SetFloat(this._changeMaterialSetting, value);
			this._renderers[i].SetPropertyBlock(this._propBlock);
		}
	}

	// Token: 0x04000271 RID: 625
	[SerializeField]
	[Range(0f, 5f)]
	private float _value = 1f;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	private string _changeMaterialSetting = "_Worn_Level";

	// Token: 0x04000273 RID: 627
	private Renderer[] _renderers;

	// Token: 0x04000274 RID: 628
	private MaterialPropertyBlock _propBlock;
}
