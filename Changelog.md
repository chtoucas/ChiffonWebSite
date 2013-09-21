Version 1.0.27
==============

* Validation des formulaires en amont via JavaScript, via jQuery.validate.
* On ne met plus à jour la version de l'assemblée lors de la phase de packaging. 
	Maintenant, il faut lancer manuellement le script PostDeploy.cmd après chaque mise en production.
* Temporisation lors de la prise en charge de l'évènement "resize" sur la fenêtre du navigateur.

Version 1.0.26 (2013-09-19)
===========================

* Au lieu de préciser manuellement le numéro de version des JS et des CSS, on utilise par défaut
	le numéro de version de l'assemblée.
* Il est dorénavant possible d'utiliser un serveur de statiques.
* BUG: La minification du HTML était trop agressive, vu qu'elle remplacer certains espaces blancs
	par une chaîne vide.
