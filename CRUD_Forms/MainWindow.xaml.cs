using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RealEstateManagement
{
    public partial class MainWindow : Window
    {
        private List<Client> clients = new List<Client>();
        private List<Property> properties = new List<Property>();
        private List<PropertyVisit> visits = new List<PropertyVisit>(); // Змінено на правильний тип

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LoadClients();
            LoadProperties();
            LoadVisits();
        }

        private void LoadClients()
        {
            try
            {
                clients = Client.ReadClients();
                ClientList.ItemsSource = clients;
                VisitClientComboBox.ItemsSource = clients;

                ClientList.DisplayMemberPath = "ToString";
                VisitClientComboBox.DisplayMemberPath = "ToString";

                Console.WriteLine("Loaded clients: " + clients.Count);
            }
            catch (Exception ex)
            {
                ShowError("Error loading clients", ex);
            }
        }

        private void LoadProperties()
        {
            try
            {
                properties = Property.ReadProperties();
                PropertyList.ItemsSource = properties;
                VisitPropertyComboBox.ItemsSource = properties;
            }
            catch (Exception ex)
            {
                ShowError("Error loading properties", ex);
            }
        }

        private void LoadVisits()
        {
            try
            {
                visits = PropertyVisit.ReadVisits(); // Змінено на правильний тип
                VisitList.ItemsSource = visits;
            }
            catch (Exception ex)
            {
                ShowError("Error loading visits", ex);
            }
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(ClientBudget.Text, out decimal budget))
                {
                    Client.CreateClient(ClientName.Text, ClientEmail.Text, ClientPhone.Text, budget);
                    LoadClients();
                }
                else
                {
                    MessageBox.Show("Invalid budget value.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error adding client", ex);
            }
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientList.SelectedItem is Client selectedClient && decimal.TryParse(ClientBudget.Text, out decimal budget))
                {
                    Client.UpdateClient(selectedClient.Id, ClientName.Text, ClientEmail.Text, ClientPhone.Text, budget);
                    LoadClients();
                }
                else
                {
                    MessageBox.Show("Invalid budget value or no client selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error updating client", ex);
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientList.SelectedItem is Client selectedClient)
                {
                    Client.DeleteClient(selectedClient.Id);
                    LoadClients();
                }
                else
                {
                    MessageBox.Show("No client selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error deleting client", ex);
            }
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(PropertyPrice.Text, out decimal price))
                {
                    Property.CreateProperty(PropertyAddress.Text, price, PropertyType.Text, PropertyStatus.Text);
                    LoadProperties();
                }
                else
                {
                    MessageBox.Show("Invalid price value.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error adding property", ex);
            }
        }

        private void UpdateProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PropertyList.SelectedItem is Property selectedProperty && decimal.TryParse(PropertyPrice.Text, out decimal price))
                {
                    Property.UpdateProperty(selectedProperty.NewId, PropertyAddress.Text, price, PropertyType.Text, PropertyStatus.Text);
                    LoadProperties();
                }
                else
                {
                    MessageBox.Show("Invalid price value or no property selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error updating property", ex);
            }
        }

        private void DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PropertyList.SelectedItem is Property selectedProperty)
                {
                    Property.DeleteProperty(selectedProperty.NewId);
                    LoadProperties();
                }
                else
                {
                    MessageBox.Show("No property selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error deleting property", ex);
            }
        }

        private void AddVisit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VisitPropertyComboBox.SelectedItem is Property selectedProperty &&
                    VisitClientComboBox.SelectedItem is Client selectedClient &&
                    VisitDate.SelectedDate.HasValue)
                {
                    var visit = new PropertyVisit
                    {
                        PropertyId = selectedProperty.NewId,
                        ClientId = selectedClient.Id,
                        VisitDate = VisitDate.SelectedDate.Value,
                        Feedback = VisitFeedback.Text
                    };
                    PropertyVisit.CreatePropertyVisit(visit);
                    LoadVisits();
                }
                else
                {
                    MessageBox.Show("Please select a property, a client, and a valid visit date.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error adding visit", ex);
            }
        }

        private void UpdateVisit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VisitList.SelectedItem is PropertyVisit selectedVisit)
                {
                    selectedVisit.Feedback = VisitFeedback.Text;
                    PropertyVisit.UpdatePropertyVisit(selectedVisit.Id, selectedVisit);
                    LoadVisits();
                }
                else
                {
                    MessageBox.Show("No visit selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error updating visit", ex);
            }
        }

        private void DeleteVisit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VisitList.SelectedItem is PropertyVisit selectedVisit)
                {
                    PropertyVisit.DeletePropertyVisit(selectedVisit.Id);
                    LoadVisits();
                }
                else
                {
                    MessageBox.Show("No visit selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error deleting visit", ex);
            }
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientList.SelectedItem is Client selectedClient)
            {
                ClientName.Text = selectedClient.Name;
                ClientEmail.Text = selectedClient.Email;
                ClientPhone.Text = selectedClient.Phone;
                ClientBudget.Text = selectedClient.Budget.ToString();
            }
        }

        private void PropertyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertyList.SelectedItem is Property selectedProperty)
            {
                PropertyAddress.Text = selectedProperty.Address;
                PropertyPrice.Text = selectedProperty.Price.ToString();
                PropertyType.Text = selectedProperty.Type;
                PropertyStatus.Text = selectedProperty.Status;
            }
        }

        private void VisitList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisitList.SelectedItem is PropertyVisit selectedVisit)
            {
                var property = properties.Find(p => p.NewId == selectedVisit.PropertyId);
                var client = clients.Find(c => c.Id == selectedVisit.ClientId);

                VisitPropertyComboBox.SelectedItem = property;
                VisitClientComboBox.SelectedItem = client;
                VisitDate.SelectedDate = selectedVisit.VisitDate;
                VisitFeedback.Text = selectedVisit.Feedback;
            }
        }
    }
}
