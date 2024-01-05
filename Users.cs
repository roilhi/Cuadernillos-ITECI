using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Cuadernillos_ITECI
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            Populate();
        }
        private void Populate()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>("usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("status","current");
            var cursor = collection.Find(filter);
            var list = cursor.ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("username", typeof(string));
            dt.Columns.Add("contraseña", typeof(string));
            dt.Columns.Add("email", typeof(string));

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[0] = item["wholeName"];
                    dr[1] = item["user"];
                    dr[2] = item["password"];
                    dr[3] = item["email"];
                }
                dt.Rows.Add(dr);
            }

            UsersDGV.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || tbPassword.Text == "" || tbEmail.Text == "" || tbUsername.Text == "")
            {
                MessageBox.Show("Falta información, favor de completar");
            }
            else
            {
                try
                {
                    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase("Cuadernillos_ITECI");
                    var collection = database.GetCollection<BsonDocument>("usuarios");
                    var doc = new BsonDocument
                    {
                        { "user", tbUsername.Text.Trim()},
                        { "password", tbPassword.Text.Trim()},
                        { "WholeName", tbName.Text.Trim().ToUpper() },
                        { "email", tbEmail.Text + "@itecipreparatoria.edu.mx"},
                    };
                    collection.InsertOne(doc);
                    MessageBox.Show("¡Usuario guardado!");
                    Populate();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Reset() 
        {
            tbEmail.Text = "";
            tbName.Text = "";
            tbUsername.Text = "";
            tbPassword.Text = "";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        int key = 0;
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Falta información");
            }
            else
                try
                {
                    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase("Cuadernillos_ITECI");
                    var collection = database.GetCollection<BsonDocument>("usuarios");
                    var doc = new BsonDocument
                    {
                        { "user", tbUsername.Text.Trim().ToUpper()},
                        { "password", tbPassword.Text.Trim().ToUpper()},
                        { "WholeName", tbName.Text.Trim().ToUpper() },
                    };
                    collection.DeleteOne(doc);
                    MessageBox.Show("¡Items eliminados!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void UsersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                tbName.Text = UsersDGV.SelectedRows[0].Cells[1].Value.ToString();
                tbEmail.Text = UsersDGV.SelectedRows[0].Cells[2].Value.ToString();
                tbPassword.Text = UsersDGV.SelectedRows[0].Cells[3].Value.ToString();
                tbUsername.Text = UsersDGV.SelectedRows[0].Cells[4].Value.ToString();

            }
            if (tbName.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(UsersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tbEmail.Text == "" || tbName.Text == "" || tbPassword.Text == "" || tbUsername.Text == "")
            {
                MessageBox.Show("Falta información, favor de completar");
            }
            else
            {
                try
                {
                    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase("Cuadernillos_ITECI");
                    var collection = database.GetCollection<BsonDocument>("usuarios");
                    var doc = new BsonDocument
                    {
                        { "user", tbUsername.Text.Trim().ToUpper()},
                        { "password", tbPassword.Text.Trim().ToUpper()},
                        { "WholeName", tbName.Text.Trim().ToUpper() }
                    };
                    collection.InsertOne(doc);
                    MessageBox.Show("¡Items editados!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void lbLogout_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void lbDashboard_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            Obj.Show();
            this.Hide();
        }
    }
}
