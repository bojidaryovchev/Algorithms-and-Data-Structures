using System;

namespace DataStructures
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class DoublyLinkedList<T> : ICollection<T>
    {
        private LinkedListNode<T> _head;
        private LinkedListNode<T> _tail;

        public DoublyLinkedList()
        {
            this._head = this._tail = null;
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get { return false; } }

        // O(1)
        public void AddFirst(T item)
        {
            var node = new LinkedListNode<T>(item) { Next = this._head };

            this._head.Previous = node;
            this._head = node;
            this.Count++;
        }

        // O(1)
        public void AddLast(T item)
        {
            var node = new LinkedListNode<T>(item) {Previous = this._tail};

            this._tail.Next = node;
            this._tail = node;
            this.Count++;
        }

        // O(1)
        public void Add(T item)
        {
            if (this.Count > 0)
            {
                this.AddLast(item);
            }
            else
            {
                this._head = this._tail = new LinkedListNode<T>(item);
                this.Count++;
            }
        }

        // O(1)
        public void Clear()
        {
            this._head = this._tail = null;
            this.Count = 0;
        }

        // O(n)
        public bool Contains(T item)
        {
            bool contains = false;

            var node = this._head;

            while (node != null)
            {
                if (node.Value.Equals(item))
                {
                    contains = true;
                    break;
                }

                node = node.Next;
            }

            return contains;
        }

        // O(n)
        public void CopyTo(T[] array, int arrayIndex)
        {
            var enumerator = this.GetEnumerator();

            using (enumerator)
            {
                if (enumerator.MoveNext())
                {
                    for (int index = arrayIndex; index < array.Length; index++)
                    {
                        array[index] = enumerator.Current;

                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                    }
                }
            }
        }

        // O(n)
        public bool Remove(T item)
        {
            bool itemRemoved = false;

            if (this.Count > 1)
            {
                var node = this._head;

                while (node != null)
                {
                    var nextNode = node.Next;

                    if (node.Value.Equals(item))
                    {
                        nextNode = RemoveNode(node);

                        itemRemoved = true;
                        this.Count--;
                    }

                    node = nextNode;
                }
            }
            else if (this.Any() && this._head.Value.Equals(item) && this._tail.Value.Equals(item))
            {
                this.Clear();

                itemRemoved = true;
            }

            return itemRemoved;
        }

        // O(n)
        public IEnumerator<T> GetEnumerator()
        {
            var node = this._head;

            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static LinkedListNode<T> RemoveNode(LinkedListNode<T> node)
        {
            LinkedListNode<T> nextNode = null;

            if (node.Previous != null && node.Next != null)
            {
                nextNode = node.Next;

                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }
            else if (node.Previous != null && node.Next == null)
            {
                nextNode = node.Previous;

                node.Previous.Next = null;
                node.Previous = null;
            }
            else if (node.Previous == null && node.Next != null)
            {
                nextNode = node.Next;

                node.Next.Previous = null;
                node.Next = null;
            }

            return nextNode;
        }

        private class LinkedListNode<TK>
        {
            public LinkedListNode(TK value)
            {
                this.Value = value;
            }

            public TK Value { get; private set; }

            public LinkedListNode<TK> Previous { get; set; }

            public LinkedListNode<TK> Next { get; set; }
        }
    }
}
