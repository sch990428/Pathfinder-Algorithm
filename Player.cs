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
			_paths.Add(new Pos(PosY, PosX));

			int _dir = (int)Dir.Up;

			int[] frontY = new int[] { -1, 0, 1, 0 };
			int[] frontX = new int[] { 0, -1, 0, 1 };
			int[] rightY = new int[] { 0, -1, 0, 1 };
			int[] rightX = new int[] { 1, 0, -1, 0 };

			//미로 탈출 오른손 법칙
			while (PosY != maze.DestY || PosX != maze.DestX) {
				if (maze.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] != Maze.TileType.Wall) {
					//바라보는 방향의 오른쪽으로 갈 수 있는가
					//오른쪽으로 돌기
					_dir = (_dir - 1 + 4) % 4;
					//전진
					PosY += frontY[_dir];
					PosX += frontX[_dir];
					_paths.Add(new Pos(PosY, PosX));
				} else if (maze.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] != Maze.TileType.Wall) {
					//바라보는 방향으로 전진할 수 있는가
					PosY += frontY[_dir];
					PosX += frontX[_dir];
					_paths.Add(new Pos(PosY, PosX));
				} else {
					//왼쪽으로 돌기
					_dir = (_dir + 1 + 4) % 4;
				}
			}
		}

		const int MOVE_TICK = 100;
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
