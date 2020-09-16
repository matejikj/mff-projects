using Harmonogram.Helper;
using Harmonogram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Harmonogram.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserViewModel manager;
        List<User> users;

        public LoginWindow()
        {
            InitializeComponent();
            cb_servers.ItemsSource = LoginDataHelper.servers;
            cb_servers.SelectedItem = LoginDataHelper.servers[0];
            cb_users.ItemsSource = LoginDataHelper.users;
            cb_users.SelectedItem = LoginDataHelper.users[0];
            tb_server.Text = LoginDataHelper.servers[0];
            tb_Username.Text = LoginDataHelper.users[0];

        }

        public void cb_ServersSelectionChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            string selectedItem = (string)cb.SelectedItem;

            tb_server.Text = selectedItem;
        }

        public void cb_UsersSelectionChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            string selectedItem = (string)cb.SelectedItem;

            tb_Username.Text = selectedItem;

        }

        public void changeServer()
        {
            string connectionString = "metadata=res://*/LignisModel.csdl|res://*/LignisModel.ssdl|res://*/LignisModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=" + tb_server.Text + ";initial catalog=HarmonogramDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"";
            //string connectionString = "metadata=res://*/LignisModel.csdl|res://*/LignisModel.ssdl|res://*/LignisModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=" + tb_server.Text + ";initial catalog=HarmonogramDB;user id=sa;password=Modulsoft321;MultipleActiveResultSets=True;App=EntityFramework\"";

            try
            {
                AppSettingsHelper setting = new AppSettingsHelper();
                setting.SaveConnectionString("HarmonogramDBEntities", connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void login()
        {
            this.manager = new UserViewModel();
            users = this.manager.Users.ToList();

            if (tb_Username.Text != string.Empty && pb_Password.Password != string.Empty)
            {
                var logUser = users.FirstOrDefault(a => a.Username.Equals(tb_Username.Text));
                if (logUser != null)
                {
                    bool isAdmin = logUser.IsAdmin.HasValue ? logUser.IsAdmin.Value : false;

                    if (pb_Password.Password == logUser.Password)
                    {
                        StaticResources.UserName = tb_Username.Text;
                        StaticResources.User = logUser;
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        tb_result.Background = Brushes.Red;

                        tb_result.Text = "spatne heslo!!";
                    }
                }
                else
                {
                    tb_result.Background = Brushes.Red;

                    tb_result.Text = "Uzivatel se zadanym jmenem neexistuje!!";
                }
            }
            else
            {
                tb_result.Background = Brushes.Red;

                tb_result.Text = "Prazdne policka, zadej je!!!!!";
            }

        }

        public void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginDataHelper.addUser(tb_Username.Text);
            LoginDataHelper.addServer(tb_server.Text);
            LoginDataHelper.addLastLogin(tb_Username.Text);
            LoginDataHelper.addLastServer(tb_server.Text);

            changeServer();
            login();
        }

        private void pb_Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginDataHelper.addUser(tb_Username.Text);
                LoginDataHelper.addServer(tb_server.Text);
                LoginDataHelper.addLastLogin(tb_Username.Text);
                LoginDataHelper.addLastServer(tb_server.Text);

                changeServer();
                login();
            }
            else
            {

            }

        }
    }
}
