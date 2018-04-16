using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DeviceMonitor.Lib
{
    /// <summary>
    /// SQL Server 2008 RC のデータベースと接続する為のクラス
    /// </summary>
    public class SqlServer2008RC : GeneralLibrary.ViewModelModelBase, DeviceMonitor.Lib.IDataController
    {
        #region Field

        /// <summary>
        /// シングルトン
        /// </summary>
        public static readonly SqlServer2008RC Instance = new SqlServer2008RC();

        /// <summary>
        /// 内部保持用データソース
        /// </summary>
        private DataTable _dt;

        /// <summary>
        /// 外部公開用データソース
        /// カラム情報としては device と value のみ
        /// device は デバイスアドレス
        /// value は アドレスの値
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (_dt != null)
                {
                    DataTable tmp;
                    tmp = _dt.Copy();
                    if (tmp.Columns.Contains("id"))
                    {
                        tmp.Columns.Remove("id");
                    }

                    if (tmp.Columns.Contains("device"))
                    {
                        tmp.Columns["device"].ReadOnly = true;
                    }
                    return tmp;
                }
                else
                {
                    return _dt;
                }
            }

            set
            {
                _dt = value;
                this.OnPropertyChanged("DataSource");
            }
        }

        /// <summary>
        /// SQL Server データベースを開くために使用する接続用文字列
        /// </summary>
        private string stConnectionString = string.Empty;

        /// <summary>
        /// データベースの名前
        /// </summary>
        private string dataBaseName = "devices_plc_db";

        /// <summary>
        /// テーブルの名前
        /// </summary>
        private string tableName = "dbo.value_table";

        #endregion // Field

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private SqlServer2008RC()
        {
            stConnectionString += "Data Source         = localhost\\SQLExpress2;";
            stConnectionString += "Initial Catalog     = " + dataBaseName + ";";
            stConnectionString += "Integrated Security = SSPI;";
        }

        #endregion // Constructor

        /// <summary>
        /// 全レコード取得
        /// </summary>
        /// <returns></returns>
        public DataTable Read()
        {
            DataSource = SendSQL(() => "select * from " + tableName);
            return DataSource;
        }

        /// <summary>
        /// device に value を書き込む
        /// </summary>
        /// <param name="device"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataTable Update(string device, string value)
        {
            SendSQL(() => "update " + tableName + String.Format(" set value={0} where device='{1}'", value, device));
            return this.Read();
        }

        /// <summary>
        /// addrList に格納されたアドレス情報でレコードを作成
        /// </summary>
        /// <param name="addrList"></param>
        /// <returns></returns>
        public DataTable Insert(IEnumerable<string> addrList)
        {
            SendSQL(() =>
            {
                string CmdString = "insert into " + tableName + " (device, value) values";
                foreach (string address in addrList)
                {
                    CmdString += String.Format("('{0}', 0),", address);
                }
                return CmdString = CmdString.TrimEnd(',');
            });
            return this.Read();
        }

        /// <summary>
        /// テーブルを初期化。テーブルが既に存在する場合はレコードを全削除。
        /// テーブルが存在しない場合はテーブルを新規作成する。
        /// </summary>
        /// <returns></returns>
        public DataTable InitTable()
        {
            if (ExistTable(GetTableObjectID()))
            {
                DeleteAllRecords();
            }
            else
            {
                CreateTable();
            }
            return null;
        }

        /// <summary>
        /// View用のデータソースを取得
        /// </summary>
        /// <returns></returns>
        public DataView GetDataSource()
        {
            if (this.DataSource != null)
            {
                return this.DataSource.DefaultView;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// テーブルの存在可否判定
        /// GetTableObjectID()で取得した DataTableは
        /// テーブルが存在しない場合。dt.Rows[0][0]が
        /// DBNullとなるため、DBNullでない場合
        /// テーブルが存在すると判断している。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ExistTable(DataTable dt)
        {
            return !(dt.Rows[0][0] is DBNull);
        }

        /// <summary>
        /// テーブルのオブジェクトIDを取得
        /// テーブルの存在可否確認で使用
        /// </summary>
        /// <returns></returns>
        private DataTable GetTableObjectID()
        {
            return SendSQL(() => "select object_id('" + tableName + "')");
        }

        /// <summary>
        /// 指定のテーブルがない場合に限りテーブルを生成する。
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTable()
        {
            string sqlString = string.Format("create table {0}(id int identity primary key, device varchar(255) default '...', value int default 0)", tableName);
            return SendSQL(() => sqlString);
        }

        /// <summary>
        /// テーブルに存在する全てのレコードを削除する。
        /// </summary>
        /// <returns></returns>
        private DataTable DeleteAllRecords()
        {
            return SendSQL(() => "truncate table " + tableName);
        }

        /// <summary>
        /// SQLを実行し、DataTableを取得する。
        /// </summary>
        /// <param name="CmdString">SQL文</param>
        /// <param name="cSqlConnection">実行するためのSQLオブジェクト</param>
        /// <returns></returns>
        private DataTable ExecuteSqlCmd(string CmdString, SqlConnection cSqlConnection)
        {
            SqlCommand cmd = new SqlCommand(CmdString, cSqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable(dataBaseName);
            sda.Fill(dt);
            return dt;
        }

        /// <summary>
        /// SQLを実行し、DataTableを取得する。
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        private DataTable SendSQL(Func<string> function)
        {
            SqlConnection cSqlConnection = null;
            DataTable dt = null;
            using (cSqlConnection = (new SqlConnection(stConnectionString)))
            {
                try
                {
                    string cmdString = function();
                    dt = ExecuteSqlCmd(cmdString, cSqlConnection);
                }
                catch (InvalidOperationException exc)
                {
                    // System.Windows.MessageBox.Show(exc.Message);
                }
                catch (SqlException exc)
                {
                    // System.Windows.MessageBox.Show(exc.Message);
                }
            }
            return dt;
        }
    }
}
