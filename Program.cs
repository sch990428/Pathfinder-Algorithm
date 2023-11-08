namespace Pathfinder_Algorithm {
	internal class Program {
		static void Main(string[] args) {
			Maze maze = new Maze();
			Player player = new Player();
			maze.Init(25, player);
			player.Init(1, 1, maze);

			Console.CursorVisible = false; // 콘솔 커서 표시 안함

			const int WAIT_TICK = 1000 / 30;
			int lastTick = 0;

			while (true) {
				#region FPS (1초에 루프가 60번 실행되도록)
				int currentTick = Environment.TickCount & Int32.MaxValue;

				if (currentTick - lastTick < WAIT_TICK)
					continue;
				int deltaTick = currentTick - lastTick;
				lastTick = currentTick;
				#endregion
				// INPUT

				// LOGIC
				player.Update(deltaTick);

				// RENDERING
				Console.SetCursorPosition(0, 0); // 콘솔 커서의 위치를 0, 0으로 고정
				
				maze.Render();
			}
		}
	}
}