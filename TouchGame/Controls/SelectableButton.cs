using System.Runtime.CompilerServices;

namespace TouchGame;

public class SelectableButton : Button
{
    public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(
        nameof(SelectedValue),
        typeof(object),
        typeof(SelectableButton),
        propertyChanged: (b, o, n) =>
        {
            if (b is SelectableButton btn && btn.BindingContext != null)
            {
                btn.IsSelected = btn.BindingContext.Equals(n);
            }
        });

    public object SelectedValue
    {
        get { return GetValue(SelectedValueProperty); }
        set { SetValue(SelectedValueProperty, value); }
    }

    private static readonly BindablePropertyKey IsSelectedPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsSelected),
        typeof(bool),
        typeof(SelectableButton),
        false);

    public static readonly BindableProperty IsSelectedProperty = IsSelectedPropertyKey.BindableProperty;

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        private set { SetValue(IsSelectedPropertyKey, value); }
    }

    public static readonly BindableProperty SelectedStyleProperty = BindableProperty.Create(
        nameof(SelectedStyle),
        typeof(Style),
        typeof(SelectableButton));

    public Style SelectedStyle
    {
        get { return (Style)GetValue(SelectedStyleProperty); }
        set { SetValue(SelectedStyleProperty, value); }
    }

    public static readonly BindableProperty UnselectedStyleProperty = BindableProperty.Create(
        nameof(UnselectedStyle),
        typeof(Style),
        typeof(SelectableButton));

    public Style UnselectedStyle
    {
        get { return (Style)GetValue(UnselectedStyleProperty); }
        set { SetValue(UnselectedStyleProperty, value); }
    }

    private Dictionary<BindableProperty, object> SettedProperties = new Dictionary<BindableProperty, object>();

    public SelectableButton()
	{
		
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext != null)
        {
            this.IsSelected = this.BindingContext.Equals(SelectedValue);
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsSelectedProperty.PropertyName)
        {
            if (IsSelected)
            {
                foreach (var setter in SelectedStyle.Setters)
                {
                    var prop = setter.Property;
                    var value = setter.Value;

                    var settedPropValue = this.GetValue(prop);

                    if (value != settedPropValue)
                    {
                        SettedProperties.AddOrUpdate(prop, settedPropValue);

                        this.SetValue(prop, value);
                    }
                }
            }
            else
            {
                foreach (var item in SettedProperties)
                {
                    this.SetValue(item.Key, item.Value);
                }
            }
        }
    }
}
