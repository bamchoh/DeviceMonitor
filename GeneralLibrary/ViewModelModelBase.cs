using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GeneralLibrary
{
    /// <summary>
    /// ViewModel, Model の基底クラス
    /// </summary>
    /// <remarks>
    /// プロパティが更新された場合、ModelならViewModelに、ViewModelならViewに通知する。<br />
    /// 各プロパティの更新を通知するかしないかは継承先のクラスで指定する。<br />
    /// <br />
    /// また、プロパティのデータ検証を行うメソッドの実装も行っている。<br />
    /// プロパティのデータ検証が正しいかどうかは継承先のクラスで指定する。<br />
    /// このクラスでは検証されたデータがエラーだった場合、エラーであることをViewやViewModelに通知するのみ。<br />
    /// </remarks>
    public abstract class ViewModelModelBase : INotifyPropertyChanged, IDataErrorInfo
    {

        #region INotifyPropertyChanged 実装部
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// SETプロパティでこのメソッドを呼び出すことで、PropertyChangedイベントを呼び出し
        /// PropertyChangedイベントを参照している各クラスに通知することができる
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }
        #endregion



        #region IDataErrorInfo Implementation Parts
        /// <summary>
        /// プロパティの値を検証した結果、Error情報として提供する
        /// たとえば、範囲が 0 < x < 100 のようなプロパティに対して
        /// -1 が入力された場合、エラーとなり、所定のエラーメッセージが
        /// このプロパティを通じて取得可能となる。
        /// </summary>
        public string Error
        {
            get
            {
                // とりあえず全エラー連結
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(
                    this,
                    new ValidationContext(this, null, null),
                    results))
                {
                    return string.Empty;
                }
                return string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            }
        }

        /// <summary>
        /// columnName で渡されたプロパティでエラーが発生しているかどうかを検証する
        /// 検証した結果エラーがあればそのエラーを返す
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                // TryValidatePropertyで、プロパティを検証
                var results = new List<ValidationResult>();
                var s = GetType().GetProperty(columnName).GetValue(this, null);
                if (Validator.TryValidateProperty(
                    s,
                    new ValidationContext(this, null, null) { MemberName = columnName },
                    results))
                {
                    return null;
                }
                // エラーがあれあ最初のエラーを返す
                return results.First().ErrorMessage;
            }
        }
        #endregion
    }
}
