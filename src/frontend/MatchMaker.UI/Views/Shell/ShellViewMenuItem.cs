using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Shell
{

    public class ShellViewMenuItem
    {
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public ContentPage Page { get; set; }
    }
}