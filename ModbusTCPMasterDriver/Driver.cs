using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;

///<summary>
/// MODBUS TCP Master Driver に関するクラス群を内包する
///</summary>
namespace ModbusTCPMasterDriver
{
    /// <summary>
    /// ドライバの情報を取得するクラス
    /// </summary>
    public class DriverInfo : CommonDriver.DriverInfo
    {
        /// <summary>
        /// Driver View Model
        /// 取得元では先にGetResourceでリソースディクショナリを取得して
        /// リソースをマージしておくこと。
        /// </summary>
        public override CommonDriver.ViewModel.IDriverViewModel GetViewModel
        {
            get { return new ViewModel.ModbusTCPMasterViewModel(); }
        }

        /// <summary>
        /// Driver Model
        /// 基本的にデバイスアドレス情報のみが取得可能。
        /// </summary>
        public override CommonDriver.Model.IDriverModel GetModel
        {
            get { return Model.ModbusTCPMasterModel.Instance; }
        }

        /// <summary>
        /// ドライバの表示名称
        /// </summary>
        public override string Name
        {
            get { return "Modbus TCP Master"; }
        }

        /// <summary>
        /// リソースディクショナリ
        /// View - ViewModel の関係を DataTemplate で定義したリソースディクショナリが取得可能。
        /// </summary>
        public override ResourceDictionary GetResource
        {
            get
            {
                var asm = Assembly.GetExecutingAssembly();

                var name = asm.GetName().Name;

                var dict = new ResourceDictionary();

                var uri = String.Format("pack://application:,,,/{0};component/View/DriverResources.xaml", name);

                dict.Source = new Uri(uri);
                return dict;
            }
        }
    }
}
