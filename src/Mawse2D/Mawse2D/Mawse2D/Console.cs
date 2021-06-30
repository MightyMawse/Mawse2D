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
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Console_KeyStroke);
            this.KeyPreview = true;
        }

        List<ConVar> conVars = new List<ConVar>(); //List of all console variables

        private void Console_Load(object sender, EventArgs e)
        {
            ConVar_Init();
        }

        private void Console_KeyStroke(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                //Check to see if convar exists

                ConVar_Check(console_input.Text);
            }
        }

        private void ConVar_Check(string name)
        {
            foreach(ConVar conVar in conVars)
            {
                if(conVar.ConVar_Name.Contains(name))
                {
                    //Send this convar to main handler
                    game_window main = Application.OpenForms.OfType<game_window>().Single();
                    main.Mawse2D_ConVar(conVar);
                    console_input.Text = null;
                    this.Hide();
                }
                else
                {
                    console_input.Text += "\nError";
                    console_input.Text = null;
                    this.Hide();
                }
            }
            
        }



        private void ConVar_Init()
        {
            //Code 0 = null

            //Code 1 = map fetch
            ConVar mapFetch = new ConVar("map", 1);
            conVars.Add(mapFetch);

            //Code 2 = exit
            ConVar exit = new ConVar("exit", 2);
            conVars.Add(exit);

            //Code 3 = dev
            ConVar dev = new ConVar("dev", 3);
            conVars.Add(dev);

            //Code 3 = assertion iterator
            ConVar assert_lst = new ConVar("assert_lst", 4);
            conVars.Add(assert_lst);

        }
    }

    //==================================================
    //ConVar
    //Console Variable
    //==================================================
    public class ConVar
    {
        public string ConVar_Name;
        public int ConVar_Code = 0;

        public ConVar(string name, int code)
        {
            ConVar_Name = name;
            ConVar_Code = code;
        }
    }
}
