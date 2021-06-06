using Blur.Test;
using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            expander.Expanded += new RoutedEventHandler(expander_MouseDown);
            expander.Collapsed += new RoutedEventHandler(expanderHasCollapsed);
            WindowBlur.SetIsEnabled(this, true);
            var wmi =
            new ManagementObjectSearcher("select * from Win32_OperatingSystem")
             .Get()
             .Cast<ManagementObject>()
             .First();
            osver.Text = ((string)wmi["Caption"]).Trim();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style _style = null;
            _style = (Style)this.FindResource("GadgetStyle");
            this.Style = _style;
        }
        public void ok()
        {
        }
        [DllImport("uxtheme.dll", EntryPoint = "#95")]
        public static extern uint GetImmersiveColorFromColorSetEx(uint dwImmersiveColorSet, uint dwImmersiveColorType, bool bIgnoreHighContrast, uint dwHighContrastCacheMode);
        [DllImport("uxtheme.dll", EntryPoint = "#96")]
        public static extern uint GetImmersiveColorTypeFromName(IntPtr pName);
        [DllImport("uxtheme.dll", EntryPoint = "#98")]
        public static extern int GetImmersiveUserColorSetPreference(bool bForceCheckRegistry, bool bSkipCheckOnFail);
        public Color GetThemeColor()
        {
            var colorSetEx = GetImmersiveColorFromColorSetEx(
                (uint)GetImmersiveUserColorSetPreference(false, false),
                 GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni("ImmersiveStartSelectionBackground")),
                 false, 0);

            var colour = Color.FromArgb((byte)((0xFF000000 & colorSetEx) >> 24), (byte)(0x000000FF & colorSetEx),
                    (byte)((0x0000FF00 & colorSetEx) >> 8), (byte)((0x00FF0000 & colorSetEx) >> 16));

            return colour;
        }

        private void expander_MouseDown(object sender, RoutedEventArgs args)
        {

            expander.Header = "Hide Contents";
        }
        private void expanderHasCollapsed(object sender, RoutedEventArgs args)
        {
            expander.Header = "Show Contents";
        }
    }
}