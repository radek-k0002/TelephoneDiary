using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TelephoneDiary
{
    public class Data
    {
        private SqlConnection connection;
        private List<Contact> contacts;
        private uint highestID;

        public Data(SqlConnection connection)
        {
            contacts = new List<Contact>();
            this.connection = connection;
        }

        public uint HighestID { get { return highestID; } }

        public List<Contact> Contacts { get { return contacts; } }

        public void Insert(Contact contact)
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"INSERT INTO PhoneDiary (FirstName, LastName,
                    PhoneNumber, Email, Info) VALUES ('{contact.FirstName}',
                    '{contact.LastName}', '{contact.PhoneNumber}', '{contact.Email}',
                    '{contact.Info}')", connection);

                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
            finally
            {
                connection.Close();
                highestID++;
                AddToContacts(contact);
            }
        }

        private void AddToContacts(Contact contact)
        {
            int index = contacts.FindIndex(item => item.LastName.CompareTo(contact.LastName) >= 0);
            if (contacts.Count == 0) index = 0;
            else if (index < 0) index = contacts.Count;
            contacts.Insert(index, contact);
        }

        public void LoadData()
        {
            try
            {
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter("Select * from PhoneDiary Order By LastName", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow item in dt.Rows)
                {
                    uint tmp = Convert.ToUInt32(item[0]);
                    Contact contact = new Contact(tmp, item[1].ToString(), item[2].ToString(),
                        item[3].ToString(), item[4].ToString(), item[5].ToString());
                    highestID = Math.Max(highestID, tmp);
                    contacts.Add(contact);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
            finally
            {
                connection.Close();
                highestID++;
            }
        }

        public void UpdateData(Contact contact, Contact updatedContact)
        {
            int index = contacts.FindIndex(item => item == contact);
            if (index < 0)
            {
                MessageBox.Show("Error :(");
                return;
            }
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"Update PhoneDiary Set
                    FirstName='{updatedContact.FirstName}',
                    LastName='{updatedContact.LastName}',
                    PhoneNumber='{updatedContact.PhoneNumber}',
                    Email='{updatedContact.Email}',
                    Info='{updatedContact.Info}' Where
                    ID='{contact.ID}'", connection);
                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
            finally
            {
                connection.Close();
                contacts[index] = updatedContact;
                contacts.Sort((item1, item2) => item1.LastName.CompareTo(item2.LastName));
            }
        }

        public void DeleteData(Contact contact)
        {
            if (contact == null) return;
            int index = contacts.FindIndex(item => item == contact);
            if (index < 0)
            {
                MessageBox.Show("Error: There is no item with that index :(");
                return;
            }
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"Delete From PhoneDiary
                            Where ID='{contact.ID}'", connection);
                command.ExecuteNonQuery();
                contacts.RemoveAt(index);

                // To make sure TABLE is empty
                if (contacts.Count == 0)
                {
                    command = new SqlCommand($@"Truncate Table PhoneDiary", connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error: {exc.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}