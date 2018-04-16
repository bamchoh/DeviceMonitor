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
    /// ConnectorView.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// 通信設定画面へ遷移するためのボタンと
    /// 接続するためのボタンを持つ。
    /// 処理内容はConnectorViewModelに委譲する。
    /// </remarks>
    public partial class ConnectorView : UserControl
    {
        public ConnectorView()
        {
            InitializeComponent();
        }
    }
}
