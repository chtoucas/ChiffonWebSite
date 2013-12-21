namespace Chiffon.Services
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class MemberService/*Impl*/ : IMemberService
    {
        readonly ChiffonConfig _config;

        public MemberService(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        string ConnectionString_ { get { return _config.SqlConnectionString; } }

        #region IMemberService

        public Maybe<MemberInfo> MayLogOn(string emailAddress, string password)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.

            MemberInfo memberInfo = null;

            using (var cnx = new SqlConnection(ConnectionString_)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_GetContactByPublicKey";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@email_address", SqlDbType.NVarChar).Value = emailAddress;
                    p.Add("@password", SqlDbType.NVarChar).Value = password;

                    cnx.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            memberInfo = new MemberInfo {
                                FirstName = rdr.GetString("firstname"),
                                LastName = rdr.GetString("lastname")
                            };
                        }
                    }
                }
            }

            return Maybe.Create(memberInfo);
        }

        #endregion
    }
}