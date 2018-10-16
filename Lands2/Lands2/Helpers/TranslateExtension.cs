namespace Lands2.Helpers
{
    using Lands2.Interfaces;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Text")]
    public class TranslateExtension:IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "Lands2.Resources.Resource";
             
        static readonly Lazy<ResourceManager> ResMgr =
            new Lazy<ResourceManager>(() => new ResourceManager(
                   ResourceId,
                   typeof(TranslateExtension).GetTypeInfo().Assembly));

        public TranslateExtension()
        {
            try
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
            catch (Exception e)
            {
                string msj = e.ToString();
            }            
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }

            var translation = ResMgr.Value.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format(
                        "Key '{0}' was not found in resources '{1}' for culture '{2}'.",
                        Text, ResourceId, ci.Name), "Text");
#else
                translation = Text; // Devuelve valor de la llave a ser desplegado para el usuario
#endif
            }

            return translation;
        }
    }
}
