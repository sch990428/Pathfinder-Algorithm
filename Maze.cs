using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Pathfinder_Algorithm {
	internal class Maze {
		const char TILE = '\u25a0';

		public TileType[,] _tile;
		public int _size;

		public enum TileType {
			Empty,
			Wall,
		}

		public void init(int size) {
			if (size % 2 == 0) {
				return;
			}

			_tile = new TileType[size, size];
			_size = size;

			
			GenerateBinaryTree();
		}
		public void GenerateBinaryTree() {
			//일단 길을 전부 막기
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						_tile[y, x] = TileType.Wall;
					} else {
						_tile[y, x] = TileType.Empty;
					}
				}
			}

			//랜덤으로 우측 혹은 아래로 길을 뚫기
			Random rand = new Random();
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					if (x % 2 == 0 || y % 2 == 0) {
						continue;
					}

					if (y == _size - 2 && x == _size - 2) {
						continue;
					}

					if (y == _size - 2) {
						_tile[y, x + 1] = TileType.Empty;
						continue;
					}

					if (x == _size - 2) {
						_tile[y + 1, x] = TileType.Empty;
						continue;
					}

					if (rand.Next(0, 2) == 0) {
						_tile[y, x + 1] = TileType.Empty;
					} else {
						_tile[y + 1, x] = TileType.Empty;
					}
				}
			}
		}

		public void Render() {
			ConsoleColor prevColor = Console.ForegroundColor;

			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Console.ForegroundColor = GetTileColor(_tile[y,x]);
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
