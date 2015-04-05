using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace OldPluginsFW
{
    [Export(typeof(IHuman))]
    public interface IHuman
    {
        string Says();
    }


    public class Hippo : IHuman
    {
        public string Says()
        {
            return "Nothing...";
        }
    }

    
    class Program
    {
        
        static void Main(string[] args)
        {
            var h = Composable.GetExport<IHuman>();
            Console.WriteLine(h.Says());
            Console.ReadLine();
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 10000; i++)
            {
                //var dd = new Cat(new BostonZoo(), new MrJones());
                var ii = Composable.GetExport<IHuman>();
                //Console.WriteLine(ii.Manager.Name);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.Milliseconds);

            Console.ReadLine();
        }
    }
}
