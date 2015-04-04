namespace Chiffon.Services
{
    using System;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Persistence;
    using Narvalo;
    using Narvalo.Fx;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Services.IMemberService"/>.
    /// </summary>
    public sealed class MemberService/*Impl*/ : IMemberService
    {
        private const int PASSWORD_LENGTH = 7;
        private const string PASSWORD_LETTERS = "abcdefghijkmnpqrstuvwxyz";
        private const string PASSWORD_NUMBERS = "23456789";

        private readonly IDbCommands _commands;
        private readonly IMessenger _messenger;
        private readonly IDbQueries _queries;

        public MemberService(IDbQueries queries, IDbCommands commands, IMessenger messenger)
        {
            Require.NotNull(commands, "commands");
            Require.NotNull(messenger, "messenger");
            Require.NotNull(queries, "queries");

            _commands = commands;
            _messenger = messenger;
            _queries = queries;
        }

        public event EventHandler<MemberCreatedEventArgs> MemberCreated;

        public VoidOrBreak RegisterMember(RegisterMemberRequest request)
        {
            Require.NotNull(request, "request");

            // 1. On vérifie que l'addresse email n'est pas déjà prise.

            var password = _queries.GetPassword(request.Email);

            if (!String.IsNullOrEmpty(password))
            {
                return VoidOrBreak.Break(Strings.MemberService_EmailAlreadyTaken);
            }

            // 2. Génération d'un nouveau mot de passe.

            password = CreatePassword_();

            // 3. Création du compte en base de données.

            var cmdParameters = MapMember_(request, EncryptPassword_(password));

            _commands.NewMember(cmdParameters);

            var member = new Member {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            // 4. On enclenche tout de suite l'événement (au cas où les opérations suivantes échouent).

            OnMemberCreated_(new MemberCreatedEventArgs(member));

            // 5. Envoi des notifications.

            if (request.Recipients != MessageRecipients.None)
            {
                _messenger.Publish(new NewMemberMessage {
                    CompanyName = request.CompanyName,
                    EmailAddress = member.EmailAddress,
                    Password = password,
                    Recipients = request.Recipients,
                });
            }

            return VoidOrBreak.Void;
        }

        public Maybe<Member> MayLogOn(string email, string password)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.
            return Maybe.Of(_queries.GetMember(email, password));
        }

        // FIXME: Encrypter le mot de passe. 
        // FIXME: Utiliser un tableau de caractères.
        // Cf. http://madskristensen.net/post/Generate-random-password-in-C.aspx
        // Cf. http://stackoverflow.com/questions/54991/generating-random-passwords
        private static string CreatePassword_()
        {
            // TODO: Changer le comportement suivant.
            // Pour le moment, on génére des mots de passe assez faibles mais 
            // cela n'a que peu d'importance car ils ont une durée de vie assez courte.

            var chars = new char[PASSWORD_LENGTH];
            var rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < PASSWORD_LENGTH; i++)
            {
                if (useLetter)
                {
                    chars[i] = PASSWORD_LETTERS[rd.Next(0, PASSWORD_LETTERS.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = PASSWORD_NUMBERS[rd.Next(0, PASSWORD_NUMBERS.Length)];
                    useLetter = true;
                }
            }

            return new String(chars);
        }

        private static string EncryptPassword_(string password)
        {
            // FIXME: Utiliser BountyCastleOrg. 
            return password;
        }

        private static NewMemberParameters MapMember_(RegisterMemberRequest request, string encryptedPassword)
        {
            Require.NotNull(request, "query");
            Require.NotNullOrEmpty(encryptedPassword, "encryptedPassword");

            return new NewMemberParameters {
                CompanyName = request.CompanyName,
                Email = request.Email,
                EncryptedPassword = encryptedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NewsletterChecked = request.NewsletterChecked,
            };
        }

        private void OnMemberCreated_(MemberCreatedEventArgs e)
        {
            EventHandler<MemberCreatedEventArgs> localHandler = MemberCreated;

            if (localHandler != null)
            {
                localHandler(this, e);
            }
        }
    }
}