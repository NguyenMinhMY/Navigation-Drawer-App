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

namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Red_BlackTree tree = new Red_BlackTree();
        public MainWindow()
        {
            InitializeComponent();

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

        

        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;

        }
        private void mainCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (tree.Find(Convert.ToInt32(tb_delete.Text)) == null)
                MessageBox.Show("Number is not exit");
            else
            {
                tree(Convert.ToInt32(tb_delete.Text));
            } 
                
            mainCanvas.Opacity = 1;
        }
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }
        private void btn_find_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (!double.TryParse(tb_find.Text, out value))
                MessageBox.Show("Number is not exit");
            else if (tree.Find(Convert.ToInt32(tb_find.Text))==null)
            {
                MessageBox.Show("Find unseen");
            }
            
                
            mainCanvas.Opacity = 1;
        }
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {

            mainCanvas.Children.Clear();
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 0.3;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Opacity = 1;
        }


        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
         
            this.WindowState= WindowState.Minimized;
        }
        private void EnlargeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }



        struct point
        {
            public double x, y;
        }
        static double x = 464, y = 20, count = 0, floor = 0,time=2 ;

      

        void addLine(double x1, double y1, double x2, double y2)
        {
            Line line_1 = new Line();
            line_1.X1 = x1;
            line_1.Y1 = y1;
            line_1.X2 = x2;
            line_1.Y2 = y2;
            line_1.Stroke = System.Windows.Media.Brushes.Black;
            line_1.StrokeThickness = 2;
            mainCanvas.Children.Add(line_1);
        }

       

        point find_point(int flo, int pos) // set toa do diem ve circle
        {
            point toado;
            double distane = (928) / (flo * 2 + 1);
            toado.x = (distane) * pos;
            toado.y = y + 60 * flo;

            return toado;
        }

       

        private void btn_insert_Click(object sender, RoutedEventArgs e)
        {
            time = 4.1- 0.04 * slider.Value;
           
            double value = 0;
            if (!double.TryParse(tb_insert.Text, out value))
                MessageBox.Show("Insert number again");
            else
            {
                Ellipse childCtrl = new Ellipse();
                //  childCtrl.Name = "Elip"+value_input;
                childCtrl.StrokeThickness = 2;
                childCtrl.Stroke = Brushes.Black;
                childCtrl.Width = 40;
                childCtrl.Height = 40;
                childCtrl.Opacity = 0.5;
              childCtrl.Fill = System.Windows.Media.Brushes.Yellow;
                

                tree.Add(Convert.ToInt32(tb_insert.Text));
                Label child = new Label();
                child.Content = tb_insert.Text;
                child.Foreground = System.Windows.Media.Brushes.Black;
                child.HorizontalContentAlignment = HorizontalAlignment.Center;
                

                
                mainCanvas.Children.Add(child);



                if (count == 0)
                {
                    Canvas.SetTop(childCtrl, y);
                    Canvas.SetLeft(childCtrl, x);

                    Canvas.SetTop(child, y);
                    Canvas.SetLeft(child, x);

                }
                else
                {
                    Node cur = tree.Find(Convert.ToInt32(tb_insert.Text));
                    Canvas.SetTop(childCtrl, find_point(cur.flo, cur.pos).y);
                    Canvas.SetLeft(childCtrl, find_point(cur.flo, cur.pos).x);

                    Canvas.SetTop(child, find_point(cur.flo, cur.pos).y);
                    Canvas.SetLeft(child, find_point(cur.flo, cur.pos).x);

                    double pos_par = Math.Ceiling(cur.pos / 2.0);
                    double x2 = find_point(cur.flo, cur.pos).x;
                    double y2 = find_point(cur.flo, cur.pos).y;
                   
                    if (cur.flo - 1 == 0 && pos_par == 1)
                    {
                        addLine(484, 40, x2 + 20, y2 + 20);
                        move(childCtrl, 464 - x2, 20 - y2, 0, 0, time);
                        move1(child, 464 - x2, 20 - y2, 0, 0, time);
                    }
                    else
                    {
                        double x1 = find_point(cur.flo - 1, (int)pos_par).x;
                        double y1 = find_point(cur.flo - 1, (int)pos_par).y;
                        addLine(x1 + 20, y1 + 20, x2 + 20, y2 = 20);
                        move(childCtrl, x1 - x2, y1 - y2, 0, 0, time);
                        move1(child, x1 - x2, y1 - y2, 0, 0, time);
                    }




                }
                mainCanvas.Children.Add(childCtrl);
                count++;
                mainCanvas.Opacity = 1;
            
            tb_insert.Clear();
            }

        }
        public void move(Ellipse target,  double oldX, double oldY, double newX,
       double newY, double time)
        {
            TranslateTransform trans = new TranslateTransform();
        
           target.RenderTransform = trans;
            target.RenderTransformOrigin =  new Point(0, 0);
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