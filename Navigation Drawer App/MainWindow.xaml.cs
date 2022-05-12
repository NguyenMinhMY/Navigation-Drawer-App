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
        Red_BlackTree tree = null;

        public MainWindow()
        {
            InitializeComponent();
            tree = new Red_BlackTree();
        }
        double x = 464, y = 20, time = 2;
        int cur_page = 0, max_page = 0;
        point find_point(Node node) // set toa do diem ve circle
        {
            point toado;
            toado.x = 464 + (node.pos - tree.root.pos) * 40;
            toado.y = 20 + 60 * node.flo;

            return toado;
        }
        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility
            if (Tg_Btn.IsChecked == true)
            {
                tt_insert.Visibility = Visibility.Collapsed;
                tt_remove.Visibility = Visibility.Collapsed;
                tt_find.Visibility = Visibility.Collapsed;
                tt_download.Visibility = Visibility.Collapsed;
                tt_refresh.Visibility = Visibility.Collapsed;

            }
            else

            {
                tt_insert.Visibility = Visibility.Visible;
                tt_remove.Visibility = Visibility.Visible;
                tt_find.Visibility = Visibility.Visible;
                tt_download.Visibility = Visibility.Visible;
                tt_refresh.Visibility = Visibility.Visible;

            }
        }


        ///
        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
        }
        ///
        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;

        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {

            mainCanvas.Children.Clear();
        }

        ///  
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void EnlargeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
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

        ///

        private void mainCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }



        ///  OK 


        private async void btn_insert_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
            time = 4.1 - 0.04 * slider.Value;
            slider.Value = Math.Round(slider.Value);
            int value = 0;
            if (!int.TryParse(tb_insert.Text, out value))
                MessageBox.Show("Insert number again");
            else
            {
                tree.Insert(Convert.ToInt32(tb_insert.Text), mainCanvas);
                cur_page++;
                max_page++;
                curpage.Content = cur_page.ToString();
                maxpage.Content = "/" + max_page.ToString();

            }

            tb_insert.Clear();


        }


        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
            if (tree.Find(Convert.ToInt32(tb_delete.Text)) == null)
                MessageBox.Show("Number is not exit");
            else
            {
                tree.Delete(Convert.ToInt32(tb_delete.Text), mainCanvas);
            }


        }

        private async void btn_find_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
            int value;
            if (!int.TryParse(tb_find.Text, out value))
                MessageBox.Show("Number is not integer");
            else if (tree.Find(value) == null)
            {
                MessageBox.Show("No find value in tree");
            }
            else
            {
                Ellipse Circle = new Ellipse();
                Circle.StrokeThickness = 3;
                Circle.Stroke = Brushes.Blue;
                Circle.Width = 41;
                Circle.Height = 41;
                Circle.Opacity = 1;
                mainCanvas.Children.Add(Circle);
                Node cur = tree.root;
                if (value == tree.root.data)
                {
                    Canvas.SetTop(Circle, y);
                    Canvas.SetLeft(Circle, x);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                while (cur != null)
                {
                    time = 4.1 - 0.04 * slider.Value;
                    slider.Value = Math.Round(slider.Value);
                    if (cur.data == value)
                    {
                        break;
                    }
                    else if (value < cur.data) cur = cur.left;
                    else if (value > cur.data) cur = cur.right;
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




        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            --cur_page;
            curpage.Content = cur_page.ToString();
            if (cur_page >= 1)
            {

                FileStream fs = File.Open("State" + cur_page.ToString() + ".xaml", FileMode.Open, FileAccess.Read);
                Canvas savedCanvas = XamlReader.Load(fs) as Canvas;
                fs.Close();
                mainCanvas.Children.Clear();
                while (savedCanvas.Children.Count > 0)
                {
                    UIElement uie = savedCanvas.Children[0];
                    savedCanvas.Children.Remove(uie);

                    mainCanvas.Children.Add(uie);

                }

            }
            else if (cur_page == 0)
            {
                mainCanvas.Children.Clear();
                cur_page = 0;
            }
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            ++cur_page;
            curpage.Content = cur_page.ToString();
            if (cur_page <= max_page)
            {
                FileStream fs = File.Open("State" + cur_page.ToString() + ".xaml", FileMode.Open, FileAccess.Read);
                Canvas savedCanvas = XamlReader.Load(fs) as Canvas;
                fs.Close();
                mainCanvas.Children.Clear();
                while (savedCanvas.Children.Count > 0)
                {
                    UIElement uie = savedCanvas.Children[0];
                    savedCanvas.Children.Remove(uie);

                    mainCanvas.Children.Add(uie);

                }
            }
            else
            {
                --cur_page;
                MessageBox.Show("Page current");
            }
        }




        ///



        ///

        public void move(Ellipse target, double oldX, double oldY, double newX,
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
        public void move1(Label target, double oldX, double oldY, double newX,
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

    }
}
