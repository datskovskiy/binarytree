using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTree
{
    public class BinaryTree<T> : IEnumerable<T>
    {
        private readonly IComparer<T> _comparer;

        public Node<T> Root { get; set; }
        /// <summary>
        /// Handles events for adding and removing elements
        /// </summary>
        /// <param name="sender">Instance of <see cref="BinaryTree<T>"/> that called the event</param>
        /// <param name="args">Arguments passed by sender for subscribers</param>
        public delegate void TreeEventHandler(object sender, TreeEventArgs<T> args);

        /// <summary>
        /// Defines how many elements tree contains
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Checks if type T implements <see cref="IComparable<T>"/>
        /// If it does: saves and uses as default comparer
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when T doesn't implement <see cref="IComparable<T>"</exception>
        public BinaryTree()
        {
            Root = null;
            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Creates instance of tree and saves custom comparer passed by parameter
        /// </summary>
        /// <param name="comparer"><see cref="IComparer<T>"/></param>
        public BinaryTree(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Adds element to the tree according to comparer
        /// </summary>
        /// <param name="item">Object that should be added in tree</param>
        /// <exception cref="ArgumentNullException">Thrown if parameter was null</exception>
        public void Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "cant be null.");

            var nodeToInsert = new Node<T>(item, _comparer);

            if (Root is null)
                Root = nodeToInsert;

            var current = Root;

            while (current != null)
            {
                if (nodeToInsert > current)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                        continue;
                    }

                    current.Right = nodeToInsert;
                    continue;
                }

                if (nodeToInsert < current)
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                        continue;
                    }

                    current.Left = nodeToInsert;
                    continue;
                }

                Count++;

                return;
            }
        }

        /// <summary>
        /// Removes element from tree by its reference
        /// </summary>
        /// <param name="item">Object that should be removed from tree</param>
        /// <returns>True if element was deleted succesfully, false if element wasn't found in tree</returns>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns item with the highest value
        /// </summary>
        /// <returns>The element with highest value</returns>
        /// <exception cref="InvalidOperationException">Thrown if tree is empty</exception> 
        public T TreeMax()
        {
            if (Root is null)
                throw new InvalidOperationException("Tree cant be empty.");

            var current = Root;

            while (current.Right != null)
                current = current.Right;

            return current.Data;
        }

        /// <summary>
        /// Returns item with the lowest value
        /// </summary>
        /// <returns>The element with lowest value</returns>
        /// <exception cref="InvalidOperationException">Thrown if tree is empty</exception>
        public T TreeMin()
        {
            if (Root is null)
                throw new InvalidOperationException("Tree cant be empty.");

            var current = Root;

            while (current.Left != null)
                current = current.Left;

            return current.Data;
        }

        /// <summary>
        /// Checks if tree contains element by its reference
        /// </summary>
        /// <param name="item">Object that should (or not) be found in tree</param>
        /// <returns>True if tree contains item, false if it doesn't</returns>
        public bool Contains(T data)
        {
            if (Root is null)
                return false;

            var nodeToSearch = new Node<T>(data, _comparer);

            Node<T> tempNode = Root;
            while (tempNode != null)
            {
                if (nodeToSearch == tempNode)
                    return true;
                else if (nodeToSearch < tempNode)
                    tempNode = tempNode.Left;
                else
                    tempNode = tempNode.Right;
            }

            return false;
        }

        /// <summary>
        /// Makes tree traversal
        /// </summary>
        /// <param name="traverseType"><see cref="TraverseType"></param>
        /// <returns>Sequense of elements of tree according to traverse type</returns>
        public IEnumerable<T> Traverse(TraverseType traverseType)
        {
           return TraverseNode(Root, traverseType);
        }

        /// <summary>
        /// Makes in-order traverse
        /// Serves as a default <see cref="TraverseType"/> for tree
        /// </summary>
        /// <returns>Enumerator for iterations in foreach cycle</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Root.GetEnumerator();
        }

        /// <summary>
        /// Makes in-order traverse
        /// Serves as a default <see cref="TraverseType"/> for tree
        /// </summary>
        /// <returns>Enumerator for iterations in foreach cycle</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected IEnumerable<T> TraverseNode(Node<T> node, TraverseType method)
        {
            var TraverseLeft = node.Left == null ? new T[0] : TraverseNode(node.Left, method);
            var TraverseRight = node.Right == null ? new T[0] : TraverseNode(node.Right, method);
            var Self = new T[1] { node.Data };

            return method switch
            {
                TraverseType.PreOrder => Self.Concat(TraverseLeft).Concat(TraverseRight),
                TraverseType.InOrder => TraverseLeft.Concat(Self).Concat(TraverseRight),
                TraverseType.PostOrder => TraverseLeft.Concat(TraverseRight).Concat(Self),
                _ => throw new ArgumentException("Incorrect traverse type."),
            };
        }
    }
}
