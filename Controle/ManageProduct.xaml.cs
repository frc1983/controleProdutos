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
	/// Interaction logic for ManageProduct.xaml
	/// </summary>
	public partial class ManageProduct : UserControl
	{
		public ManageProduct()
		{
			InitializeComponent();
			LoadCategories();
			LoadMaker();
			LoadProviders();
		}

		private void LoadCategories()
		{
			using (var cat = Database.GetInstance.redisClient.As<Category>())
			{
				foreach (Category category in cat.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = category.Name;
					item.ToolTip = category.Description;
					item.Tag = category.Id;

					comboCategoria.Items.Add(item);
				}

			}
		}

		private void LoadMaker()
		{
			using (var mak = Database.GetInstance.redisClient.As<Make>())
			{
				foreach (Make make in mak.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = make.Name;
					item.ToolTip = make.Name;
					item.Tag = make.Id;

					comboFabricante.Items.Add(item);
				}

			}
		}

		private void LoadProviders()
		{
			using (var pro = Database.GetInstance.redisClient.As<Provider>())
			{
				foreach (Provider provider in pro.GetAll())
				{
					ComboBoxItem item = new ComboBoxItem();
					item.Content = provider.Name;
					item.ToolTip = provider.Name;
					item.Tag = provider.Id;

					comboFornecedor.Items.Add(item);
				}

			}
		}

		private void insert_Click(object sender, RoutedEventArgs e)
		{
			for (var i = 1; i <= 1000; i++)
			{
				var car = new Product()
				{
					Id = Database.GetInstance.redisClient.As<Product>().GetNextSequence(),
					Name = "Titulo" + i,
					Code = i*1000,
					Description = "Descricao" + i,
					Quantity = i,
					UnityValue = (decimal)10.32,
					WeightValue = (decimal)10.82,
					Category = Database.GetInstance.redisClient.As<Category>().GetById(comboCategoria.SelectedValue),
					Weight = 1.00F,
					Make = Database.GetInstance.redisClient.As<Make>().GetById(comboFabricante.SelectedValue),
					Provider = Database.GetInstance.redisClient.As<Provider>().GetById(comboFornecedor.SelectedValue),
					EAN = 123456789,
					LastModification = DateTime.Now
				};

				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Product>(car));

					trans.Commit();
				}
			}
		}

		private void search_Click(object sender, RoutedEventArgs e)
		{
			using (var cars = Database.GetInstance.redisClient.As<Product>())
			{
				var mpvm = new List<ManageProductVM>();
				foreach (Product item in cars.GetAll())
					mpvm.Add(item); 

				dataGrid1.ItemsSource = mpvm;
			}
		}

		private void deleteAll_Click(object sender, RoutedEventArgs e)
		{
			Database.GetInstance.redisClient.DeleteAll<Product>();
		}
	}
}
