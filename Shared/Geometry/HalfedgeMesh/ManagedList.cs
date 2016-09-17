using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Shared.Geometry.HalfedgeMesh
{
    public class ManagedList<T> : IEnumerable where T : class, IIndexable
    {
        internal ManagedList(int initialSize)
        {
            List = new List<T>(initialSize);
            FreeList = new Stack<int>();
        }

        protected Stack<int> FreeList;
        protected List<T> List;

        internal void Add(T val)
        {
            if (val == null)
            {
                List.Add(val);
                FreeList.Push(List.Count - 1);
                return;
            }

            if (FreeList.Count > 0)
            {
                var index = FreeList.Pop();
                List[index] = val;
                val.Index = index;
                return;
            }

            List.Add(val);
            val.Index = Count - 1;
        }

        internal void AddUnique(T val, out T res, Func<T, T, bool> equals)
        {
            for (int i = 0; i < List.Count; i++)
            {
                var valInList = List[i];
                if (valInList == null) continue;
                if (equals(val, valInList) && !FreeList.Contains(i))
                {
                    res = valInList;
                    return;
                }
            }
            res = null;
            Add(val);
        }

        public void Remove(int index)
        {
            List[index].Index = -1;
            List[index] = null;
            FreeList.Push(index);
        }

        public int Count
        {
            get
            {
                var elems = List.Count - FreeList.Count;
                return elems <= 0 ? 0 : elems;
            }
        }

        public T this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                if(index >= List.Count)
                    throw new IndexOutOfRangeException();
                List[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ManagedListEnumerator(List);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T[] ToArray()
        {
            T[] arr = new T[this.Count];
            int i = 0;
            foreach (var val in List)
            {
                if (val != null)
                    arr[i++] = val;
            }
            return arr;
        }

        public T[] ToRawArray()
        {
            return List.ToArray();
        }

        internal class ManagedListEnumerator : IEnumerator<T>
        {
            private readonly List<T> _list;
            private int _curIndex;

            internal ManagedListEnumerator(List<T> list)
            {
                _list = list;
                _curIndex = -1;
            }

            public bool MoveNext()
            {
                _curIndex++;

                for (int j = _curIndex; j < _list.Count; j++)
                {
                    if (_list[j] != null)
                    {
                        return true;
                    }
                    _curIndex++;
                }

                return false;
            }

            public void Reset()
            {
                _curIndex = -1;
            }

            public T Current
            {
                get { return _list[_curIndex]; }
            }

            public void Dispose()
            {
                
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        internal bool Contains(T val)
        {
            foreach (var lval in List)
            {
                if (lval != null && lval.Equals(val))
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            List.Clear();
            FreeList.Clear();
        }

        public void Compact()
        {
            bool noMoreElements = false;
            int i = 0;
            for (; i < List.Count; i++)
            {
                var val = List[i];
                if (val == null) // we have found a gap
                {
                    int j = i + 1;
                    for (; j < List.Count; j++)
                    {
                        var val2 = List[j];
                        if (val2 != null)
                        {
                            break;
                        }
                    }
                    if (j < List.Count) // if we are not at the end
                    {
                        List[i] = List[j];
                        List[i].Index = i;
                        List[j] = null;
                    }
                    else
                    {
                        noMoreElements = true;
                    }
                    
                }
                if (noMoreElements)
                    break;
            }
            if (i < List.Count) // if there are null values at the end
            {
                Debug.Assert(FreeList.Count == List.Count - i);
                List.RemoveRange(i, List.Count - i);
                FreeList.Clear();
            }
        }
    }
}
