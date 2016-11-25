using System.Collections;
using System;

public class MinHeap<T> where T : IHeapItem<T> {
	// Imported for optimization

	T[] items;
	public int Size;

	public MinHeap (int MaxSize) {
		items = new T[MaxSize];
	}

	public void Push(T item) {
        item.Index = Size;
        items[Size] = item;
        SortUp(item);
        Size++;
	}

	public T Pop () {
		T Top = items [0];
		Size--;
		items [0] = items [Size];
		items [0].Index = 0;
		SortDown (items[0]);
		return Top;
	}

	public int Count {
		get {
			return Size;
		}
	}

	public void UpdateItem (T item) {
		SortUp (item);
	}

	public bool Contains (T item) {
		return Equals (items [item.Index], item);
	}

	void SortDown (T item) {
		while (true) {
			int Index = item.Index + 1,
			LeftChildIndex = Index * 2,
			RightChildIndex = Index * 2 + 1,
			ChildIndex = 0;

			if (LeftChildIndex < Size) {
				ChildIndex = LeftChildIndex;
				if (RightChildIndex < Size) {
					if (items [LeftChildIndex].CompareTo (items [RightChildIndex]) < 0) {
						ChildIndex = RightChildIndex;
					}
				}
				if (item.CompareTo (items [ChildIndex]) < 0) {
					Swap (item, items [ChildIndex]);
				} else
					return;
			} else
				return;
		}
	}

	void SortUp(T item) {
		int Parenti = ParentIndex(item);
		while (true) {
			if (item.CompareTo (items [Parenti]) > 0) {
				Swap ( item, items [Parenti]);
			} else
				break;
			Parenti = ParentIndex (item);
		}
	}

	int ParentIndex(T item) {
		return (item.Index - 1) / 2;
	}

	void Swap (T A, T B) {
		items [A.Index] = B;
		items [B.Index] = A;
		int temp = A.Index;
		A.Index = B.Index;
		B.Index = temp;
	}
}

public interface IHeapItem<T> : IComparable<T> {
	int Index {
		get;
		set;
	}
}