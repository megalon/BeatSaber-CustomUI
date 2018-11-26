using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomUI.Settings
{
    public class BoolViewController : SwitchSettingsController
    {
        public delegate bool GetBool();
        public event GetBool GetValue;

        public delegate void SetBool(bool value);
        public event SetBool SetValue;

        protected override bool GetInitValue()
        {
            bool value = false;
            if (GetValue != null)
            {
                value = GetValue();
            }
            return value;
        }

        protected override void ApplyValue(bool value)
        {
            if (SetValue != null)
            {
                SetValue(value);
            }
        }

        protected override string TextForValue(bool value)
        {
            return (value) ? "ON" : "OFF";
        }
    }

    public abstract class IntSettingsController : IncDecSettingsController
    {
        private int _value;
        protected int _min;
        protected int _max;
        protected int _increment;

        protected abstract int GetInitValue();
        protected abstract void ApplyValue(int value);
        protected abstract string TextForValue(int value);


        public override void Init()
        {
            _value = this.GetInitValue();
            this.RefreshUI();
        }
        public override void ApplySettings()
        {
            this.ApplyValue(this._value);
        }
        private void RefreshUI()
        {
            this.text = this.TextForValue(this._value);
            this.enableDec = _value > _min;
            this.enableInc = _value < _max;
        }
        public override void IncButtonPressed()
        {
            this._value += _increment;
            if (this._value > _max) this._value = _max;
            this.RefreshUI();
        }
        public override void DecButtonPressed()
        {
            this._value -= _increment;
            if (this._value < _min) this._value = _min;
            this.RefreshUI();
        }
    }

    public class IntViewController : IntSettingsController
    {
        public delegate int GetInt();
        public event GetInt GetValue;

        public delegate void SetInt(int value);
        public event SetInt SetValue;

        public void SetValues(int min, int max, int increment)
        {
            _min = min;
            _max = max;
            _increment = increment;
        }

        public void UpdateIncrement(int increment)
        {
            _increment = increment;
        }

        private int FixValue(int value)
        {
            if (value % _increment != 0)
            {
                value -= (value % _increment);
            }
            if (value > _max) value = _max;
            if (value < _min) value = _min;
            return value;
        }

        protected override int GetInitValue()
        {
            int value = 0;
            if (GetValue != null)
            {
                value = FixValue(GetValue());
            }
            return value;
        }

        protected override void ApplyValue(int value)
        {
            if (SetValue != null)
            {
                SetValue(FixValue(value));
            }
        }

        protected override string TextForValue(int value)
        {
            return value.ToString();
        }
    }

    public class ListViewController : ListSettingsController
    {
        public Func<float> GetValue = () => default(float);
        public Action<float> SetValue = (_) => { };
        public Func<float, string> GetTextForValue = (_) => "?";

        public delegate string StringForValue(float value);
        public event StringForValue FormatValue;

        public List<float> values;

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            numberOfElements = values.Count;
            var value = GetValue();

            numberOfElements = values.Count();
            idx = values.FindIndex(v => v.Equals(value));
        }

        protected override void ApplyValue(int idx)
        {
            SetValue(values[idx]);
        }

        protected override string TextForValue(int idx)
        {
            if (FormatValue != null)
            {
                return FormatValue(values[idx]);
            }
            return GetTextForValue(values[idx]);
        }

        public override void IncButtonPressed()
        {
            base.IncButtonPressed();
            ApplySettings();
        }

        public override void DecButtonPressed()
        {
            base.DecButtonPressed();
            ApplySettings();
        }
    }

    public class TupleViewController<T> : ListSettingsController
    {
        public Func<T> GetValue = () => default(T);
        public Action<T> SetValue = (_) => { };
        public Func<T, string> GetTextForValue = (_) => "?";

        public List<T> values;

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            numberOfElements = values.Count;
            var value = GetValue();

            numberOfElements = values.Count();
            idx = values.FindIndex(v => v.Equals(value));
        }

        protected override void ApplyValue(int idx)
        {
            SetValue(values[idx]);
        }

        protected override string TextForValue(int idx)
        {
            return GetTextForValue(values[idx]);
        }
    }
}
