using System;
using System.Collections.Generic;
using EasyButtons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002AC RID: 684
	public class Snake : TVApp
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003F5CB File Offset: 0x0003D7CB
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x0003F5D3 File Offset: 0x0003D7D3
		public Vector2 HeadPosition { get; private set; } = new Vector2(10f, 6f);

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x0003F5E4 File Offset: 0x0003D7E4
		public List<Vector2> Tail { get; private set; } = new List<Vector2>();

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0003F5ED File Offset: 0x0003D7ED
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x0003F5F5 File Offset: 0x0003D7F5
		public Vector2 LastTailPosition { get; private set; } = Vector2.zero;

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0003F5FE File Offset: 0x0003D7FE
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x0003F606 File Offset: 0x0003D806
		public Vector2 Direction { get; private set; } = Vector2.right;

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0003F60F File Offset: 0x0003D80F
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x0003F617 File Offset: 0x0003D817
		public Vector2 QueuedDirection { get; private set; } = Vector2.right;

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0003F620 File Offset: 0x0003D820
		// (set) Token: 0x06000E4D RID: 3661 RVA: 0x0003F628 File Offset: 0x0003D828
		public Vector2 NextDirection { get; private set; } = Vector2.zero;

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0003F631 File Offset: 0x0003D831
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x0003F639 File Offset: 0x0003D839
		public Snake.EGameState GameState { get; private set; }

		// Token: 0x06000E50 RID: 3664 RVA: 0x0003F642 File Offset: 0x0003D842
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0003F64C File Offset: 0x0003D84C
		private void Update()
		{
			if (base.IsPaused)
			{
				return;
			}
			if (!base.IsOpen)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateMovement();
			this._timeOnGameOver += Time.deltaTime;
			this.ScoreText.text = this.Tail.Count.ToString();
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
		private void UpdateInput()
		{
			if (this._timeOnGameOver < 0.3f)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Forward) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (this.Direction != Vector2.down)
				{
					this.QueuedDirection = Vector2.up;
				}
				this.NextDirection = Vector2.up;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.up);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Backward) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (this.Direction != Vector2.up)
				{
					this.QueuedDirection = Vector2.down;
				}
				this.NextDirection = Vector2.down;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.down);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (this.Direction != Vector2.right)
				{
					this.QueuedDirection = Vector2.left;
				}
				this.NextDirection = Vector2.left;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.left);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Right) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (this.Direction != Vector2.left)
				{
					this.QueuedDirection = Vector2.right;
				}
				this.NextDirection = Vector2.right;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.right);
				}
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0003F808 File Offset: 0x0003DA08
		private void UpdateMovement()
		{
			if (this.GameState != Snake.EGameState.Playing)
			{
				return;
			}
			this._timeSinceLastMove += Time.deltaTime;
			if (this._timeSinceLastMove >= this.TimePerTile)
			{
				this._timeSinceLastMove -= this.TimePerTile;
				this.MoveSnake();
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0003F858 File Offset: 0x0003DA58
		private void MoveSnake()
		{
			this.Direction = this.QueuedDirection;
			Vector2 vector = this.HeadPosition + this.Direction;
			SnakeTile tile = this.GetTile(vector);
			if (tile == null)
			{
				this.GameOver();
				return;
			}
			if (tile.Type == SnakeTile.TileType.Snake && this.Tail.Count > 0 && tile.Position != this.Tail[this.Tail.Count - 1])
			{
				this.GameOver();
				return;
			}
			bool flag = false;
			if (tile.Type == SnakeTile.TileType.Food)
			{
				this.Eat();
				flag = true;
				if (this.GameState != Snake.EGameState.Playing)
				{
					return;
				}
			}
			this.GetTile(vector).SetType(SnakeTile.TileType.Snake, 0);
			Vector2 vector2 = this.HeadPosition;
			this.HeadPosition = vector;
			for (int i = 0; i < this.Tail.Count; i++)
			{
				if (i == this.Tail.Count - 1)
				{
					this.LastTailPosition = this.Tail[i];
				}
				Vector2 vector3 = this.Tail[i];
				this.Tail[i] = vector2;
				this.GetTile(this.Tail[i]).SetType(SnakeTile.TileType.Snake, 1 + i);
				vector2 = vector3;
			}
			this.GetTile(vector2).SetType(SnakeTile.TileType.Empty, 0);
			this.LastTailPosition = vector2;
			if (this.NextDirection != Vector2.zero && this.NextDirection != -this.Direction)
			{
				this.QueuedDirection = this.NextDirection;
			}
			if (flag)
			{
				this.SpawnFood();
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0003F9E4 File Offset: 0x0003DBE4
		private SnakeTile GetTile(Vector2 position)
		{
			if (position.x < 0f || position.x >= 20f || position.y < 0f || position.y >= 12f)
			{
				return null;
			}
			return this.Tiles[(int)position.y * 20 + (int)position.x];
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0003FA40 File Offset: 0x0003DC40
		private void StartGame(Vector2 initialDir)
		{
			SnakeTile tile = this.GetTile(this.lastFoodPosition);
			if (tile != null)
			{
				tile.SetType(SnakeTile.TileType.Empty, 0);
			}
			this.SpawnFood();
			SnakeTile tile2 = this.GetTile(this.HeadPosition);
			if (tile2 != null)
			{
				tile2.SetType(SnakeTile.TileType.Empty, 0);
			}
			this.HeadPosition = new Vector2(10f, 6f);
			for (int i = 0; i < this.Tail.Count; i++)
			{
				this.GetTile(this.Tail[i]).SetType(SnakeTile.TileType.Empty, 0);
			}
			this.Tail.Clear();
			this.QueuedDirection = initialDir;
			this.NextDirection = Vector2.zero;
			this._timeSinceLastMove = 0f;
			this.MoveSnake();
			this.GameState = Snake.EGameState.Playing;
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0003FB16 File Offset: 0x0003DD16
		private void Eat()
		{
			this.Tail.Add(this.LastTailPosition);
			if (this.onEat != null)
			{
				this.onEat.Invoke();
			}
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0003FB3C File Offset: 0x0003DD3C
		private void SpawnFood()
		{
			List<SnakeTile> list = new List<SnakeTile>();
			foreach (SnakeTile snakeTile in this.Tiles)
			{
				if (snakeTile.Type == SnakeTile.TileType.Empty)
				{
					list.Add(snakeTile);
				}
			}
			if (list.Count == 0)
			{
				this.Win();
				return;
			}
			SnakeTile snakeTile2 = list[UnityEngine.Random.Range(0, list.Count)];
			snakeTile2.SetType(SnakeTile.TileType.Food, 0);
			this.lastFoodPosition = snakeTile2.Position;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0003FBB0 File Offset: 0x0003DDB0
		private void GameOver()
		{
			this.GameState = Snake.EGameState.Ready;
			this._timeOnGameOver = 0f;
			if (this.onGameOver != null)
			{
				this.onGameOver.Invoke();
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0003FBD7 File Offset: 0x0003DDD7
		private void Win()
		{
			this.GameState = Snake.EGameState.Ready;
			this._timeOnGameOver = 0f;
			if (this.onWin != null)
			{
				this.onWin.Invoke();
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0003FBFE File Offset: 0x0003DDFE
		protected override void TryPause()
		{
			if (this.GameState == Snake.EGameState.Ready)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0003FC18 File Offset: 0x0003DE18
		[Button]
		public void CreateTiles()
		{
			SnakeTile[] tiles = this.Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(tiles[i].gameObject);
			}
			this.Tiles = new SnakeTile[240];
			float tileSize = this.PlaySpace.rect.width / 20f;
			for (int j = 0; j < 12; j++)
			{
				for (int k = 0; k < 20; k++)
				{
					SnakeTile snakeTile = UnityEngine.Object.Instantiate<SnakeTile>(this.TilePrefab, this.PlaySpace);
					snakeTile.SetType(SnakeTile.TileType.Empty, 0);
					snakeTile.SetPosition(new Vector2((float)k, (float)j), tileSize);
					this.Tiles[j * 20 + k] = snakeTile;
				}
			}
		}

		// Token: 0x04000EC1 RID: 3777
		public const int SIZE_X = 20;

		// Token: 0x04000EC2 RID: 3778
		public const int SIZE_Y = 12;

		// Token: 0x04000EC3 RID: 3779
		[Header("Settings")]
		public SnakeTile TilePrefab;

		// Token: 0x04000EC4 RID: 3780
		public float TimePerTile = 0.4f;

		// Token: 0x04000EC5 RID: 3781
		[Header("References")]
		public RectTransform PlaySpace;

		// Token: 0x04000EC6 RID: 3782
		public SnakeTile[] Tiles;

		// Token: 0x04000EC7 RID: 3783
		public TextMeshProUGUI ScoreText;

		// Token: 0x04000ECE RID: 3790
		private Vector2 lastFoodPosition = Vector2.zero;

		// Token: 0x04000ED0 RID: 3792
		private float _timeSinceLastMove;

		// Token: 0x04000ED1 RID: 3793
		private float _timeOnGameOver;

		// Token: 0x04000ED2 RID: 3794
		public UnityEvent onStart;

		// Token: 0x04000ED3 RID: 3795
		public UnityEvent onEat;

		// Token: 0x04000ED4 RID: 3796
		public UnityEvent onGameOver;

		// Token: 0x04000ED5 RID: 3797
		public UnityEvent onWin;

		// Token: 0x020002AD RID: 685
		public enum EGameState
		{
			// Token: 0x04000ED7 RID: 3799
			Ready,
			// Token: 0x04000ED8 RID: 3800
			Playing
		}
	}
}
