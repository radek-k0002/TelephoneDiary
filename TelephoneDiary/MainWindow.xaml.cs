using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TelephoneDiary
{
    public class Test
    {
        public int FirstName { get; set; }
        public int TestB { get; set; }
        public int TestC { get; set; }
    }

    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string info;
        private Data data;
        private Contact cnt;

        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=PhoneDiary;Integrated Security=True");
            data = new Data(connection);
        }

        private void PhoneDiary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            cnt = PhoneDiary.SelectedItem as Contact;
            if (cnt != null)
            {
                Fname.Text = cnt.FirstName;
                Lname.Text = cnt.LastName;
                Pnumber.Text = cnt.PhoneNumber;
                Email.Text = cnt.Email;
                Info.Text = cnt.Info;
                GetAllFields();
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            cnt = null;
            PhoneDiary.SelectedItem = null;
            PhoneDiary.SelectedIndex = -1;
            Fname.Text = Lname.Text = Pnumber.Text = Email.Text = Info.Text = "";
            Lname.BorderThickness = Pnumber.BorderThickness = Email.BorderThickness = new Thickness(2);
            Lname.BorderBrush = Pnumber.BorderBrush = Email.BorderBrush = Brushes.LightGray;
            Fname.Focus();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {

            if (!SelectErrorFields())
            {
                cnt = new Contact(data.HighestID, firstName, lastName, phoneNumber, email, info);
                data.Insert(cnt);

                PhoneDiary.ItemsSource = data.Contacts;
                PhoneDiary.Items.Refresh();
            }
        }

        private List<TextBox> ValidFields()
        {
            GetAllFields();
            List<TextBox> errors = new List<TextBox>();

            if (!Validator.IsValidString(lastName))
                errors.Add(Lname);

            if (!Validator.IsValidEmail(email))
                errors.Add(Email);

            if (!Validator.IsValidPhoneNumber(phoneNumber))
                errors.Add(Pnumber);

            return errors;
        }

        private bool SelectErrorFields()
        {
            List<TextBox> errors = ValidFields();
            foreach (TextBox item in errors)
            {
                item.BorderThickness = new Thickness(3);
                item.BorderBrush = Brushes.Red;
            }

            return errors.Count != 0;
        }

        private void GetAllFields()
        {
            firstName = Fname.Text;
            lastName = Lname.Text;
            phoneNumber = Pnumber.Text;
            email = Email.Text;
            info = Info.Text;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            GetAllFields();
            cnt = (Contact)PhoneDiary.SelectedItem;
            int nextIndex = PhoneDiary.SelectedIndex + 1;
            if (nextIndex < PhoneDiary.Items.Count) PhoneDiary.SelectedItem = PhoneDiary.Items[nextIndex];

            data.DeleteData(cnt);
            cnt = null;
            PhoneDiary.ItemsSource = data.Contacts;
            PhoneDiary.Items.Refresh();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (cnt == null || SelectErrorFields()) return;

            Contact updatedContact = new Contact(cnt.ID, firstName, lastName,
                phoneNumber, email, info);
            data.UpdateData(cnt, updatedContact);
            PhoneDiary.ItemsSource = data.Contacts;
            PhoneDiary.Items.Refresh();
            cnt = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fname.Focus();

            data.LoadData();
            PhoneDiary.ItemsSource = data.Contacts;
            PhoneDiary.Items.Refresh();
        }

        private void TextBox_Input(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;

            box.BorderThickness = new Thickness(2);
            box.BorderBrush = Brushes.LightGray;
        }

        private void Search_Input(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            string text = box.Text.ToLower();
            List<Contact> filteredContacts = data.Contacts.FindAll(item => item.LastName.ToLower().Contains(text)
            || item.FirstName.ToLower().Contains(text)
            || item.PhoneNumber.ToLower().Contains(text)
            || item.Email.ToLower().Contains(text)
            || item.Info.ToLower().Contains(text));

            PhoneDiary.ItemsSource = filteredContacts;
            PhoneDiary.Items.Refresh();
        }
    }
}