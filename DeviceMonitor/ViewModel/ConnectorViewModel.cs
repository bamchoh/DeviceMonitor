using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using DeviceMonitor.Properties;

namespace DeviceMonitor.ViewModel
{
    /// <summary>
    /// モニタリング状態を管理するViewModel
    /// </summary>
    /// <remarks>
    /// モニタリング開始/停止ボタンを押下するたびに<br />
    /// 状態を切り替える
    /// </remarks>
    public class ConnectorViewModel : GeneralLibrary.ViewModelModelBase
    {
        #region Fields

        /// <summary>
        /// ConnectCommandをコマンドを使用する為内部的なプロパティ
        /// </summary>
        private GeneralLibrary.Lib.RelayCommand _connectCommand;

        /// <summary>
        /// 現在の接続状態を示すクラス
        /// </summary>
        private Model.MonitorModel _model;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// モデルの初期化を行う
        /// </remarks>
        public ConnectorViewModel()
        {
            _model = Model.MonitorModel.Instance;
            _model.PropertyChanged += ModelPropertyChanged;
        }

        #endregion // Constructor

        #region ConnectCommand
        /// <summary>
        /// モニタリング開始/停止ボタンが押されるたびに呼ばれるコマンド
        /// 
        /// シーケンス図(クリックで拡大)<br />
        /// \htmlonly <a href="connect_command_sequence_001.png"><img src="connect_command_sequence_001.png" width=25% /></a> \endhtmlonly
        /// 
        /// </summary>

        #region シーケンス図
        // @startuml{connect_command_sequence_001.png}
        // hide footbox
        // 
        // participant ConnectorViewModel as connectViewModel #90ff80
        // participant Lib.RelayCommand as relayCommand #ffd2a0
        // participant MonitorModel as monitorModel
        // participant MonitorViewModel as monitorViewModel #90ff80
        // participant Lib.SqlServer2008RC as db #ffd2a0
        // participant DriverDllList as driver_dll_list
        // participant DriverDllInfo as driver_dll_info
        // participant ModbusTCPMasterModel as driver_model
        // 
        // [-> connectViewModel : ConnectCommand.Execute
        // activate connectViewModel
        //   connectViewModel -> relayCommand : Execute
        //   activate relayCommand
        //     connectViewModel <- relayCommand : _execute()
        //     activate connectViewModel
        //       note left of connectViewModel
        //         _execute() に ConnectViewModel.StartMonitoring
        //         をActionとして登録してあるので、
        //         RelayCommand は _execute()を実行することで
        //         ConnectViewModel.StartMonitoring を呼び出すことが可能
        //       end note
        //       connectViewModel -> monitorModel : StartMonitoring()
        //       activate monitorModel
        //         monitorModel -> monitorModel : Change State
        //         activate monitorModel
        //           monitorModel -> monitorViewModel : ModelPropertyChanged()
        //           activate monitorViewModel
        //             opt モニタリング状態
        //               monitorViewModel -> db : InitTable()
        //               activate db
        //                 alt DBにテーブルがある
        //                   db -> db : DeleteAllRecords()
        //                 else DBにテーブルがない
        //                   db -> db : CreateTable()
        //                 end
        //                 monitorViewModel <-- db
        //               deactivate db
        //
        //               monitorViewModel -> driver_dll_list : SelectedDriverInfo.GetModel.GetStartAddress()
        //               driver_dll_list -> driver_dll_info : GetModel.GetStartAddress()
        //               driver_dll_info -> driver_model : GetStartAddress()
        //               activate driver_model
        //                 note right of driver_model
        //                   読み出し個数と現在選択されているデバイスアドレスから
        //                   読み出すアドレスをリスト化する
        //                 end note
        //                 driver_dll_info <-- driver_model
        //               deactivate driver_model
        //               driver_dll_list <-- driver_dll_info
        //               monitorViewModel <-- driver_dll_list
        //
        //               monitorViewModel -> db : Insert("アドレスリスト")
        //               activate db
        //                 db -> db : アドレスリストからインサート文実行
        //                 monitorViewModel <-- db
        //               deactivate db
        //             end
        //             monitorModel <-- monitorViewModel
        //           deactivate monitorViewModel
        //         deactivate monitorModel
        //         connectViewModel <-- monitorModel
        //       deactivate monitorModel
        //       connectViewModel --> relayCommand
        //     deactivate connectViewModel
        //     connectViewModel <-- relayCommand
        //   deactivate relayCommand
        //   [<-- connectViewModel
        // deactivate connectViewModel
        // 
        // @enduml
        #endregion シーケンス図
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                {
                    _connectCommand = new GeneralLibrary.Lib.RelayCommand(param => this.StartMonitoring());
                }
                return _connectCommand;
            }
        }

        #endregion // ConnectCommand

        #region Private Helpers

        /// <summary>
        /// データベースと接続するためのコマンド
        /// </summary>
        void StartMonitoring()
        {
            this._model.StartMonitoring();
        }

        #endregion // Private Helpers

        #region Properties

        /// <summary>
        /// モニタリング状態（文字列）を表すプロパティ
        /// </summary>
        /// <remarks>
        /// モニタ状態がモニタリング中なら「モニタ停止」<br />
        /// モニタ状態がモニタリング停止中なら「モニタ開始」<br />
        /// を返す
        /// </remarks>
        public string ConnectState
        {
            get
            {
                if (_model.State)
                {
                    return Strings.ConnectorViewModel_ConnectState;
                }
                else
                {
                    return Strings.ConnectorViewModel_DisconnectState;
                }
            }
        }

        #endregion // Properties

        #region PrivateMethods

        /// <summary>
        /// モニタリングモデルクラスのモニタリング状態が変更されると呼び出されるイベント。
        /// このイベントを起点にViewModel上のConnectStateのGetプロパティを呼び出す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ConnectState");
        }

        #endregion // PrivateMethods

    }
}
