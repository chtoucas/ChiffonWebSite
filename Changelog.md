Version 1.0.27
==============

* Validation des formulaires en amont via JavaScript, via jQuery.validate.
* On ne met plus � jour la version de l'assembl�e lors de la phase de packaging. 
	Maintenant, il faut lancer manuellement le script PostDeploy.cmd apr�s chaque mise en production.
* Temporisation lors de la prise en charge de l'�v�nement "resize" sur la fen�tre du navigateur.

Version 1.0.26 (2013-09-19)
===========================

* Au lieu de pr�ciser manuellement le num�ro de version des JS et des CSS, on utilise par d�faut
	le num�ro de version de l'assembl�e.
* Il est dor�navant possible d'utiliser un serveur de statiques.
* BUG: La minification du HTML �tait trop agressive, vu qu'elle remplacer certains espaces blancs
	par une cha�ne vide.
