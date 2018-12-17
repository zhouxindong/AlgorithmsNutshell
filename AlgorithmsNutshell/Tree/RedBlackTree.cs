using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AlgorithmsNutshell.Tree
{
    /// <summary>
    /// Red Black Tree Implementation
    /// 性质1. 节点是红色或黑色。
    /// 性质2. 根节点是黑色。
    /// 性质3 每个叶节点（NIL节点，空节点）是黑色的。
    /// 性质4 每个红色节点的两个子节点都是黑色。(从每个叶子到根的所有路径上不能有两个连续的红色节点)
    /// 性质5.从任一节点到其每个叶子的所有路径都包含相同数目的黑色节点。
    /// 红黑树的关键性质: 从根到叶子的最长的可能路径不多于最短的可能路径的两倍长。
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Data type</typeparam>
    /// <example>
    /// var x = new RedBlackTree&lt;Guid,String&gt;();
    /// </example>
    public class RedBlackTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TValue : class
        where TKey : IComparable<TKey>, IComparable, IEquatable<TKey>
    {
        // a simple randomized hash code. The hash code could be used as a key
        // if it is "unique" enough.
        // The IComparable interface would need to be replaced with int.
        private int _hash_code;

        // identifies the owner of the tree
        private int _identifier;

        // the tree
        private RedBlackNode<TKey, TValue> _tree_base_node = RedBlackNode<TKey, TValue>.SentinelNode;

        // the node that was last found; used to optimize searches
        private RedBlackNode<TKey, TValue> _last_node_found = RedBlackNode<TKey, TValue>.SentinelNode;

        private readonly Random _rand = new Random();

        private int _count;

        /// <summary>
        /// Constructor that initializes a blank Red Black Tree
        /// </summary>
        /// <example>
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// </example>
        public RedBlackTree()
        {
            Initialize(_rand.Next());
        }

        /// <summary>
        /// Get a value stored in the tree using a key
        /// </summary>
        /// <param name="key">key of the item to be returned</param>
        /// <returns>value stored in the tree, null if it does not exist</returns>
        /// <example>
        /// Guid id = Guid.NewGuid();
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// var y = new KeyValuePair&lt;Guid,String&gt; { id, "Hello" };
        /// x.Add(y);
        /// var y = x[id];
        /// </example>
        public TValue this[TKey key]
        {
            get { return GetNode(key).Data; }
            set { GetNode(key).Data = value; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <example>
        /// Guid id = Guid.NewGuid();
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// var y = new KeyValuePair&lt;Guid,String&gt; { id, "Hello" };
        /// x.Add(y);
        /// x.Remove(y);
        /// </example>
        /// <remarks>The Red Black Tree implementation actually ignores the Value portion in the case of the delete, it removes the node with the matching Key</remarks>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Returns the number of items currently in the tree
        /// </summary>
        public int Count { get { return _count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        ///<summary>
        /// GetData
        /// Gets the data object associated with the specified key
        ///</summary>
        /// <remarks>This is the equivalent of calling T this[K key]</remarks>
        public TValue GetData(TKey key)
        {
            return GetNode(key).Data;
        }

        ///<summary>
        /// GetMinKey
        /// Returns the minimum key value
        ///</summary>
        public TKey GetMinKey()
        {
            var work_node = _tree_base_node;

            if (work_node == null || work_node == RedBlackNode<TKey, TValue>.SentinelNode)
                throw new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty);

            // traverse to the extreme left to find the smallest key
            while (work_node.Left != RedBlackNode<TKey, TValue>.SentinelNode)
                work_node = work_node.Left;

            _last_node_found = work_node;

            return work_node.Key;
        }

        ///<summary>
        /// GetMaxKey
        /// Returns the maximum key value
        ///</summary>
        public TKey GetMaxKey()
        {
            var work_node = _tree_base_node;

            if (work_node == null || work_node == RedBlackNode<TKey, TValue>.SentinelNode)
                throw new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty);

            // traverse to the extreme right to find the largest key
            while (work_node.Right != RedBlackNode<TKey, TValue>.SentinelNode)
                work_node = work_node.Right;

            _last_node_found = work_node;

            return work_node.Key;
        }

        ///<summary>
        /// GetMinValue
        /// Returns the object having the minimum key value
        ///</summary>
        public TValue GetMinValue()
        {
            return GetData(GetMinKey());
        }

        ///<summary>
        /// GetMaxValue
        /// Returns the object having the maximum key
        ///</summary>
        public TValue GetMaxValue()
        {
            return GetData(GetMaxKey());
        }

        ///<summary>
        /// GetEnumerator
        /// return an enumerator that returns the tree nodes in order
        ///</summary>
        public IEnumerator<TValue> GetEnumerator()
        {
            return GetAll().Select(i => i.Data).GetEnumerator();
        }

        ///<summary>
        /// IsEmpty
        /// Is the tree empty?
        ///</summary>
        public bool IsEmpty()
        {
            return _tree_base_node == RedBlackNode<TKey, TValue>.SentinelNode;
        }

        ///<summary>
        /// RemoveMin
        /// removes the node with the minimum key
        ///</summary>
        public void RemoveMin()
        {
            if (_tree_base_node == null)
                throw new RedBlackException(Properties.Resources.ExceptionNodeIsNull);

            Delete(GetNode(GetMinKey()));
        }

        ///<summary>
        /// RemoveMax
        /// removes the node with the maximum key
        ///</summary>
        public void RemoveMax()
        {
            if (_tree_base_node == null)
                throw new RedBlackException(Properties.Resources.ExceptionNodeIsNull);

            Delete(GetNode(GetMaxKey()));
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            New(item.Key, item.Value);
        }

        ///<summary>
        /// Clear
        /// Empties or clears the tree
        ///</summary>
        public void Clear()
        {
            _tree_base_node = RedBlackNode<TKey, TValue>.SentinelNode;
            _count = 0;
            InvokeOnClear(new EventArgs());
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="array_index">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="array_index"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="array_index"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int array_index)
        {
            if (array_index < 0)
                throw new ArgumentOutOfRangeException("array_index", "ArrayIndex cannot be less than 0");
            if (array == null)
                throw new ArgumentNullException("array", "Array cannot be null");
            if ((array.Length - array_index) < Count)
                throw new ArgumentException();
            var current_position = array_index;
            foreach (var item in GetAll()
                .Select(i => i.Data))
            {
                array.SetValue(item, current_position);
                current_position++;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(TValue item)
        {
            return GetAll().Select(i => i.Data).Any(i => i == item);
        }

        ///<summary>
        /// Equals
        ///</summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is RedBlackNode<TKey, TValue>))
                return false;

            return this == obj || (ToString().Equals(obj.ToString()));
        }

        ///<summary>
        /// HashCode
        ///</summary>
        public override int GetHashCode()
        {
            return _hash_code;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetAll()
                .Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Data))
                .GetEnumerator();
        }

        ///<summary>
        /// ToString
        ///</summary>
        public override string ToString()
        {
            return _identifier.ToString();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            try
            {
                var node = GetNode(key);
                return node != null;
            }
            catch (RedBlackException)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param><param name="value">The object to use as the value of the element to add.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public virtual void Add(TKey key, TValue value)
        {
            New(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public virtual bool Remove(TKey key)
        {
            try
            {
                Delete(GetNode(key));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = GetNode(key).Data;
            return (value != null);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return GetNode(key).Data; }
            set { GetNode(key).Data = value; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get { return GetAll().Select(i => i.Key).ToArray(); }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TValue> Values
        {
            get { return GetAll().Select(i => i.Data).ToArray(); }
        }

        /// <summary>
        /// Invoked when Item is added
        /// </summary>
        public event EventHandler<RedBlackEventArgs<TKey, TValue>> OnAdd = (sender, args) => { };

        protected void InvokeOnAdd(RedBlackEventArgs<TKey, TValue> e)
        {
            OnAdd(this, e);
        }

        /// <summary>
        /// Invoked when Item is removed
        /// </summary>
        public event EventHandler<RedBlackEventArgs<TKey, TValue>> OnRemove = (sender, args) => { };

        protected void InvokeOnRemove(RedBlackEventArgs<TKey, TValue> e)
        {
            OnRemove(this, e);
        }

        /// <summary>
        /// Invoked when tree is cleared
        /// </summary>
        public event EventHandler OnClear = (sender, args) => { };

        protected void InvokeOnClear(EventArgs e)
        {
            OnClear(this, e);
        }

        #region "Private Methods"

        private void New(TKey key, TValue data)
        {
            if (data == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeKeyAndDataMustNotBeNull));

            // traverse tree - find where node belongs

            // create new node
            var new_node = new RedBlackNode<TKey, TValue>(key, data);

            // grab the rbTree node of the tree
            var work_node = _tree_base_node;

            while (work_node != RedBlackNode<TKey, TValue>.SentinelNode)
            {
                // find Parent
                new_node.Parent = work_node;
                int result = key.CompareTo(work_node.Key);
                if (result == 0)
                    throw (new RedBlackException(Properties.Resources.ExceptionNodeWithSameKeyAlreadyExists));
                work_node = result > 0
                    ? work_node.Right
                    : work_node.Left;
            }

            // insert node into tree starting at parent's location
            if (new_node.Parent != null)
            {
                if (new_node.Key.CompareTo(new_node.Parent.Key) > 0)
                    new_node.Parent.Right = new_node;
                else
                    new_node.Parent.Left = new_node;
            }
            else
                // first node added
                _tree_base_node = new_node;

            // restore red-black properties
            BalanceTreeAfterInsert(new_node);

            _last_node_found = new_node;

            Interlocked.Increment(ref _count);
            InvokeOnAdd(new RedBlackEventArgs<TKey, TValue> { Item = data, Key = key });
        }

        ///<summary>
        /// Delete
        /// Delete a node from the tree and restore red black properties
        ///</summary>
        private void Delete(RedBlackNode<TKey, TValue> delete_node)
        {
            // A node to be deleted will be: 
            //		1. a leaf with no children
            //		2. have one child
            //		3. have two children
            // If the deleted node is red, the red black properties still hold.
            // If the deleted node is black, the tree needs rebalancing

            // work node
            RedBlackNode<TKey, TValue> work_node;

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (delete_node.Left == RedBlackNode<TKey, TValue>.SentinelNode ||
                delete_node.Right == RedBlackNode<TKey, TValue>.SentinelNode)
                // node has sentinel as a child
                work_node = delete_node;
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                // traverse right subtree
                work_node = delete_node.Right;
                // to find next node in sequence
                while (work_node.Left != RedBlackNode<TKey, TValue>.SentinelNode)
                    work_node = work_node.Left;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            var linked_node = work_node.Left != RedBlackNode<TKey, TValue>.SentinelNode
                                                 ? work_node.Left
                                                 : work_node.Right;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            linked_node.Parent = work_node.Parent;
            if (work_node.Parent != null)
                if (work_node == work_node.Parent.Left)
                    work_node.Parent.Left = linked_node;
                else
                    work_node.Parent.Right = linked_node;
            else
                // make x the root node
                _tree_base_node = linked_node;

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (work_node != delete_node)
            {
                delete_node.Key = work_node.Key;
                delete_node.Data = work_node.Data;
            }

            if (work_node.Color == RedBlackNodeType.Black)
                BalanceTreeAfterDelete(linked_node);

            _last_node_found = RedBlackNode<TKey, TValue>.SentinelNode;

            Interlocked.Decrement(ref _count);
            InvokeOnRemove(new RedBlackEventArgs<TKey, TValue> { Item = delete_node.Data, Key = delete_node.Key });
        }

        ///<summary>
        /// BalanceTreeAfterDelete
        /// Deletions from red-black trees may destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterDelete(RedBlackNode<TKey, TValue> linked_node)
        {
            // maintain Red-Black tree balance after deleting node
            while (linked_node != _tree_base_node && linked_node.Color == RedBlackNodeType.Black)
            {
                RedBlackNode<TKey, TValue> work_node;
                // determine sub tree from parent
                if (linked_node == linked_node.Parent.Left)
                {
                    // y is x's sibling
                    work_node = linked_node.Parent.Right;
                    if (work_node.Color == RedBlackNodeType.Red)
                    {
                        // x is black, y is red - make both black and rotate
                        linked_node.Parent.Color = RedBlackNodeType.Red;
                        work_node.Color = RedBlackNodeType.Black;
                        RotateLeft(linked_node.Parent);
                        work_node = linked_node.Parent.Right;
                    }
                    if (work_node.Left.Color == RedBlackNodeType.Black &&
                        work_node.Right.Color == RedBlackNodeType.Black)
                    {
                        // children are both black
                        // change parent to red
                        work_node.Color = RedBlackNodeType.Red;
                        // move up the tree
                        linked_node = linked_node.Parent;
                    }
                    else
                    {
                        if (work_node.Right.Color == RedBlackNodeType.Black)
                        {
                            work_node.Left.Color = RedBlackNodeType.Black;
                            work_node.Color = RedBlackNodeType.Red;
                            RotateRight(work_node);
                            work_node = linked_node.Parent.Right;
                        }
                        linked_node.Parent.Color = RedBlackNodeType.Black;
                        work_node.Color = linked_node.Parent.Color;
                        work_node.Right.Color = RedBlackNodeType.Black;
                        RotateLeft(linked_node.Parent);
                        linked_node = _tree_base_node;
                    }
                }
                else
                {	// right subtree - same as code above with right and left swapped
                    work_node = linked_node.Parent.Left;
                    if (work_node.Color == RedBlackNodeType.Red)
                    {
                        linked_node.Parent.Color = RedBlackNodeType.Red;
                        work_node.Color = RedBlackNodeType.Black;
                        RotateRight(linked_node.Parent);
                        work_node = linked_node.Parent.Left;
                    }
                    if (work_node.Right.Color == RedBlackNodeType.Black &&
                        work_node.Left.Color == RedBlackNodeType.Black)
                    {
                        work_node.Color = RedBlackNodeType.Red;
                        linked_node = linked_node.Parent;
                    }
                    else
                    {
                        if (work_node.Left.Color == RedBlackNodeType.Black)
                        {
                            work_node.Right.Color = RedBlackNodeType.Black;
                            work_node.Color = RedBlackNodeType.Red;
                            RotateLeft(work_node);
                            work_node = linked_node.Parent.Left;
                        }
                        work_node.Color = linked_node.Parent.Color;
                        linked_node.Parent.Color = RedBlackNodeType.Black;
                        work_node.Left.Color = RedBlackNodeType.Black;
                        RotateRight(linked_node.Parent);
                        linked_node = _tree_base_node;
                    }
                }
            }
            linked_node.Color = RedBlackNodeType.Black;
        }

        internal Stack<RedBlackNode<TKey, TValue>> GetAll()
        {
            Stack<RedBlackNode<TKey, TValue>> stack = new Stack<RedBlackNode<TKey, TValue>>();

            // use depth-first traversal to push nodes into stack
            // the lowest node will be at the top of the stack
            if (_tree_base_node != RedBlackNode<TKey, TValue>.SentinelNode)
            {
                WalkNextLevel(_tree_base_node, stack);
            }
            return stack;
        }

        private static void WalkNextLevel(RedBlackNode<TKey, TValue> node, Stack<RedBlackNode<TKey, TValue>> stack)
        {
            if (node.Right != RedBlackNode<TKey, TValue>.SentinelNode)
                WalkNextLevel(node.Right, stack);
            stack.Push(node);
            if (node.Left != RedBlackNode<TKey, TValue>.SentinelNode)
                WalkNextLevel(node.Left, stack);
        }

        /// <summary>
        /// Returns a node from the tree using the supplied key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The Node or null if the key does not exist</returns>
        private RedBlackNode<TKey, TValue> GetNode(TKey key)
        {
            int result;
            if (_last_node_found != RedBlackNode<TKey, TValue>.SentinelNode)
            {
                result = key.CompareTo(_last_node_found.Key);
                if (result == 0)
                    return _last_node_found;
            }

            // begin at root
            var tree_node = _tree_base_node;

            // traverse tree until node is found
            while (tree_node != RedBlackNode<TKey, TValue>.SentinelNode)
            {
                result = key.CompareTo(tree_node.Key);
                if (result == 0)
                {
                    _last_node_found = tree_node;
                    return tree_node;
                }
                tree_node = result < 0
                    ? tree_node.Left
                    : tree_node.Right;
            }
            return null;
        }

        ///<summary>
        /// Rotate Right
        /// Rebalance the tree by rotating the nodes to the right
        ///</summary>
        private void RotateRight(RedBlackNode<TKey, TValue> rotate_node)
        {
            // pushing node rotateNode down and to the Right to balance the tree. rotateNode's Left child (_workNode)
            // replaces rotateNode (since rotateNode < _workNode), and _workNode's Right child becomes rotateNode's Left child 
            // (since it's < rotateNode but > _workNode).

            // get rotateNode's Left node, this becomes _workNode
            RedBlackNode<TKey, TValue> work_node = rotate_node.Left;

            // set rotateNode's Right link
            // _workNode's Right child becomes rotateNode's Left child
            rotate_node.Left = work_node.Right;

            // modify parents
            if (work_node.Right != RedBlackNode<TKey, TValue>.SentinelNode)
                // sets _workNode's Right Parent to rotateNode
                work_node.Right.Parent = rotate_node;

            if (work_node != RedBlackNode<TKey, TValue>.SentinelNode)
                // set _workNode's Parent to rotateNode's Parent
                work_node.Parent = rotate_node.Parent;

            // null=rbTree, could also have used rbTree
            if (rotate_node.Parent != null)
            {	// determine which side of it's Parent rotateNode was on
                if (rotate_node == rotate_node.Parent.Right)
                    // set Right Parent to _workNode
                    rotate_node.Parent.Right = work_node;
                else
                    // set Left Parent to _workNode
                    rotate_node.Parent.Left = work_node;
            }
            else
                // at rbTree, set it to _workNode
                _tree_base_node = work_node;

            // link rotateNode and _workNode 
            // put rotateNode on _workNode's Right
            work_node.Right = rotate_node;
            // set _workNode as rotateNode's Parent
            if (rotate_node != RedBlackNode<TKey, TValue>.SentinelNode)
                rotate_node.Parent = work_node;
        }

        ///<summary>
        /// Rotate Left
        /// Rebalance the tree by rotating the nodes to the left
        ///</summary>
        private void RotateLeft(RedBlackNode<TKey, TValue> rotate_node)
        {
            // pushing node rotateNode down and to the Left to balance the tree. rotateNode's Right child (_workNode)
            // replaces rotateNode (since _workNode > rotateNode), and _workNode's Left child becomes rotateNode's Right child 
            // (since it's < _workNode but > rotateNode).

            // get rotateNode's Right node, this becomes _workNode
            RedBlackNode<TKey, TValue> workNode = rotate_node.Right;

            // set rotateNode's Right link
            // _workNode's Left child's becomes rotateNode's Right child
            rotate_node.Right = workNode.Left;

            // modify parents
            if (workNode.Left != RedBlackNode<TKey, TValue>.SentinelNode)
                // sets _workNode's Left Parent to rotateNode
                workNode.Left.Parent = rotate_node;

            if (workNode != RedBlackNode<TKey, TValue>.SentinelNode)
                // set _workNode's Parent to rotateNode's Parent
                workNode.Parent = rotate_node.Parent;

            if (rotate_node.Parent != null)
            {	// determine which side of it's Parent rotateNode was on
                if (rotate_node == rotate_node.Parent.Left)
                    // set Left Parent to _workNode
                    rotate_node.Parent.Left = workNode;
                else
                    // set Right Parent to _workNode
                    rotate_node.Parent.Right = workNode;
            }
            else
                // at rbTree, set it to _workNode
                _tree_base_node = workNode;

            // link rotateNode and _workNode
            // put rotateNode on _workNode's Left
            workNode.Left = rotate_node;
            // set _workNode as rotateNode's Parent
            if (rotate_node != RedBlackNode<TKey, TValue>.SentinelNode)
                rotate_node.Parent = workNode;
        }

        private void Initialize(int identifier)
        {
            _identifier = identifier;
            _hash_code = _rand.Next();
            _count = 0;
        }

        ///<summary>
        /// Balance Tree After Insert
        /// Additions to red-black trees usually destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterInsert(RedBlackNode<TKey, TValue> inserted_node)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            // maintain red-black tree properties after adding newNode
            while (inserted_node != _tree_base_node && inserted_node.Parent.Color == RedBlackNodeType.Red)
            {
                // Parent node is .Colored red; 
                RedBlackNode<TKey, TValue> work_node;
                if (inserted_node.Parent == inserted_node.Parent.Parent.Left)	// determine traversal path			
                {										// is it on the Left or Right subtree?
                    work_node = inserted_node.Parent.Parent.Right;			// get uncle
                    if (work_node != null && work_node.Color == RedBlackNodeType.Red)
                    {	// uncle is red; change x's Parent and uncle to black
                        inserted_node.Parent.Color = RedBlackNodeType.Black;
                        work_node.Color = RedBlackNodeType.Black;
                        // grandparent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        inserted_node.Parent.Parent.Color = RedBlackNodeType.Red;
                        inserted_node = inserted_node.Parent.Parent;	// continue loop with grandparent
                    }
                    else
                    {
                        // uncle is black; determine if newNode is greater than Parent
                        if (inserted_node == inserted_node.Parent.Right)
                        {	// yes, newNode is greater than Parent; rotate Left
                            // make newNode a Left child
                            inserted_node = inserted_node.Parent;
                            RotateLeft(inserted_node);
                        }
                        // no, newNode is less than Parent
                        inserted_node.Parent.Color = RedBlackNodeType.Black;	// make Parent black
                        inserted_node.Parent.Parent.Color = RedBlackNodeType.Red;		// make grandparent black
                        RotateRight(inserted_node.Parent.Parent);					// rotate right
                    }
                }
                else
                {	// newNode's Parent is on the Right subtree
                    // this code is the same as above with "Left" and "Right" swapped
                    work_node = inserted_node.Parent.Parent.Left;
                    if (work_node != null && work_node.Color == RedBlackNodeType.Red)
                    {
                        inserted_node.Parent.Color = RedBlackNodeType.Black;
                        work_node.Color = RedBlackNodeType.Black;
                        inserted_node.Parent.Parent.Color = RedBlackNodeType.Red;
                        inserted_node = inserted_node.Parent.Parent;
                    }
                    else
                    {
                        if (inserted_node == inserted_node.Parent.Left)
                        {
                            inserted_node = inserted_node.Parent;
                            RotateRight(inserted_node);
                        }
                        inserted_node.Parent.Color = RedBlackNodeType.Black;
                        inserted_node.Parent.Parent.Color = RedBlackNodeType.Red;
                        RotateLeft(inserted_node.Parent.Parent);
                    }
                }
            }
            _tree_base_node.Color = RedBlackNodeType.Black;		// rbTree should always be black
        }

        #endregion

    }

}