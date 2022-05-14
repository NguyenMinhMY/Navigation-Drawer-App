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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
     
        RBTree tree = null;
        //List<Node> list_node= new List<Node>();
        List<RBTree> list_tree = new List<RBTree>();
        public MainWindow()
        {
            InitializeComponent();
            tree = new RBTree();
        }
        int hide = 0;
        point find_point(Node node) // set toa do diem ve circle
        {
            point toado;
            toado.x = 464 + (node.pos - tree.root.pos) * 40;
            toado.y = 20 + 60 * node.flo;

            return toado;
        }


        ///
        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hide == 0)
            {
                hide++;
                mainCanvas.Opacity = 0.3;

                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 0;
                myDoubleAnimation.To = 300;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

                Storyboard.SetTargetName(myDoubleAnimation, "insert_Grid");
                Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Button.WidthProperty));

                Storyboard insert_Storyboard = new Storyboard();
                insert_Storyboard.Children.Add(myDoubleAnimation);


                insert_Storyboard.Begin(insertBtn);


            }

        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hide == 0)
            {
                hide++;
                mainCanvas.Opacity = 0.3;

                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 0;
                myDoubleAnimation.To = 300;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

                Storyboard.SetTargetName(myDoubleAnimation, "delete_Grid");
                Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Button.WidthProperty));

                Storyboard delete_Storyboard = new Storyboard();
                delete_Storyboard.Children.Add(myDoubleAnimation);


                delete_Storyboard.Begin(findBtn);


            }
        }
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hide == 0)
            {
                hide++;
                mainCanvas.Opacity = 0.3;



                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 0;
                myDoubleAnimation.To = 300;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

                Storyboard.SetTargetName(myDoubleAnimation, "find_Grid");
                Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Button.WidthProperty));

                Storyboard find_Storyboard = new Storyboard();
                find_Storyboard.Children.Add(myDoubleAnimation);


                find_Storyboard.Begin(findBtn);



            }
        }
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hide == 0)
            {
                
                tree.root = null;
                //list_node.Clear();
                list_tree.Clear();
                mainCanvas.Children.Clear();
            }
        }

        ///  
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {

            this.WindowState = WindowState.Minimized;
        }
     
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private List<int> num_string(String s)
        {
            List<int> list = new List<int>(); 
           string start = s.Substring(1, s.IndexOf('.') - 1);
            string end = s.Substring(s.LastIndexOf('.')+1, s.Length-2- s.LastIndexOf('.'));

            for (int i = Convert.ToInt32(start); i <= Convert.ToInt32(end); i++) 
            {
                list.Add(i);
             
            }
            return list; 
          

        }
        private async void btn_insert_Click(object sender, RoutedEventArgs e)
        {


            mainCanvas.Opacity = 1;
    ;
            int value = 0;
             if (!int.TryParse(tb_insert.Text, out value))
                MessageBox.Show("Insert number again");
         
            else
            {
                tree.insert(Convert.ToInt32(tb_insert.Text), mainCanvas);
                //Node newRoot = tree.clone();
                //list_node.Add(newRoot);
                RBTree newTree = tree.cloneTree();
                list_tree.Add(newTree);
            }

            tb_insert.Clear();

            hide--;

        }
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

            mainCanvas.Opacity = 1;
            int value = 0;
            if (!int.TryParse(tb_delete.Text, out value))
                MessageBox.Show("Enter the number to delete");
            else if (tree.search(value) == null)
                MessageBox.Show("Number is not exit");
            else
            {
                tree.deleteByVal(value, mainCanvas);


            }
            tb_delete.Clear();
            hide--;

        }

        private async void btn_find_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
            int value;
            if (!int.TryParse(tb_find.Text, out value))
                MessageBox.Show("Number is not integer");
            else if ( tree.search(value).val !=value)
            {
                MessageBox.Show("This value is not in tree");
            }
            else
            {
                tree.find(value, mainCanvas);
              
            }

            tb_find.Clear();

            hide--;

        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if(list_tree.Count == 0) 
                return;
            list_tree.RemoveAt(list_tree.Count-1);

            if (list_tree.Count > 0)
            {
                tree = list_tree[list_tree.Count-1];
            }
            else tree.root = null;
            tree.InOrderTraversal();
            tree.update(mainCanvas);

        }

    }
}
