using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Terminal.Gui.Issue306
{
    public interface IValueConverter
    {
        object Convert(object value, object parameter = null);
        
    }

    public class Binding
    {
        public View Target {get; private set;}
        public View Source {get; private set;}

        public string SourcePropertyName {get; private set;}
        public string TargetPropertyName {get; private set;}

        private object sourceDataContext;
        private PropertyInfo sourceBindingProperty;
        private IValueConverter valueConverter;

        public Binding(View source, string sourcePropertyName, View target, string targetPropertyName, IValueConverter valueConverter = null)
        {
            Target = target;
            Source = source;
            SourcePropertyName = sourcePropertyName;
            TargetPropertyName = targetPropertyName;
            sourceDataContext = Source.GetType().GetProperty("DataContext").GetValue(Source);
            sourceBindingProperty = sourceDataContext.GetType().GetProperty(SourcePropertyName);
            this.valueConverter = valueConverter;
            UpdateTarget();

            var notifier = ((INotifyPropertyChanged)sourceDataContext);
            if (notifier != null)
            {
                notifier.PropertyChanged += (s, e) => 
                {
                    if (e.PropertyName == SourcePropertyName)
                    {
                        UpdateTarget();
                    }
                };
            }
        }

        private void UpdateTarget()
        {
            try
            {                            
                var sourceValue = sourceBindingProperty.GetValue(sourceDataContext);
                if (sourceValue == null)
                {
                    return;
                }
                
                var finalValue = valueConverter?.Convert(sourceValue) ?? sourceValue;
               
                var targetProperty = Target.GetType().GetProperty(TargetPropertyName);                
                targetProperty.SetValue(Target, finalValue);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Binding failed: {ex}");
                
                throw;
            }
        }
    }
}
