using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StealthyGame.Editors.Animations
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		int tilesize;
		int index;
		int[] length;
		BitmapImage source;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if(ofd.ShowDialog() == true)
			{
				txtBoxFile.Text = ofd.FileName;
			}
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e)
		{
			overview.Source = new ImageSourceConverter().ConvertFromString(txtBoxFile.Text) as ImageSource;
			source = new BitmapImage(new Uri(txtBoxFile.Text));
		}

		private void txtBoxTileSize_TextChanged(object sender, TextChangedEventArgs e)
		{
			int.TryParse(txtBoxTileSize.Text, out tilesize);
			TileSizeChanged();
		}

		private void txtBoxIndex_TextChanged(object sender, TextChangedEventArgs e)
		{
			int.TryParse(txtBoxIndex.Text, out index);
			DrawRectangle();
		}

		void TileSizeChanged()
		{
			if (overview == null)
				return;
			DrawRectangle();
		}

		void DrawRectangle()
		{
			rect.Margin = new Thickness(0, index * tilesize, 0, 0);

			rect.Width = source.PixelWidth;
			rect.Height = tilesize;
		}
	}
}
