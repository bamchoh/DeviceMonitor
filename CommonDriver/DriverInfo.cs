using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CommonDriver.ViewModel;
using CommonDriver.Model;
using System.Windows;

/// <summary>
/// ドライバの共通クラスを取り纏めるネームスペース
/// </summary>
namespace CommonDriver
{
    /// <summary>
    /// ドライバ情報を格納するクラス
    /// <p>abstract クラスなので、継承先クラスでの実装を期待する。<br />
    /// このクラスは以下のプロパティを持つ。</p>
    /// <ul>
    ///   <li>ドライバのViewModel</li>
    ///   <li>ドライバのアドレス等の固有の情報をもつModel</li>
    ///   <li>ドライバの名称</li>
    ///   <li>ドライバのViewリソース</li>
    /// </ul>
    /// 各情報を個別に取得することは可能だが、ドライバのViewModel だけ期待する使用方法がある。詳細は DriverInfo.GetViewModel の説明を参照。
    /// </summary>
    public abstract class DriverInfo
    {
        /// <summary>
        /// ドライバのViewModelを取得できる<br /><br />
        /// <i>期待する使用方法</i><br /><br />
        /// DriverInfo.GetResource でドライバの View - ViewModel のバインド情報を取得し
        /// それを取得元のリソースとマージしたあと、GetViewModel で
        /// ViewModel を取得することを期待する。
        /// 
        /// このようにすることで、View 上 の ContentControl の Content
        /// に ViewModel がバインドされるタイミングでドライバリソース内
        /// の DataTemplate が呼び出され DataTemplate で定義されている
        /// View が ViewModel の替わりにバインドされるようになる。
        /// </summary>
        public abstract IDriverViewModel GetViewModel { get; }

        /// <summary>
        /// ドライバのアドレス情報などを持つModelクラスを取得できる
        /// </summary>
        public abstract IDriverModel GetModel { get; }

        /// <summary>
        /// ドライバの名称を取得できる
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// ドライバのViewリソース情報を持つ ResourceDictionary を取得できる
        /// </summary>
        public abstract ResourceDictionary GetResource { get; }

    }
}
