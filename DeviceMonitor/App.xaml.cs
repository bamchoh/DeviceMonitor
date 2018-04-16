using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using DeviceMonitor.View;
using DeviceMonitor.ViewModel;
using System.Reflection;
using System.Linq;

/// \mainpage MODBUS モニタリングツール
///
/// \section intro クラス図 全体像
/// 
/// クラス図 全体像を以下に示す。<br />
/// クリックすると大きい画像が見れます。<br />
/// 各クラスの詳細は上記メニューバーの [クラス] を参照<br />
/// 
/// \htmlonly <a href="class_relation.png"><img src="class_relation.png" width=25% /></a> \endhtmlonly
/// 

/// <summary>
/// モニタアプリに関係するネームクラス。
/// ドライバ部分以外のクラスはここに存在する。
/// </summary>
namespace DeviceMonitor
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// アプリケーション起動時に呼ばれるクラス。
    /// MainWindowの生成を行う。
    /// </remarks>
    public partial class App : Application
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App() : base()
        {
            this.Startup += new StartupEventHandler(App_Startup);

            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        /// <summary>
        /// アプリ起動時に呼ばれるイベント
        /// 
        /// @image html app_sequence_001.png "アプリケーションの起動シーケンス"
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region シーケンス図生成用コメント
        // @startuml{app_sequence_001.png}
        // hide footbox
        // 
        // participant App as app
        //
        // box "namespace:View" #80c7ff
        //   participant DevMonWindow as window
        //   participant ConnectorView as connectView
        //   participant MonitorView as monitorView
        // end box
        //
        // box "namespace:ViewModel" #90ff80
        //   participant DevMonViewModel as devmonViewModel 
        //   participant ConnectorViewModel as connectViewModel
        //   participant MonitorViewModel as monitorViewModel
        // end box
        //
        // box "namespace:CommonDriver" #ffd2a0
        //   participant ViewModel.IDriverViewModel as driverViewModel
        //   participant View.DriverView as driverView
        // end box
        // 
        // app -> app : App_Startup()
        // activate app
        // create window
        // app -> window : << new >>
        // create devmonViewModel
        // app -> devmonViewModel : << new >>
        // activate devmonViewModel
        //   note right of devmonViewModel
        //     1. Driverフォルダ下のdllファイルを取得
        //     2. dllファイル内のDriverInfoを取得
        //     3. DriverInfoとAppのリースファイルをマージ
        //     4. リソースファイルを更新したのでDriverViewModel更新
        //   end note
        //   devmonViewModel --> app
        // deactivate devmonViewModel
        //   
        // deactivate devmonViewModel
        // app -> window : DataContext = View.DevMonWindow
        // app -> window : Show()
        // activate window
        //
        // group XAML 上での処理
        //     window -> devmonViewModel : SelectIndex
        //     activate devmonViewModel
        //       devmonViewModel --> window
        //     deactivate devmonViewModel
        //     
        //     window -> window : ConnectorView のバインド
        //     activate window
        //       window -> devmonViewModel : ConnectorViewModel
        //       activate devmonViewModel
        //         create connectViewModel
        //         devmonViewModel -> connectViewModel : << new >>
        //         devmonViewModel --> window
        //       deactivate devmonViewModel
        //
        //       create connectView
        //       window -> connectView : << new >>
        //     deactivate window
        //
        //     window -> window : DriverView のバインド
        //     activate window
        //     window -> devmonViewModel : DriverViewModel
        //       activate devmonViewModel
        //         create driverViewModel
        //         devmonViewModel -> driverViewModel : << new >>
        //         devmonViewModel --> window
        //       deactivate devmonViewModel
        //
        //       create driverView
        //       window -> driverView : << new >>
        //     deactivate window
        //
        //     window -> window : MonitorViewModel のバインド
        //     activate window
        //       window -> devmonViewModel : MonitorViewModel
        //       activate devmonViewModel
        //         create monitorViewModel
        //         devmonViewModel -> monitorViewModel : << new >>
        //         devmonViewModel --> window
        //       deactivate devmonViewModel
        //
        //       create monitorView
        //       window -> monitorView : << new >>
        //     deactivate window
        //
        // end
        //
        // app <-- window : 表示完了
        // deactivate window
        // deactivate app
        // 
        // @enduml
        #endregion // シーケンス図生成用コメント
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var window = new View.DevMonWindow();

            var devmonViewModel = new ViewModel.DevMonViewModel();
            window.DataContext = devmonViewModel;

            this.MainWindow.Show();
        }

        /// <summary>
        /// ドライバDLL内のリソースファイルをアプリケーションのリソースファイルにマージ
        /// </summary>
        public void ChangeResource(ResourceDictionary r)
        {
            Application.Current.Resources.MergedDictionaries.Add(r);
        }
    }
}
