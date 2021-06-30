using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mawse2D
{
    public partial class MapNameInput : Form
    {
        public MapNameInput()
        {
            InitializeComponent();
        }

        

        private void btnOpen_Click(object sender, EventArgs e)
        {
            game_window main = Application.OpenForms.OfType<game_window>().Single();
            main.OpenmapName = mapName.Text;
            main.Mawse2D_MapFetcher();
            this.Hide();
        }

        private void MapNameInput_Load(object sender, EventArgs e)
        {

        }
    }
}
