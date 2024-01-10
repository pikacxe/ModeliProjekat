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
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Windows.Media.Effects;

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
        private int iteratorId = -1;
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
            btn_GEV.IsEnabled = false;
            btn_GV.IsEnabled = false;
        }

        public int NumOfResc = 0;
        public int RescLeft = 0;
        public int PerView = 10;

        private void PopulateInitialData()
        {
            try
            {
                GdaQueryProxy.Open();
                var rs = modelResourcesDesc.AllDMSTypes;
                rs.Remove(DMSType.MASK_TYPE);
                cmbxResources.ItemsSource = rs;
                cmbxAssociationTypes.ItemsSource = rs;
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
                cmbxResources.Effect = null;
                lbxProperties.SelectedItems.Clear();
                ModelCode mc;
                if (ModelCodeHelper.GetModelCodeFromString(cmbxResources.SelectedItem.ToString(), out mc))
                {
                    List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);
                    lbxProperties.ItemsSource = properties;
                }
                btn_GEV.IsEnabled = true;
                btn_GRV.IsEnabled = true;
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
                        GdaQueryProxy.IteratorClose(iteratorId);
                        iteratorId = GdaQueryProxy.GetExtentValues(mc, properties);
                        Populate_DataGrid(iteratorId);
                    }
                }
                else
                {
                    MessageBox.Show("Please select resource type", "Error", MessageBoxButton.OK);
                    cmbxResources.Focus();
                    cmbxResources.Effect = new DropShadowEffect() { Color = Colors.Red };
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

        public void GetValues(long gid)
        {
            try
            {
                GdaQueryProxy.Open();
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIdsForEntityId(gid);
                ResourceDescription rd = GdaQueryProxy.GetValues(gid, properties);
                if (rd != null)
                {
                    lv_resource.ItemsSource = rd.Properties;
                }
                else
                {
                    MessageBox.Show("Resource not found!", "Error", MessageBoxButton.OK);
                    tb_gv.Focus();
                    tb_gv.Effect = new DropShadowEffect() { Color = Colors.Red };
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

        public void GetRelatedValues()
        {
            if (cmbxAssociationTypes.SelectedItem == null)
            {
                MessageBox.Show("Please select association type", "Error", MessageBoxButton.OK);
                cmbxResources.Focus();
                cmbxResources.Effect = new DropShadowEffect() { Color = Colors.Red };
                return;
            }
            if (string.IsNullOrEmpty(tb_grv.Text))
            {
                MessageBox.Show("Please input source Gid", "Error", MessageBoxButton.OK);
                tb_grv.Focus();
                tb_grv.Effect = new DropShadowEffect() { Color = Colors.Red };
                return;
            }
            if (string.IsNullOrEmpty(tbx_associationPropId.Text))
            {
                MessageBox.Show("Please input asssociation property id", "Error", MessageBoxButton.OK);
                tb_grv.Focus();
                tb_grv.Effect = new DropShadowEffect() { Color = Colors.Red };
                return;
            }
            try
            {
                gdaQueryProxy.Open();
                ModelCode mc;
                Association a = new Association();
                List<ModelCode> properties = new List<ModelCode>();
                if (ModelCodeHelper.GetModelCodeFromString(cmbxAssociationTypes.SelectedItem.ToString(), out mc))
                {
                    a.Type = mc;
                }
                else
                {
                    MessageBox.Show("Association type is not valid", "Error", MessageBoxButton.OK);
                    cmbxAssociationTypes.Focus();
                    cmbxAssociationTypes.Effect = new DropShadowEffect() { Color = Colors.Red };
                    return;
                }
                if (lv_association.SelectedItems.Count > 0)
                {
                    foreach (var x in lv_association.SelectedItems)
                    {
                        if (ModelCodeHelper.GetModelCodeFromString(x.ToString(), out mc))
                        {
                            properties.Add(mc);
                        }
                    }
                }
                else
                {
                    properties = modelResourcesDesc.GetAllPropertyIds(mc);
                }
                long gid = 0;
                if (tb_grv.Text.StartsWith("0x"))
                {
                    gid = long.Parse(tb_grv.Text.Substring(2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    gid = long.Parse(tb_grv.Text);
                }
                if (ModelCodeHelper.GetModelCodeFromString(tbx_associationPropId.Text, out mc))
                {
                    a.PropertyId = mc;
                }
                else
                {
                    MessageBox.Show("Association property id is not valid", "Error", MessageBoxButton.OK);
                    tbx_associationPropId.Focus();
                    tbx_associationPropId.Effect = new DropShadowEffect() { Color = Colors.Red };
                    return;
                }
                var res = GdaQueryProxy.GetRelatedValues(gid, properties, a);
                Populate_DataGrid(res);
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

        public void Populate_DataGrid(int it)
        {
            if (GdaQueryProxy.IteratorResourcesTotal(it) > 0)
            {

                List<ResourceDescription> rdsSorted = GdaQueryProxy.IteratorNext(PerView, it).OrderBy(x => x.Id).ToList();
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
                        Type = ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(entry.Id)).ToString(),
                        Properties = props
                    });
                }
                UpdateStats(it);
            }
            else
            {
                MessageBox.Show("All resources retreived", "Service sent", MessageBoxButton.OK);
            }
        }

        private void btn_GEV_Click(object sender, RoutedEventArgs e)
        {
            GetByDmsType();
        }

        private void btn_GV_Click(object sender, RoutedEventArgs e)
        {
            string temp = tb_gv.Text.Trim();
            if (!string.IsNullOrEmpty(temp))
            {
                long gid = 0;
                if (long.TryParse(temp, out gid))
                {
                    GetValues(gid);
                    return;
                }
                else if (temp.StartsWith("0x"))
                {
                    if (long.TryParse(temp.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out gid))
                    {
                        GetValues(gid);
                        return;
                    }
                }

            }
            MessageBox.Show("Global id must be a number!", "Error", MessageBoxButton.OK);
            tb_gv.Focus();
            tb_gv.Effect = new DropShadowEffect() { Color = Colors.Red };

        }

        private void UpdateStats(int it)
        {
            if (it == -1)
            {
                return;
            }
            NumOfResc = GdaQueryProxy.IteratorResourcesTotal(it);
            RescLeft = GdaQueryProxy.IteratorResourcesLeft(it);
            tb_stats.Text = string.Format("Total: {0,10} | Left: {1,10}", NumOfResc, RescLeft);
        }

        private void tb_gv_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_gv.Effect = null;
            btn_GV.IsEnabled = !string.IsNullOrEmpty(tb_gv.Text);
        }

        private void btn_GRV_Click(object sender, RoutedEventArgs e)
        {
            GetRelatedValues();
        }

        private void tb_grv_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_grv.Effect = null;
            btn_GRV.IsEnabled = !string.IsNullOrEmpty(tb_grv.Text);
        }

        private void cmbxAssociationTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbxAssociationTypes.Effect = null;
            lv_association.SelectedItems.Clear();
            if (cmbxAssociationTypes.SelectedItem != null)
            {
                ModelCode mc;
                if (ModelCodeHelper.GetModelCodeFromString(cmbxAssociationTypes.SelectedItem.ToString(), out mc))
                {
                    List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(mc);
                    lv_association.ItemsSource = properties;
                }
            }
            else
            {
                MessageBox.Show("Please select resource type", "Error", MessageBoxButton.OK);
                cmbxResources.Focus();
                cmbxResources.Effect = new DropShadowEffect() { Color = Colors.Red };
            }
        }
    }
}
