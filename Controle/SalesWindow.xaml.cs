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
using System.Collections.ObjectModel;
using Controle.database;
using ServiceStack.Redis;
using Controle.Domain;
using Xceed.Wpf.Toolkit;
using WPF.MDI;
using MahApps.Metro.Controls;
using Controle.ViewModel;
using Controle.Helpers;

namespace Controle
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class SalesWindow : MetroWindow
	{
		Product _produto = null;

		ObservableCollection<ProductListVM> listaProdutos { get; set; }

		public SalesWindow()
		{
			InitializeComponent();
			InitSales();
		}

		private void InitSales()
		{
			listaProdutos = new ObservableCollection<ProductListVM>();
			this.DataContext = listaProdutos;
		}

		private void OnSalesApplicationExit(object sender, EventArgs e)
		{
			try
			{
				//Database.GetInstance.redisClient.SaveAsync();
			}
			catch (RedisException ex) { throw new RedisException(ex.Message); }
		}

		private void ReadProductCode_Click(object sender, RoutedEventArgs e)
		{
            long cod = 5432543254325543543;
			findProductByEAN(cod);
		}

		private void findProductByEAN(long ean)
		{
			List<String> list = new List<string>();
			list.Add(ean.ToString());

			IList<Product> rs = Database.GetInstance.redisClient.As<Product>().GetAll();

			foreach (var item in rs)
			{
				if (item.EAN == ean)
				{
					_produto = item;
					lblName.Content = item.Name;
					lblMaker.Content = item.Make.Name;
					lblProvider.Content = item.Provider.Name;
					lblUnityValue.Content = String.Format("{0:c}", item.UnityValue);
					lblWeightValue.Content = String.Format("{0:c}", item.WeightValue);

					if (String.IsNullOrEmpty(txtQuantity.Text)) txtQuantity.Text = "1";

					txtQuantity_TextChanged(null, null);

					AdicionaCompra(_produto);
                    break;
				}
                else
                    ErrorInfo.DisplayErrorMessage("Nenhum produto encontrado.");
			}
		}

		private void AdicionaCompra(Product _produto)
		{
			int qtd = Convert.ToInt32(txtQuantity.Text);
			decimal? total = 0;
			if (_produto.UnityValue != null && _produto.UnityValue > 0)
				total = (_produto.UnityValue * qtd);
			else if (_produto.WeightValue != null && _produto.WeightValue > 0)
				total = (_produto.WeightValue * qtd);

			ProductListVM convert = ProductListVM.ToProductListVM(_produto, qtd, total);
			listaProdutos.Add(convert);

			lblValorTotalGasto.Content = CalculaTotalListaCompras(listaProdutos);
		}

		private string CalculaTotalListaCompras(ObservableCollection<ProductListVM> listaProdutos)
		{
			decimal? total = 0;
			foreach (var item in listaProdutos)
			{
				total += item.TotalProduto;
			}
			return String.Format("{0:c}", total);
		}

		private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
		{
			int qtd;
			Int32.TryParse(txtQuantity.Text, out qtd);
			
			if (_produto != null)
			{
				if (_produto.UnityValue != null && _produto.UnityValue > 0)
					lblTotalProduto.Content = String.Format("{0:c}", (_produto.UnityValue * qtd));				
				else if (_produto.WeightValue != null && _produto.WeightValue > 0)
					lblTotalProduto.Content = String.Format("{0:c}", (_produto.WeightValue * qtd));
			}
		}
	}
}
