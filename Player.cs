namespace Pathfinder_Algorithm {
	class Pos {
		public Pos(int y, int x) { Y = y; X = x; }
		public int Y;
		public int X;
	}
	internal class Player {
		public int PosY { get; private set; }
		public int PosX { get; private set; }
		Random _random = new Random();
		Maze _maze;

		List<Pos> _paths = new List<Pos>();
		public enum Dir {
			Up,
			Left,
			Down,
			Right
		}
		public void Init(int posY, int posX, Maze maze) {
            PosY = posY;
			PosX = posX;

			_maze = maze;

			//RightHand();

			BFS();
		}

		private void RightHand() {
			_paths.Add(new Pos(PosY, PosX));

			int _dir = (int)Dir.Up;

			int[] frontY = new int[] { -1, 0, 1, 0 };
			int[] frontX = new int[] { 0, -1, 0, 1 };
			int[] rightY = new int[] { 0, -1, 0, 1 };
			int[] rightX = new int[] { 1, 0, -1, 0 };

			//미로 탈출 오른손 법칙
			while (PosY != _maze.DestY || PosX != _maze.DestX) {
				if (_maze.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] != Maze.TileType.Wall) {
					//바라보는 방향의 오른쪽으로 갈 수 있는가
					//오른쪽으로 돌기
					_dir = (_dir - 1 + 4) % 4;
					//전진
					PosY += frontY[_dir];
					PosX += frontX[_dir];
					_paths.Add(new Pos(PosY, PosX));
				} else if (_maze.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] != Maze.TileType.Wall) {
					//바라보는 방향으로 전진할 수 있는가
					PosY += frontY[_dir];
					PosX += frontX[_dir];
					_paths.Add(new Pos(PosY, PosX));
				} else {
					//왼쪽으로 돌기
					_dir = (_dir + 1 + 4) % 4;
				}
			}
		} //오른손법칙 알고리즘

		private void BFS() {
			int[] deltaY = new int[] { -1, 0, 1, 0 };
			int[] deltaX = new int[] { 0, -1, 0, 1 };

			bool[,] found = new bool[_maze.Size,_maze.Size];
			Pos[,] parent = new Pos[_maze.Size, _maze.Size];

			Queue<Pos> q = new Queue<Pos>();
			q.Enqueue(new Pos(PosY, PosX));
			found[PosY, PosX] = true;
			parent[PosY, PosX] = new Pos(PosY, PosX);

			while (q.Count > 0) {
				Pos pos = q.Dequeue();
				int nowY = pos.Y;
				int nowX = pos.X;

				for (int i = 0; i < 4; i++) {
					int nextY = nowY + deltaY[i];
					int nextX = nowX + deltaX[i];

					if (nextX < 0 || nextX >= _maze.Size || nextY < 0 || nextY >= _maze.Size) { continue; }
					if (_maze.Tile[nextY, nextX] == Maze.TileType.Wall) { continue; }
					if (found[nextY, nextX]) { continue; }

					q.Enqueue(new Pos(nextY, nextX));
					found[nextY, nextX] = true;
					parent[nextY, nextX] = new Pos(nowY, nowX);
				}
			}

			int y = _maze.DestY;
			int x = _maze.DestX;

			Console.WriteLine(y + " " + x);
			while (parent[y, x].Y != y || parent[y, x].X != x) {
				_paths.Add(new Pos(y, x));
				Pos pos = parent[y, x];
				y = pos.Y;
				x = pos.X;
			}
			_paths.Add(new Pos(y, x));
			_paths.Reverse();
		}

		const int MOVE_TICK = 10;
		int _sumTick = 0;
		int _lastIndex = 0;
		public void Update(int deltaTick) {
			if (_lastIndex >= _paths.Count) {
				return;
			}

			_sumTick += deltaTick;
			if (_sumTick >= MOVE_TICK) {
				_sumTick = 0;
				PosY = _paths[_lastIndex].Y;
				PosX = _paths[_lastIndex].X;
				_lastIndex++;
			}
		}
	}
}