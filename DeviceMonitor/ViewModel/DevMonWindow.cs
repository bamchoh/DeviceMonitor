using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows;

namespace DeviceMonitor.ViewModel
{
    /// <summary>
    /// デバイスモニタに関係するViewModelやDriver情報を管理するクラス
    /// </summary>
    public class DevMonViewModel : GeneralLibrary.ViewModelModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// 
        /// シーケンス図(クリックで拡大)<br />
        /// \htmlonly <a href="devicemonitor_viewmodel_devmonviewmodel_sequence.png"><img src="devicemonitor_viewmodel_devmonviewmodel_sequence.png" width=25% /></a> \endhtmlonly
        /// 
        /// 
        /// </remarks>

        #region シーケンス図
        // @startuml{devicemonitor_viewmodel_devmonviewmodel_sequence.png}
        // hide footbox
        // participant App as app
        // participant ViewModel.DevMonViewModel as devmonViewModel #90ff80
        // participant Model.DriverDllList as driver_dll_list << static >> #ffcea0
        // participant Model.DriverDllInfo as dll_info #ffcea0
        // participant CommonDriver.DriverInfo as driver_info
        // 
        // create devmonViewModel
        // app -> devmonViewModel : << new >>
        // activate devmonViewModel
        //   devmonViewModel -> devmonViewModel : SelectedIndex
        //   activate devmonViewModel
        //     create driver_dll_list
        //     devmonViewModel -> driver_dll_list : Instance.SelectedIndex = value
        //     activate driver_dll_list
        //       note right of driver_dll_list
        //         Driverフォルダ下の
        //         dllファイルを取得
        //       end note
        //
        //       loop 0, dllファイル個数
        //         driver_dll_list -> driver_dll_list : Add
        //         activate driver_dll_list
        //           create dll_info
        //           driver_dll_list -> dll_info : << new >>
        //
        //           note right of dll_info
        //             filenameからDriverの
        //             dllファイルを動的取得
        //             dllファイルから
        //             DriverInfoを取得する
        //
        //           end note
        //           activate dll_info
        //             dll_info -> dll_info : GetDriver(filename)
        //             activate dll_info
        //               create driver_info
        //             deactivate dll_info
        //             dll_info -> driver_info : << new >>
        //           deactivate dll_info
        //         deactivate driver_dll_list
        //       end
        //
        //       driver_dll_list --> devmonViewModel
        //
        //     deactivate driver_dll_list
        //
        //     devmonViewModel -> driver_dll_list : SelectedDriverInfo.GetResource
        //     activate driver_dll_list
        //       devmonViewModel <-- driver_dll_list
        //     deactivate driver_dll_list
        //     devmonViewModel -> app : ChangeResource(Resource)
        //
        //     note right of devmonViewModel
        //       DriverDllListから取得した
        //       DriverInfoの
        //       リソースファイルとマージ
        //     end note
        //
        //     activate app
        //       app -> app : Add(Resource)
        //       activate app
        //       deactivate app
        //       devmonViewModel <-- app
        //     deactivate app
        //
        //     note right of devmonViewModel
        //       リソースファイルを更新したので
        //       DriverViewModel更新
        //     end note
        //
        //     devmonViewModel -> devmonViewModel : DriverViewModel
        //     activate devmonViewModel
        //       devmonViewModel -> driver_dll_list : SelectedDriverInfo
        //       activate driver_dll_list
        //         driver_dll_list -> dll_info : GetViewModel
        //         activate dll_info
        //           dll_info --> driver_dll_list
        //         deactivate dll_info
        //         driver_dll_list --> devmonViewModel
        //       deactivate driver_dll_list
        //     deactivate devmonViewModel
        //
        //   deactivate devmonViewModel
        // deactivate devmonViewModel
        // @enduml
        #endregion シーケンス図
        public DevMonViewModel()
        {
            SelectedIndex = 0;
        }

        /// <summary>
        /// ドライバ情報のリストを取得するプロパティ
        /// </summary>
        public List<Model.DriverDllInfo> DriverList
        {
            get
            {
                return Model.DriverDllList.GetDriverList;
            }
        }

        /// <summary>
        /// 現在選択しているドライバ情報リストのインデックスを取得/設定するプロパティ
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return Model.DriverDllList.Instance.SelectedIndex;
            }

            set
            {
                Model.DriverDllList.Instance.SelectedIndex = value;
                ((App)Application.Current).ChangeResource(Model.DriverDllList.SelectedDriverInfo.GetResource);
                this.OnPropertyChanged("DriverViewModel");
            }
        }

        /// <summary>
        /// ドライバ情報を管理するクラスを取得するプロパティ
        /// </summary>
        public CommonDriver.ViewModel.IDriverViewModel DriverViewModel
        {
            get
            {
                return Model.DriverDllList.SelectedDriverInfo.GetViewModel;
            }
        }

        /// <summary>
        /// モニタリング状態を管理するクラスを取得するプロパティ
        /// </summary>
        public ViewModel.ConnectorViewModel ConnectorViewModel
        {
            get
            {
                return new ViewModel.ConnectorViewModel();
            }
        }

        /// <summary>
        /// デバイスの値をモニタリングする為の情報を管理するクラスを取得するプロパティ
        /// </summary>
        public ViewModel.MonitorViewModel MonitorViewModel
        {
            get
            {
                return new ViewModel.MonitorViewModel(Model.DriverDllList.SelectedDriverInfo.GetModel);
            }
        }

    }
}
