using System;
using System.Collections.Generic;
using System.Linq;
//Ty Larson
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

/// <summary>
/// Ty Larson
/// </summary>
namespace TheWritersCompanion
{
    /// <summary>
    /// Interaction logic for SyncToBackupStorageWindow.xaml
    /// </summary>
    public partial class SyncToBackupStorageWindow : Window
    {
        private MainWindow parent;
        private IAuthorizer authorizer;
        private string userName;
        private string userPassword;

        private const string MESSAGE_BOX_TITLE = "The Writer's Companion";

        /// <summary>
        /// Constructor for SyncToBackupStorageWindow
        /// </summary>
        /// <param name="mainWindowIn">parent window</param>
        public SyncToBackupStorageWindow(MainWindow mainWindowIn)
        {
            InitializeComponent();
            parent = mainWindowIn;
        }

        /// <summary>
        /// Attempts to sync the current project to the database
        /// </summary>
        private void OnSyncConfirm()
        {
            userName = usernameTextBox.Text;
            userPassword = passwordPasswordBox.Password;
            authorizer = new MySQLAuthorizer(userName, userPassword);
            if (authorizer.Login(userName, userPassword))
            {
                OutcomeCode outcome;
                outcome = parent.SyncToDatabase(parent.ProjectDirectory, userName, userPassword);
                if (outcome != OutcomeCode.SUCCESS)
                {
                    DisplayErrorMessageBox(outcome);
                    DialogResult = false;
                }
                else
                {
                    DialogResult = true;
                }
            } else
            {
                DisplayErrorMessageBox(OutcomeCode.INAVLID_USER_CREDENTIALS);
            }
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        private void OnCancel()
        {
            DialogResult = false;
        }

        /// <summary>
        /// Displays the proper outcome code
        /// </summary>
        /// <param name="outcome">Outcome code returned</param>
        private void DisplayErrorMessageBox(OutcomeCode outcome)
        {
            string message = OutcomeCodeTranslator.GetUserFriendlyErrorMessage(outcome);

            MessageBox.Show(message, MESSAGE_BOX_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Attempts to sync the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void syncButton_Click(object sender, RoutedEventArgs e)
        {
            OnSyncConfirm();
        }

        /// <summary>
        /// Cancels the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel();
        }
    }
}
