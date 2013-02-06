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

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageCategory.xaml
	/// </summary>
	public partial class ManageProvider : UserControl
	{
		Provider _editing = null;

		public ManageProvider()
		{
			InitializeComponent();
			ListaFornecedores();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Provider provider;

			provider = new Provider()
			{
				Name = txtFornecedor.Text
			};

			if (_editing == null)
				provider.Id = Database.GetInstance.redisClient.As<Make>().GetNextSequence();
			else
				provider.Id = _editing.Id;

			if (provider.Validate())
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Provider>(provider));

					trans.Commit();
				}
			}

			ListaFornecedores();
			LimpaCampos();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var fornecedorSelecionado = (Provider)dataGrid1.SelectedItem;

			if (fornecedorSelecionado != null)
			{
				MessageBoxResult result = Helpers.ErrorInfo.ConfirmatioMessage("Deseja excluir o registro?");
				if (result == MessageBoxResult.Yes)
				{
					Database.GetInstance.redisClient.As<Provider>().DeleteById(fornecedorSelecionado.Id);
				}
			}
			ListaFornecedores();
			LimpaCampos();
		}

		private void ListaFornecedores()
		{
			using (var mak = Database.GetInstance.redisClient.As<Provider>())
			{
				var mpvm = new List<Provider>();
				foreach (Provider item in mak.GetAll())
					mpvm.Add(item);

				dataGrid1.ItemsSource = mpvm;
			}
		}

		private void LimpaCampos()
		{
			txtFornecedor.Text = String.Empty;
			_editing = null;
		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var fabricanteSelecionado = (Provider)dataGrid1.SelectedItem;

			if (fabricanteSelecionado != null)
			{
				txtFornecedor.Text = fabricanteSelecionado.Name;

				_editing = new Provider
				{
					Id = fabricanteSelecionado.Id,
					Name = txtFornecedor.Text,
				};
			}
		}
	}
}
