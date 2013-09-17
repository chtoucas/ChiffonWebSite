namespace Chiffon.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;

    public class MemberService/*Impl*/ : IMemberService
    {
        readonly ChiffonConfig _config;

        public MemberService(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        // FIXME:

        string ConnectionString_ { get { return _config.SqlConnectionString; } }

        #region IMemberService

        // FIXME: c'est un peu laxiste et franchement dangereux...
        public string LogOn(string publicKey)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.

            string userName = String.Empty;

            using (var cnx = new SqlConnection(ConnectionString_)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_GetContactByPublicKey";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@public_key", SqlDbType.NVarChar).Value = publicKey;

                    cnx.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            userName = rdr.GetString("firstname") + " " + rdr.GetString("lastname");
                        }
                    }
                }
            }

            return userName;
        }

        #endregion
    }
}