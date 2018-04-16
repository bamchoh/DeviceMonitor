using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModbusTCPMasterDriver.Model
{
    /// <summary>
    /// デバイスアドレスを格納するクラス
    /// </summary>
    public class DeviceAddress : GeneralLibrary.ViewModelModelBase, IValidatableObject
    {

        /// <summary>
        /// デバイスアドレスの初期値
        /// </summary>
        public int Default { get; set; }

        /// <summary>
        /// デバイスアドレスの入力範囲最下限(内部保持用)
        /// </summary>
        private static int _min;

        /// <summary>
        /// デバイスアドレスの入力範囲最下限
        /// </summary>
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// デバイスアドレスの入力範囲最上限(内部保持用)
        /// </summary>
        private static int _max;

        /// <summary>
        /// デバイスアドレスの入力範囲最上限
        /// </summary>
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// デバイスの名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示用フォーマット
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 現在入力中のアドレス
        /// </summary>
        [CustomValidation(typeof(DeviceAddress), "ValidateValueProperty")]
        public int Value
        {
            get { return _address; }

            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        /// <summary>
        /// 現在入力中のアドレス(内部保持用)
        /// </summary>
        private int _address;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">デバイスの名前</param>
        /// <param name="min">デバイスアドレスの入力範囲最下限</param>
        /// <param name="max">デバイスアドレスの入力範囲最上限</param>
        /// <param name="def">デバイスアドレスの初期値</param>
        /// <param name="format">デバイスアドレス表示用フォーマット</param>
        public DeviceAddress(
            string name = "",
            int min = 0,
            int max = 0,
            int def = 0,
            string format = "{0}")
        {
            Name = name;
            Min = min;
            Max = max;
            Default = def;
            Value = def;
            Format = format;
        }

        /// <summary>
        /// アドレス入力値検証用メソッド。範囲外の値を入力された場合は所定のエラーメッセージを返す
        /// </summary>
        /// <param name="value">入力されたアドレス値</param>
        /// <param name="c">検証用コンテキスト</param>
        /// <returns></returns>
        public static ValidationResult ValidateValueProperty(int value, ValidationContext c)
        {
            var cxt = (DeviceAddress)c.ObjectInstance;
            if (value < cxt.Min || value > cxt.Max)
                return new ValidationResult(string.Format("アドレス範囲は {0} から {1} の間です.", cxt.Min, cxt.Max));
            return ValidationResult.Success;
        }

        /// <summary>
        /// クラスレベルの検証ルールを定義(現状はクラスレベルでの検証は必要ないので常に成功を返す)
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new[] { ValidationResult.Success };
        }
    }


    /// <summary>
    /// デバイス読み出し数を表すクラス
    /// </summary>
    public class Count : GeneralLibrary.ViewModelModelBase, IValidatableObject
    {
        /// <summary>
        /// 初期値
        /// </summary>
        public int Default { get; set; }
        /// <summary>
        /// 最下限
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 最上限
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// 現在の読み出し数
        /// </summary>
        [CustomValidation(typeof(Count), "ValidateValueProperty")]
        public int Value
        {
            get { return _count; }

            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged("Count");
                }
            }
        }
        /// <summary>
        /// 現在の読み出し数(内部保持用)
        /// </summary>
        private int _count;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">最下限</param>
        /// <param name="max">最上限</param>
        /// <param name="def">初期値</param>
        public Count(int min = 0, int max = 0, int def = 0)
        {
            Min = min;
            Max = max;
            Default = def;
            Value = def;
        }

        /// <summary>
        /// 読み出し数検証用メソッド。範囲外の値を入力された場合は所定のエラーメッセージを返す
        /// </summary>
        /// <param name="value">入力された読み出し数</param>
        /// <param name="c">検証用コンテキスト</param>
        /// <returns></returns>
        public static ValidationResult ValidateValueProperty(int value, ValidationContext c)
        {
            var cxt = (Count)c.ObjectInstance;
            if (value < cxt.Min || value > cxt.Max)
                return new ValidationResult(string.Format("読み出し点数は{0} から {1} の範囲で指定してください.", cxt.Min, cxt.Max));
            return ValidationResult.Success;
        }

        /// <summary>
        /// クラスレベルの検証ルールを定義(現状はクラスレベルでの検証は必要ないので常に成功を返す)
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Console.WriteLine("IValidatableObject.Validate");
            return new[] { ValidationResult.Success };
        }
    }

    
    /// <summary>
    /// デバイスアドレス、読み出し数を管理するクラス
    /// </summary>
    class ModbusTCPMasterModel : CommonDriver.Model.IDriverModel
    {
        /// <summary>
        /// インスタンス(シングルトン)
        /// </summary>
        public static readonly ModbusTCPMasterModel Instance = new ModbusTCPMasterModel();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// デバイスアドレスの初期化
        /// 読み出し数の初期化
        /// </remarks>
        private ModbusTCPMasterModel()
        {
            _deviceInfo = new List<DeviceAddress>();
            _count = new Count(min: 1, max: 100, def: 10);

            _deviceInfo.Add(new DeviceAddress(
                name: "Coil",             format: "{0:D5}", min: 0, max: 65535 ));
            _deviceInfo.Add(new DeviceAddress(
                name: "Input Status",     format: "{0:D5}", min: 0, max: 65535 ));
            _deviceInfo.Add(new DeviceAddress(
                name: "Input Register",   format: "{0:D5}", min: 0, max: 65535 ));
            _deviceInfo.Add(new DeviceAddress(
                name: "Holding Register", format: "{0:D5}", min: 0, max: 65535 ));
        }

        /// <summary>
        /// デバイスアドレスの一覧を取得するプロパティ
        /// </summary>
        public IList<DeviceAddress> DeviceAddressList
        {
            get { return _deviceInfo; }
            set { _deviceInfo = value; }
        }

        /// <summary>
        /// 現在選択されているデバイスアドレスを取得するプロパティ
        /// </summary>
        public DeviceAddress CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; }
        }

        /// <summary>
        /// 読み出し数を取得するプロパティ
        /// </summary>
        public Count Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// 現在選択されているデバイスアドレスから読み出し数分のアドレス情報を持つリストを返す
        /// </summary>
        /// <remarks>
        /// Holding Register の 10 番目 から 10 ワードの読み出しの場合<br />
        /// 400011<br />
        /// 400012<br />
        /// 400013<br />
        /// ...<br />
        /// 400019<br />
        /// 400020<br />
        /// が配列として取得できる。<br />
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<string> ReadingAddressList()
        {
            for (int i = 0; i < Count.Value; i++)
            {
                yield return String.Format(CurrentDevice.Format, CurrentDevice.Value + i);
            }
        }

        /// <summary>
        /// デバイスアドレスの一覧を取得するプロパティ(内部保持用)
        /// </summary>
        private IList<DeviceAddress> _deviceInfo;

        /// <summary>
        /// 現在選択されているデバイスアドレスを取得するプロパティ(内部保持用)
        /// </summary>
        private DeviceAddress _currentDevice;

        /// <summary>
        /// 読み出し数を取得するプロパティ
        /// </summary>
        private Count _count;
    }
}
