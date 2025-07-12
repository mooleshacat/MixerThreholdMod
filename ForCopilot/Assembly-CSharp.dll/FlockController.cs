using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class FlockController : MonoBehaviour
{
	// Token: 0x060001AE RID: 430 RVA: 0x00009A1C File Offset: 0x00007C1C
	public void Start()
	{
		this._thisT = base.transform;
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		this._posBuffer = this._thisT.position + this._startPosOffset;
		if (!this._slowSpawn)
		{
			this.AddChild(this._childAmount);
		}
		if (this._randomPositionTimer > 0f)
		{
			base.InvokeRepeating("SetFlockRandomPosition", this._randomPositionTimer, this._randomPositionTimer);
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00009ABC File Offset: 0x00007CBC
	public void AddChild(int amount)
	{
		if (this._groupChildToNewTransform)
		{
			this.InstantiateGroup();
		}
		for (int i = 0; i < amount; i++)
		{
			FlockChild flockChild = UnityEngine.Object.Instantiate<FlockChild>(this._childPrefab);
			flockChild._spawner = this;
			this._roamers.Add(flockChild);
			this.AddChildToParent(flockChild.transform);
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00009B0E File Offset: 0x00007D0E
	public void AddChildToParent(Transform obj)
	{
		if (this._groupChildToFlock)
		{
			obj.parent = base.transform;
			return;
		}
		if (this._groupChildToNewTransform)
		{
			obj.parent = this._groupTransform;
			return;
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00009B3C File Offset: 0x00007D3C
	public void RemoveChild(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Component component = this._roamers[this._roamers.Count - 1];
			this._roamers.RemoveAt(this._roamers.Count - 1);
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00009B90 File Offset: 0x00007D90
	public void Update()
	{
		if (this._activeChildren > 0f)
		{
			if (this._updateDivisor > 1)
			{
				this._updateCounter++;
				this._updateCounter %= this._updateDivisor;
				this._newDelta = Time.deltaTime * (float)this._updateDivisor;
			}
			else
			{
				this._newDelta = Time.deltaTime;
			}
		}
		this.UpdateChildAmount();
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00009BFC File Offset: 0x00007DFC
	public void InstantiateGroup()
	{
		if (this._groupTransform != null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		this._groupTransform = gameObject.transform;
		this._groupTransform.position = this._thisT.position;
		if (this._groupName != "")
		{
			gameObject.name = this._groupName;
			return;
		}
		gameObject.name = this._thisT.name + " Fish Container";
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00009C7A File Offset: 0x00007E7A
	public void UpdateChildAmount()
	{
		if (this._childAmount >= 0 && this._childAmount < this._roamers.Count)
		{
			this.RemoveChild(1);
			return;
		}
		if (this._childAmount > this._roamers.Count)
		{
			this.AddChild(1);
		}
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00009CBC File Offset: 0x00007EBC
	public void OnDrawGizmos()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (!Application.isPlaying && this._posBuffer != this._thisT.position + this._startPosOffset)
		{
			this._posBuffer = this._thisT.position + this._startPosOffset;
		}
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this._posBuffer, new Vector3(this._spawnSphere * 2f, this._spawnSphereHeight * 2f, this._spawnSphereDepth * 2f));
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this._thisT.position, new Vector3(this._positionSphere * 2f + this._spawnSphere * 2f, this._positionSphereHeight * 2f + this._spawnSphereHeight * 2f, this._positionSphereDepth * 2f + this._spawnSphereDepth * 2f));
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00009E04 File Offset: 0x00008004
	public void SetFlockRandomPosition()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._positionSphere, this._positionSphere) + this._thisT.position.x;
		zero.z = UnityEngine.Random.Range(-this._positionSphereDepth, this._positionSphereDepth) + this._thisT.position.z;
		zero.y = UnityEngine.Random.Range(-this._positionSphereHeight, this._positionSphereHeight) + this._thisT.position.y;
		this._posBuffer = zero;
		if (this._forceChildWaypoints)
		{
			for (int i = 0; i < this._roamers.Count; i++)
			{
				this._roamers[i].Wander(UnityEngine.Random.value * this._forcedRandomDelay);
			}
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00009ED8 File Offset: 0x000080D8
	public void destroyBirds()
	{
		for (int i = 0; i < this._roamers.Count; i++)
		{
			UnityEngine.Object.Destroy(this._roamers[i].gameObject);
		}
		this._childAmount = 0;
		this._roamers.Clear();
	}

	// Token: 0x04000191 RID: 401
	public FlockChild _childPrefab;

	// Token: 0x04000192 RID: 402
	public int _childAmount = 250;

	// Token: 0x04000193 RID: 403
	public bool _slowSpawn;

	// Token: 0x04000194 RID: 404
	public float _spawnSphere = 3f;

	// Token: 0x04000195 RID: 405
	public float _spawnSphereHeight = 3f;

	// Token: 0x04000196 RID: 406
	public float _spawnSphereDepth = -1f;

	// Token: 0x04000197 RID: 407
	public float _minSpeed = 6f;

	// Token: 0x04000198 RID: 408
	public float _maxSpeed = 10f;

	// Token: 0x04000199 RID: 409
	public float _minScale = 0.7f;

	// Token: 0x0400019A RID: 410
	public float _maxScale = 1f;

	// Token: 0x0400019B RID: 411
	public float _soarFrequency;

	// Token: 0x0400019C RID: 412
	public string _soarAnimation = "Soar";

	// Token: 0x0400019D RID: 413
	public string _flapAnimation = "Flap";

	// Token: 0x0400019E RID: 414
	public string _idleAnimation = "Idle";

	// Token: 0x0400019F RID: 415
	public float _diveValue = 7f;

	// Token: 0x040001A0 RID: 416
	public float _diveFrequency = 0.5f;

	// Token: 0x040001A1 RID: 417
	public float _minDamping = 1f;

	// Token: 0x040001A2 RID: 418
	public float _maxDamping = 2f;

	// Token: 0x040001A3 RID: 419
	public float _waypointDistance = 1f;

	// Token: 0x040001A4 RID: 420
	public float _minAnimationSpeed = 2f;

	// Token: 0x040001A5 RID: 421
	public float _maxAnimationSpeed = 4f;

	// Token: 0x040001A6 RID: 422
	public float _randomPositionTimer = 10f;

	// Token: 0x040001A7 RID: 423
	public float _positionSphere = 25f;

	// Token: 0x040001A8 RID: 424
	public float _positionSphereHeight = 25f;

	// Token: 0x040001A9 RID: 425
	public float _positionSphereDepth = -1f;

	// Token: 0x040001AA RID: 426
	public bool _childTriggerPos;

	// Token: 0x040001AB RID: 427
	public bool _forceChildWaypoints;

	// Token: 0x040001AC RID: 428
	public float _forcedRandomDelay = 1.5f;

	// Token: 0x040001AD RID: 429
	public bool _flatFly;

	// Token: 0x040001AE RID: 430
	public bool _flatSoar;

	// Token: 0x040001AF RID: 431
	public bool _birdAvoid;

	// Token: 0x040001B0 RID: 432
	public int _birdAvoidHorizontalForce = 1000;

	// Token: 0x040001B1 RID: 433
	public bool _birdAvoidDown;

	// Token: 0x040001B2 RID: 434
	public bool _birdAvoidUp;

	// Token: 0x040001B3 RID: 435
	public int _birdAvoidVerticalForce = 300;

	// Token: 0x040001B4 RID: 436
	public float _birdAvoidDistanceMax = 4.5f;

	// Token: 0x040001B5 RID: 437
	public float _birdAvoidDistanceMin = 5f;

	// Token: 0x040001B6 RID: 438
	public float _soarMaxTime;

	// Token: 0x040001B7 RID: 439
	public LayerMask _avoidanceMask = -1;

	// Token: 0x040001B8 RID: 440
	public List<FlockChild> _roamers;

	// Token: 0x040001B9 RID: 441
	public Vector3 _posBuffer;

	// Token: 0x040001BA RID: 442
	public int _updateDivisor = 1;

	// Token: 0x040001BB RID: 443
	public float _newDelta;

	// Token: 0x040001BC RID: 444
	public int _updateCounter;

	// Token: 0x040001BD RID: 445
	public float _activeChildren;

	// Token: 0x040001BE RID: 446
	public bool _groupChildToNewTransform;

	// Token: 0x040001BF RID: 447
	public Transform _groupTransform;

	// Token: 0x040001C0 RID: 448
	public string _groupName = "";

	// Token: 0x040001C1 RID: 449
	public bool _groupChildToFlock;

	// Token: 0x040001C2 RID: 450
	public Vector3 _startPosOffset;

	// Token: 0x040001C3 RID: 451
	public Transform _thisT;
}
