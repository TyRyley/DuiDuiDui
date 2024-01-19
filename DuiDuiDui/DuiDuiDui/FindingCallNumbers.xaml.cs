using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace DuiDuiDui
{
    /// <summary>
    /// Interaction logic for FindingCallNumbers.xaml
    /// </summary>
    public partial class FindingCallNumbers : Window
    {
        // variables

        // timer 
        private DispatcherTimer timer;
        private DateTime startTime;

        // tree 
        Tree<CallClass> tree;

        // textfile 
        String textfile = "CallNumbers.txt";

        // levels
        int levelOneIndex = 0;
        int levelTwoIndex = 0;

        // question
        int questionHolderOne = 0;
        int questionHolderTwo = 0;
        int questionHolderThree = 0;

        // counter
        int count = 0;

        // answer
        String ansNumber = "";

        public FindingCallNumbers()
        {
            // Window loads
            InitializeComponent();

            // Start the timer when the window loads
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            startTime = DateTime.Now;
            timer.Start();

            // read values from textfile
            loadDefaults();

            // handle items in tree
            treeHandling();
        }

        // Timer tick event handler
        private void TimerTick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;
            tbTimer.Text = $"Time: {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
        }
        
        // exit button
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // open main window
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();
        }

        // submit button
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // check if user selected an answer
            if (ansNumber == "")
            {
                MessageBox.Show("Please select an answer.");
            }
            else
            {
                if (count == 0)
                {
                    // check if user's answer is correct
                    if (ansNumber.Equals(tree.Root.Children[questionHolderOne].Data.CallNum))
                    {
                        ansNumber = "";
                        count++;
                        Random rand = new Random(Guid.NewGuid().GetHashCode());
                        var corResponses = new List<string> { "Yup, that's right!", "LET'S go, spot on!", "Spectacular, good work!" };
                        int choice = rand.Next(corResponses.Count);
                        MessageBox.Show(corResponses[choice] + " On to the next level.");
                    }
                    else
                    {
                        Random rand = new Random(Guid.NewGuid().GetHashCode());
                        var wroResponses = new List<string> { "Oops, that's wrong!", "Close, but not close enough. Incorrect!", "You tried, but that's wrong!" };
                        int choice = rand.Next(wroResponses.Count);
                        MessageBox.Show(wroResponses[choice]);
                    }
                    treeHandling();
                }
                else
                {
                    // check if user's answer is correct
                    if (ansNumber.Equals(tree.Root.Children[questionHolderOne].Children[questionHolderTwo].Data.CallNum))
                    {
                        ansNumber = "";

                        // THE USER HAS WON

                        // stop timer
                        TimeSpan elapsedTime = DateTime.Now - startTime;
                        tbTimer.Text = $"Total: {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";

                        // change text color to green for time taken to play
                        TimerBorder.Background = Brushes.Lime;

                        MessageBox.Show("WINNER WINNER CHICKEN DINNER");
                        this.Hide();
                        GamificationFeature gf = new GamificationFeature();
                        gf.Show();
                    }
                    else
                    {
                        Random rand = new Random(Guid.NewGuid().GetHashCode());
                        var wroResponses = new List<string> { "Oops, that's wrong!", "Close, but not close enough. Incorrect!", "You tried, but that's wrong!" };
                        int choice = rand.Next(wroResponses.Count);
                        MessageBox.Show(wroResponses[choice]);
                    }
                    count = 0;

                    ansNumber = "";
                    treeHandling();
                }

                rbAnswer1.IsChecked = false;
                rbAnswer2.IsChecked = false;
                rbAnswer3.IsChecked = false;
                rbAnswer4.IsChecked = false;
            }
        }

        // handle user's answer
        private void rbAnswer1_Checked(object sender, RoutedEventArgs e)
        {
            ansNumber = (rbAnswer1.Content.ToString()).Substring(0, 3);
            rbAnswer2.IsChecked = false;
            rbAnswer3.IsChecked = false;
            rbAnswer4.IsChecked = false;
        }

        private void rbAnswer2_Checked(object sender, RoutedEventArgs e)
        {
            ansNumber = (rbAnswer2.Content.ToString()).Substring(0, 3);
            rbAnswer1.IsChecked = false;
            rbAnswer3.IsChecked = false;
            rbAnswer4.IsChecked = false;
        }

        private void rbAnswer3_Checked(object sender, RoutedEventArgs e)
        {
            ansNumber = (rbAnswer3.Content.ToString()).Substring(0, 3);
            rbAnswer1.IsChecked = false;
            rbAnswer2.IsChecked = false;
            rbAnswer4.IsChecked = false;
        }

        private void rbAnswer4_Checked(object sender, RoutedEventArgs e)
        {
            ansNumber = (rbAnswer4.Content.ToString()).Substring(0, 3);
            rbAnswer1.IsChecked = false;
            rbAnswer2.IsChecked = false;
            rbAnswer3.IsChecked = false;
        }

        // loadDefaults method - read and extract values from textfile
        public void loadDefaults()
        {
            // create tree object
            tree = new Tree<CallClass>();
            // create CallClass object
            CallClass holder = new CallClass();
            // set the Root of the tree
            tree.Root = new TreeNode<CallClass>() { Data = holder };
            // start timer
            timer.Start();

            // array declared to read lines from textfile
            string[] lines = File.ReadAllLines(textfile);

            // loop through each line in the textfile
            foreach (String item in lines)
            {
                CallClass holder1 = new CallClass();

                string number = item.Substring(0, 3);  // extract first 3 characters - numbers
                string word = item.Substring(4); // extract characters from 4th position to the end of the line - text
                holder1.CallNum = number;
                holder1.CallName = word;
                if (number.Substring(1).Equals("00")) // extract last 2 numbers
                {
                    LevelOne(holder1);
                    levelOneIndex++;
                    LevelTwo(holder1);
                    levelTwoIndex = 0;
                }
                else if (number.Substring(2).Equals("0")) // extract last number
                {
                    levelTwoIndex++;
                    LevelTwo(holder1);

                }
                else
                {
                    LevelThree(holder1);

                }
            }
        }

        // treeHandling method - traverse through tree
        public void treeHandling()
        {
            // array declared
            int[] randomNum = new int[4];

            Array.Clear(randomNum, 0, randomNum.Length);

            if (count == 0)
            {
                int counter = 0;
                Boolean loopCheck = false;
                while (loopCheck == false)
                {
                    // generate 3 random numbers to access a node in tree structure
                    Random j = new Random();
                    questionHolderOne = j.Next(0, 9);
                    questionHolderTwo = j.Next(0, 9);
                    questionHolderThree = j.Next(0, 9);
                    try
                    {
                        string holder = tree.Root.Children[questionHolderOne].Children[questionHolderTwo].Children[questionHolderThree].Data.CallNum;
                        loopCheck = true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }


                while (counter != 3)
                {

                    Random r = new Random();
                    int holder = r.Next(0, 9);
                    if (!randomNum.Contains(holder) && holder != questionHolderOne)
                    {
                        randomNum[counter] = holder;
                        counter++;


                    }
                }
                lblQuestion.Content = tree.Root.Children[questionHolderOne].Children[questionHolderTwo].Children[questionHolderThree].Data.CallName;

                randomNum[3] = questionHolderOne;
                Array.Sort(randomNum);
                Array.Reverse(randomNum);
                rbAnswer1.Content = tree.Root.Children[randomNum[0]].Data.CallNum + "-" + tree.Root.Children[randomNum[0]].Data.CallName;
                rbAnswer2.Content = tree.Root.Children[randomNum[1]].Data.CallNum + "-" + tree.Root.Children[randomNum[1]].Data.CallName;
                rbAnswer3.Content = tree.Root.Children[randomNum[2]].Data.CallNum + "-" + tree.Root.Children[randomNum[2]].Data.CallName;
                rbAnswer4.Content = tree.Root.Children[randomNum[3]].Data.CallNum + "-" + tree.Root.Children[randomNum[3]].Data.CallName;
            }
            else
            {

                int counter1 = 0;
                while (counter1 != 3)
                {

                    Random B = new Random();
                    int holder = B.Next(0, 9);
                    if (!randomNum.Contains(holder) && holder != questionHolderTwo)
                    {

                        try
                        {
                            string tester1 = tree.Root.Children[questionHolderOne].Children[holder].Data.CallNum;
                            randomNum[counter1] = holder;
                            counter1++;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                lblQuestion.Content = tree.Root.Children[questionHolderOne].Children[questionHolderTwo].Children[questionHolderThree].Data.CallName;

                randomNum[3] = questionHolderTwo;
                Array.Sort(randomNum);
                Array.Reverse(randomNum);

                rbAnswer1.Content = tree.Root.Children[questionHolderOne].Children[randomNum[0]].Data.CallNum + "-" + tree.Root.Children[questionHolderOne].Children[randomNum[0]].Data.CallName;
                rbAnswer2.Content = tree.Root.Children[questionHolderOne].Children[randomNum[1]].Data.CallNum + "-" + tree.Root.Children[questionHolderOne].Children[randomNum[1]].Data.CallName;
                rbAnswer3.Content = tree.Root.Children[questionHolderOne].Children[randomNum[2]].Data.CallNum + "-" + tree.Root.Children[questionHolderOne].Children[randomNum[2]].Data.CallName;
                rbAnswer4.Content = tree.Root.Children[questionHolderOne].Children[randomNum[3]].Data.CallNum + "-" + tree.Root.Children[questionHolderOne].Children[randomNum[3]].Data.CallName;

            }


        }

        // level methods
        private void LevelOne(CallClass holder)
        {
            if (tree.Root.Children == null)
            {
                tree.Root.Children = new List<TreeNode<CallClass>>()
                {
                    new TreeNode<CallClass>()
                    {
                        Data = holder
                    }
                };
            }
            else
            {
                tree.Root.Children.Add(new TreeNode<CallClass>()
                {
                    Data = holder
                }
                );
            }
        }

        private void LevelTwo(CallClass holder)
        {
            if (tree.Root.Children[(levelOneIndex - 1)].Children == null)
            {
                tree.Root.Children[(levelOneIndex - 1)].Children = new List<TreeNode<CallClass>>()
                {
                    new TreeNode<CallClass>()
                    {
                        Data = holder,
                        Parent = tree.Root.Children[(levelOneIndex - 1)].Data
                    }
                };
            }
            else
            {
                tree.Root.Children[(levelOneIndex - 1)].Children.Add(new TreeNode<CallClass>()
                {
                    Data = holder,
                    Parent = tree.Root.Children[(levelOneIndex - 1)].Data
                }
                );
            }
        }

        private void LevelThree(CallClass holder)
        {
            if (tree.Root.Children[(levelOneIndex - 1)].Children[levelTwoIndex].Children == null)
            {
                tree.Root.Children[(levelOneIndex - 1)].Children[levelTwoIndex].Children = new List<TreeNode<CallClass>>()
                {
                    new TreeNode<CallClass>()
                    {
                        Data = holder,
                        Parent = tree.Root.Children[(levelOneIndex - 1)].Children[levelTwoIndex].Data
                    }
                };
            }
            else
            {
                tree.Root.Children[(levelOneIndex - 1)].Children[levelTwoIndex].Children.Add(new TreeNode<CallClass>()
                {
                    Data = holder,
                    Parent = tree.Root.Children[(levelOneIndex - 1)].Children[levelTwoIndex].Data
                }
                );
            }
        }

    } // FindingCallNumbers ends

} // namespace ends
