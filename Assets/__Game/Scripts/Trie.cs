using System.Collections.Generic;

namespace __Game.Scripts
{
    public class TrieNode
    {
        public char value;
        public bool isEndOfWord;
        public Dictionary<char, TrieNode> children;

        public TrieNode(char ch)
        {
            value = ch;
            isEndOfWord = false;
            children = new Dictionary<char, TrieNode>();
        }
    }
    
    public class Trie
    {
        private TrieNode root;

        public Trie()
        {
            root = new TrieNode(' ');
        }

        public void Insert(string word)
        {
            var node = root;
            foreach (var ch in word)
            {
                if (!node.children.ContainsKey(ch))
                {
                    node.children[ch] = new TrieNode(ch);
                }
                node = node.children[ch];
            }
            node.isEndOfWord = true;
        }

        public bool Search(string word)
        {
            var node = root;
            foreach (var ch in word)
            {
                if (!node.children.ContainsKey(ch))
                {
                    return false;
                }
                node = node.children[ch];
            }
            return node.isEndOfWord;
        }
    }
}