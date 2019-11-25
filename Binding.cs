using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using NStack;

namespace Terminal.Gui.Issue306
{
    public class Binding
    {
        public View Target {get; private set;}
        public View Source {get; private set;}

        public string SourcePropertyName {get; private set;}
        public string TargetPropertyName {get; private set;}

        private object sourceDataContext;
        private PropertyInfo sourceBindingProperty;
        public Binding(View source, string sourcePropertyName, View target, string targetPropertyName)
        {
            Target = target;
            Source = source;
            SourcePropertyName = sourcePropertyName;
            TargetPropertyName = targetPropertyName;
            sourceDataContext = Source.GetType().GetProperty("DataContext").GetValue(Source);
            sourceBindingProperty = sourceDataContext.GetType().GetProperty(SourcePropertyName);
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
                    return;

                var targetProperty = Target.GetType().GetProperty(TargetPropertyName);
                var bytes = Encoding.ASCII.GetBytes(sourceValue.ToString());
                targetProperty.SetValue(Target, ustring.Make(bytes));                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Binding failed: {ex}");
            }
        }
    }
}
