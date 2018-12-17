using System;

namespace AlgorithmsNutshell.Tree
{
    public class RedBlackEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; set; }
        public TValue Item { get; set; }
    }
}