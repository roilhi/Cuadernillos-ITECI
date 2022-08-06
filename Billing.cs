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
    public partial class Billing : Form
    {

        public Billing()
        {
            DateTime thisDay = DateTime.Today;
            string hoy = thisDay.ToString("d");
            InitializeComponent();
            MessageBox.Show("Escanea el número de serie del cuadernillo");
            tbSerie.Select();
            tbSerie.Focus();
            lbFechaVenta.Text = "Fecha: " + hoy;
            lbSeller.Text = Login.UserName;
        }


        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public bool serialExists(string serie) 
        {
            bool result = false;
            // string claveMateria = tbSerie.Text.Substring(2, 4);
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>("ventas");
            var filter = Builders<BsonDocument>.Filter.Eq("serie", serie);
            var BsonDoc = collection.Find(filter).FirstOrDefault();
            try
            {
                if(BsonDoc != null) 
                { 
                    result = true;
                }
            }
            catch 
            {
                MessageBox.Show("Error en la conexión a la base de datos");
            }
            return result;
        }

        private void tbSerie_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!serialExists(tbSerie.Text))
                {
                    MessageBox.Show("Selecciona la modalidad");
                    cboModalidad.Select();
                    cboModalidad.Focus();
                }
                else 
                {
                    MessageBox.Show("Error, el número de serie ya fue vendido, vuelva a escanear");
                    tbSerie.Text = "";
                    tbSerie.Select();
                    tbSerie.Focus();
                }
            }
        }
        private void updateStock() 
        {
            int newStock;
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>(cboModalidad.Text);
            if (cboModalidad.Text == "semiescolarizado")
            {
                string claveMateria = tbSerie.Text.Substring(2, 4);
                var filter = Builders<BsonDocument>.Filter.Eq("id", claveMateria);
                //var result = await collection.ReplaceOne(filter, update);
                var BsonDoc = collection.Find(filter).FirstOrDefault();
                try
                {
                    string OldQty = BsonDoc["qty"].ToString();
                    newStock = Convert.ToInt32(OldQty) - 1;
                    var update = Builders<BsonDocument>.Update.Set("qty", Convert.ToString(newStock));
                    var result = collection.UpdateOne(filter, update);
                }
                catch
                {
                    MessageBox.Show("Error en la conexión a la base de datos");
                }
            }
            else if (cboModalidad.Text == "escolarizado" || cboModalidad.Text == "cuatrimestral (SQ)")
            {
                string claveMateria = tbSerie.Text.Substring(2, 7);
                var filter = Builders<BsonDocument>.Filter.Eq("id", claveMateria);
                var BsonDoc = collection.Find(filter).FirstOrDefault();

                try
                {
                    string OldQty = BsonDoc["qty"].ToString();
                    newStock = Convert.ToInt32(OldQty) - 1;
                    var update = Builders<BsonDocument>.Update.Set("qty", Convert.ToString(newStock));
                    var result = collection.UpdateOne(filter, update);
                }
                catch
                {
                    MessageBox.Show("Error en la conexión a la base de datos");
                }
            }
        }
        private void cboModalidad_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboModalidad.SelectedIndex == -1) 
            {
                MessageBox.Show("Por favor selecciona un tipo de modalidad");
            }
            else if (cboModalidad.Text == "semiescolarizado") 
            {
                string claveMateria = tbSerie.Text.Substring(2, 4);
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                var client = new MongoClient(settings);
                var database = client.GetDatabase("Cuadernillos_ITECI");
                var collection = database.GetCollection<BsonDocument>("claves");
                var filter = Builders<BsonDocument>.Filter.Eq("clave", claveMateria);
                var BsonDoc = collection.Find(filter).FirstOrDefault();
                try
                {
                    string NombreMateria = BsonDoc["nombre"].AsString;
                    string Periodo = BsonDoc["periodo"].AsString;
                    tbTitle.Text = NombreMateria;
                    tbPeriodo.Text = Periodo;
                }
                catch 
                {
                    MessageBox.Show("Número de serie incorrecto, favor de verificar");
                }
            }
            else if (cboModalidad.Text == "escolarizado" || cboModalidad.Text == "cuatrimestral (SQ)") 
            {
                string claveMateria = tbSerie.Text.Substring(2, 7);
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                var client = new MongoClient(settings);
                var database = client.GetDatabase("Cuadernillos_ITECI");
                var collection = database.GetCollection<BsonDocument>("claves");
                var filter = Builders<BsonDocument>.Filter.Eq("clave", claveMateria);
                var BsonDoc = collection.Find(filter).FirstOrDefault();
                try
                {
                    string NombreMateria = BsonDoc["nombre"].AsString;
                    string Periodo = BsonDoc["periodo"].AsString;
                    tbTitle.Text = NombreMateria;
                    tbPeriodo.Text = Periodo;
                }
                catch
                {
                    MessageBox.Show("Número de serie incorrecto, favor de verificar");
                }
            }
            MessageBox.Show("Captura el correo electrónico del alumno");
            tbMail.Select();
            tbMail.Focus();
        }
        //int n = 0;
        int GrdTotal = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbMail.Text == "" || tbSerie.Text == "" || cboModalidad.SelectedIndex == -1)
            {
                MessageBox.Show("No es posible agregar la venta, la información está incompleta");
            }
            else
            {
                if (!serialExists(tbSerie.Text))
                {
                    try
                    {
                        var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
                        var client = new MongoClient(settings);
                        var database = client.GetDatabase("Cuadernillos_ITECI");
                        var collection = database.GetCollection<BsonDocument>("ventas");
                        var doc = new BsonDocument
                    {
                        { "serie", tbSerie.Text.Trim().ToUpper()},
                        { "materia", tbTitle.Text.Trim()},
                        { "periodo", tbPeriodo.Text.Trim()},
                        { "modalidad", cboModalidad.Text },
                        { "email", tbMail.Text + "@itecipreparatoria.edu.mx" },
                        { "vendedor", lbSeller.Text.Trim() },
                        { "fecha", DateTime.Now.ToString() }
                    };
                        collection.InsertOne(doc);
                        MessageBox.Show("¡Items guardados!");
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(billDGV);
                        //n++;
                        int qty = 1, precio = 100;
                        int total = qty * precio;
                        newRow.Cells[0].Value = tbSerie.Text;
                        newRow.Cells[1].Value = tbTitle.Text;
                        newRow.Cells[2].Value = 1;
                        newRow.Cells[3].Value = 100;
                        billDGV.Rows.Add(newRow);
                        updateStock();
                        GrdTotal = GrdTotal + total;
                        lbTotal.Text = "Total: " + GrdTotal;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else 
                {
                    MessageBox.Show("Ya has vendido este número de serie, presiona reset y realiza un nuevo registro");
                }
            }
        }
        private void Reset() 
        {
            tbTitle.Text = "";
            tbSerie.Text = "";
            tbTitle.Text = "";
            tbMail.Text = "";
            tbPeriodo.Text = "";
            cboModalidad.SelectedIndex = -1;
            tbSerie.Select();
            tbSerie.Focus();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnRecibo_Click(object sender, EventArgs e)
        {
            //printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600); original
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 1240, 574); // A6
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK) 
            {
                printDocument1.Print();
            }
        }
        int prodprice, tottal, pos = 60;

        private void lbLogout_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        string prodName, prodid;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Cuadernillos ITECI", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(80, pos));
            e.Graphics.DrawString("ID           Producto           Cantidad           Precio", new Font("Century Gothic",10,FontStyle.Bold), Brushes.Blue, new Point(26, pos+40));
            foreach(DataGridViewRow row in billDGV.Rows) 
            { 
                prodid = "" + row.Cells["Column1"].Value;
                prodName = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                tottal = Convert.ToInt32(row.Cells["Column4"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Century Gothic",8, FontStyle.Bold), Brushes.Blue, new Point(26,pos+60));
                //e.Graphics.DrawString("" + prodName, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString("" + prodName, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(136, pos+60));
                //e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(526, pos+60));
                //e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(726, pos+60));
                pos = pos + 20;
            }
            e.Graphics.DrawString("**** Total a cuenta ****: " + GrdTotal, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Crimson, new Point(60, pos + 50));
            //e.Graphics.DrawString("** Tienda Cuadernillos ITECI ****", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
            e.Graphics.DrawString("Cliente: " + tbMail.Text + "@itecipreparatoria.edu.mx", new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
            e.Graphics.DrawString("Fecha de venta: " + DateTime.Today.ToString("d") , new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 105));
            e.Graphics.DrawString("Nombre del venedor: " + lbSeller.Text, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 125));
            billDGV.Rows.Clear();
            billDGV.Refresh();
            pos = 100;
            GrdTotal = 0;
        }
    }
}
