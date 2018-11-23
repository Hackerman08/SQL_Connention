using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int databesa;
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        DataTable dt;

        MySqlConnection con = new MySqlConnection("Server = localhost; Database = regisztracio; Uid = root; Pwd = ;");
        public Form1()
        {
            InitializeComponent();
            displayData();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nev = textBox1.Text;
            string jelszo = textBox2.Text;
            DateTime regdatum = dateTimePicker1.Value;
            using(var conn=new MySqlConnection("Server = localhost; Database = regisztracio; Uid = root; Pwd = ;"))
            {
                conn.Open();
                var ellenÖrzes = conn.CreateCommand();
                ellenÖrzes.CommandText = "SELECT COUNT(*) FROM `felhasznalo` WHERE nev=@nev ";
                ellenÖrzes.Parameters.AddWithValue("@nev", nev);
                var darab = (long)ellenÖrzes.ExecuteScalar();
                if (darab != 0)
                {
                    MessageBox.Show("Van már!!!");
                    return;
                }
               



                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO `felhasznalo`( `nev`, `jelszo`, `regdatum`) VALUES (@nev,@jelszo,@regdatum)";
                command.Parameters.AddWithValue("@nev", nev);
                command.Parameters.AddWithValue("@jelszo", jelszo);
                command.Parameters.AddWithValue("@regdatum", regdatum);
                int erintettSorok = command.ExecuteNonQuery();
                MessageBox.Show("felvéve!!!!");
                displayData();
                conn.Close();
                Clear();
                
            }
        }
        public void displayData()
        {
            con.Open();
            adpt = new MySqlDataAdapter("select * from felhasznalo", con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

       

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            databesa = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string nev = textBox1.Text;
            string jelszo = textBox2.Text;
            DateTime regdatum = dateTimePicker1.Value;
            
            using (var conn = new MySqlConnection("Server = localhost; Database = regisztracio; Uid = root; Pwd = ;"))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "UPDATE felhasznalo SET`id`=@databesa,`nev`=@nev,`jelszo`=@jelszo,`regdatum`=@regdatum  WHERE id=@databesa";
                command.Parameters.AddWithValue("@nev", nev);
                command.Parameters.AddWithValue("@jelszo", jelszo);
                command.Parameters.AddWithValue("@regdatum", regdatum);
                command.Parameters.AddWithValue("@databesa", databesa);
                int erintettSorok = command.ExecuteNonQuery();
                MessageBox.Show("Módósítva!!!!");
                displayData();
                conn.Close();
                Clear();
            }
               




        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            using (var conn = new MySqlConnection("Server = localhost; Database = regisztracio; Uid = root; Pwd = ;"))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "DELETE FROM `felhasznalo` WHERE id=@databesa";
                
                command.Parameters.AddWithValue("@databesa", databesa);
                int erintettSorok = command.ExecuteNonQuery();
                MessageBox.Show("Törölve!!!!");
                displayData();
                conn.Close();
                Clear();
            }
        }
    }
}
