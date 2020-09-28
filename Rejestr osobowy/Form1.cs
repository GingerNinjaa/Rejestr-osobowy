using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rejestr_osobowy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panelSubMenu.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            // jeśli submenu jest schowane 
            if (subMenu.Visible == false)
            {
                //chowaj wszystkie submenu
                // hideSubMenu();

                //pokaż to nasze submenu
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
