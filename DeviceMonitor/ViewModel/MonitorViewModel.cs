using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Data;
using System.Windows.Threading;
using DeviceMonitor.Model;
using System.Windows.Input;

namespace DeviceMonitor.ViewModel
{
    /// <summary>
    /// デバイスの値を読み出すViewとデバイスの値を管理するModelを管理するクラス
    /// </summary>
    public class MonitorViewModel : GeneralLibrary.ViewModelModelBase
    {
        #region Field

        /// <summary>
        /// 現在の接続状態を示すクラス
        /// </summary>
        readonly MonitorModel _model;

        /// <summary>
        /// (現在未使用)一定周期でDBを更新する用のタイマ
        /// </summary>
        readonly DispatcherTimer _dispatcherTimer;

        /// <summary>
        /// DBアクセス用のインスタンス
        /// </summary>
        private Lib.SqlServer2008RC _database = Lib.SqlServer2008RC.Instance;

        #endregion // Field

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// モデルの初期化と、タイマの初期化を行う
        /// </remarks>
        /// <param name="driver"></param>
        public MonitorViewModel(CommonDriver.Model.IDriverModel driver)
        {
            _model = Model.MonitorModel.Instance;
            _model.PropertyChanged += ModelPropertyChanged;
            _database.PropertyChanged += DataBasePropertyChanged;

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        }

        #endregion // Constructor

        #region Property

        /// <summary>
        /// 接続状態を表すプロパティ
        /// </summary>
        public System.Windows.Visibility ConnectState
        {
            get
            {
                if (this._model.State)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// DataGrid に表示する用のデータソースを取得する為のプロパティ
        /// </summary>
        public DataView DataSource
        {
            get
            {
                return this._database.GetDataSource();
            }
        }

        #endregion // Property

        #region PrivateMethods

        /// <summary>
        /// 接続状態が変更されたときに発生するイベント
        /// このイベントを起点にDBにアクセスするかどうかを決定する
        /// モニタリング状態が「モニタリング中」ならDBのテーブルを初期化して
        /// ドライバ情報をもとにレコードを追加する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this._model.State)
            {
                // _dispatcherTimer.Start();
                _database.InitTable();
                _database.Insert(Model.DriverDllList.SelectedDriverInfo.GetModel.ReadingAddressList());
            }
            else
            {
                // m_parent._view.deviceMonitorPanel.Children.Remove(m_parent.controller);
                // _dispatcherTimer.Stop();
            }
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// (現在未使用)一定周期でデータベースの状態を更新するタイマ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _database.Read();
        }

        /// <summary>
        /// データベースの状態が変更された場合に発生するイベント
        /// このイベントを起点にデータソースを更新する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBasePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion // PrifateMethods
    }
}
