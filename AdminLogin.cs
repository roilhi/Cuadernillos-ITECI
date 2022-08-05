using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cuadernillos_ITECI
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text == "p@55w0rD")
            {
                Books Obj = new Books();
                Obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta");
            }
        }

        private void lbCancel_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

    }
}
