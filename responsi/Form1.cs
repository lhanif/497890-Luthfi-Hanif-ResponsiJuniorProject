using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace responsi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
        }
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=usti;Database=responsi";
        string sql = null;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private int id_dep;
        private object r;

        private void tbKaryawan_TextChanged(object sender, EventArgs e)
        {
        }

        private void LoadData()
        {
            conn = new NpgsqlConnection(connectionString);
            conn.Open();
            dataGridView1.DataSource = null;
            //sql = "select * from karyawan";
            sql = "select * from st_select()";
            cmd = new NpgsqlCommand(sql, conn);
            dt = new DataTable();
            NpgsqlDataReader rd = cmd.ExecuteReader();
            dt.Load(rd);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new NpgsqlConnection(connectionString);

                conn.Open();
                sql = @"select * from st_insert(:_nama, :_idDepartemen)";

                
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbKaryawan.Text);
                cmd.Parameters.AddWithValue("_idDepartemen", 0);


                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Berhasil Diinputkan","Nice",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Tidak Berhasil Diinputkan", "Retry", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void drDepartemen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(drDepartemen.SelectedIndex == 0)
            {
                id_dep = 0;
            }
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectionString);
            conn.Open();
            sql = "select * FROM st_edit(:_id,_nama,:_id_dep)";

            cmd = new NpgsqlCommand(sql, conn);

            //cmd.Parameters.AddWithValue("_id", Row.Cells["id"].Value);
            cmd.Parameters.AddWithValue("_nama", tbKaryawan.Text);
            cmd.Parameters.AddWithValue("_id_dep", drDepartemen.SelectedIndex);

            if ((int)cmd.ExecuteScalar() == 200)
            {
                MessageBox.Show("Data Berhasil Diinputkan", "Nice", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                Row = dataGridView1.Rows[e.RowIndex];
                tbKaryawan.Text = Row.Cells["nama_karyawan"].Value.ToString();

            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new NpgsqlConnection(connectionString);

                conn.Open();
                sql = @"select * from ST_Delete(_id)";

                cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("_id", r.Cells["id"].Value.ToString());
                
            }
            catch
            {

            }
        }
    }
}
