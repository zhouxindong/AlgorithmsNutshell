using System;

namespace AlgorithmsNutshell.Tree
{
    public enum RedBlackNodeType
    {
        Red = 0,
        Black = 1
    }

    /// <summary>
    /// The RedBlackNode class encapsulates a node in the tree
    /// </summary>
    public class RedBlackNode<TKey, TValue>
        where TValue : class
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public TValue Data { get; set; }

        public TKey Key { get; set; }

        public RedBlackNodeType Color { get; set; }

        public RedBlackNode<TKey, TValue> Left { get; set; }

        public RedBlackNode<TKey, TValue> Right { get; set; }

        public RedBlackNode<TKey, TValue> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlackNodeType.Red;

            Right = SentinelNode;
            Left = SentinelNode;
        }

        public RedBlackNode(TKey key, TValue data)
            : this()
        {
            Key = key;
            Data = data;
        }

        // sentinelNode is convenient way of indicating a leaf node.
        // set up the sentinel node. the sentinel node is the key to a successfull
        // implementation and for understanding the red-black tree properties.
        public static readonly RedBlackNode<TKey, TValue> SentinelNode =
            new RedBlackNode<TKey, TValue>
            {
                Left = null,
                Right = null,
                Parent = null,
                Color = RedBlackNodeType.Black
            };
    }
}