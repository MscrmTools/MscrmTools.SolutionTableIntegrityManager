using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.SolutionTableIntegrityManager
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Solution Table Integrity Manager"),
        ExportMetadata("Description", "This tool helps you to respect best practices when it comes to use managed tables in development solutions"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgEAYAAAAj6qa3AAAAIGNIUk0AAHomAACAhAAA+gAAAIDoAAB1MAAA6mAAADqYAAAXcJy6UTwAAAAGYktHRAAAAAAAAPlDu38AAAAJcEhZcwAAAGAAAABgAPBrQs8AAAAHdElNRQfnChsNBihhCtWKAAAB7ElEQVRo3u2Vv0vDQBTHXzzJIBSCBBykIdAOnTp1qBootJtToJtDCZ0yiFhCwVn/gK4dHG50DChuWZOCTgHBoVkCLkKGDpWGUOJQiuLP/CJnTD5LuEve5fu+vPcOoCDfUGEDTNM0TdP3OY7jOC59wU7X6TrdyaSiVbSKtrcX97yN9FOIx3w0H81HzabVsTpWxzByZ0DSRmwmLUySJEmSkk8YY4wx/sGIgTWwBoYRtjUyNwN+g2EYhmGowHlltgWSIvMtEJeiBUgLJk3uDQgwAzhuOvX99ape1/VV6et6EgIwPjhwXQBRLJeXy/QNIF4BinJ/T9MAz8+LRfDO/UcGOI7rUhTA6endHU2n///Er8Go3N4+PSEEwDBXV1tbYSJlmaaj1w7xCkgKQRAEQfB9URRFUXybWbkxICqFAaQFkCb3BvyZWyA48ab+PzBgxXrqsyzLsuzn9x9vA1VVVVX9bFzRAqQFROXkxLZtG6DRqNVqtd+/V9Wv93NfAYUBpAWE4AjGjqPhdq/d291FCCGEAKZTz/O8759rHi/7w/6wVPp4aIAZYNvV6vvpKcuEDJABADbsfbyPLy5Kze2H7YfgwfQ1GqNxq7Va3dys97NUAQAA8HI+O5wdHh/v7PA8z4cIrPtn/pmikNZf8Nd4BVgqvoeZHqwiAAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class Plugin : PluginBase, IPayPalPlugin
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Plugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        public string DonationDescription => "Donation for Solution Table Integrity Manager";

        public string EmailAccount => "tanguy92@hotmail.com";

        public override IXrmToolBoxPluginControl GetControl()
        {
            return new PluginControl();
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}