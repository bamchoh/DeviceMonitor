using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceMonitor.Model
{
    /// <summary>
    /// DLLファイル名からドライバ情報クラスを取得出来るクラス
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DriverDllInfo
    {
        /// <summary>
        /// ドライバ情報クラス
        /// </summary>
        public CommonDriver.DriverInfo Driver { get; private set; }

        /// <summary>
        /// ドライバ情報クラスが格納されているDLLファイル名
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// ドライバの名称
        /// </summary>
        public string Name
        {
            get
            {
                return Driver.Name;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filename">DLLファイル名</param>
        public DriverDllInfo(string filename)
        {
            Driver = GetDriver(filename);
            Filename = filename;
        }

        /// <summary>
        /// DLLファイル名からリフレクションを使ってドライバ情報クラスを取得する
        /// </summary>
        /// <param name="filename">DLLファイル名</param>
        /// <returns>ドライバ情報クラス</returns>
        private CommonDriver.DriverInfo GetDriver(string filename)
        {
            var asm = System.Reflection.Assembly.LoadFrom(filename);
            var type = asm.GetTypes().First(t => (t.Name.IndexOf("DriverInfo") >= 0));
            var driver = (CommonDriver.DriverInfo)Activator.CreateInstance(type);
            return driver;
        }
    }

    /// <summary>
    /// ドライバ情報クラスをリスト化したクラス
    /// </summary>
    public class DriverDllList
    {
        /// <summary>
        /// (Private)ドライバ情報リストクラスのインスタンス
        /// </summary>
        private static DriverDllList _instance;

        /// <summary>
        /// (シングルトン)ドライバ情報クラスのインスタンス
        /// </summary>
        public static DriverDllList Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new DriverDllList();
                return _instance;
            }
        }

        /// <summary>
        /// (Private)ドライバ情報クラスのリスト
        /// </summary>
        private static List<DriverDllInfo> _driverList;

        /// <summary>
        /// ドライバ情報クラスのリスト
        /// </summary>
        public static List<DriverDllInfo> GetDriverList
        {
            get
            {
                return _driverList;
            }

            private set
            {
                _driverList = value;
            }
        }

        /// <summary>
        /// ドライバ情報クラスリストの中で現在選択されているリストのインデックス
        /// </summary>
        private static int _selected_index;

        /// <summary>
        /// ドライバ情報クラスリストの中で現在選択されているリストのインデックス
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selected_index;
            }

            set
            {
                _selected_index = value;
            }
        }

        /// <summary>
        /// 現在選択されているドライバ情報クラス
        /// </summary>
        public static CommonDriver.DriverInfo SelectedDriverInfo
        {
            get
            {
                return _driverList[_selected_index].Driver;
            }
        }

        /// <summary>
        /// コンストラクタ<br />
        /// ドライバDLLが格納されているDriverフォルダ内のdllを全て検索し
        /// dll内のドライバ情報を取得、リスト化する。
        /// </summary>
        private DriverDllList()
        {
            var dirs = System.IO.Directory.GetDirectories("Driver");

            foreach(var path in dirs)
            {
                var dll_filenames = System.IO.Directory.GetFiles(
                    path,
                    "Protocol.*.dll",
                    System.IO.SearchOption.TopDirectoryOnly);

                GetDriverList = new List<DriverDllInfo>();
                foreach (var dll_filename in dll_filenames)
                {
                    GetDriverList.Add(new DriverDllInfo(dll_filename));
                }
            }
            SelectedIndex = 0;
        }
    }
}
