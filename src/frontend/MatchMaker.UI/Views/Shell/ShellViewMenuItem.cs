using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.UI.Views.Shell
{

    public class ShellViewMenuItem
    {
        public ShellViewMenuItem()
        {
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}