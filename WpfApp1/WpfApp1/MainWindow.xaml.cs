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
using System.Threading;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Random List
            
        }

        
        memoryGame currentGame;
        Button[] buttons = new Button[20];
        Random rndGenerate = new Random();
        int[] profInputList = new int[20];

        public enum matchIconProf
        {
            ckchan = 1,
            marcoho = 2,
            williamhui = 3,
            winlau = 4,
            jacklee = 5,
            loktatming = 6,
            kehuan = 7,
            minhua = 8,
            sherman = 9,
            dahua = 10,
        }

        private void resetGame_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame != null)
            {
                for (int i = 0; i < 20; i++)
                {
                    currentGame.Reset(buttons);
                }
                currentGame = null;
            }
        }
        private void startGame_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame == null)
            {
                var test = Enumerable.Range(1, 20).OrderBy(x => rndGenerate.Next()).ToArray();
                for (int i = 0; i < 20; i++)
                {
                    buttons[i] = wrapPanel.Children[i] as Button;
                    profInputList[i] = (test[i] - 1) % 10 + 1;
                }
                currentGame = new memoryGame(buttons, profInputList);
            }
        }

        private void mtpButtonsOnclick(object sender, RoutedEventArgs e)
        {
            if (currentGame != null)
            {
                int buttonName = int.Parse((sender as Button).Name.Substring(6))-1;
                currentGame.classOnclick(buttons, buttonName);
            }
        }

        public class memoryGame {
            int[] testarray = new int[20];
            int conditionCounter = 0;
            private Button[] xBox;
            int counter = 0;
            int[] pressed = new int[2];
            bool[] opened = new bool[20];

            public memoryGame(Button[] box, int[] TArray)   
            {
                xBox = box;
                testarray = TArray;

                for (int i = 0; i < 20; i++)
                {
                    box[i].Content = "";
                }
            }

            public StackPanel setPic(matchIconProf test) {
                test.ToString();
                Image img = new Image();
                StackPanel stackPnl = new StackPanel();
                img.Source = new BitmapImage(new Uri(test.ToString()+".jpg", UriKind.Relative));
                stackPnl.Orientation = Orientation.Horizontal;
                stackPnl.Margin = new Thickness(-2);
                stackPnl.Children.Add(img);
                return stackPnl;
            }
            

            public void classOnclick(Button[] box, int index)
            {
                if (counter == 2 || opened[index] || counter == 1 && pressed[0] == index)
                    return;
                
                xBox[index].Content = setPic((matchIconProf)testarray[index]);
                pressed[counter] = index;
                counter++;
                if (counter == 2)
                {
                    buttonCompare(box, pressed[0], pressed[1]);
                }
            }

            public void buttonCompare(Button[] box, int check1, int check2)
            {
                
                if (testarray[check1] == testarray[check2])
                {
                    conditionCounter++;
                    opened[check1] = true;
                    opened[check2] = true;
                    if (conditionCounter == 10) {
                        MessageBox.Show("You Won!!!!!");
                    }
                    counter = 0;
                }
                else {
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                    timer.Start();
                    timer.Tick += (sender, args) =>
                    {
                        timer.Stop();
                        xBox[check1].Content = "";
                        xBox[check2].Content = "";
                        counter = 0;
                    };
                }
            }

            public void Reset(Button[] box)
            {
                for (int i = 0; i < 20; i++)
                {
                    xBox[i].Content = "";
                }
            }
        }
        // Class End here.....................

    }
}