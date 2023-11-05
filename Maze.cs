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
			_tile = new TileType[size, size];
			_size = size;

			for (int y = 0; y < size; y++) {
				for (int x = 0; x < size; x++) {
					if (x == 0 || x == size - 1 || y == 0 || y == size - 1) {
						_tile[y, x] = TileType.Wall;
					} else {
						_tile[y,x] = TileType.Empty;
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
					return ConsoleColor.DarkGreen;
				case TileType.Wall:
					return ConsoleColor.Red;
				default:
					return ConsoleColor.Blue;
			}
		}
	}
}
