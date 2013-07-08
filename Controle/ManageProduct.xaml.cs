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
using MahApps.Metro.Controls;

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageProduct.xaml
	/// </summary>
	public partial class ManageProduct : MetroWindow
	{
		Product _editing = null;

		public ManageProduct()
		{
			InitializeComponent();
			LoadCategories();
			LoadMaker();
			LoadProviders();
			ListaProdutos();
		}

		private void ListaProdutos()
		{
			search_Click(null, null);
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

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Product product = null;

			long code;
			long ean;
			long.TryParse(txtCodigo.Text, out code);
			long.TryParse(txtCodigoBarras.Text, out ean);

			product = new Product()
			{
				Id = Database.GetInstance.redisClient.As<Product>().GetNextSequence(),
				Name = txtProduto.Text,
				Code = code,
				Description = txtDescricao.Text,
				Quantity = (String.IsNullOrEmpty(txtQuantidade.Text)) ? 0 : Convert.ToInt32(txtQuantidade.Text),
				UnityValue = (String.IsNullOrEmpty(txtValorUnitario.Text)) ? 0 : Convert.ToDecimal(txtValorUnitario.Text),
				WeightValue = (String.IsNullOrEmpty(txtValorQuilo.Text)) ? 0 : Convert.ToDecimal(txtValorQuilo.Text),
				Category = Database.GetInstance.redisClient.As<Category>().GetById(comboCategoria.SelectedValue),
				Weight = (String.IsNullOrEmpty(txtPeso.Text)) ? 0 : Convert.ToDouble(txtPeso.Text),
				Make = Database.GetInstance.redisClient.As<Make>().GetById(comboFabricante.SelectedValue),
				Provider = Database.GetInstance.redisClient.As<Provider>().GetById(comboFornecedor.SelectedValue),
				EAN = ean,
				LastModification = DateTime.Now
			};

			if (_editing == null)
				product.Id = Database.GetInstance.redisClient.As<Product>().GetNextSequence();
			else
				product.Id = _editing.Id;

			if (product.Validate())
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Product>(product));

					trans.Commit();
				}
				ListaProdutos();
				LimpaCampos();
			}			
		}

		private void LimpaCampos()
		{
			txtProduto.Text = String.Empty;
			txtCodigo.Text = String.Empty;
			txtDescricao.Text = String.Empty;
			txtQuantidade.Text = String.Empty;
			txtValorUnitario.Text = String.Empty;
			txtValorQuilo.Text = String.Empty;
			txtPeso.Text = String.Empty;
			txtCodigoBarras.Text = String.Empty;
			comboFabricante.SelectedIndex = -1;
			comboFornecedor.SelectedIndex = -1;
			comboCategoria.SelectedIndex = -1;
			_editing = null;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var produtoSelecionado = (ManageProductVM)dataGrid1.SelectedItem;

			if (produtoSelecionado != null)
			{
				MessageBoxResult result = Helpers.ErrorInfo.ConfirmatioMessage("Deseja excluir o registro?");
				if (result == MessageBoxResult.Yes)
				{
					Database.GetInstance.redisClient.As<Product>().DeleteById(produtoSelecionado.Id);
				}
			}
			ListaProdutos();
			LimpaCampos();
		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var produtoSelecionado = (ManageProductVM)dataGrid1.SelectedItem;

			if (produtoSelecionado != null)
			{
				txtProduto.Text = produtoSelecionado.Name;
				txtCodigo.Text = produtoSelecionado.Code.ToString();
				txtDescricao.Text = produtoSelecionado.Description;
				txtQuantidade.Text = produtoSelecionado.Quantity.ToString();
				txtValorUnitario.Text = produtoSelecionado.UnityValue.ToString();
				txtValorQuilo.Text = produtoSelecionado.WeightValue.ToString();
				txtPeso.Text = produtoSelecionado.Weight.ToString();
				txtCodigoBarras.Text = produtoSelecionado.EAN.ToString();

				foreach (ComboBoxItem item in comboFabricante.Items)
					if (item.Content.Equals(produtoSelecionado.Make))
						comboFabricante.SelectedItem = item;

				foreach (ComboBoxItem item in comboFornecedor.Items)
					if (item.Content.Equals(produtoSelecionado.Provider))
						comboFornecedor.SelectedItem = item;

				foreach (ComboBoxItem item in comboCategoria.Items)
					if (item.Content.Equals(produtoSelecionado.Category))
						comboCategoria.SelectedItem = item;

				_editing = new Product
				{
					Id = produtoSelecionado.Id,
					Name = produtoSelecionado.Name,
					Code = produtoSelecionado.Code,
					Description = produtoSelecionado.Description,
					Quantity = produtoSelecionado.Quantity,
					UnityValue = produtoSelecionado.UnityValue,
					WeightValue = produtoSelecionado.WeightValue,
					Category = Database.GetInstance.redisClient.As<Category>().GetById(comboCategoria.SelectedValue),
					Weight = produtoSelecionado.Weight,
					Make = Database.GetInstance.redisClient.As<Make>().GetById(comboFabricante.SelectedValue),
					Provider = Database.GetInstance.redisClient.As<Provider>().GetById(comboFornecedor.SelectedValue),
					EAN = produtoSelecionado.EAN,
					LastModification = DateTime.Now
				};
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

		private string selectCombo(object classe, string valorSelecionado)
		{
			Database.GetInstance.redisClient.As<Make>().ContainsKey(valorSelecionado);
			return "";
		}
	}
}
