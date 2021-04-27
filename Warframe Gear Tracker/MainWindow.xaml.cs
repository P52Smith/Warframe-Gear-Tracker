using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Warframe_Gear_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public List<WarframeWeapon> Weapons { get; set; } = new List<WarframeWeapon>();
        public List<WarframeWarframe> Warframes { get; set; } = new List<WarframeWarframe>();
        public List<WarframeRelic> Relics { get; set; } = new List<WarframeRelic>();
        public List<WarframeArcane> Arcanes { get; set; } = new List<WarframeArcane>();
        public List<WarframeRecipe> Recipes { get; set; } = new List<WarframeRecipe>();
        public List<WarframeItem> OtherItems { get; set; } = new List<WarframeItem>();

        public MainWindow()
        {
            InitializeComponent();
            WarframeItem.OwnedLog = JsonConvert.DeserializeObject<Dictionary<string, bool>>(Properties.Settings.Default.OwnershipLog);
            System.Net.WebClient wc = new System.Net.WebClient();
            const string webPath = "http://content.warframe.com/PublicExport/Manifest/";
            foreach (string manifestPath in GetManifestPaths())
            {
                JObject manifest = JObject.Parse( wc.DownloadString(webPath + manifestPath));
                if (manifest.ContainsKey("ExportWeapons"))
                {
                    foreach(JToken weapon in manifest["ExportWeapons"].Children().ToList())
                    {
                        if (weapon["slot"] != null)
                        {
                            Weapons.Add(weapon.ToObject<WarframeWeapon>());
                        }
                    }
                }
                else if (manifest.ContainsKey("ExportWarframes"))
                {
                    foreach(JToken warframe in manifest["ExportWarframes"].Children().ToList())
                    {
                        Warframes.Add(warframe.ToObject<WarframeWarframe>());
                    }
                }
                else if (manifest.ContainsKey("ExportRecipes"))
                {
                    foreach(JToken recipe in manifest["ExportRecipes"].Children().ToList())
                    {
                        Recipes.Add(recipe.ToObject<WarframeRecipe>());
                    }
                }
                else if (manifest.ContainsKey("ExportRelicArcane"))
                {
                    foreach(JToken relic in manifest["ExportRelicArcane"].Children().ToList())
                    {
                        if (relic["name"].ToString().Contains("RELIC"))
                        {
                            if (relic["uniqueName"].ToString().Contains("Platinum"))
                            {
                                Relics.Add(relic.ToObject<WarframeRelic>());
                            }
                        }
                        else
                        {
                            Arcanes.Add(relic.ToObject<WarframeArcane>());
                        }
                    }
                }
                else
                {
                    foreach(JToken token in manifest.SelectTokens("*"))
                    {
                        foreach(JToken item in token.Children().ToList())
                        {
                            try
                            {
                                OtherItems.Add(item.ToObject<WarframeItem>());
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            WeaponViewer.ItemsSource = Weapons;
            WarframeViewer.ItemsSource = Warframes;
            RelicViewer.ItemsSource = Relics;
            ArcaneViewer.ItemsSource = Arcanes;
            OtherViewer.ItemsSource = OtherItems;

            PropertyGroupDescription groupDescription_Type = new PropertyGroupDescription("Type");
            PropertyGroupDescription groupDescription_Tier = new PropertyGroupDescription("Tier");
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(WeaponViewer.ItemsSource);
            view.GroupDescriptions.Add(groupDescription_Type);
            view = (CollectionView)CollectionViewSource.GetDefaultView(WarframeViewer.ItemsSource);
            view.GroupDescriptions.Add(groupDescription_Type);
            view = (CollectionView)CollectionViewSource.GetDefaultView(RelicViewer.ItemsSource);
            view.GroupDescriptions.Add(groupDescription_Tier);


        }


        private List<string> GetManifestPaths()
        {
            string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.DownloadFile("http://content.warframe.com/PublicExport/index_en.txt.lzma", System.IO.Path.Combine(workingDirectory, "index_en.txt.lzma"));
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "7za.exe",
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
                Arguments = "x -so index_en.txt.lzma",
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
            Process dc = Process.Start(startInfo);
            dc.WaitForExit();
            List<string> retList = new List<string>();
            do
            {
                retList.Add(dc.StandardOutput.ReadLine());
            }
            while (!dc.StandardOutput.EndOfStream);
            System.IO.File.Delete(System.IO.Path.Combine(workingDirectory, "index_en.txt.lzma"));
            return retList;
         }

        private GridViewColumnHeader WeaponViewerSortCol = null;
        private SortAdorner WeaponViewerSortAdorner = null;
        private GridViewColumnHeader WarframeViewerSortCol = null;
        private SortAdorner WarframeViewerSortAdorner = null;
        private GridViewColumnHeader OtherViewerSortCol = null;
        private SortAdorner OtherViewerSortAdorner = null;
        private GridViewColumnHeader RelicViewerSortCol = null;
        private SortAdorner RelicViewerSortAdorner = null;
        private GridViewColumnHeader ArcaneViewerSortCol = null;
        private SortAdorner ArcaneViewerSortAdorner = null;

        private void WeaponViewerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (WeaponViewerSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(WeaponViewerSortCol).Remove(WeaponViewerSortAdorner);
                WeaponViewer.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (WeaponViewerSortCol == column && WeaponViewerSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            WeaponViewerSortCol = column;
            WeaponViewerSortAdorner = new SortAdorner(WeaponViewerSortCol, newDir);
            AdornerLayer.GetAdornerLayer(WeaponViewerSortCol).Add(WeaponViewerSortAdorner);
            WeaponViewer.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void WarframeViewerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (WarframeViewerSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(WarframeViewerSortCol).Remove(WarframeViewerSortAdorner);
                WarframeViewer.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (WarframeViewerSortCol == column && WarframeViewerSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            WarframeViewerSortCol = column;
            WarframeViewerSortAdorner = new SortAdorner(WarframeViewerSortCol, newDir);
            AdornerLayer.GetAdornerLayer(WarframeViewerSortCol).Add(WarframeViewerSortAdorner);
            WarframeViewer.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void OtherViewerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (OtherViewerSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(OtherViewerSortCol).Remove(OtherViewerSortAdorner);
                OtherViewer.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (OtherViewerSortCol == column && OtherViewerSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            OtherViewerSortCol = column;
            OtherViewerSortAdorner = new SortAdorner(OtherViewerSortCol, newDir);
            AdornerLayer.GetAdornerLayer(OtherViewerSortCol).Add(OtherViewerSortAdorner);
            OtherViewer.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void RelicViewerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (RelicViewerSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(RelicViewerSortCol).Remove(RelicViewerSortAdorner);
                RelicViewer.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (RelicViewerSortCol == column && RelicViewerSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            RelicViewerSortCol = column;
            RelicViewerSortAdorner = new SortAdorner(RelicViewerSortCol, newDir);
            AdornerLayer.GetAdornerLayer(RelicViewerSortCol).Add(RelicViewerSortAdorner);
            RelicViewer.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void ArcaneViewerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (ArcaneViewerSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(ArcaneViewerSortCol).Remove(ArcaneViewerSortAdorner);
                ArcaneViewer.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (ArcaneViewerSortCol == column && ArcaneViewerSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            ArcaneViewerSortCol = column;
            ArcaneViewerSortAdorner = new SortAdorner(ArcaneViewerSortCol, newDir);
            AdornerLayer.GetAdornerLayer(ArcaneViewerSortCol).Add(ArcaneViewerSortAdorner);
            ArcaneViewer.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }


        public class SortAdorner : Adorner
        {
            private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

            private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
            {
                this.Direction = dir;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                TranslateTransform transform = new TranslateTransform
                    (
                        AdornedElement.RenderSize.Width - 15,
                        (AdornedElement.RenderSize.Height - 5) / 2
                    );
                drawingContext.PushTransform(transform);

                Geometry geometry = ascGeometry;
                if (this.Direction == ListSortDirection.Descending)
                    geometry = descGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);

                drawingContext.Pop();
            }
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.OwnershipLog = JsonConvert.SerializeObject(WarframeItem.OwnedLog);
            Properties.Settings.Default.Save();
        }
    }
}
