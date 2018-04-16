using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModbusTCPMasterDriver.ViewModel
{
    /// <summary>
    /// MODBUS TCP Master の デバイスアドレス関係の View と Model を管理するクラス
    /// </summary>
    class ModbusTCPMasterViewModel : GeneralLibrary.ViewModelModelBase, CommonDriver.ViewModel.IDriverViewModel
    {
        
        #region Field

        /// <summary>
        /// (現状未使用) 通信設定を表示するようのコマンド(内部保持用)
        /// </summary>
        private GeneralLibrary.Lib.RelayCommand _communicationConfigurationCommand;
        /// <summary>
        /// デバイスアドレスのデータを管理するモデルクラス
        /// </summary>
        private Model.ModbusTCPMasterModel _drv_model;

        #endregion // Field

        #region CommunicationConfigurationCommand

        /// <summary>
        /// (現状未使用) 通信設定を表示する用のコマンド
        /// </summary>
        public ICommand CommunicationConfigurationCommand
        {
            get
            {
                if (_communicationConfigurationCommand == null)
                {
                    _communicationConfigurationCommand = null;
                }
                return _communicationConfigurationCommand;
            }
        }

        #endregion // CommunicationConfigurationCommand

        #region Private Helper

        /// <summary>
        /// 通信設定ダイアログを表示する為のコマンド
        /// </summary>
        void ConfigureCommunicaitonSettings()
        {
            //                System.Windows.Window ccs = controller.getDialog();
            //                ccs.ShowDialog();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// モデルインスタンスの初期化
        /// デバイスアドレスの初期化
        /// </remarks>
        public ModbusTCPMasterViewModel()
        {
            _drv_model = ModbusTCPMasterDriver.Model.ModbusTCPMasterModel.Instance;

            SelectedIndex = 3;
        }

        #endregion // Constructor

        #region Propertys
        /// <summary Name="Address">
        /// 現在選択しているデバイスアドレスを取得する
        /// </summary>
        public Model.DeviceAddress Address
        {
            get { return _drv_model.CurrentDevice; }
        }

        /// <summary Name="Count">
        /// 現在の読み出し数を取得する
        /// </summary>
        public Model.Count Count
        {
            get
            {
                return _drv_model.Count;
            }
        }

        /// <summary Name="SelectedIndex">
        /// 現在選択中のリストのインデックスを取得する
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectIndex;
            }

            set
            {
                _selectIndex = value;
                _drv_model.CurrentDevice = _drv_model.DeviceAddressList[_selectIndex];
            }
        }
        private int _selectIndex;

        /// <summary name="DeviceName">
        /// デバイスアドレスのリストを取得する。
        /// </summary>
        public IList<Model.DeviceAddress> DeviceAddress
        {
            get
            {
                return _drv_model.DeviceAddressList;
            }
        }

        #endregion
    }
}
