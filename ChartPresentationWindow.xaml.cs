using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualGraf
{
    public partial class ChartPresentationWindow : Window
    {
        private PaymentBaseEntities _context = new PaymentBaseEntities();
        public ChartPresentationWindow()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments")
            {
                IsValueShownAsLabel = true
            };

            ChartPayments.Series.Add(currentSeries);
                
            cmbUsers.ItemsSource = _context.Users.ToList();
            cmbChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if(cmbUsers.SelectedItem is Users currentUser &&
                cmbChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = _context.Categories.ToList();
                foreach(var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.Name, _context.Payments.ToList().Where(p => p.Users == currentUser && p.Categories == category).Sum(p => p.Price * p.Num));
                }
            }
        }
    }
}
