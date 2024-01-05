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
    public partial class Books : Form
    {
        public Books()
        {
            InitializeComponent();
            //populate();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbTitle.Text == "" || tbId.Text == "" || tbQty.Text == "" || tbPrecio.Text == "" || cboPeriodo.SelectedIndex == -1 || cboModalidad.SelectedIndex == -1)
            {
                MessageBox.Show("Falta información, favor de completar");
            }
            /*else if (tbId.Text.Length < 7)
            {
                MessageBox.Show("Clave de materia incorrecta, verificar por favor");
            }*/
            else
            {
                try
                {
                    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase("Cuadernillos_ITECI");
                    var collection = database.GetCollection<BsonDocument>(cboModalidad.Text);
                    var doc = new BsonDocument
                    {
                        { "id", tbId.Text.Trim().ToUpper()},
                        { "materia", tbTitle.Text.Trim().ToUpper() },
                        { "qty", tbQty.Text.Trim() },
                        { "periodo", cboPeriodo.Text.Trim() },
                        { "modalidad", cboModalidad.Text },
                        { "price", tbPrecio.Text }
                    };
                    collection.InsertOne(doc);
                    MessageBox.Show("¡Items guardados!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
             }

        }

        private void Populate() 
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>(cboFiltroModalidad.Text);
            var filter = Builders<BsonDocument>.Filter.Eq("modalidad", cboFiltroModalidad.Text);
            var cursor = collection.Find(filter);
            var list = cursor.ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("materia", typeof(string));
            dt.Columns.Add("qty", typeof(string));
            dt.Columns.Add("periodo", typeof(string));
            dt.Columns.Add("modalidad",typeof(string));
            dt.Columns.Add("precio", typeof(string));
       
            foreach (var item in list) 
            {
                DataRow dr = dt.NewRow();
                for (int i =0; i<dt.Columns.Count; i++) 
                {
                    dr[0] = item["id"];
                    dr[1] = item["materia"];
                    dr[2] = item["qty"];
                    dr[3] = item["periodo"];
                    dr[4] = item["modalidad"];
                    dr[5] = item["price"];
                }
                dt.Rows.Add(dr);
            }

           BookDGV.DataSource = dt;       
        }
        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cboFiltroModalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Populate();
        }
        private void Reset() 
        {
            tbId.Text = "";
            tbPrecio.Text = "";
            tbQty.Text = "";
            tbTitle.Text = "";
            cboModalidad.SelectedIndex = -1;
            cboPeriodo.SelectedIndex = -1;
            cboFiltroModalidad.SelectedIndex = -1;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Reset();
        }
        int key = 0;
        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dim row as DataGridViewRow = BookDGV.Rows(e.NewSelectedIndex);
            //int a = BookDGV.Rows.Count();
            //string FirstValue = BookDGV.SelectedRows[0].Cells[0].Value.ToString();
            //Console.WriteLine(FirstValue);
            //Console.WriteLine(BookDGV.SelectedRows[0].Cells[0].Value.ToString());
            if(e.ColumnIndex == 0 && e.RowIndex >= 0) 
            {
                tbTitle.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
                tbQty.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
                cboPeriodo.SelectedItem = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
                cboModalidad.SelectedItem = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
                tbPrecio.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            }
            if (tbTitle.Text == "") 
            { 
                key=0;
            }
            else 
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

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
                    var collection = database.GetCollection<BsonDocument>(cboModalidad.Text);
                    var doc = new BsonDocument
                    {
                        { "id", tbId.Text.Trim().ToUpper()},
                        { "materia", tbTitle.Text.Trim().ToUpper() },
                        { "qty", tbQty.Text.Trim() },
                        { "periodo", cboPeriodo.Text.Trim() },
                        { "modalidad", cboModalidad.Text },
                        { "price", tbPrecio.Text }
                    };
                    collection.DeleteOne(doc);
                    MessageBox.Show("¡Items eliminados!");
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show(ex.Message);
                }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tbTitle.Text == "" || tbId.Text == "" || tbQty.Text == "" || tbPrecio.Text == "" || cboPeriodo.SelectedIndex == -1 || cboModalidad.SelectedIndex == -1)
            {
                MessageBox.Show("Falta información, favor de completar");
            }
            else if (tbId.Text.Length < 7)
            {
                MessageBox.Show("Clave de materia incorrecta, verificar por favor");
            }
            else
            {
                try
                {
                    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb2:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase("Cuadernillos_ITECI");
                    var collection = database.GetCollection<BsonDocument>(cboModalidad.Text);
                    var doc = new BsonDocument
                    {
                        { "id", tbId.Text.Trim().ToUpper()},
                        { "materia", tbTitle.Text.Trim().ToUpper() },
                        { "qty", tbQty.Text.Trim() },
                        { "periodo", cboPeriodo.Text.Trim() },
                        { "modalidad", cboModalidad.Text },
                        { "price", tbPrecio.Text }
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void lbUsers_Click(object sender, EventArgs e)
        {
            Users Obj = new Users();
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
