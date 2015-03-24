namespace Chiffon.Services
{
    using System;

    using Chiffon.Entities;
    using Narvalo.Fx;

    /// <summary>
    /// Service fournissant les opérations permettant de gérer le compte d'un membre. 
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// Evénement déclenché lorsqu'un membre a été créé.
        /// </summary>
        event EventHandler<MemberCreatedEventArgs> MemberCreated;

        /// <summary>
        /// Tente de vérifier les informations de connexion pour un membre.
        /// </summary>
        /// <param name="email">L'adresse e-mail du membre.</param>
        /// <param name="password">Le mot de passe du membre.</param>
        /// <returns>
        /// Retourne une monade contenant le membre si e-mail et mot de passe correspondent. 
        /// </returns>
        Maybe<Member> MayLogOn(string email, string password);

        /// <summary>
        /// Crée un nouveau membre.
        /// </summary>
        /// <param name="request">Informations nécessaires à la création d'un membre.</param>
        VoidOrBreak RegisterMember(RegisterMemberRequest request);
    }
}
