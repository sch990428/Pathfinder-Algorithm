using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Algorithm {
	class PriorityQueue<T> where T : IComparable<T> {
		List<T> _heap = new List<T>();
		public T Pop() {
			//반환값을 따로 저장
			T ret = _heap[0];

			//마지막 데이터를 루트로 이동
			int lastIndex = _heap.Count - 1;
			_heap[0] = _heap[lastIndex];
			_heap.RemoveAt(lastIndex);
			lastIndex--;

			int now = 0;
			while (true) {
				int left = 2 * now + 1;
				int right = 2 * now + 2;

				int next = now;
				//왼쪽 값이 현재값보다 크면 왼쪽으로 이동
				if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0) { next = left; }
				//오른쪽 값이 현재값보다 크면 오른쪽으로 이동
				if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0) { next = right; }
				//왼쪽 오른쪽 모두 현재 값보다 작으면 종료
				if (next == now) { break; }

				//두 값을 스왑
				T temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;

				now = next;
			}

			return ret;
		}
		public void Push(T n) {
			// 힙의 맨 끝에 새로운 데이터 삽입
			_heap.Add(n);

			int now = _heap.Count - 1;

			//부모 노드와 비교해가며 스왑
			while (now > 0) {
				int next = (now - 1) / 2;
				if (_heap[now].CompareTo(_heap[next]) < 0) { break; }

				T temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;

				now = next;
			}
		}
		public int Count { get { return _heap.Count; } }
	}
}
