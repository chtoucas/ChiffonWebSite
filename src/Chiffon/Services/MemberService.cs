namespace Chiffon.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Messaging;
    using Narvalo;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class MemberService/*Impl*/ : IMemberService
    {
        const int RandomPasswordLength_ = 7;
        const string RandomPasswordLetters_ = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const string RandomPasswordNumbers_ = "23456789";

        readonly IMessenger _messenger;
        readonly Queries _queries;

        public MemberService(ChiffonConfig config, IMessenger messenger)
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(messenger, "messenger");

            _messenger = messenger;

            _queries = new Queries(config);
        }

        public event EventHandler<MemberCreatedEventArgs> MemberCreated;

        #region IMemberService

        public Outcome<Member> RegisterMember(RegisterMemberQuery query)
        {
            // 1. On vérifie que l'addresse email n'est pas déjà prise.

            // TODO: Fusionner cette méthode avec celle qui suit pour éviter deux appels DB.
            if (_queries.IsEmailAlreadyTaken(query.Email)) {
                return Outcome<Member>.Failure(SR.MemberService_EmailAlreadyTaken);
            }

            // 2. Génération d'un nouveau mot de passe.

            var password = CreateRandomPassword_(RandomPasswordLength_);

            // 3. Création du compte en base de données.

            var member = _queries.RegisterMember(query, password);

            // 4. On enclenche tout de suite l'événement (au cas où les opérations suivantes échouent).

            OnMemberCreated_(new MemberCreatedEventArgs(member));

            // 5. Envoi des notifications.

            _messenger.Publish(new NewMemberMessage {
                CompanyName = query.CompanyName,
                MemberAddress = member.EmailAddress,
                Password = password
            });

            return Outcome.Success(member);
        }

        // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.
        public Maybe<Member> MayLogOn(string email, string password)
        {
            return Maybe.Create(_queries.GetMember(email, password));
        }

        #endregion

        // Cf. http://madskristensen.net/post/Generate-random-password-in-C.aspx
        // Cf. http://stackoverflow.com/questions/54991/generating-random-passwords
        static string CreateRandomPassword_(int passwordLength)
        {
            var chars = new char[passwordLength];
            var rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < passwordLength; i++) {
                if (useLetter) {
                    chars[i] = RandomPasswordLetters_[rd.Next(0, RandomPasswordLetters_.Length)];
                    useLetter = false;
                }
                else {
                    chars[i] = RandomPasswordNumbers_[rd.Next(0, RandomPasswordNumbers_.Length)];
                    useLetter = true;
                }

            }

            return new String(chars);
        }

        void OnMemberCreated_(MemberCreatedEventArgs e)
        {
            EventHandler<MemberCreatedEventArgs> localHandler = MemberCreated;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        #region TODO: À basculer correctement dans Chiffon.Data.

        private class Queries
        {
            readonly ChiffonConfig _config;

            public Queries(ChiffonConfig config)
            {
                Requires.NotNull(config, "config");

                _config = config;
            }

            protected string ConnectionString { get { return _config.SqlConnectionString; } }

            public Member RegisterMember(RegisterMemberQuery query, string password)
            {
                using (var cnx = new SqlConnection(ConnectionString)) {
                    using (var cmd = new SqlCommand()) {
                        cmd.CommandText = "usp_NewMember";
                        cmd.Connection = cnx;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameterCollection p = cmd.Parameters;
                        p.Add("@email_address", SqlDbType.NVarChar).Value = query.Email;
                        p.Add("@firstname", SqlDbType.NVarChar).Value = query.FirstName;
                        p.Add("@lastname", SqlDbType.NVarChar).Value = query.LastName;
                        p.Add("@company_name", SqlDbType.NVarChar).Value = query.CompanyName;
                        p.Add("@password", SqlDbType.NVarChar).Value = password;
                        p.Add("@newsletter", SqlDbType.Bit).Value = query.NewsletterChecked;

                        cnx.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return new Member(query.Email, query.FirstName, query.LastName);
            }

            public Member GetMember(string email, string password)
            {
                Member member = null;

                using (var cnx = new SqlConnection(ConnectionString)) {
                    using (var cmd = new SqlCommand()) {
                        cmd.CommandText = "usp_GetContactByPublicKey";
                        cmd.Connection = cnx;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameterCollection p = cmd.Parameters;
                        p.Add("@email_address", SqlDbType.NVarChar).Value = email;
                        p.Add("@password", SqlDbType.NVarChar).Value = password;

                        cnx.Open();
                        using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                            if (rdr.Read()) {
                                member = new Member(email, rdr.GetString("firstname"), rdr.GetString("lastname"));
                            }
                        }
                    }
                }

                return member;
            }

            public bool IsEmailAlreadyTaken(string email)
            {
                bool exists = false;

                using (var cnx = new SqlConnection(ConnectionString)) {
                    using (var cmd = new SqlCommand()) {
                        cmd.CommandText = "usp_GetPublicKeyByEmailAddress";
                        cmd.Connection = cnx;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameterCollection p = cmd.Parameters;
                        p.Add("@email_address", SqlDbType.NVarChar).Value = email;

                        cnx.Open();
                        using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                            if (rdr.Read()) {
                                exists = true;
                            }
                        }
                    }
                }

                return exists;
            }
        }

        #endregion
    }
}