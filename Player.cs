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
			//BFS();
			AStar();
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

			CalcPathFromParent(parent);
		}
		void CalcPathFromParent(Pos[,] parent) {
			int y = _maze.DestY;
			int x = _maze.DestX;
			while (parent[y, x].Y != y || parent[y, x].X != x) {
				_paths.Add(new Pos(y, x));
				Pos pos = parent[y, x];
				y = pos.Y;
				x = pos.X;
			}
			_paths.Add(new Pos(y, x));
			_paths.Reverse();
		}
		struct PQNode : IComparable<PQNode> {
			public int F;
			public int G;
			public int Y;
			public int X;

			public int CompareTo(PQNode other) {
				if (F == other.F) { return 0; }
				return F < other.F ? 1 : -1;
			}
		}
		private void AStar() {
			// U L D R UL DL DR UR
			int[] deltaY = new int[] { -1, 0, 1, 0 };
			int[] deltaX = new int[] { 0, -1, 0, 1 };
			int[] cost = new int[] { 10, 10, 10, 10 };

			//점수 도출식 : F = G + H
			//F : 최종 점수, G : 시작점에서 해당 좌표까지 이동하는데 드는 비용, H : 목적지에서 얼마나 가까운지

			bool[,] closed = new bool[_maze.Size, _maze.Size]; //해당 좌표를 이미 방문했는가
			int[,] open = new int[_maze.Size, _maze.Size]; //경로를 한번이라도 발견했는가

			for (int y = 0; y < _maze.Size; y++) {
				for (int x = 0; x <  _maze.Size; x++) {
					open[y, x] = Int32.MaxValue;
				}
			}

			Pos[,] parent = new Pos[_maze.Size, _maze.Size]; 

			PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>(); //open 리스트의 정보들 중 최적의 후보를 뽑기위한 우선순위 큐

			//시작점 예약
			open[PosY, PosX] = 0 + (Math.Abs(_maze.DestY - PosY) + Math.Abs(_maze.DestX - PosX)) * 10;
			pq.Push(new PQNode() { F = (Math.Abs(_maze.DestY - PosY) + Math.Abs(_maze.DestX - PosX)) * 10, G = 0, Y = PosY, X = PosX});

			parent[PosY, PosX] = new Pos(PosY, PosX);

			while (pq.Count > 0) {
				//최적의 후보 찾기
				PQNode node = pq.Pop();
				if (closed[node.Y, node.X]) { continue; }

				closed[node.Y, node.X] = true; //방문

				if (node.Y == _maze.DestY && node.X == _maze.DestX) { break; } //목적지 도달 시 종료

				//이동할 수 있는 좌표인지 확인 후 예약
				for (int i = 0; i < 4; i++) {
					int nextY = node.Y + deltaY[i];
					int nextX = node.X + deltaX[i];

					//유효 조건에서 어긋나면 스킵
					if (nextX < 0 || nextX >= _maze.Size || nextY < 0 || nextY >= _maze.Size) { continue; }
					if (_maze.Tile[nextY, nextX] == Maze.TileType.Wall) { continue; }
					if (closed[nextY, nextX]) { continue; }

					//비용 계산
					int g = node.G + cost[i];
					int h = (Math.Abs(_maze.DestY - nextY) + Math.Abs(_maze.DestX - nextX)) * 10;

					//더 빠른 길을 찾은 경우 스킵
					if (open[nextY, nextX] < g + h) { continue; }

					//예약
					open[nextY, nextX] = g + h;
					pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
					parent[nextY, nextX] = new Pos(node.Y, node.X);
				}
			}

			CalcPathFromParent(parent);
		}

		const int MOVE_TICK = 10;
		int _sumTick = 0;
		int _lastIndex = 0;
		public void Update(int deltaTick) {
			if (_lastIndex >= _paths.Count) {
				_lastIndex = 0;
				_paths.Clear();
				_maze.Init(_maze.Size, this);
				Init(1, 1, _maze);
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