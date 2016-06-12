using System.IO;
using System.Windows;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace AutoAramBuildCreator.Other_Views
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            SaveButton.Click += SettingsSaveButtonOnClick;
        }

        private void SettingsSaveButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var color = ThemeManager.GetAccent(AppearanceColorComboBox.Text);

            if (color != null && color != ThemeManager.DetectAppStyle(Application.Current).Item2)
            {
                ThemeManager.ChangeAppStyle(Application.Current, color,
                    ThemeManager.DetectAppStyle(Application.Current).Item1);

                if (File.Exists(Directory.GetCurrentDirectory() + "\\settings.cfg"))
                    File.Delete(Directory.GetCurrentDirectory() + "\\settings.cfg");

                using (var sw = File.AppendText(Directory.GetCurrentDirectory() + "\\settings.cfg"))
                {
                    sw.Write("Color=" + AppearanceColorComboBox.Text);
                }
            }

            Close();
        }
    }
}