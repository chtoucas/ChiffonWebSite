CHANGELOG
=========

Version 1.0.28
--------------

* Vrai système de connexion : inscription, connexion.
* Création des mails de service : inscription, rappel de mot de passe.
* Passage en modale des pages de création de compte et de connexion.
* Validation des formulaires en amont via JavaScript, via jQuery.validate.
* Nouveau système de Build : automatisation de la création de packages.
* Site séparé pour la distribution des motifs.
* Temporisation lors de la prise en charge de l'évènement "resize" sur la fenêtre du navigateur.

Version 1.0.26 (2013-09-19)
---------------------------

* Au lieu de préciser manuellement le numéro de version des JS et des CSS, on utilise par défaut
	le numéro de version de l'assemblée.
* Il est dorénavant possible d'utiliser un serveur de statiques.
* BUG: La minification du HTML était trop agressive, vu qu'elle remplacer certains espaces blancs
	par une chaîne vide.
