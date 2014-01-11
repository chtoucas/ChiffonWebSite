namespace Chiffon.Data
{
    using Chiffon.Entities;

    /// <summary>
    /// Représente l'ensemble des opérations permettant l'accès en écriture au stockage persistant.
    /// </summary>
    public interface IWriteQueries
    {
        /// <summary>
        /// Sauvegarde les informations concernant un nouveau membre.
        /// </summary>
        /// <param name="parameters">Informations relatives au nouveau membre.</param>
        /// <returns>Retourne l'entité représentant le membre qui vient d'être sauvegardé.</returns>
        Member NewMember(NewMemberParameters parameters);
    }
}
