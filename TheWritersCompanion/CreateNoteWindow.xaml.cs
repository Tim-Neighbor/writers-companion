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
    /// Interaction logic for CreateNoteWindow.xaml
    /// Used to create notes in MainWindow.
    /// Author: Dylan Schulz
    /// .xaml file created by Dylan Schulz
    /// </summary>
    public partial class CreateNoteWindow : Window
    {
        /// <summary>
        /// The CategoryName that is selected by the user,
        /// to be accessed by MainWindow.
        /// "" if the dialog was canceled.
        /// </summary>
        public string CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// The NoteName that is input by the user,
        /// to be accessed by MainWindow.
        /// "" if the dialog was canceled.
        /// </summary>
        public string NoteName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the window, adds the given
        /// categories to the ComoBox, and selects
        /// the default category.
        /// Gives focus to noteNameTextBox.
        /// </summary>
        /// <param name="categories">A list of the available
        /// categories in the project.</param>
        /// <param name="defaultCategoryName">The category name
        /// that should be the default selection</param>
        public CreateNoteWindow(List<Category> categories, string defaultCategoryName)
        {
            InitializeComponent();
            InitializeCategoryComboBox(categories, defaultCategoryName);

            noteNameTextBox.Focus();
        }

        /// <summary>
        /// Initialize the combo box full of categories and
        /// make the default selection.
        /// </summary>
        /// <param name="categories">A list of the
        /// categories to put in the box.</param>
        /// <param name="defaultCategoryName">The category name
        /// that should be the default selection</param>
        private void InitializeCategoryComboBox(List<Category> categories, string defaultCategoryName)
        {
            foreach (Category category in categories)
            {
                categoryComboBox.Items.Add(category.Name);

                if (defaultCategoryName.Equals(category.Name))
                {
                    categoryComboBox.SelectedItem = category.Name;
                }
            }
        }

        /// <summary>
        /// When the categoryNameTextBox gets user focus,
        /// select all text within it.
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void noteNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            noteNameTextBox.SelectAll();
        }

        /// <summary>
        /// Handles a click on the confirmButton
        /// by telling the window creator that the dialog
        /// was successful and setting the output category
        /// and note to the given category and note.
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = categoryComboBox.Text;
            NoteName = noteNameTextBox.Text;
            DialogResult = true;
        }

        /// <summary>
        /// Handles a click on the cancelButton
        /// by telling the window creator that the dialog
        /// was not successful and setting output
        /// category and note to "".
        /// </summary>
        /// <param name="sender">The control that sent this event</param>
        /// <param name="e">Event args for this event</param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = "";
            NoteName = "";
            DialogResult = false;
        }
    }
}
