//Ty Larson
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

/// <summary>
/// Ty Larson
/// </summary>
namespace TheWritersCompanion
{
    /// <summary>
    /// Interaction logic for ImportProjectWindow.xaml
    /// </summary>
    public partial class ImportProjectWindow : Window
    {
        private MainWindow parent;
        private IAuthorizer authorizer;
        private IProjectImporter importer;
        private string directory;
        private string userName;
        private string userPassword;

        private const string MESSAGE_BOX_TITLE = "The Writer's Companion";

        /// <summary>
        /// Constructor for ImportProjectWindow
        /// </summary>
        /// <param name="directoryIn"></param>
        public ImportProjectWindow(string directoryIn)
        {
            InitializeComponent();
            directory = directoryIn;
            importer = new ProjectImporter();
        }

        /// <summary>
        /// Event for User clicking on confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            OnImportConfirm();
        }

        /// <summary>
        /// Event for User clicking on cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Confirms the selection of importing a project with the provided info
        /// </summary>
        private void OnImportConfirm()
        {
            userName = usernameTextBox.Text;
            userPassword = passwordPasswordBox.Password;
            authorizer = new MySQLAuthorizer(userName, userPassword);
            if (authorizer.Login(userName, userPassword))
            {
                OutcomeCode outcome;
                outcome = importer.ImportProject(projectNameTextBox.Text, directory, userName, userPassword);
                if (outcome != OutcomeCode.SUCCESS)
                {
                    DisplayErrorMessageBox(outcome);
                }
                this.Close();
            } else
            {
                DisplayErrorMessageBox(OutcomeCode.INAVLID_USER_CREDENTIALS);
            }
        }

        /// <summary>
        /// Exits the ImportProjectWindow
        /// </summary>
        private void OnCancel()
        {
            this.Close();
        }

        /// <summary>
        /// Provides the User with context if something goes wrong
        /// </summary>
        /// <param name="outcomeCode"></param>
        private void DisplayErrorMessageBox(OutcomeCode outcome)
        {
            string message = OutcomeCodeTranslator.GetUserFriendlyErrorMessage(outcome);

            MessageBox.Show(message, MESSAGE_BOX_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
