using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema
{
    public partial class AddClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BDCinemaConnetion"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                int countNord = GetCount(conn, "nord");
                int countEst = GetCount(conn, "est");
                int countSud = GetCount(conn, "sud");
                int countRidNord = GetRidCount(conn, "nord");
                int countRidEst = GetRidCount(conn, "est");
                int countRidSud = GetRidCount(conn, "sud");
                lblNord.Text = countNord.ToString();
                lblEst.Text = countEst.ToString();
                lblSud.Text = countSud.ToString();
                lblRidottiEst.Text = countRidEst.ToString();
                lblRidottiNord.Text = countRidNord.ToString();
                lblRidottiSud.Text = countRidSud.ToString();
            }
            catch
            {
                Response.Write("qualcosa è andato storto");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }


        }

        protected void btnAcquista_Click(object sender, EventArgs e)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["BDCinemaConnetion"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string nome = txtNome.Text;
                string cognome = txtCognome.Text;
                string sala = rdbSala.Text;
                string tipoBiglietto = rdbTipoBiglietto.Text;



                int countSalaSelected = GetCount(conn, sala);

                


                SqlCommand insertClient = new SqlCommand($"Insert Into Utenti (Nome,Cognome,Sala,TipoBiglietto) values ('{nome}','{cognome}','{sala}','{tipoBiglietto}') ", conn);
                if(countSalaSelected <= 120) 
                { 
                   int affectedRow = insertClient.ExecuteNonQuery();
                    if (affectedRow !=0)
                    {
                        Response.Write("Biglietto acquistato");

                       
                    }
                    else
                    {
                              Response.Write("Non è stato possibile");
                    }
                }
                else
                {
                    Response.Write($"Nella sala {sala} sono esauriti i posti");
                }

            }
            catch
            {
                Response.Write("qualcosa è andato storto");
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

           

        }

        private int GetCount(SqlConnection conn, string sala)
        {
            using (SqlCommand countSala = new SqlCommand("select count(*) from Utenti where Sala=@sala", conn))
            {
                countSala.Parameters.AddWithValue("@sala", sala);
                return (int)countSala.ExecuteScalar();
            }
        }
        private int GetRidCount(SqlConnection conn, string sala)
        {
            using (SqlCommand countSala = new SqlCommand("select count(*) from Utenti where Sala=@sala and TipoBiglietto='ridotto'", conn))
            {
                countSala.Parameters.AddWithValue("@sala", sala);
                return (int)countSala.ExecuteScalar();
            }
        }

    }
}