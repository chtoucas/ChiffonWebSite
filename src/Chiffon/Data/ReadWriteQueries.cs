namespace Chiffon.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class ReadWriteQueries : IReadWriteQueries
    {
        readonly ChiffonConfig _config;

        public ReadWriteQueries(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        #region IReadWriteQueries

        public Member NewMember(NewMemberModel model)
        {
            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_NewMember";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@email_address", SqlDbType.NVarChar).Value = model.Email;
                    p.Add("@firstname", SqlDbType.NVarChar).Value = model.FirstName;
                    p.Add("@lastname", SqlDbType.NVarChar).Value = model.LastName;
                    p.Add("@company_name", SqlDbType.NVarChar).Value = model.CompanyName;
                    p.Add("@password", SqlDbType.NVarChar).Value = model.Password;
                    p.Add("@newsletter", SqlDbType.Bit).Value = model.NewsletterChecked;

                    cnx.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return new Member(model.Email, model.FirstName, model.LastName);
        }

        #endregion
    }

}
