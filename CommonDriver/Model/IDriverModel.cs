using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDriver.Model
{
    /// <summary>
    /// ドライバの各種情報を管理するインターフェース
    /// </summary>
    /// <remarks>
    /// 通信ドライバの情報を持つクラスはこのインターフェースを実装する。<br />
    /// 利用する側のクラスはドライバの内部設計に依存することなく情報を利用可能。<br />
    /// たとえば、ドライバが持っているデバイスの名前を取得したい場合は<br />
    /// このインターフェースを通じてやり取りされることが望ましい。
    /// </remarks>
    public interface IDriverModel
    {
        /// <summary>
        /// データベースで読みだす為のアドレス一覧。
        /// 返される内容は実装依存
        /// </summary>
        /// <returns>デバイスアドレス情報のコレクション</returns>
        IEnumerable<string> ReadingAddressList();
    }
}
