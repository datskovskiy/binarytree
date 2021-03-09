using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BinaryTree
{
    public class Node<T> : IComparable<T>
    {
        private readonly IComparer<T> _comparer;

        public T Data { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public Node(T data, IComparer<T> comparer)
        {
            Data = data;
            _comparer = comparer;
        }

        public int CompareTo(T other)
        {
            return _comparer.Compare(Data, other);
        }

        public override bool Equals(object obj)
        {
            if (obj is Node<T> other)
            {
                if (ReferenceEquals(Data, other.Data)) return true;
                return Data.Equals(other.Data);
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var v in Left)
                {
                    yield return v;
                }
            }

            yield return Data;

            if (Right != null)
            {
                foreach (var v in Right)
                {
                    yield return v;
                }
            }
        }

        public override int GetHashCode() => Data == null ? 0 : Data.GetHashCode();
        public static bool operator ==(Node<T> left, Node<T> right) => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);
        public static bool operator !=(Node<T> left, Node<T> right) => !(left == right);

        public static bool operator <(Node<T> left, Node<T> right) => left.CompareTo(right.Data) < 0;
        public static bool operator >(Node<T> left, Node<T> right) => left.CompareTo(right.Data) > 0;

        public static bool operator <=(Node<T> left, Node<T> right) => left.CompareTo(right.Data) <= 0;
        public static bool operator >=(Node<T> left, Node<T> right) => left.CompareTo(right.Data) >= 0;
    }
}
