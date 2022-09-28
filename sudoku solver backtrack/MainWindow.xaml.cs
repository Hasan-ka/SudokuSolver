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

namespace sudoku_solver_backtrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            markchg();
            printScreen();
        }

        //here you put the sudoku table you want to solve... 0 = blank
        int[,] sudoku =
           {{ 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0}};

        //to check what in the sudoku table have been modified so backtrack wont touch it
        int[,] modifychk = new int[9, 9];

        //this button when press Well... solve
        private async void Solvebtn_Click(object sender, RoutedEventArgs e)
        {
            //to not over write
            Solvebtn.IsEnabled = false;

            await Task.Run(() => solve());

            /* you can enable this to make the process faster ~~~ note you have to disable it in the solve function
              |
              v */
            //printScreen();


        }

        //solving process
        void solve()
        {
            //keep track with current position
            int currentx = 0;
            int currenty = 0;
            int currentgridx;
            int currentgridy;

            do
            {
                //check for Unmodify
                if (modifychk[currenty, currentx] == 1)
                {
                    currentx++;
                    //correct currentx path if it went outside of the array
                    if (currentx == 9)
                    {
                        currentx = 0;
                        currenty++;
                    }
                }
                else
                {
                    bool valid = true;

                    //try num
                    sudoku[currenty, currentx] = sudoku[currenty, currentx] + 1;

                    //if the number lager than 9 it get a reset and go back to nearest unmodified number
                    if (sudoku[currenty, currentx] == 10)
                    {
                        sudoku[currenty, currentx] = 0;
                        do
                        {
                            currentx--;
                            if (currentx < 0)
                            {
                                currentx = 8;
                                currenty--;
                            }
                            if (currenty < 0) { break; }
                        } while (modifychk[currenty, currentx] == 1);
                        valid = false;
                    }
                    //check for repeat in x axis, y axis ,and it grid
                    if (valid)
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            if (sudoku[currenty, currentx] == sudoku[currenty, x] && currentx != x) { valid = false; break; }
                        }
                    }
                    if (valid)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            if (sudoku[currenty, currentx] == sudoku[y, currentx] && currenty != y) { valid = false; break; }
                        }
                    }
                    if (valid)
                    {
                        currentgridx = (currentx / 3) * 3;
                        currentgridy = (currenty / 3) * 3;
                        for (int gridy = currentgridy; gridy < currentgridy + 3; gridy++)
                        {
                            for (int gridx = currentgridx; gridx < currentgridx + 3; gridx++)
                            {
                                if (sudoku[currenty, currentx] == sudoku[gridy, gridx] && currenty != gridy && currentx != gridx) { valid = false; break; }
                            }
                        }
                    }
                    if (valid)
                    {
                        currentx++;
                        //correct currentx path if it went outside of the array
                        if (currentx == 9)
                        {
                            currentx = 0;
                            currenty++;
                        }
                    }
                }
                
                this.Dispatcher.Invoke(() =>
                {
                    //it will update the ui to show progress with the cost of slow down in solving time and filling up more memory
                    printScreen();
                });
            } while (currenty < 9 && currenty > -1);
        }

        //mark every changed number in the array, 0 unchanged ~~ 1 changed
        void markchg()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != modifychk[i, j])
                    {
                        modifychk[i, j] = 1;
                    }
                }
            }
        }

        //print sudoku array to the table (only numbers bitween 1 to 9) also mark solved numbers with red
        void printScreen()
        {
            var labels = new List<Label> 
            { L11 , L12 , L13 , L14 , L15 , L16 , L17 , L18 , L19 
            , L21 , L22 , L23 , L24 , L25 , L26 , L27 , L28 , L29
            , L31 , L32 , L33 , L34 , L35 , L36 , L37 , L38 , L39
            , L41 , L42 , L43 , L44 , L45 , L46 , L47 , L48 , L49
            , L51 , L52 , L53 , L54 , L55 , L56 , L57 , L58 , L59
            , L61 , L62 , L63 , L64 , L65 , L66 , L67 , L68 , L69
            , L71 , L72 , L73 , L74 , L75 , L76 , L77 , L78 , L79
            , L81 , L82 , L83 , L84 , L85 , L86 , L87 , L88 , L89
            , L91 , L92 , L93 , L94 , L95 , L96 , L97 , L98 , L99 };

            int x = 0;
            int y = 0;

            foreach (var label in labels)
            {
                if (sudoku[y, x] > 0 && sudoku[y, x] < 10)
                {
                    label.Content = sudoku[y, x];
                    if (modifychk[y, x] == 0)
                    {
                        label.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    label.Content = "";
                }
                x++;
                if(x == 9)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
}
