using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScintillaNET
{
    // Do error checking higher up
    // http://www.codeproject.com/Articles/20910/Generic-Gap-Buffer
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class GapBuffer<T> : IEnumerable<T>
    {
        private T[] buffer;
        private int gapStart;
        private int gapEnd;

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void AddRange(ICollection<T> collection)
        {
            InsertRange(Count, collection);
        }

        private void EnsureGapCapacity(int length)
        {
            if (length > this.gapEnd - this.gapStart)
            {
                // How much to grow the buffer is a tricky question.
                // Our current algo will double the capacity unless that's not enough.
                int minCapacity = Count + length;
                int newCapacity = this.buffer.Length * 2;
                if (newCapacity < minCapacity)
                {
                    newCapacity = minCapacity;
                }

                var newBuffer = new T[newCapacity];
                int newGapEnd = newBuffer.Length - (this.buffer.Length - this.gapEnd);

                Array.Copy(this.buffer, 0, newBuffer, 0, this.gapStart);
                Array.Copy(this.buffer, this.gapEnd, newBuffer, newGapEnd, newBuffer.Length - newGapEnd);
                this.buffer = newBuffer;
                this.gapEnd = newGapEnd;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
                yield return this[i];

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(int index, T item)
        {
            PlaceGapStart(index);
            EnsureGapCapacity(1);

            this.buffer[index] = item;
            this.gapStart++;
        }

        public void InsertRange(int index, ICollection<T> collection)
        {
            int count = collection.Count;
            if (count > 0)
            {
                PlaceGapStart(index);
                EnsureGapCapacity(count);

                collection.CopyTo(this.buffer, this.gapStart);
                this.gapStart += count;
            }
        }

        private void PlaceGapStart(int index)
        {
            if (index != this.gapStart)
            {
                if (this.gapEnd - this.gapStart == 0)
                {
                    // There is no gap
                    this.gapStart = index;
                    this.gapEnd = index;
                }
                else if (index < this.gapStart)
                {
                    // Move gap left (copy contents right)
                    int length = this.gapStart - index;
                    int deltaLength = this.gapEnd - this.gapStart < length ? this.gapEnd - this.gapStart : length;
                    Array.Copy(this.buffer, index, this.buffer, this.gapEnd - length, length);
                    this.gapStart -= length;
                    this.gapEnd -= length;

                    Array.Clear(this.buffer, index, deltaLength);
                }
                else
                {
                    // Move gap right (copy contents left)
                    int length = index - this.gapStart;
                    int deltaIndex = index > this.gapEnd ? index : this.gapEnd;
                    Array.Copy(this.buffer, this.gapEnd, this.buffer, this.gapStart, length);
                    this.gapStart += length;
                    this.gapEnd += length;

                    Array.Clear(this.buffer, deltaIndex, this.gapEnd - deltaIndex);
                }
            }
        }

        public void RemoveAt(int index)
        {
            PlaceGapStart(index);
            this.buffer[this.gapEnd] = default;
            this.gapEnd++;
        }

        public void RemoveRange(int index, int count)
        {
            if (count > 0)
            {
                PlaceGapStart(index);
                Array.Clear(this.buffer, this.gapEnd, count);
                this.gapEnd += count;
            }
        }

        public int Count
        {
            get
            {
                return this.buffer.Length - (this.gapEnd - this.gapStart);
            }
        }

#if DEBUG
        // Poor man's DebuggerTypeProxy because I can't seem to get that working
        private List<T> Debug
        {
            get
            {
                var list = new List<T>(this);
                return list;
            }
        }
#endif

        public T this[int index]
        {
            get
            {
                if (index < this.gapStart)
                    return this.buffer[index];

                return this.buffer[index + (this.gapEnd - this.gapStart)];
            }
            set
            {
                if (index >= this.gapStart)
                    index += this.gapEnd - this.gapStart;

                this.buffer[index] = value;
            }
        }

        public GapBuffer(int capacity = 0)
        {
            this.buffer = new T[capacity];
            this.gapEnd = this.buffer.Length;
        }
    }
}
