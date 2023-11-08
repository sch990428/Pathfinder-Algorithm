using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Pathfinder_Algorithm {
	internal class Maze {
		const char TILE = '\u25a0';

		public TileType[,] Tile { get; private set; }
		public int Size { get; private set; }

		public int DestY { get; private set; }
		public int DestX { get; private set; }

		public Player _player;

		public enum TileType {
			Empty,
			Wall,
		}

		public void Init(int size, Player player) {
			if (size % 2 == 0) {
				return;
			}

			_player = player;
			Tile = new TileType[size, size];
			Size = size;

			DestY = Size - 2;
			DestX = Size - 2;
			//GenerateByBinaryTree();
			GenerateBySideWinder();
		}
		public void GenerateBinaryTree() {
			//일단 길을 전부 막기
			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						Tile[y, x] = TileType.Wall;
					} else {
						Tile[y, x] = TileType.Empty;
					}
				}
			}

			//랜덤으로 우측 혹은 아래로 길을 뚫기
			Random rand = new Random();
			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						continue;
					}

					if (y == Size - 2 && x == Size - 2) {
						continue;
					}

					if (y == Size - 2) {
						Tile[y, x + 1] = TileType.Empty;
						continue;
					}

					if (x == Size - 2) {
						Tile[y + 1, x] = TileType.Empty;
						continue;
					}

					if (rand.Next(0, 2) == 0) {
						Tile[y, x + 1] = TileType.Empty;
					} else {
						Tile[y + 1, x] = TileType.Empty;
					}
				}
			}
		}

		public void GenerateBySideWinder() {
			//일단 길을 전부 막기
			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						Tile[y, x] = TileType.Wall;
					} else {
						Tile[y, x] = TileType.Empty;
					}
				}
			}

			//랜덤으로 우측 혹은 아래로 길을 뚫기
			Random rand = new Random();
			for (int y = 0; y < Size; y++) {
				int count = 1;

				for (int x = 0; x < Size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						continue;
					}

					if (y == Size - 2 && x == Size - 2) {
						continue;
					}

					if (y == Size - 2) {
						Tile[y, x + 1] = TileType.Empty;
						continue;
					}

					if (x == Size - 2) {
						Tile[y + 1, x] = TileType.Empty;
						continue;
					}

					if (rand.Next(0, 2) == 0) {
						Tile[y, x + 1] = TileType.Empty;
						count++;
					} else {
						int randomIndex = rand.Next(0, count);
						Tile[y + 1, x - (randomIndex * 2)] = TileType.Empty;
						count = 1;
					}
				}
			}
		}
		public void Render() {
            Console.WriteLine("d");
            ConsoleColor prevColor = Console.ForegroundColor;
			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					if (_player.PosX == x && _player.PosY == y) {
						Console.ForegroundColor = ConsoleColor.DarkBlue;
					} else if (DestY == y && DestX == x) {
						Console.ForegroundColor = ConsoleColor.Yellow;
					} else {
						Console.ForegroundColor = GetTileColor(Tile[y, x]);
					}
					Console.Write(TILE + " ");
				}
				Console.WriteLine();
			}

			Console.ForegroundColor = prevColor;
		}

		ConsoleColor GetTileColor(TileType type) {
			switch (type) {
				case TileType.Empty:
					return ConsoleColor.Black;
				case TileType.Wall:
					return ConsoleColor.Red;
				default:
					return ConsoleColor.Blue;
			}
		}
	}
}