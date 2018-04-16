using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace DeviceMonitor.Model
{
    /// <summary>
    /// DBをモニタリングしているかどうかを表すクラス<br />
    /// </summary>
    /// <remarks>
    /// Monitoring => モニタリング中<br />
    /// Stop => モニタリング停止中<br />
    /// </remarks>
    class MonitoringState
    {
        #region Field

        /// <summary>
        /// モニタリング状態を表すプロパティ
        /// </summary>
        public static readonly bool Monitoring = true;

        /// <summary>
        /// モニタリング停止状態を表すプロパティ
        /// </summary>
        public static readonly bool Stop = false;

        /// <summary>
        /// 現状の状態を示すプロパティ<br />
        /// MonitoringState.Monitoring か <br />
        /// MonitoringState.Stop が格納されている<br />
        /// </summary>
        protected bool _currentState;

        #endregion // EndField

        #region Constructor

        /// <summary>
        /// コンストラクタ<br />
        /// </summary>
        /// <param name="defaultState">初期状態を決めることができる true なら モニタリング中, false なら モニタリング停止中</param>
        public MonitoringState(bool defaultState = false)
        {
            _currentState = defaultState;
        }

        #endregion // Constructor

        #region Property

        /// <summary>
        /// 現状の状態を返す
        /// </summary>
        public bool CurrentState
        {
            get
            {
                return _currentState;
            }

            set
            {
                this._currentState = value;
            }
        }

        #endregion // Property

    }

    /// <summary>
    /// DBとの接続状態を示すクラス
    /// </summary>
    class MonitorModel : GeneralLibrary.ViewModelModelBase
    {

        #region Field

        /// <summary>
        /// シングルトン
        /// </summary>
        public static readonly MonitorModel Instance = new MonitorModel();

        /// <summary>
        /// モニタリングの状態を表す内部プロパティ
        /// </summary>
        private MonitoringState _monitoringState;

        #endregion // Field

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected MonitorModel()
        {
            _monitoringState = new MonitoringState();
        }
        
        /// <summary>
        /// モニタリングの現在の状態を表すプロパティ
        /// </summary>
        public bool State {
            get
            {
                return this._monitoringState.CurrentState;
            }
            set
            {
                if (this._monitoringState.CurrentState != value)
                {
                    this._monitoringState.CurrentState = value;
                    this.OnPropertyChanged("ConnectState");
                }
            }
        }

        /// <summary>
        /// モニタリングの状態をトグルする
        /// Monitoring なら Stopに
        /// Stop なら Monitoring にする
        /// </summary>
        public void StartMonitoring()
        {
            if (!this.State)
            {
                // タイマ開始
                /*
                 */
                this.State = MonitoringState.Monitoring;
            }
            else
            {
                // タイマ停止
                this.State = MonitoringState.Stop;
            }
        }

    }
}
