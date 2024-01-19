using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DuiDuiDui
{
    public class ListControls
    {
        // method to swap the indexes -- 1 - 7
        public static void SwapIndexes(int change, ListBox listbox)
        {
            // first ensure the item is enabled
            if (listbox.SelectedIndex == null || listbox.SelectedIndex < 0)
            {
                return;
            }

            // target destination
            int newIndex = listbox.SelectedIndex + change;
            // ensure new destination exists
            if (newIndex < 0 || newIndex >= listbox.Items.Count)
                return;

            // object selected
            object selected = listbox.SelectedItem;
            // insert into a new location
            listbox.Items.Remove(selected);
            listbox.Items.Insert(newIndex, selected);
            listbox.SelectedIndex = newIndex;
        }

        public static void randomizeList(ListBox listBox)
        {
            // new list type of
            var list = new List<string>();
            Random rand = new Random();  // to generate a random list every time

            list = listBox.Items.Cast<String>().ToList();

            // shuffle the list of items
            int n = list.Count;
            while (n > 1)
            {
                int k = rand.Next(n);
                n--;  // decrements the value
                string value = list[k];
                list[k] = list[n]; // swapping
                list[n] = value;
            }
            listBox.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                listBox.Items.Add(list[i]);
            }
        }
    }
}
