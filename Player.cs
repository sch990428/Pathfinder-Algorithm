namespace Pathfinder_Algorithm {
	internal class Player {
		public int PosY { get; private set; }
		public int PosX { get; private set; }
		Random _random = new Random();
		Maze _maze;

		public void Init(int posY, int posX, int destY, int destX, Maze maze) {
			PosY = posY;
			PosX = posX;

			_maze = maze;
		}

		const int MOVE_TICK = 100;
		int _sumTick = 0;
		public void Update(int deltaTick) {
			_sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK) {
				_sumTick = 0;

				int randValue = _random.Next(0, 4);
				switch(randValue) {
					case 0:
						if (PosY > 0 && _maze.Tile[PosY - 1, PosX] == Maze.TileType.Empty)
							PosY -= 1;
						break;
					case 1:
						if (PosY < _maze.Size - 1 && _maze.Tile[PosY + 1, PosX] == Maze.TileType.Empty) {
							PosY += 1;
						}
						break;
					case 2:
						if (PosX > 0 && _maze.Tile[PosY, PosX - 1] == Maze.TileType.Empty)
							PosX -= 1;
						break;
					case 3:
						if (PosX < _maze.Size - 1 && _maze.Tile[PosY, PosX + 1] == Maze.TileType.Empty)
							PosX += 1;
						break;
				}
			}
        }
	}
}
