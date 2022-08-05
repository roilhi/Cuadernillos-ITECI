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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private bool rightUser(string userName) 
        { 
            bool isUser = false;
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>("usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("user", userName);
            var BsonDoc = collection.Find(filter).FirstOrDefault();
            try
            {
                if (BsonDoc != null)
                {
                    isUser = true;
                }
            }
            catch
            {
                MessageBox.Show("Error con la conexión a la base de datos");
            }
            return isUser; 
        }
        private bool rightPassword(string userPassword)
        {
            bool isUser = false;
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>("usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("password", userPassword);
            var BsonDoc = collection.Find(filter).FirstOrDefault();
            try
            {
                if (BsonDoc != null)
                {
                    isUser = true;
                }
            }
            catch
            {
                MessageBox.Show("Error con la conexión a la base de datos");
            }
            return isUser;
        }
        private string GetWholeName(string userName) 
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://itecidb:iteci2021@clusteriteci.rnxhk.mongodb.net/Prepa_ITECI_Ens?connect=replicaSet");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Cuadernillos_ITECI");
            var collection = database.GetCollection<BsonDocument>("usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("user", userName);
            var BsonDoc = collection.Find(filter).FirstOrDefault();
            string WholeName = BsonDoc["wholeName"].ToString();
            return WholeName;
        }
        public static string UserName = "";
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if ( rightUser(tbUser.Text) && rightPassword(tbPassword.Text) ) 
            {
                UserName = GetWholeName(tbUser.Text);
                Billing obj = new Billing();
                obj.Show();
                this.Hide();
            }
            else 
            {
                MessageBox.Show("Usuario o contraseña incorrectos, favor de verificar");
            }
        }

        private void lblAdmin_Click(object sender, EventArgs e)
        {
            AdminLogin Obj = new AdminLogin();
            Obj.Show();
            this.Hide();
        }
    }
}
