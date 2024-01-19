using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace DuiDuiDui
{
    /// <summary>
    /// Interaction logic for ReplacingBooks.xaml
    /// </summary>
    public partial class ReplacingBooks : Window
    {

        // global var
        DoublyLinkedList<string> linkedList = new DoublyLinkedList<string>();

        string[] books = new string[10];
        public ReplacingBooks()
        {
            InitializeComponent();

            //// when the project loads
            //// then populate the list
            //linkedList.Add(5);
            //linkedList.Add(2);
            //linkedList.Add(8);
            //linkedList.Add(1);
            //linkedList.Add(10);

            //updateListBox(); // should show up when you hit run


            // hide answer from user
            lbxSortedItems.Visibility = Visibility.Hidden;
            correctOrderTitle.Visibility = Visibility.Hidden;

            genBooks();
            updateListBox();


        }

        public void genBooks()
        {
            // new method
            String item = "";

            for (int i = 0; i < 10; i++)
            {
                item = "";

                    Random r = new Random();
                    int randomThreeDigitNumber = r.Next(100, 1000);
                    item = item + randomThreeDigitNumber.ToString();
                
                item = item + ".";

                    int randomTwoDigitNumber = r.Next(10, 100);
                    item = item + randomTwoDigitNumber.ToString();

                for (int l = 0; l < 3; l++)
                {
                    StringBuilder randomString = new StringBuilder();
                    char randomLetter = (char)('A' + r.Next(26));
                    randomString.Append(randomLetter);
                    item = item + randomString.ToString();
                }
                MessageBox.Show("Book " + (i+1) + ": " + item);
                books[i] = item;
                linkedList.Add(item);
            }
            
        }


        // event handler -- what happens when the mouse moves, it initiates a drag drop as soon as you click on it
        // it starts to initialize dragging, it sets it up to allow you to drag drop
        private void ListBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                if (draggedItem != null)
                {
                    // allow the drag drop to happen
                    DragDrop.DoDragDrop(draggedItem, draggedItem.Content,
                        DragDropEffects.Move);
                }
            }
        }

        // if you want to drag it up or down, you need to know where it is and where it needs to go
        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            ListBoxItem targetItem = sender as ListBoxItem;
            if (targetItem != null)
            {
                int targetIndex = lbxGeneratedItems.Items.IndexOf(targetItem.Content);  // where it is
                int draggedIndex = linkedList.IndexOf((string)e.Data.GetData(typeof(string)));  // where it is going

                if (targetIndex >= 0 && draggedIndex >= 0)  // check if it is a valid item in the list
                {
                    linkedList.Move(draggedIndex, targetIndex);
                    updateListBox(); // refreshing after you move again
                }
            }

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            // help message
            MessageBox.Show("-- Welcome to this fun learning activity - Replacing Books \n" +
                "\n- In order to master the Dewey Decimal System, knowing how to sort books in the correct order is essential. Sort these books in the ascending order. Let's test your skills. Sort the items in the listbox by simply dragging and dropping the calling numbers in order to swap them. \n" +
                "\n- Once you are satified with your sorted list, click on the Submit button to view your results, Good luck! \n" +
                "\n -Click on the Reset button to start again, or click on the Home button to return to the Home page.");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            // navigate to home page
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // check how many items the user got right

            int score = 0;
            // sort books in ascending order
            Array.Sort(books);
            for (int i = 0; i < 10; i++)
            {
                if (books[i] == lbxGeneratedItems.Items[i])
                {
                    score = score + 1;
                }
            }
            if (score < 10)
            {
                // display score
                MessageBox.Show("Your score is: " + score + " / 10");
                // display correct answer
                lbxSortedItems.Visibility = Visibility.Visible;
                correctOrderTitle.Visibility = Visibility.Visible;
                foreach (var item in books)
                {
                    lbxSortedItems.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("WINNER WINNER CHICKEN DINNER");
                this.Hide();
                GamificationFeature gf = new GamificationFeature();
                gf.Show();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // start again
            this.Hide();
            ReplacingBooks replacingBooks = new ReplacingBooks();
            replacingBooks.Show();
        }

        // method to send to the listbox
        public void updateListBox()
        {
            lbxGeneratedItems.Items.Clear();
            // add whats in the linked list to the list box
            foreach (var item in linkedList)
            {
                lbxGeneratedItems.Items.Add(item);
            }

            lbxGeneratedItems.UpdateLayout();
        }

    }  // class ends

    // class for linked list
    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }
        }

        public Node head; // start
        public Node tail; // end

        // method to add values
        public void Add(T value)
        {
            Node newNode = new Node { Value = value };
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        // T value  >>>   // used to find index of a specific value 
        public int IndexOf(T value)
        {
            Node current = head;
            int index = 0;
            while (current != null)
            {
                // it compaes current value to value
                if (EqualityComparer<T>.Default.Equals(current.Value, value))
                    return index;// if it is successful the index is returned
                index++;
                current = current.Next;
            }
            // once the position --> ie item is sorted
            // take away that slot as avail
            return -1;  // indicates the value isnt found
        }

        public void Move(int sourceIndex, int destinationIndex)
        {
            // checks while you move things around in the list ?
            if (sourceIndex < 0 || sourceIndex >= Count
                || destinationIndex < 0 || destinationIndex >= Count)
                return;

            Node sourceNode = GetNodeAtIndex(sourceIndex);
            Node destNode = GetNodeAtIndex(destinationIndex);

            T temp = sourceNode.Value;
            sourceNode.Value = destNode.Value;  // flipping them around
            destNode.Value = temp;  // flipping them around

        }

        private Node GetNodeAtIndex(int index)
        {
            // right click and choose generate method above
            // remove the throws exception
            Node current = head;
            for (int i = 0; i < index && current != null; i++)
            {
                current = current.Next;  // will move onto the next node
            }
            return current;
        }

        public int Count
        {
            get
            {
                int count = 0;
                Node current = head;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }
                return count;
            }
        }
    }
}