CHANGELOG
=========

[2014-10-17] Release 1.13
-------------------------

* Maintenant DefaultSiteMap prend correctement en compte le répertoire
  virtuel de l'application.

[2014-10-16] Release 1.12
-------------------------

* On n'utilise plus une version locale de nodejs/npm.
* Mise à jour de nodejs & npm.
* Mise à jour de toutes les dépendances JS et CSS.
* Désactivation de l'indexation Google et des données SEO.
* Suppression de (presque) toutes les références à pourquelmotifsimone.
* Désactivation de la version multilangue (trop de bugs...).
  Il faudrait revoir ISiteMap (ajouter un paramètre langue à chaque méthode),
  ChiffonEnvironmentResolver et LayoutViewModel (voir les FIXME).

[2013-12-22] Infrastructure
---------------------------

On configure tous les AppPools pour qu'ils ne se recyclent qu'en fonction de la mémoire.
On supprime aussi les timeout qui forcent le redémarrage des AppPools.

[2013-12-07] Release 1.7
------------------------

* Désactivation des modales sauf pour le lien de connexion.
* Dans la page "motif", on enlève la liste des aperçus des motifs dans la même catégorie.
* Pour les fenêtres les plus petites, le texte du pied de page passe en minuscule.
* Si le visiteur est connecté on utilise un texte différent dans la page information.
* Dans les pages des designers, on agrandit un peu le texte et on met l'adresse email plus en évidence.
* Dans la page d'accueil, on crée les watermarks en pure CSS et plus en JS, sinon les anciens
  navigateurs ne les voient pas.

Changements d'ordre technique :
* Internalisation de MvcMailer (en cours).
* Réécriture complète de ChiffonContext (l'ancienne version était incorrecte).
* Multiple corrections relatives à FxCop.
* Mise à jour de tous les projets vers .NET 4.5.1 et VS 2013.
* Mise à jour vers la dernière version de ASP.NET.
* Lorsqu'on travaille dans VS, on garde le choix de la langue en session.
* On scinde le fichier less.

[2013-12-02] Infrastructure
---------------------------

Installation en production de .NET 4.5.1

[2013-11-28] Release 1.6.0.1
----------------------------

* Mise en avant du motif en cours de consultation.
* Affichage de la référence lorsqu'on passe la souris au-dessus d'un motif.

[2013-11-27] Release 1.5.0.1
----------------------------

* On affiche un indicateur visuel afin de signaler le chargement de la page suivante dans
  une pagination infinie.

Changements d'ordre technique :
* Intégration de fastclick.js pour améliorer le temps de réaction des tablettes lors d'un "clic".

[2013-11-26] Release 1.4.0.3
----------------------------

* Réactivation des modales pour les tablettes.
* Nouvelles traductions.
* Pagination infinie de la liste des motifs.

Changements d'ordre technique :
* On sépare les traductions contenant du HTML (vues partielles) et celles
  n'en contenant pas (resources .NET).
* Intégration de jQuery.Waypoints pour la pagination infinie.
* On utilise NProgress pour signaler le début et la fin des appels Ajax.
* Pour les petits écrans, on adapte le fonctionnement des modales.

[2013-11-25] Release 1.3.0.1
----------------------------

* Version stable de la feuille de style pour un site web adaptatif.
* Désactivation de la gestion du bloc info en JavaScript.

[2013-11-21] Release 1.2.0.1
----------------------------

* Version minimale du site en Responsive Design.

Changements d'ordre technique :
* On réorganise la CSS de telle sorte que les blocs similaires soient proches dans le code.

Correctifs :
* Placement du lien "Se connecter". Le lien est positionné en absolu mais il manquait
  une directive "top: 0". Par défaut, le bloc est alors positionné par rapport au bas
  du bloc conteneur, ce qui est l'inverse de ce qu'on souhaite.
* nprogress.css n'était pas inclus dans le build.

[2013-11-19] Release 1.1.1.1
----------------------------

* Nouvelle page d'inscription.
* Utilisation d'un texte différent par langue pour le bouton de connexion.
* À la place de textes, on utilise des images pour les watermarks des vignettes
  dans l'espace connecté.
* On change les watermarks dans la page contact.
* Désactivation des modales pour les tablettes.
* Pendant qu'une modale se charge on affiche un indicateur d'attente.

Changements d'ordre technique :
* Optimisation des images CSS.
* Désactivation de la minification HTML en mode Release.
* Appeler Grunt directement de MSBuild.
* Ne pas inclure les fichiers statiques inutiles: bower_components, less...

Correctifs :
* La couleur utilisée dans les paginations était la couleur par défaut (gris).
* Suppression du package source avant la mise à jour d'un outil.

[2013-11-18] Release 1.1.0.23
-----------------------------

* Ajout d'un lien de connexion dans le menu principal.
* Envoi d'un mail de confirmation à la création d'un compte (sous forme texte pour le moment).
* Passage en modale des pages de création de compte et de connexion.
* Pour faciliter les clics dans les tablettes, on donne plus d'impact physique aux liens.
* Mise à jour du watermark pour Christine Légeret -> Petroleum Blue.

Changements d'ordre technique :
* Pour les navigateurs qui n'implémentent pas les API HTML5,
  on valide les formulaires via jQuery.validate.
* Temporisation lors de la prise en charge de l'évènement "resize" de la fenêtre du navigateur.
* Désactivation des JS pour les navigateurs ne supportant pas ECMA v5.
* Réécriture des CSS à l'aide de lesscss.
* Site séparé pour la distribution des motifs (non utilisé en production).
* Configuration de IIS Application Initialization Module.
* Compilation des JS via UglifyJS.
* Analyse statique des css et des js via csslint, recess et jshint.
* Migration vers Visual Studio 2013.
* Utilitaire de restauration automatique de toutes les dépendances
  extérieures au projet (nodejs, npm, 7-zip, ...).
* Nouveau système de Build : automatisation de la création de packages.
* Utilitaire de déploiement.
* Utilitaire de sauvegarde de la base de données.

[2013-09-19] Release 1.0.26
---------------------------

* Au lieu de préciser manuellement le numéro de version des JS et des CSS, on utilise par défaut
  le numéro de version de l'assemblée.
* Il est dorénavant possible d'utiliser un serveur de statiques.

Correctifs :
* La minification du HTML était trop agressive, vu qu'elle remplacer certains espaces blancs
  par une chaîne vide.

[2013-08-22]
------------

* Création d'un fichier humantxt.
* On n'utilise plus les ViewHelpers pour Google Analytics.

[2013-08-21]
------------

* Code JavaScript permettant à IE de reconnaite les éléments HTML5.
* Utilisation de l'encodeur AntiXSS qui en plus ne pourrit pas les attributs en UTF-8.
* On sépare les configurations de développement et de production.
* Activation de Google Analytics en production.
* Via l'outil d'administration de IIS, création d'une machineKey.

[2013-08-21] Infrastructure
---------------------------

* Déclaration du sous-domaine anglais.
* Configuration de Google Webmaster Tools.
* Configuration de Google Analytics.
* Mise en place du serveur Azure.
