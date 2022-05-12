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
            this.colour = Colour.Red;
            this.right = this.left = this.parent = null;
        }
        public Node() { }
    }
    enum Colour
    {
        Red,
        Black
    }
    struct point
    {
        public double x, y;
    }
    class Red_BlackTree : Window
    {

        double x = 464, y = 20, time = 1, max_page = 0;
        public Node root { get; set; }
        public Red_BlackTree()
        {
            //InitializeComponent();
            this.root = null;
        }
        //static int index = 0;
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
            if (X.parent != null)
            {
                if (X == X.parent.left)
                {
                    X.parent.left = Y;
                }
                else
                {
                    X.parent.right = Y;
                }
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
            if (Y.parent != null)
            {
                if (Y == Y.parent.right)
                {
                    Y.parent.right = X;
                }
                if (Y == Y.parent.left)
                {
                    Y.parent.left = X;
                }
            }

            X.right = Y;
            if (Y != null)
            {
                Y.parent = X;
            }
        }
        public Node Find(int key)
        {
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
                else if (key > temp.data)
                {
                    temp = temp.right;
                }
                else
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
        public async void update(Canvas mainCanvas)
        {
            //InOrderTraversal();
            mainCanvas.Children.Clear();
            DisplayTree(this.root, mainCanvas);

            //await Task.Delay(TimeSpan.FromSeconds(this.time));
        }
        private bool ll = false;
        private bool rr = false;
        private bool lr = false;
        private bool rl = false;

        public async void Insert(int item, Canvas mainCanvas)
        {
            Node newItem = new Node(item);
            if (root == null)
            {
                root = newItem;
                root.colour = Colour.Black;
                InOrderTraversal();
                update(mainCanvas);
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

            InOrderTraversal();
            update(mainCanvas);
            await Task.Delay(TimeSpan.FromSeconds(this.time));

            InsertFixUp(newItem, mainCanvas);
            max_page++;
            //await Task.Delay(TimeSpan.FromSeconds(this.time));
            FileStream fs = File.Open("State" + max_page.ToString() + ".xaml", FileMode.Create);
            XamlWriter.Save(mainCanvas, fs);
            fs.Close();
        }

        private void InOrderTraversal(Node current, ref int index)
        {
            if (current != null)
            {
                InOrderTraversal(current.left, ref index);

                current.pos = index;
                index++;

                InOrderTraversal(current.right, ref index);
            }
        }

        public async void InOrderTraversal()
        {
            int index = 0;
            InOrderTraversal(this.root, ref index);
        }
        private async void InsertFixUp(Node item, Canvas mainCanvas)
        {
            //Checks Red-Black Tree properties
            while (item.parent != null && item.parent.colour == Colour.Red)
            {
                if (item.parent == item.parent.parent.left)
                {
                    Node Y = item.parent.parent.right;
                    if (Y != null)//Case 1: uncle is red
                    {
                        if (Y.colour == Colour.Red)
                        {
                            item.parent.colour = Colour.Black;

                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            Y.colour = Colour.Black;

                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item.parent.parent.colour = Colour.Red;

                            //InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item = item.parent.parent;
                        }
                        else //Case 2: uncle is black
                        {
                            if (item == item.parent.right)
                            {
                                item = item.parent;
                                LeftRotate(item);

                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                            //Case 3: recolour & rotate
                            item.parent.colour = Colour.Black;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            Y.colour = Colour.Black;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item.parent.parent.colour = Colour.Red;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                        }
                    }
                    else
                    {
                        if (item == item.parent.right)
                        {
                            item = item.parent;
                            LeftRotate(item);

                            InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = Colour.Black;

                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        item.parent.parent.colour = Colour.Red;

                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        RightRotate(item.parent.parent);

                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                    }

                }
                else if (item.parent == item.parent.parent.right)
                {
                    //mirror image of code above
                    Node X = null;

                    X = item.parent.parent.left;
                    if (X != null)//Case 1
                    {
                        if (X.colour == Colour.Black)
                        {
                            item.parent.colour = Colour.Red;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            X.colour = Colour.Red;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item.parent.parent.colour = Colour.Black;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item = item.parent.parent;

                        }
                        else //Case 2
                        {
                            if (item == item.parent.left)
                            {
                                item = item.parent;
                                RightRotate(item);

                                InOrderTraversal();
                                update(mainCanvas);
                                await Task.Delay(TimeSpan.FromSeconds(this.time));
                            }
                            //Case 3: recolour & rotate
                            item.parent.colour = Colour.Black;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            X.colour = Colour.Black;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));

                            item.parent.parent.colour = Colour.Red;

                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                    }
                    else
                    {
                        if (item == item.parent.left)
                        {
                            item = item.parent;
                            RightRotate(item);

                            InOrderTraversal();
                            update(mainCanvas);
                            await Task.Delay(TimeSpan.FromSeconds(this.time));
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = Colour.Black;

                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        item.parent.parent.colour = Colour.Red;

                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        LeftRotate(item.parent.parent);

                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                    }

                }
                root.colour = Colour.Black;

                update(mainCanvas);
                await Task.Delay(TimeSpan.FromSeconds(this.time));
            }
        }
        public void Delete(int key, Canvas mainCanvas)
        {

            Node item = Find(key);
            Node X = null;
            Node Y = null;

            if (item == null)
            {
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
                DeleteFixUp(X, mainCanvas);
            }

            update(mainCanvas);
        }
        private async void DeleteFixUp(Node X, Canvas mainCanvas)
        {

            while (X != null && X != root && X.colour == Colour.Black)
            {
                if (X == X.parent.left)
                {
                    Node W = X.parent.right;
                    if (W.colour == Colour.Red)
                    {
                        W.colour = Colour.Black; //case 1

                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        X.parent.colour = Colour.Red; //case 1

                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        LeftRotate(X.parent); //case 1

                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        W = X.parent.right; //case 1
                    }
                    if (W.left.colour == Colour.Black && W.right.colour == Colour.Black)
                    {
                        W.colour = Colour.Red; //case 2

                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        X = X.parent; //case 2
                    }
                    else if (W.right.colour == Colour.Black)
                    {
                        W.left.colour = Colour.Black; //case 3

                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        W.colour = Colour.Red; //case 3

                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        RightRotate(W); //case 3

                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));

                        W = X.parent.right; //case 3
                    }
                    W.colour = X.parent.colour; //case 4
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    X.parent.colour = Colour.Black; //case 4
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    W.right.colour = Colour.Black; //case 4
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    LeftRotate(X.parent); //case 4
                    InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    X = root; //case 4
                }
                else
                {
                    Node W = X.parent.left;
                    if (W.colour == Colour.Red)
                    {
                        W.colour = Colour.Black;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        X.parent.colour = Colour.Red;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        RightRotate(X.parent);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        W = X.parent.left;
                    }
                    if (W.right.colour == Colour.Black && W.left.colour == Colour.Black)
                    {
                        W.colour = Colour.Black;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        X = X.parent;
                    }
                    else if (W.left.colour == Colour.Black)
                    {
                        W.right.colour = Colour.Black;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        W.colour = Colour.Red;
                        //InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        LeftRotate(W);
                        InOrderTraversal();
                        update(mainCanvas);
                        await Task.Delay(TimeSpan.FromSeconds(this.time));
                        W = X.parent.left;
                    }
                    W.colour = X.parent.colour;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    X.parent.colour = Colour.Black;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    W.left.colour = Colour.Black;
                    //InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    RightRotate(X.parent);
                    InOrderTraversal();
                    update(mainCanvas);
                    await Task.Delay(TimeSpan.FromSeconds(this.time));
                    X = root;
                }
            }
            if (X != null)
            {
                X.colour = Colour.Black;
                //InOrderTraversal();
                update(mainCanvas);
                await Task.Delay(TimeSpan.FromSeconds(this.time));
            }
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
            if (node.colour == 0)
                Circle.Fill = System.Windows.Media.Brushes.Red;
            else
                Circle.Fill = System.Windows.Media.Brushes.Gray;
            // add content
            Label content = new Label();
            content.Content = node.data.ToString();
            //content.Content = node.pos.ToString();
            content.Foreground = System.Windows.Media.Brushes.Black;
            content.HorizontalContentAlignment = HorizontalAlignment.Center;
            mainCanvas.Children.Add(Circle);
            mainCanvas.Children.Add(content);

            point coordinate_Circle = find_point(node);


            Canvas.SetTop(Circle, coordinate_Circle.y);
            Canvas.SetLeft(Circle, coordinate_Circle.x);

            Canvas.SetTop(content, coordinate_Circle.y);
            Canvas.SetLeft(content, coordinate_Circle.x);


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
                    /*if (node.parent.left == node)
                        node.pos = node.parent.pos * 2 - 1;
                    else if (node.parent.right == node)
                        node.pos = node.parent.pos * 2;*/
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

    }
}
