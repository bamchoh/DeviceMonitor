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

namespace DeviceMonitor.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// デバイスモニタのベースウィンドウビュー<br />
    /// モニタリング対象のドライバリスト、
    /// モニタリング対象のデバイスリスト、
    /// デバイスをモニタリングする為の表示部分を持つ
    /// </remarks>
    public partial class DevMonWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DevMonWindow()
        {
            InitializeComponent();


        }
    }
}
