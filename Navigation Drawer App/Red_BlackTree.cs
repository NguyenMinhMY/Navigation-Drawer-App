using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Drawer_App
{
    class Node
    {
        public Node left { get; set; }
        public Node right { get; set; }

        public Node parent { get; set; }
        public int data { get; set; }
        public Colour colour;
        public int flo { get; set; }
        public int pos { get; set; }
        public Node(Colour colour) { this.colour = colour; }
        public Node(int data, Colour colour) { this.data = data; this.colour = colour; }
        public Node(int data)
        {
            this.data = data;
            this.flo = 0;
            this.pos = 1;
            this.colour = Colour.Red;
            this.right = this.left = this.parent = null;
        }
    }
    enum Colour
    {
        Red,
        Black
    }
    class Red_BlackTree
    {
        public Node root { get; set; }
        public Red_BlackTree() {
            this.root = null;
        }
        private void LeftRotate(Node X)
        {
            Node Y = X.right;
            X.right = Y.left;
            if (Y.left != null)
            {
                Y.left.parent = X;
            }
            if (Y != null)
            {
                Y.parent = X.parent;//link X's parent to Y
            }
            if (X.parent == null)
            {
                root = Y;
            }
            if (X == X.parent.left)
            {
                X.parent.left = Y;
            }
            else
            {
                X.parent.right = Y;
            }
            Y.left = X; //put X on Y's left
            if (X != null)
            {
                X.parent = Y;
            }

        }
        private void RightRotate(Node Y)
        {
            Node X = Y.left;
            Y.left = X.right;
            if (X.right != null)
            {
                X.right.parent = Y;
            }
            if (X != null)
            {
                X.parent = Y.parent;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            if (Y == Y.parent.right)
            {
                Y.parent.right = X;
            }
            if (Y == Y.parent.left)
            {
                Y.parent.left = X;
            }

            X.right = Y;
            if (Y != null)
            {
                Y.parent = X;
            }
        }
        public Node Find(int key)
        {
            Button
            bool isFound = false;
            Node temp = root;
            Node item = null;
            while (!isFound)
            {
                if (temp == null)
                {
                    break;
                }
                if (key < temp.data)
                {
                    temp = temp.left;
                }
                if (key > temp.data)
                {
                    temp = temp.right;
                }
                if (key == temp.data)
                {
                    isFound = true;
                    item = temp;
                }
            }
            if (isFound)
            {
                return temp;
            }
            else
            {
                return null;
            }
        }
        public void Insert(int item)
        {
            Node newItem = new Node(item);
            if (root == null)
            {
                root = newItem;
                root.colour = Colour.Black;
                return;
            }
            Node Y = null;
            Node X = root;
            while (X != null)
            {
                Y = X;
                if (newItem.data < X.data)
                {
                    X = X.left;
                }
                else
                {
                    X = X.right;
                }
            }
            newItem.parent = Y;
            if (Y == null)
            {
                root = newItem;
            }
            else if (newItem.data < Y.data)
            {
                Y.left = newItem;
            }
            else
            {
                Y.right = newItem;
            }
            newItem.left = null;
            newItem.right = null;
            newItem.colour = Colour.Red;
            InsertFixUp(newItem);
        }
        private void InOrderDisplay(Node current)
        {
            if (current != null)
            {
                InOrderDisplay(current.left);
                Console.Write("({0}) ", current.data);
                InOrderDisplay(current.right);
            }
        }
        private void InsertFixUp(Node item)
        {
            //Checks Red-Black Tree properties
            while (item != root && item.parent.colour == Colour.Red)
            {
                /*We have a violation*/
                if (item.parent == item.parent.parent.left)
                {
                    Node Y = item.parent.parent.right;
                    if (Y != null && Y.colour == Colour.Red)//Case 1: uncle is red
                    {
                        item.parent.colour = Colour.Black;
                        Y.colour = Colour.Black;
                        item.parent.parent.colour = Colour.Red;
                        item = item.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.parent.right)
                        {
                            item = item.parent;
                            LeftRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = Colour.Black;
                        item.parent.parent.colour = Colour.Red;
                        RightRotate(item.parent.parent);
                    }

                }
                else
                {
                    //mirror image of code above
                    Node X = null;

                    X = item.parent.parent.left;
                    if (X != null && X.colour == Colour.Black)//Case 1
                    {
                        item.parent.colour = Colour.Red;
                        X.colour = Colour.Red;
                        item.parent.parent.colour = Colour.Black;
                        item = item.parent.parent;
                    }
                    else //Case 2
                    {
                        if (item == item.parent.left)
                        {
                            item = item.parent;
                            RightRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = Colour.Black;
                        item.parent.parent.colour = Colour.Red;
                        LeftRotate(item.parent.parent);

                    }

                }
                root.colour = Colour.Black;
            }
        }
        public void Delete(int key)
        {

            Node item = Find(key);
            Node X = null;
            Node Y = null;

            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (item.left == null || item.right == null)
            {
                Y = item;
            }
            else
            {
                Y = TreeSuccessor(item);
            }
            if (Y.left != null)
            {
                X = Y.left;
            }
            else
            {
                X = Y.right;
            }
            if (X != null)
            {
                X.parent = Y;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            else if (Y == Y.parent.left)
            {
                Y.parent.left = X;
            }
            else
            {
                Y.parent.left = X;
            }
            if (Y != item)
            {
                item.data = Y.data;
            }
            if (Y.colour == Colour.Black)
            {
                DeleteFixUp(X);
            }

        }
        private void DeleteFixUp(Node X)
        {

            while (X != null && X != root && X.colour == Colour.Black)
            {
                if (X == X.parent.left)
                {
                    Node W = X.parent.right;
                    if (W.colour == Colour.Red)
                    {
                        W.colour = Colour.Black; //case 1
                        X.parent.colour = Colour.Red; //case 1
                        LeftRotate(X.parent); //case 1
                        W = X.parent.right; //case 1
                    }
                    if (W.left.colour == Colour.Black && W.right.colour == Colour.Black)
                    {
                        W.colour = Colour.Red; //case 2
                        X = X.parent; //case 2
                    }
                    else if (W.right.colour == Colour.Black)
                    {
                        W.left.colour = Colour.Black; //case 3
                        W.colour = Colour.Red; //case 3
                        RightRotate(W); //case 3
                        W = X.parent.right; //case 3
                    }
                    W.colour = X.parent.colour; //case 4
                    X.parent.colour = Colour.Black; //case 4
                    W.right.colour = Colour.Black; //case 4
                    LeftRotate(X.parent); //case 4
                    X = root; //case 4
                }
                else 
                {
                    Node W = X.parent.left;
                    if (W.colour == Colour.Red)
                    {
                        W.colour = Colour.Black;
                        X.parent.colour = Colour.Red;
                        RightRotate(X.parent);
                        W = X.parent.left;
                    }
                    if (W.right.colour == Colour.Black && W.left.colour == Colour.Black)
                    {
                        W.colour = Colour.Black;
                        X = X.parent;
                    }
                    else if (W.left.colour == Colour.Black)
                    {
                        W.right.colour = Colour.Black;
                        W.colour = Colour.Red;
                        LeftRotate(W);
                        W = X.parent.left;
                    }
                    W.colour = X.parent.colour;
                    X.parent.colour = Colour.Black;
                    W.left.colour = Colour.Black;
                    RightRotate(X.parent);
                    X = root;
                }
            }
            if (X != null)
                X.colour = Colour.Black;
        }
        private Node Minimum(Node X)
        {
            while (X.left.left != null)
            {
                X = X.left;
            }
            if (X.left.right != null)
            {
                X = X.left.right;
            }
            return X;
        }
        private Node TreeSuccessor(Node X)
        {
            if (X.left != null)
            {
                return Minimum(X);
            }
            else
            {
                Node Y = X.parent;
                while (Y != null && X == Y.right)
                {
                    X = Y;
                    Y = Y.parent;
                }
                return Y;
            }
        }
    }
}