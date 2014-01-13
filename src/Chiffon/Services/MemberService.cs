namespace Chiffon.Services
{
    using System;
    using Chiffon.Persistence;
    using Chiffon.Domain;
    using Chiffon.Infrastructure.Messaging;
    using Chiffon.Internal;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Fx;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Services.IMemberService"/>.
    /// </summary>
    public class MemberService/*Impl*/ : IMemberService
    {
        const int PasswordLength_ = 7;
        const string PasswordLetters_ = "abcdefghijkmnpqrstuvwxyz";
        const string PasswordNumbers_ = "23456789";

        readonly IDbCommands _commands;
        readonly IMessenger _messenger;
        readonly IDbQueries _queries;

        public MemberService(IDbQueries queries, IDbCommands commands, IMessenger messenger)
        {
            Requires.NotNull(commands, "commands");
            Requires.NotNull(messenger, "messenger");
            Requires.NotNull(queries, "queries");

            _commands = commands;
            _messenger = messenger;
            _queries = queries;
        }

        public event EventHandler<MemberCreatedEventArgs> MemberCreated;

        #region IMemberService

        public Maybe<Error> MayRegisterMember(RegisterMemberRequest request)
        {
            Requires.NotNull(request, "request");

            // 1. On vérifie que l'addresse email n'est pas déjà prise.

            var password = _queries.GetPassword(request.Email);

            if (!String.IsNullOrEmpty(password)) {
                return Maybe.Create(Error.Create(SR.MemberService_EmailAlreadyTaken));
            }

            // 2. Génération d'un nouveau mot de passe.

            password = CreatePassword_();

            // 3. Création du compte en base de données.

            var cmdParameters = Mapper.Map(request, EncryptPassword_(password));

            _commands.NewMember(cmdParameters);

            var member = MemberFactory.NewMember(request.Email, request.FirstName, request.LastName);

            // 4. On enclenche tout de suite l'événement (au cas où les opérations suivantes échouent).

            OnMemberCreated_(new MemberCreatedEventArgs(member));

            // 5. Envoi des notifications.

            if (request.Recipients != MessageRecipients.None) {
                _messenger.Publish(new NewMemberMessage {
                    CompanyName = request.CompanyName,
                    EmailAddress = member.EmailAddress,
                    Password = password,
                    Recipients = request.Recipients,
                });
            }

            return Maybe<Error>.None;
        }

        public Maybe<Member> MayLogOn(string email, string password)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.
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

        static string EncryptPassword_(string password)
        {
            // FIXME: Utiliser BountyCastleOrg. 
            return password;
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