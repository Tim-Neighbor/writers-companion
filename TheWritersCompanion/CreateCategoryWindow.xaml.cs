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
using System.Windows.Shapes;

namespace TheWritersCompanion
{
    /// <summary>
    /// Interaction logic for CreateCategoryWindow.xaml
    /// Used to create categories in MainWindow.
    /// Author: Dylan Schulz
    /// .xaml file created by Dylan Schulz
    /// </summary>
    public partial class CreateCategoryWindow : Window
    {
        /// <summary>
        /// The CategoryName that is input by the user,
        /// to be accessed by MainWindow.
        /// "" if the dialog was canceled.
        /// </summary>
        public string CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the window and gives focus
        /// to the categoryNameTextBox.
        /// </summary>
        public CreateCategoryWindow()
        {
            InitializeComponent();

            categoryNameTextBox.Focus();
        }

        /// <summary>
        /// When the categoryNameTextBox gets user focus,
        /// select all text within it.
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void categoryNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            categoryNameTextBox.SelectAll();
        }

        /// <summary>
        /// Handles a click on the confirmButton
        /// by telling the window creator that the dialog
        /// was successful and setting the output category
        /// to the given category.
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = categoryNameTextBox.Text;
            DialogResult = true;
        }

        /// <summary>
        /// Handles a click on the cancelButton
        /// by telling the window creator that the dialog
        /// was not successful and setting output
        /// category to "".
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = "";
            DialogResult = false;
        }
    }
}
