namespace Chiffon.Data
{
    /// <summary>
    /// Représente l'ensemble des opérations permettant l'accès en écriture au stockage persistant.
    /// </summary>
    public interface IDbCommands
    {
        /// <summary>
        /// Sauvegarde les informations concernant un nouveau membre.
        /// </summary>
        /// <param name="parameters">Informations relatives au nouveau membre.</param>
        /// <returns>Retourne l'entité représentant le membre qui vient d'être sauvegardé.</returns>
        void NewMember(NewMemberParameters parameters);
    }
}
