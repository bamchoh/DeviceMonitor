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

namespace ModbusTCPMasterDriver.View
{
    /// <summary>
    /// ModbusTCPMaster.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// デバイス、アドレス、読み出し個数を設定するコントロールを持つ。
    /// 各コントロールの処理内容はModbusTCPMaterViewModelに委譲する。
    /// </remarks>
    public partial class ModbusTCPMasterView : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ModbusTCPMasterView()
        {
            InitializeComponent();
        }
    }
}
