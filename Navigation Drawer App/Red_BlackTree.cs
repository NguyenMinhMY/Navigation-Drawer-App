using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Navigation_Drawer_App
{
    /*class Node
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
            this.colour = Colour.Red;
            this.right = this.left = this.parent = null;
        }
        public Node() { }
    }*/
    /*public enum Colour
    {
        Red,
        Black
    }*/
    struct point
    {
        public double x, y;
    }
    public enum COLOR { RED,BLACK} 
    class Node
    {

        public int val;
        public COLOR color;
        public Node left, right, parent;
        public int pos, flo;

        public Node(int val)
        {
            this.val = val;
            parent = left = right = null;
            // Node is created during insertion
            // Node is red at insertion
            color = COLOR.RED;
        }
        // returns pointer to uncle
        public Node uncle()
        {
            // If no parent or grandparent, then no uncle
            if (parent == null || parent.parent == null)
                return null;

            if (parent.isOnLeft())
                // uncle on right
                return parent.parent.right;
            else
                // uncle on left
                return parent.parent.left;
        }

        // check if node is left child of parent
        public bool isOnLeft() { return this == parent.left; }

        // returns pointer to sibling
        public Node sibling()
        {
            // sibling null if no parent
            if (parent == null)
                return null;

            if (isOnLeft())
                return parent.right;

            return parent.left;
        }

        // moves node down and moves given node in its place
        public void moveDown(Node nParent)
        {
            if (parent != null)
            {
                if (isOnLeft())
                {
                    parent.left = nParent;
                }
                else
                {
                    parent.right = nParent;
                }
            }
            nParent.parent = parent;
            parent = nParent;
        }

        public bool hasRedChild()
        {
            return (left != null && left.color == COLOR.RED) ||
                (right != null && right.color == COLOR.RED);
        }
    };

    class RBTree : Window
    {
        double  time = 1, max_page = 0;
        public Node root;

        // left rotates the given node
        void leftRotate(Node x)
        {
            // new parent will be node's right child
            Node nParent = x.right;

            // update root if current node is root
            if (x == root)
                root = nParent;

            x.moveDown(nParent);

            // connect x with new parent's left element
            x.right = nParent.left;
            // connect new parent's left element with node
            // if it is not null
            if (nParent.left != null)
                nParent.left.parent = x;

            // connect new parent with x
            nParent.left = x;
        }

        void rightRotate(Node x)
        {
            // new parent will be node's left child
            Node nParent = x.left;

            // update root if current node is root
            if (x == root)
                root = nParent;

            x.moveDown(nParent);

            // connect x with new parent's right element
            x.left = nParent.right;
            // connect new parent's right element with node
            // if it is not null
            if (nParent.right != null)
                nParent.right.parent = x;

            // connect new parent with x
            nParent.right = x;
        }

        void swapColors(Node x1, Node x2)
        {
            COLOR temp;
            temp = x1.color;
            x1.color = x2.color;
            x2.color = temp;
        }

        void swapValues(Node u, Node v)
        {
            int temp;
            temp = u.val;
            u.val = v.val;
            v.val = temp;
        }

        // fix red red at given node
        async void fixRedRed(Node x,Canvas mainCanvas)
        {
            // if x is root color it black and return
            if (x == root)
            {
                x.color = COLOR.BLACK;
                //InOrderTraversal();
                update(mainCanvas);
                await Task.Delay(TimeSpan.FromSeconds(this.time));
                return;
            }

            // initialize parent, grandparent, uncle
            Node parent = x.parent, grandparent = parent.parent,
            uncle = x.uncle();

            if (parent.color != COLOR.BLACK)
            {
                if (uncle != null && uncle.color == COLOR.RED)
                {
                    // uncle red, perform recoloring and recurse
                    parent.color = COLOR.BLACK;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    uncle.color = COLOR.BLACK;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    grandparent.color = COLOR.RED;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    fixRedRed(grandparent,mainCanvas);
                }
                else
                {
                    // Else perform LR, LL, RL, RR
                    if (parent.isOnLeft())
                    {
                        if (x.isOnLeft())
                        {
                            // for left right
                            swapColors(parent, grandparent);
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                        else
                        {
                            leftRotate(parent);
                            InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                            swapColors(x, grandparent);
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                        // for left left and left right
                        rightRotate(grandparent);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                    else
                    {
                        if (x.isOnLeft())
                        {
                            // for right left
                            rightRotate(parent);
                            InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                            swapColors(x, grandparent);
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                        else
                        {
                            swapColors(parent, grandparent);
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }

                        // for right right and right left
                        leftRotate(grandparent);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                }
            }
        }

        // find node that do not have a left child
        // in the subtree of the given node
        Node successor(Node x)
        {
            Node temp = x;

            while (temp.left != null)
                temp = temp.left;

            return temp;
        }

        // find node that replaces a deleted node in BST
        Node BSTreplace(Node x)
        {
            // when node have 2 children
            if (x.left != null && x.right != null)
                return successor(x.right);

            // when leaf
            if (x.left == null && x.right == null)
                return null;

            // when single child
            if (x.left != null)
                return x.left;
            else
                return x.right;
        }

        // deletes the given node
        public async void deleteNode(Node v,Canvas mainCanvas)
        {
            Node u = BSTreplace(v);

            // True when u and v are both black

            bool uvBlack = ((u == null || u.color == COLOR.BLACK) && (v.color == COLOR.BLACK));
            Node parent = v.parent;

            if (u == null)
            {
                // u is null therefore v is leaf
                if (v == root)
                {
                    // v is root, making root null
                    root = null;
                    InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                }
                else
                {
                    if (uvBlack)
                    {
                        // u and v both black
                        // v is leaf, fix double black at v
                        fixDoubleBlack(v,mainCanvas);
                    }
                    else
                    {
                        // u or v is red
                        if (v.sibling() != null)
                        {
                            // sibling is not null, make it red"
                            v.sibling().color = COLOR.RED;
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                            
                    }

                    // delete v from the tree
                    if (v.isOnLeft())
                    {
                        parent.left = null;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                    else
                    {
                        parent.right = null;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                }
                return;
            }

            if (v.left == null || v.right == null)
            {
                // v has 1 child
                if (v == root)
                {
                    // v is root, assign the value of u to v, and delete u
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    v.val = u.val;
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    v.left = v.right = null;
                    InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                }
                else
                {
                    // Detach v from tree and move u up
                    if (v.isOnLeft())
                    {
                        parent.left = u;

                    }
                    else
                    {
                        parent.right = u;

                    }
                    u.parent = parent;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    if (uvBlack)
                    {
                        // u and v both black, fix double black at u
                        fixDoubleBlack(u,mainCanvas);
                    }
                    else
                    {
                        // u or v red, color u black
                        u.color = COLOR.BLACK;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                }
                return;
            }

            // v has 2 children, swap values with successor and recurse
            await Task.Delay(TimeSpan.FromSeconds(this.time));
            swapValues(u, v);
            //InOrderTraversal();
            await Task.Delay(TimeSpan.FromSeconds(this.time));
            update(mainCanvas);
            await Task.Delay(TimeSpan.FromSeconds(this.time));
            deleteNode(u,mainCanvas);
        }

        async void fixDoubleBlack(Node x,Canvas mainCanvas)
        {
            if (x == root)
                // Reached root
                return;

            Node sibling = x.sibling(), parent = x.parent;
            if (sibling == null)
            {
                // No sibiling, double black pushed up
                fixDoubleBlack(parent,mainCanvas);
            }
            else
            {
                if (sibling.color == COLOR.RED)
                {
                    // Sibling red
                    parent.color = COLOR.RED;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    sibling.color = COLOR.BLACK;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    if (sibling.isOnLeft())
                    {
                        // left case
                        rightRotate(parent);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                    else
                    {
                        // right case
                        leftRotate(parent);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                    fixDoubleBlack(x,mainCanvas);
                }
                else
                {
                    // Sibling black
                    if (sibling.hasRedChild())
                    {
                        // at least 1 red children
                        if (sibling.left != null && sibling.left.color == COLOR.RED)
                        {
                            if (sibling.isOnLeft())
                            {
                                // left left
                                sibling.left.color = sibling.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                sibling.color = parent.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                rightRotate(parent);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                            else
                            {
                                // right left
                                sibling.left.color = parent.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                rightRotate(sibling);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                leftRotate(parent);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                        }
                        else
                        {
                            if (sibling.isOnLeft())
                            {
                                // left right
                                sibling.right.color = parent.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                leftRotate(sibling);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                rightRotate(parent);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                            else
                            {
                                // right right
                                sibling.right.color = sibling.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                sibling.color = parent.color;
                                //InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                                leftRotate(parent);
                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                        }
                        parent.color = COLOR.BLACK;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }
                    else
                    {
                        // 2 black children
                        sibling.color = COLOR.RED;
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        if (parent.color == COLOR.BLACK)
                            fixDoubleBlack(parent, mainCanvas);
                        else
                        {
                            parent.color = COLOR.BLACK;
                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                    }
                }
            }
        }

        // prints level order for given node
        void levelOrder(Node x)
        {
            if (x == null)
                // return if node is null
                return;

            // queue for level order
            Queue<Node> q = new Queue<Node>();
            Node curr;

            // push x
            q.Enqueue(x);

            while (q.Count > 0)
            {
                // while q is not empty
                // dequeue
                curr = q.Peek();
                q.Dequeue();

                // print node value
                //if (curr == null) Console.Write("NULL");
                Console.Write("{0} ", curr.val);
                Console.Write("{0} ", curr.color == COLOR.RED ? "R " : "B ");
                Console.WriteLine("parent: {0} ", curr.parent == null ? -1 : curr.parent.val);
                // push children to queue
                if (curr.left != null)
                    q.Enqueue(curr.left);
                if (curr.right != null)
                    q.Enqueue(curr.right);
            }
        }

        // prints inorder recursively
        void inorder(Node x)
        {
            if (x == null)
                return;
            inorder(x.left);
            //cout << x.val << " ";
            inorder(x.right);
        }

        // constructor
        // initialize root
        public RBTree() { root = null; }

        Node getRoot() { return root; }

        // searches for given value
        // if found returns the node (used for delete)
        // else returns the last node while traversing (used in insert)
        void move(Ellipse target, double oldX, double oldY, double newX,
        double newY, double time)
        {
            TranslateTransform trans = new TranslateTransform();

            target.RenderTransform = trans;
            target.RenderTransformOrigin = new Point(0, 0);
            DoubleAnimation anim1 = new DoubleAnimation(oldY, newY,
    TimeSpan.FromSeconds(time));

            trans.BeginAnimation(TranslateTransform.YProperty, anim1);

            DoubleAnimation anim2 = new DoubleAnimation(oldX, newX,
    TimeSpan.FromSeconds(time));

            trans.BeginAnimation(TranslateTransform.XProperty, anim2);


        }
        public async void find(int value,Canvas mainCanvas)
        {
            
            Ellipse Circle = new Ellipse();
            Circle.StrokeThickness = 3;
            Circle.Stroke = Brushes.Blue;
            Circle.Width = 41;
            Circle.Height = 41;
            Circle.Opacity = 1;
            
            mainCanvas.Children.Add(Circle);
            Node cur = this.root;
            if ( value == cur.val)
            {
                Canvas.SetTop(Circle, 20);
                Canvas.SetLeft(Circle, 464);
                await Task.Delay(TimeSpan.FromSeconds(1));
                mainCanvas.Children.Remove(Circle);
                return;
            }
            while (cur != null)
            {
              
                if ( cur.val == value)
                {
                    break;
                }
                else if ( value < cur.val) cur = cur.left;
                else if ( value > cur.val) cur = cur.right;
                point cur_coordinate = find_point(cur);
                if (cur.flo == 1 && cur.parent.pos == 1)
                {
                    move(Circle, 464 - cur_coordinate.x, 20 - cur_coordinate.y, 0, 0, time);
                }
                else
                {
                    point par_coordinate = find_point(cur.parent);
                    move(Circle, par_coordinate.x - cur_coordinate.x, par_coordinate.y - cur_coordinate.y, 0, 0, time);
                }
                Canvas.SetTop(Circle, cur_coordinate.y);
                Canvas.SetLeft(Circle, cur_coordinate.x);
                await Task.Delay(TimeSpan.FromSeconds(time));

            }
            await Task.Delay(TimeSpan.FromSeconds(1));
            mainCanvas.Children.Remove(Circle);
        }
        public Node search(int n)
        {
            Node temp = root;
            while (temp != null)
            {
                if (n < temp.val)
                {
                    if (temp.left == null)
                        break;
                    else
                        temp = temp.left;
                }
                else if (n == temp.val)
                {
                    break;
                }
                else
                {
                    if (temp.right == null)
                        break;
                    else
                        temp = temp.right;
                }
            }

            return temp;

        }

        // inserts the given value to tree
        public async void insert(int n,Canvas mainCanvas)
        {
            Node newNode = new Node(n);
            if (root == null)
            {
                // when root is null
                // simply insert value at root
                newNode.color = COLOR.BLACK;
                //InOrderTraversal();
                root = newNode;

          

                //   this.find(n, slider, mainCanvas);           
                update(mainCanvas);
                 await Task.Delay(TimeSpan.FromSeconds(this.time));
            }
            else
            {
                Node Y = null;
                Node X = root;
                while (X != null)
                {
                    Y = X;
                    if (newNode.val < X.val)
                    {
                        X = X.left;
                    }
                    else
                    {
                        X = X.right;
                    }
                }
                newNode.parent = Y;
                if (Y == null)
                {
                    root = newNode;
                }
                else if (newNode.val < Y.val)
                {
                    Y.left = newNode;
                }
                else
                {
                    Y.right = newNode;
                }

                newNode.left = null;
                newNode.right = null;
                newNode.color = COLOR.RED;



                Ellipse Circle = new Ellipse();
                Circle.StrokeThickness = 3;
                Circle.Stroke = Brushes.Blue;
                Circle.Width = 41;
                Circle.Height = 41;
                Circle.Opacity = 1;

                mainCanvas.Children.Add(Circle);
                Node cur = this.root;
          
                while (cur != null)
                {
                  
                 
                    if (cur.val == newNode.parent.val)
                    {
                        if(cur==this.root)
                        {
                            Canvas.SetTop(Circle, 20);
                            Canvas.SetLeft(Circle, 464);
                        }    
                        break;
                    }
                    else if (newNode.parent.val < cur.val) cur = cur.left;
                    else if (newNode.parent.val > cur.val) cur = cur.right;
                    point cur_coordinate = find_point(cur);
                    if (cur.flo == 1 && cur.parent.pos == 1)
                    {
                       
                        move(Circle, 464 - cur_coordinate.x, 20 - cur_coordinate.y, 0, 0, time);
                    }
                    else
                    {
                        point par_coordinate = find_point(cur.parent);
                       
                        move(Circle, par_coordinate.x - cur_coordinate.x, par_coordinate.y - cur_coordinate.y, 0, 0, time);

                    }
                    Canvas.SetTop(Circle, cur_coordinate.y);
                    Canvas.SetLeft(Circle, cur_coordinate.x);

                    await Task.Delay(TimeSpan.FromSeconds(time));

                }
                await Task.Delay(TimeSpan.FromSeconds(1));
                mainCanvas.Children.Remove(Circle);
                InOrderTraversal();
                update(mainCanvas);
                await Task.Delay(TimeSpan.FromSeconds(this.time));
                fixRedRed(newNode,mainCanvas);
            }
        }

        // utility function that deletes the node with given value
        public async void deleteByVal(int n,Canvas mainCanvas)
        {
            if (root == null)
                // Tree is empty
                return;

            //Node v = search(n), u;
            Node v = search(n);

            Ellipse Circle = new Ellipse();
            Circle.StrokeThickness = 3;
            Circle.Stroke = Brushes.Blue;
            Circle.Width = 41;
            Circle.Height = 41;
            Circle.Opacity = 1;

            mainCanvas.Children.Add(Circle);
            Node cur = this.root;
            if (v.val == cur.val)
            {
                Canvas.SetTop(Circle, 20);
                Canvas.SetLeft(Circle, 464);
                await Task.Delay(TimeSpan.FromSeconds(1));
                mainCanvas.Children.Remove(Circle);

            }
            while (cur != null)
            {

                if (cur.val == v.val)
                {
                    break;
                }
                else if (v.val < cur.val) cur = cur.left;
                else if (v.val > cur.val) cur = cur.right;
                point cur_coordinate = find_point(cur);
                if (cur.flo == 1 && cur.parent.pos == 1)
                {
                    move(Circle, 464 - cur_coordinate.x, 20 - cur_coordinate.y, 0, 0, time);
                }
                else
                {
                    point par_coordinate = find_point(cur.parent);
                    move(Circle, par_coordinate.x - cur_coordinate.x, par_coordinate.y - cur_coordinate.y, 0, 0, time);
                }
                Canvas.SetTop(Circle, cur_coordinate.y);
                Canvas.SetLeft(Circle, cur_coordinate.x);
                await Task.Delay(TimeSpan.FromSeconds(time));

            }
            await Task.Delay(TimeSpan.FromSeconds(1));
            mainCanvas.Children.Remove(Circle);

            deleteNode(v,mainCanvas);
        }
        public void printLevelOrder()
        {
            Console.WriteLine("Level order: ");
            if (root == null)
                Console.WriteLine("Tree is empty");
            else
                levelOrder(root);
            Console.WriteLine();
        }
        public async void update(Canvas mainCanvas)
        {
            //InOrderTraversal();
            mainCanvas.Children.Clear();
            DisplayTree(this.root, mainCanvas);

            //await Task.Delay(TimeSpan.FromSeconds(this.time));
        }
        private void InOrderTraversal(Node current, ref int index)
        {
            if (current != null)
            {
                InOrderTraversal(current.left, ref index);

                current.pos = index;
                index = index + 1;

                InOrderTraversal(current.right, ref index);
            }
        }

        public async void InOrderTraversal()
        {

            int index = 0;
            InOrderTraversal(this.root, ref index);
        }
        point find_point(Node node) // set toa do diem ve circle
        {
            point toado;
            toado.x = 464 + (node.pos - this.root.pos) * 40;
            toado.y = 20 + 60 * node.flo;

            return toado;
        }
        public void Draw_Circle(Node node, Canvas mainCanvas)
        {
            // draw elip
            Ellipse Circle = new Ellipse();

            Circle.StrokeThickness = 2;
            Circle.Stroke = Brushes.Black;
            Circle.Width = 40;
            Circle.Height = 40;
            Circle.Opacity = 0.5;
            if (/*node.colour == 0*/ node.color == 0)
                Circle.Fill = System.Windows.Media.Brushes.Red;
            else
                Circle.Fill = System.Windows.Media.Brushes.Gray;
            // add content
            Label content = new Label();
            content.Content = /*node.data.ToString();*/ node.val.ToString();
            //content.Content = node.pos.ToString();
            content.Foreground = System.Windows.Media.Brushes.Black;
            content.HorizontalContentAlignment = HorizontalAlignment.Center;
            content.FontSize = 15;
            
            mainCanvas.Children.Add(Circle);
            mainCanvas.Children.Add(content);

            point coordinate_Circle = find_point(node);


            Canvas.SetTop(Circle, coordinate_Circle.y);
            Canvas.SetLeft(Circle, coordinate_Circle.x);

            Canvas.SetTop(content, coordinate_Circle.y);
            Canvas.SetLeft(content, coordinate_Circle.x+5);


            return;
        }
        public void Draw_Rectangle(Node node, Canvas mainCanvas)
        {
            Rectangle rectangle = new Rectangle();

            rectangle.StrokeThickness = 2;
            rectangle.Stroke = Brushes.Black;
            rectangle.Width = 40;
            rectangle.Height = 40;
            rectangle.Opacity = 0.5;
            rectangle.Fill = System.Windows.Media.Brushes.Gray;
            // add content
            Label content = new Label();
            content.Content = "Nil";
            content.Foreground = System.Windows.Media.Brushes.Black;
            content.HorizontalContentAlignment = HorizontalAlignment.Center;
            mainCanvas.Children.Add(rectangle);
            mainCanvas.Children.Add(content);

        }
        public void Draw_Line(Node par, Node child, Canvas mainCanvas)
        {
            Line line_1 = new Line();
            point coordinate_par = find_point(par);
            point coordinate_child = find_point(child);
            if (child.parent.parent == null)
            {
                line_1.X1 = 484;
                line_1.Y1 = 40;
                line_1.X2 = coordinate_child.x + 20;
                line_1.Y2 = coordinate_child.y + 20;
            }
            else
            {
                line_1.X1 = coordinate_par.x + 20;
                line_1.Y1 = coordinate_par.y + 20;
                line_1.X2 = coordinate_child.x + 20;
                line_1.Y2 = coordinate_child.y + 20;
            }

            line_1.Stroke = System.Windows.Media.Brushes.Black;
            line_1.StrokeThickness = 2;
            mainCanvas.Children.Add(line_1);
        }
        public async void DisplayTree(Node node, Canvas mainCanvas)
        {
            if (node != null)
            {
                if (node.parent == null)
                {
                    //node.pos = 1;
                    node.flo = 0;
                }
                else
                {
                    node.flo = node.parent.flo + 1;
                }
                if (node.parent != null)
                {
                    Draw_Line(node.parent, node, mainCanvas);
                }

                Draw_Circle(node, mainCanvas);
               
                DisplayTree(node.left, mainCanvas);
                DisplayTree(node.right, mainCanvas);


            }
            else return;
        }
        /*private void clone(Node newRoot,Node root) 
        {
            
            Node right = root.right;
            Node left = root.left;
            if (right == null)
                newRoot.right = null;
            else
            {
                newRoot.right = new Node(right.val);
                newRoot.right.color = right.color;
                //newRoot.right.flo = right.flo;
                //newRoot.right.pos = right.pos;
                newRoot.right.parent = newRoot;
                clone(newRoot.right,root.right);
            }
            if (left == null)
                newRoot.left = null;
            else
            {
                newRoot.left = new Node(left.val);
                newRoot.left.color = left.color;
                //newRoot.left.flo = left.flo;
                //newRoot.left.pos = left.pos;
                newRoot.left.parent = newRoot;

                clone(newRoot.left,root.left);
            }
        }
        public Node clone()
        {
            Node newRoot = null;
            if (this.root != null)
            {
                newRoot = new Node(this.root.val);
                newRoot.color = this.root.color;
                newRoot.flo = this.root.flo;
                newRoot.pos = this.root.pos;
                clone(newRoot, this.root);
            }
            return newRoot;
        }*/
        private Node clone(Node current)
        {
            if (current != null)
            {
                Node t = new Node(current.val);
                t.color = current.color;
                t.left = clone(current.left);
                if (t.left != null) t.left.parent = t;
                t.right = clone(current.right);
                if (t.right != null) t.right.parent = t;
                if (current.parent == null) t.parent = current.parent;
                return t;
            }
            return null;
        }
        //return root of cloned tree
        public RBTree cloneTree()
        {
            RBTree tree = new RBTree();
            tree.root = this.clone(this.root);
            return tree;
        }
    }
    
}
