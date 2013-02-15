using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controle.Domain;
using Controle.database;
using Controle.ViewModel;

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageClient.xaml
	/// </summary>
	public partial class ManageClient : UserControl
	{
		Client _editing = null;
		Address _editingAddress = null;

		public ManageClient()
		{
			InitializeComponent();
			montaCombos();
			ListaClientes();
			LoadCities();
			LoadStates();
			LoadCountry();
			LoadMarriageStates();
			LoadBirthPlace();
		}

		private void ListaClientes()
		{
			search_Click(null, null);
		}

		private void LoadCities()
		{
			using (var ct = Database.GetInstance.redisClient.As<City>())
			{
				foreach (City city in ct.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = city.Name;
					item.ToolTip = city.Name;
					item.Tag = city.Id;

					comboCidade.Items.Add(item);
				}

			}
		}

		private void LoadStates()
		{
			using (var st = Database.GetInstance.redisClient.As<State>())
			{
				foreach (State state in st.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = state.Name;
					item.ToolTip = state.Name;
					item.Tag = state.Id;

					comboEstado.Items.Add(item);
				}

			}
		}

		private void LoadBirthPlace()
		{
			using (var st = Database.GetInstance.redisClient.As<State>())
			{
				foreach (State state in st.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = state.Name;
					item.ToolTip = state.Name;
					item.Tag = state.Id;

					comboNaturalidade.Items.Add(item);
				}

			}
		}

		private void LoadCountry()
		{
			using (var st = Database.GetInstance.redisClient.As<Country>())
			{
				foreach (Country country in st.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = country.Name;
					item.ToolTip = country.Name;
					item.Tag = country.Id;

					comboPais.Items.Add(item);
				}

			}
		}

		private void LoadMarriageStates()
		{
			using (var st = Database.GetInstance.redisClient.As<MarriageState>())
			{
				foreach (MarriageState est in st.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = est.Name;
					item.ToolTip = est.Name;
					item.Tag = est.Id;

					comboEstadoCivil.Items.Add(item);
				}

			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Client client = null;
			Address address = null;

			address = new Address()
			{
				Id = Database.GetInstance.redisClient.As<Address>().GetNextSequence(),
				Place = txtLogradouro.Text,
				Number = (String.IsNullOrEmpty(txtNumero.Text)) ? 0 : Convert.ToInt32(txtNumero.Text),
				Complement = txtComplemento.Text,
				Neighborhood = txtBairro.Text,
				City = Database.GetInstance.redisClient.As<City>().GetById(comboCidade.SelectedValue),
				State = Database.GetInstance.redisClient.As<State>().GetById(comboEstado.SelectedValue),
				Country = Database.GetInstance.redisClient.As<Country>().GetById(comboPais.SelectedValue),
				LastModification = DateTime.Now
			};
			
			client = new Client()
			{
				Id = Database.GetInstance.redisClient.As<Client>().GetNextSequence(),
				Name = txtNome.Text,
				CpfCnpj = txtCPFCNPJ.Text,
				Sex = ((bool)radioSexoM.IsChecked) ? radioSexoM.Content.ToString() : radioSexoF.Content.ToString(),
				Birthplace = Database.GetInstance.redisClient.As<State>().GetById(comboNaturalidade.SelectedValue),
				BirthDate = (String.IsNullOrEmpty(dateNascimento.Text)) ? DateTime.MinValue : Convert.ToDateTime(dateNascimento.Text),
				MarriageState = Database.GetInstance.redisClient.As<MarriageState>().GetById(comboEstadoCivil.SelectedValue),
				AddressId = address.Id,
				LastModification = DateTime.Now
			};
			address.IdClient = client.Id;

			if (_editing != null && _editingAddress != null)
			{
				address.Id = _editingAddress.Id;
				client.Id = _editing.Id;
				address.IdClient = _editing.Id;
			}
			else if (_editingAddress == null && _editing != null)
			{
				client.Id = _editing.Id;
				address.IdClient = _editing.Id;
			}

			if (client.Validate() && address.Validate())
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Address>(address));

					trans.Commit();
				}
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Client>(client));

					trans.Commit();
				}
				ListaClientes();
				LimpaCampos();
			}		
		}

		private void LimpaCampos()
		{
			txtNome.Text = String.Empty;
			txtCPFCNPJ.Text = String.Empty;
			txtLogradouro.Text = String.Empty;
			txtNumero.Text = String.Empty;
			txtBairro.Text = String.Empty;
			txtComplemento.Text = String.Empty;
			dateNascimento.Text = String.Empty;
			radioSexoM.IsChecked = true;
			radioSexoF.IsChecked = false;

			comboNaturalidade.SelectedIndex = -1;
			comboEstadoCivil.SelectedIndex = -1;
			comboEstado.SelectedIndex = -1;
			comboCidade.SelectedIndex = -1;
			comboPais.SelectedIndex = -1;

			dataGrid2.ItemsSource = null;

			_editing = null;
			_editingAddress = null;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var clienteSelecionado = (ClientListVM)dataGrid1.SelectedItem;

			if (clienteSelecionado != null)
			{
				MessageBoxResult result = Helpers.ErrorInfo.ConfirmatioMessage("Deseja excluir o registro?");
				if (result == MessageBoxResult.Yes)
				{
					Database.GetInstance.redisClient.As<Client>().DeleteById(clienteSelecionado.Id);
				}
			}
			ListaClientes();
			LimpaCampos();
		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var clienteSelecionado = (ClientListVM)dataGrid1.SelectedItem;

			if (clienteSelecionado != null)
			{
				txtNome.Text = clienteSelecionado.Name;
				txtCPFCNPJ.Text = clienteSelecionado.CpfCnpj;

				radioSexoM.IsChecked = (clienteSelecionado.Sex == radioSexoM.Content.ToString());
				radioSexoF.IsChecked = (clienteSelecionado.Sex == radioSexoF.Content.ToString());

				dateNascimento.Text = clienteSelecionado.BirthDate.ToString();
								
				foreach (ComboBoxItem item in comboNaturalidade.Items)
					if (item.Content.Equals(clienteSelecionado.Birthplace))
						comboNaturalidade.SelectedItem = item;

				foreach (ComboBoxItem item in comboEstadoCivil.Items)
					if (item.Content.Equals(clienteSelecionado.MarriageState))
						comboEstadoCivil.SelectedItem = item;

				_editing = new Client
				{
					Id = clienteSelecionado.Id,
					Name = clienteSelecionado.Name,
					Sex = clienteSelecionado.Sex,
					Birthplace = Database.GetInstance.redisClient.As<State>().GetById(comboNaturalidade.SelectedValue),
					MarriageState = Database.GetInstance.redisClient.As<MarriageState>().GetById(comboEstadoCivil.SelectedValue),
					LastModification = DateTime.Now
				};

				using (var address = Database.GetInstance.redisClient.As<Address>())
				{
					var mpvm = new List<AddressListVM>();
					foreach (Address item in address.GetAll())
						if (item.IdClient == clienteSelecionado.Id)
							mpvm.Add(item);

					dataGrid2.ItemsSource = mpvm;
				}
			}
		}

		private void search_Click(object sender, RoutedEventArgs e)
		{
			using (var client = Database.GetInstance.redisClient.As<Client>())
			{
				var mpvm = new List<ClientListVM>();
				foreach (Client item in client.GetAll())
					mpvm.Add(item); 

				dataGrid1.ItemsSource = mpvm;
			}
		}

		private void deleteAll_Click(object sender, RoutedEventArgs e)
		{
			Database.GetInstance.redisClient.DeleteAll<Client>();
			Database.GetInstance.redisClient.DeleteAll<Address>();
		}

		private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var clienteSelecionado = (AddressListVM)dataGrid2.SelectedItem;

			if (clienteSelecionado != null)
			{
				txtLogradouro.Text = clienteSelecionado.Place;
				txtBairro.Text = clienteSelecionado.Neighborhood;
				txtNumero.Text = clienteSelecionado.Number;
				txtComplemento.Text = clienteSelecionado.Complement;
				
				foreach (ComboBoxItem item in comboEstado.Items)
					if (item.Content.Equals(clienteSelecionado.State))
						comboEstado.SelectedItem = item;

				foreach (ComboBoxItem item in comboCidade.Items)
					if (item.Content.Equals(clienteSelecionado.City))
						comboCidade.SelectedItem = item;

				foreach (ComboBoxItem item in comboPais.Items)
					if (item.Content.Equals(clienteSelecionado.Country))
						comboPais.SelectedItem = item;

				_editingAddress = new Address
				{
					Id = clienteSelecionado.Id,
					Place = clienteSelecionado.Place,
					Number = Convert.ToInt32(clienteSelecionado.Number),
					Complement = clienteSelecionado.Complement,
					Neighborhood = clienteSelecionado.Neighborhood,
					City = Database.GetInstance.redisClient.As<City>().GetById(comboCidade.SelectedValue),
					State = Database.GetInstance.redisClient.As<State>().GetById(comboEstado.SelectedValue),
					Country = Database.GetInstance.redisClient.As<Country>().GetById(comboCidade.SelectedValue),
					LastModification = DateTime.Now
				};
			}
		}

		private void montaCombos()
		{
			List<City> cidades = new List<City>();
			cidades.Add(new City() { Id = 1, Name = "Porto Alegre" });

			foreach (var est in cidades)
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<City>(est));

					trans.Commit();
				}
			}

			List<State> estados = new List<State>();
			estados.Add(new State() { Id = 1, Name = "Rio Grande do Sul", Acronym = "RS" });
			foreach (var est in estados)
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<State>(est));

					trans.Commit();
				}
			}

			List<Country> pais = new List<Country>();
			pais.Add(new Country() { Id = 1, Name = "Brasil", Acronym = "BR" });
			foreach (var est in pais)
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Country>(est));

					trans.Commit();
				}
			}


			List<MarriageState> estadosCivis = new List<MarriageState>();
			estadosCivis.Add(new MarriageState() { Id = 1, Name = "Solteiro" });
			estadosCivis.Add(new MarriageState() { Id = 2, Name = "Casado" });
			estadosCivis.Add(new MarriageState() { Id = 3, Name = "Divorciado" });

			foreach (var est in estadosCivis)
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<MarriageState>(est));

					trans.Commit();
				}
			}
		}
	}
}
