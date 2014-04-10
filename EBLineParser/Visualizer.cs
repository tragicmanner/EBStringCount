using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBLineParser
{
    public partial class Visualizer : Form
    {
        public Visualizer()
        {
            InitializeComponent();
        }

        public Visualizer(string aLine1, string aLine2, string aLine3)
        {
            InitializeComponent();
            line1.Text = aLine1;
            line2.Text = aLine2;
            line3.Text = aLine3;
        }

        private void Visualizer_Load(object sender, EventArgs e)
        {

        }
    }
}
