using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DuiDuiDui
{
    /// <summary>
    /// Interaction logic for IdentifyingAreas.xaml
    /// </summary>
    /// 

    public partial class IdentifyingAreas : Window
    {
        // timer var
        private DispatcherTimer timer;
        private DateTime startTime;

        // scoreboard var
        string scoreboardDetails = "SCOREBOARD\n\n";
        int noAttempts = 0;
        int noScoreboards = 1;



        // when this window opens >>>
        public IdentifyingAreas()
        {
            InitializeComponent();

            // load q and a
            loadDefaults();
            repopulateLists();

            // Start the timer when the window loads
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            startTime = DateTime.Now;
            timer.Start();

            tbScoreBoard.Visibility = Visibility.Hidden;
            scoreBoardBorder.Visibility = Visibility.Hidden;

        }

        // Timer tick event handler
        private void TimerTick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;
            tbTimer.Text = $"Time: {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
        }

        // var
        public static int qCount = 4;
        public static bool callQ = true;
        public static int myScore;
        public static int maxScore;
        Random rnd = new Random();
        IDictionary<string, string> baseQuestions = new Dictionary<string, string>();
        IDictionary<string, string> usedQuestions = new Dictionary<string, string>();


        private void loadDefaults()
        {
            baseQuestions.Clear();
            usedQuestions.Clear();
            // list of possible questions
            baseQuestions.Add("000 - 099", "General Works");
            baseQuestions.Add("100 - 199", "Philosophy and Psychology");
            baseQuestions.Add("200 - 299", "Religion");
            baseQuestions.Add("300 - 399", "Social Sciences");
            baseQuestions.Add("400 - 499", "Language");
            baseQuestions.Add("500 - 599", "Natural Sciences and Mathematics");
            baseQuestions.Add("600 - 699", "Technology");
            baseQuestions.Add("700 - 799", "Arts and Recreation");
            baseQuestions.Add("800 - 899", "Literature and Rhetoric");
            baseQuestions.Add("900 - 999", "History, Biography and Geography");
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);

            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) ||
                    (current.GetType()).GetTypeInfo().IsSubclassOf(typeof(T)))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }

        // method to calc score
        private int CalcScore()
        {
            int score = 0;
            List<ListBoxItem> list = new List<ListBoxItem>();
            FindChildren(list, lstAns);
            for (int i = 0; i < qCount; i++)
            {
                string callNumber;
                string description;
                if (!callQ)
                {
                    callNumber = lstQuestions.Items[i].ToString();
                    description = lstAns.Items[i].ToString();
                }
                else
                {  // flip them around
                    callNumber = lstAns.Items[i].ToString();
                    description = lstQuestions.Items[i].ToString();
                }
                // now deal with the used qs and unused qs
                if (usedQuestions[callNumber] == description)
                {
                    // pick a color to light them up
                    list[i].Background = new SolidColorBrush(Colors.Green);
                    // add onto the score
                    score++;
                }
                else
                {
                    // pick another color to light them up
                    list[i].Background = new SolidColorBrush(Colors.Red);
                }
            }
            for (int i = qCount; i < lstAns.Items.Count; i++)
            {
                // pick another color to light them up
                list[i].Background = new SolidColorBrush(Colors.PaleVioletRed);
            }
            return score;
        }  // end of method

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            ListControls.SwapIndexes(-1, lstAns);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            ListControls.SwapIndexes(1, lstAns);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // submits answers to be checked
            // pull the method -- setup the textblocks
            // and gamification features -- if you need to

            // Stop the timer when the submit button is clicked
            timer.Stop();
            TimeSpan elapsedTime = DateTime.Now - startTime;
            tbTimer.Text = $"Total: {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";

            // change text color to green for time taken to play
            TimerBorder.Background = Brushes.Lime;

            int score = CalcScore();

            btnUp.IsEnabled = false;
            btnDown.IsEnabled = false;
            // any other btns or features you want to be disabled at this stage

            // load the defaults
            loadDefaults();
            myScore = myScore + score;  // add to the score
            maxScore = maxScore + qCount; // checks that you cant exceed

            // scoreboard
            // display scoreboard details
            noAttempts = noAttempts + 1;
            if (noAttempts == 5)
            {
                noAttempts = 1;
                noScoreboards = noScoreboards + 1;
                scoreboardDetails = $"SCOREBOARD #{noScoreboards}\n\n";
            }
            scoreboardDetails = scoreboardDetails + $"Attempt {noAttempts}:\t  {score} correct in {elapsedTime.Seconds:D2} s\n";
            tbScoreBoard.Text = scoreboardDetails;
            tbScoreBoard.Visibility = Visibility.Visible;
            scoreBoardBorder.Visibility = Visibility.Visible;

            if (score == 4)
            {
                // pop up --> mbox --> gamification badge
                // perfect score notification
                MessageBox.Show("PERFECT SCORE 4 / 4");
            }
            else
            {
                // not perfect score
            }
            tbScore.Text = "Score: " + myScore + " / " + maxScore;

            btnSubmit.IsEnabled = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // open main window
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
           
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to start a new game?", "This will reset your score",
                    MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    myScore = 0;
                    maxScore = 0;
                    tbScore.Text = "Match columns";
                    TimerBorder.Background = Brushes.Red;
                    tbScoreBoard.Visibility = Visibility.Hidden;
                    scoreBoardBorder.Visibility = Visibility.Hidden;
                btnSubmit.IsEnabled = true;
                }
                else
                {
                    return;
                }
            

            // Restart the timer
            startTime = DateTime.Now;
            timer.Start();

            loadDefaults();
            repopulateLists();
            // btns enabled
            btnUp.IsEnabled = true;
            btnDown.IsEnabled = true;
            btnSubmit.IsEnabled = true;
        }

        // Dictionary --> kvp --> method
        private void getKVP(out string call, out string desc)
        {
            KeyValuePair<string, string> kvp;
            int index = rnd.Next(baseQuestions.Count);
            kvp = baseQuestions.ElementAt(index);
            usedQuestions.Add(kvp);
            baseQuestions.Remove(kvp);
            call = kvp.Key;
            desc = kvp.Value;
        }

        private void repopulateLists()
        {
            lstQuestions.Items.Clear();
            lstAns.Items.Clear();
            // alternate between call numbers and descriptions
            if (callQ)
            {
                // gen 4 callNos + 4 desc
                for (int i = 0; i < 4; i++)
                {
                    getKVP(out string callNo, out string desc);
                    lstQuestions.Items.Add(callNo);
                    lstAns.Items.Add(desc);
                }
                // gen 3 more desc
                for (int i = 0; i < 3; i++)
                {
                    getKVP(out _, out string desc);
                    lstAns.Items.Add(desc);
                }
                // prep alt
                callQ = false;
            }
            else
            {
                // gen 4 desc + 4 callNos
                for (int i = 0; i < 4; i++)
                {
                    getKVP(out string callNo, out string desc);
                    lstQuestions.Items.Add(desc);
                    lstAns.Items.Add(callNo);
                }
                // gen 3 more callNos
                for (int i = 0; i < 3; i++)
                {
                    getKVP(out string callNo, out _);
                    lstAns.Items.Add(callNo);
                }
                // prep alt
                callQ = true;
            }
            // TODO randomize lists
            ListControls.randomizeList(lstQuestions);
            ListControls.randomizeList(lstAns);
            // recolor();
        }

    }
}
