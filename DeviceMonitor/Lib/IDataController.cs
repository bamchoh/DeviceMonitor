using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DeviceMonitor.Lib
{
    /// <summary>
    /// データベースにアクセスする為のインターフェース
    /// </summary>
    public interface IDataController
    {
        /// <summary>
        /// データベースのテーブル情報を読み出す
        /// 読みだされるデータは実装依存
        /// </summary>
        /// <returns></returns>
        DataTable Read();

        /// <summary>
        /// データベース内の device の値を value に更新する
        /// 更新されるデータは実装依存
        /// </summary>
        /// <param name="device">データベース内のキー</param>
        /// <param name="value">キーの更新値</param>
        /// <returns></returns>
        DataTable Update(string device, string value);

        /// <summary>
        /// データベース内に startAddress の情報をもとにデータを追加する
        /// 追加されるデータは実装依存
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        DataTable Insert(IEnumerable<string> startAddress);

        /// <summary>
        /// データベース内のテーブル情報を初期化する
        /// 初期化の仕方は実装依存
        /// </summary>
        /// <returns></returns>
        DataTable InitTable();

        /// <summary>
        /// Viewに渡す用のデータソースを提供
        /// 提供されるデータソースは実装依存
        /// </summary>
        /// <returns></returns>
        DataView GetDataSource();
    }
}
