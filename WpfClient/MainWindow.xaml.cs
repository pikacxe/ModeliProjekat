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
using FTN.ServiceContracts;
using FTN.Common;
using System.Collections.ObjectModel;
using WpfClient.ViewModels;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        private NetworkModelGDAProxy gdaQueryProxy = null;
        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }
                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                return gdaQueryProxy;
            }
        }
        private int iteratorId = 0;
        private ObservableCollection<ResourceDescriptionViewModel> currentRDBatch = new ObservableCollection<ResourceDescriptionViewModel>();
        public ObservableCollection<ResourceDescriptionViewModel> CurrentRDBatch
        {
            get
            {
                return currentRDBatch;
            }
            set
            {
                if (currentRDBatch != value)
                {
                    currentRDBatch = value;
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            PopulateInitialData();
            this.DataContext = this;
        }

        private void PopulateInitialData()
        {
            try
            {
                GdaQueryProxy.Open();
                var rs = modelResourcesDesc.AllDMSTypes;
                cmbxResources.ItemsSource = rs;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                GdaQueryProxy.Close();
            }
        }

        private void cmbxResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbxResources.SelectedItem != null)
            {
                lbxProperties.SelectedItems.Clear();
                ModelCode mc;
                if (ModelCodeHelper.GetModelCodeFromString(cmbxResources.SelectedItem.ToString(), out mc))
                {
                    List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);
                    lbxProperties.ItemsSource = properties;
                }
            }
            else
            {
                MessageBox.Show("Please select resource type", "Error", MessageBoxButton.OK);
                cmbxResources.Focus();
            }
        }

        private void GetByDmsType()
        {
            try
            {
                GdaQueryProxy.Open();
                if (cmbxResources.SelectedItem != null)
                {
                    ModelCode mc;
                    string type = cmbxResources.SelectedItem.ToString();
                    if (ModelCodeHelper.GetModelCodeFromString(type, out mc))
                    {
                        List<ModelCode> properties = new List<ModelCode>();
                        if (lbxProperties.SelectedItems.Count > 0)
                        {
                            foreach (var item in lbxProperties.SelectedItems)
                            {
                                properties.Add((ModelCode)item);
                            }
                        }
                        else
                        {
                            properties = modelResourcesDesc.GetAllPropertyIds(mc);
                        }
                        int temp = GdaQueryProxy.GetExtentValues(mc, properties);
                        if (GdaQueryProxy.IteratorResourcesTotal(temp) > 0)
                        {
                            iteratorId = temp;
                            List<ResourceDescription> rdsSorted = GdaQueryProxy.IteratorNext(10, temp).OrderBy(x => x.Id).ToList();
                            CurrentRDBatch.Clear();
                            foreach (var entry in rdsSorted)
                            {
                                var props = new List<PropertyViewModel>();
                                foreach (var p in entry.Properties)
                                {
                                    props.Add(new PropertyViewModel()
                                    {
                                        Id = "0x" + p.Id.ToString("X"),
                                        Name = p.Id.ToString(),
                                        Type = p.Type.ToString(),
                                        Value = p.Type == PropertyType.Int64 ? "0x" + p.PropertyValue.LongValue.ToString("X") : p.ToString()
                                    });
                                }
                                CurrentRDBatch.Add(new ResourceDescriptionViewModel()
                                {
                                    GlobalId = entry.Id,
                                    Type = type,
                                    Properties = props
                                });
                            }

                            MessageBox.Show("Data retreived successfully", "Message", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("No data found", "Service sent", MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select resource type", "Error", MessageBoxButton.OK);
                    cmbxResources.Focus();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                GdaQueryProxy.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetByDmsType();
        }
    }
}
