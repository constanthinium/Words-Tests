using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;
using Forms = System.Windows.Forms;
using Resx = Words_Tests.Properties.Resources;

namespace Words_Tests
{
    public partial class App : Application
    {
        public static readonly XmlSerializer serializer = new XmlSerializer(typeof(List<(string question, string answer)>));

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (e.Exception is NotImplementedException)
            {
                MessageBox.Show("Эта функция еще не реализована, но скоро будет. Наверно.", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show("Произошла ошибка. Пожалуйста, отправьте эту информацию разработчику.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            var form = new Forms.Form
            {
                Width = 640,
                Height = 480,
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = Forms.FormBorderStyle.SizableToolWindow
            };
            var textBox = new Forms.TextBox
            {
                Text = e.Exception.ToString(),
                Dock = Forms.DockStyle.Fill,
                ReadOnly = true,
                Multiline = true,
                ScrollBars = Forms.ScrollBars.Vertical,
            };
            form.Controls.Add(textBox);
            var button = new Forms.Button
            {
                Text = "Скопировать в буфер обмена",
                Dock = Forms.DockStyle.Bottom
            };
            button.Click += (s, ea) => Clipboard.SetText(textBox.Text);
            form.Controls.Add(button);
            form.ShowDialog();
        }
    }
}
