CHANGELOG
=========

Version 1.1.1
-------------

* Nouvelle page d'inscription.
* Utilisation d'un texte différent par langue pour le bouton de connexion.
* À la place de textes, on utilise des images pour les watermarks des vignettes
  dans l'espace connecté.
* On change les watermarks dans la page contact.
* Désactivation des modales pour les tablettes.
* Pendant qu'une modale se charge on affiche un indicateur d'attente.

Changements techniques :
* [DEV] Optimisation des images CSS.
* [INFRA] Désactivation de la minification HTML en mode Release.
* [TOOLS] Appeler le processus directement de MSBuild.

Correctifs :
* La couleur utilisée dans les paginations était la couleur par défaut (gris).
* [TOOLS] Suppression du package source avant la mise à jour d'un outil.

Version 1.1.0.23 (2013-11-18)
-----------------------------

* Ajout d'un lien de connexion dans le menu principal.
* Envoi d'un mail de confirmation à la création d'un compte (sous forme texte pour le moment).
* Passage en modale des pages de création de compte et de connexion.
* Pour faciliter les clics dans les tablettes, on donne plus d'impact physique aux liens.
* Mise à jour du watermark pour Christine Légeret -> Petroleum Blue.

Changements techniques :
* [DEV] Pour les navigateurs qui n'implémentent pas les API HTML5,
        on valide les formulaires via jQuery.validate.
* [DEV] Temporisation lors de la prise en charge de l'évènement "resize" de la fenêtre du navigateur.
* [DEV] Réécriture des CSS en utilisant lesscss.
* [DEV] Désactivation des JS pour les navigateurs ne supportant pas ECMA v5.
* [INFRA] Site séparé pour la distribution des motifs (non utilisé en production).
* [INFRA] Configuration de IIS Application Initialization Module.
* [TOOLS] Compilation des JS via UglifyJS.
* [TOOLS] Analyse statique des css et des js via csslint, recess et jshint.
* [TOOLS] Migration vers Visual Studio 2013.
* [TOOLS] Utilitaire de restauration automatique de toutes les dépendances
          extérieures au projet (nodejs, npm, 7-zip, ...).
* [TOOLS] Nouveau système de Build : automatisation de la création de packages.
* [TOOLS] Utilitaire de déploiement.
* [TOOLS] Utilitaire de sauvegarde de la base de données.

Version 1.0.26 (2013-09-19)
---------------------------

* Au lieu de préciser manuellement le numéro de version des JS et des CSS, on utilise par défaut
  le numéro de version de l'assemblée.
* Il est dorénavant possible d'utiliser un serveur de statiques.
* BUG: La minification du HTML était trop agressive, vu qu'elle remplacer certains espaces blancs
  par une chaîne vide.
