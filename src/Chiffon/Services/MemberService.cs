namespace Chiffon.Services
{
    using System;
    using Chiffon.Data;
    using Chiffon.Entities;
    using Chiffon.Infrastructure.Messaging;
    using Narvalo;
    using Narvalo.Fx;

    public class MemberService/*Impl*/ : IMemberService
    {
        const int PasswordLength_ = 7;
        const string PasswordLetters_ = "abcdefghijkmnpqrstuvwxyz";
        const string PasswordNumbers_ = "23456789";

        readonly IMessenger _messenger;
        readonly IReadOnlyQueries _queries;
        readonly IReadWriteQueries _inserts;

        public MemberService(IReadOnlyQueries queries, IReadWriteQueries inserts, IMessenger messenger)
        {
            Requires.NotNull(queries, "queries");
            Requires.NotNull(inserts, "inserts");
            Requires.NotNull(messenger, "messenger");

            _messenger = messenger;
            _queries = queries;
            _inserts = inserts;
        }

        public event EventHandler<MemberCreatedEventArgs> MemberCreated;

        #region IMemberService

        public Outcome<Member> RegisterMember(RegisterMemberQuery query)
        {
            // 1. Nettoyage des données reçues.

            query.Normalize();

            // 2. On vérifie que l'addresse email n'est pas déjà prise.

            // TODO: Fusionner cette méthode avec celle qui suit pour éviter deux appels DB.
            var password = _queries.GetPassword(query.Email);

            if (!String.IsNullOrEmpty(password)) {
                return Outcome<Member>.Failure(SR.MemberService_EmailAlreadyTaken);
            }

            // 3. Génération d'un nouveau mot de passe.

            password = CreatePassword_();

            // 4. Création du compte en base de données.

            var model = new NewMemberModel {
                CompanyName = query.CompanyName,
                Email = query.Email,
                FirstName = query.FirstName,
                LastName = query.LastName,
                NewsletterChecked = query.NewsletterChecked,
                Password = password,
            };

            // TODO: Encrypter les mots de passe.
            var member = _inserts.NewMember(model);

            // 5. On enclenche tout de suite l'événement (au cas où les opérations suivantes échouent).

            OnMemberCreated_(new MemberCreatedEventArgs(member));

            // 6. Envoi des notifications.

            _messenger.Publish(new NewMemberMessage {
                CompanyName = query.CompanyName,
                EmailAddress = member.EmailAddress,
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
        static string CreatePassword_()
        {
            // TODO: Changer le comportement suivant.
            // Pour le moment, on génére des mots de passe assez faibles mais 
            // cela n'a que peu d'importance car ils ont une durée de vie assez courte.

            var chars = new char[PasswordLength_];
            var rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < PasswordLength_; i++) {
                if (useLetter) {
                    chars[i] = PasswordLetters_[rd.Next(0, PasswordLetters_.Length)];
                    useLetter = false;
                }
                else {
                    chars[i] = PasswordNumbers_[rd.Next(0, PasswordNumbers_.Length)];
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
    }
}