TODO
----

- DNS
  * il y a un problème avec www. et /XXX
  * rediriger le mail contact
- màj mail et site web pour V ; supprimer les mails et sites web pour les autres    
- site de démo
  * supprimer entièrement les liens en HTTP 204      
  * désactiver l'inscription, l'envoi de mail. Authentifier automatiquement
  * supprimer SHOWCASE et déterminer via la configuration si on est dans un répertoire virtuel
  * changer "virtualdirectory" en "simone" (chemins dans httpErrors) 
- multi-langues
  * VSModuleContext (activer en localhost) + session lang
  * Use CurrentCulture instead of CurrentUICulture for localization   

- ré-activer robots.txt
- utiliser plutôt Attribute Routing
- finir MailMerge
- rajouter aux package les scripts de déploiement
- encrypter les chaînes de connection
- utiliser des tableaux de caractères plutôt qu'une chaîne de caractères pour le mdp

Mise en production
------------------

1. Mettre à jour la version dans `src\AssemblyInfo.Common.cs` et `VersionInfo.xml`.
2. Lancer `build.cmd`.

Mise à jour des librairies JavaScript
-------------------------------------

Màj des dépendances nodejs:
- `tools\npm-check-updates` pour vérifier la disponibilité de nouvelles versions
- `tools\npm-check-updates -u` pour mettre à jour le fichier `package.json` 
  ou utiliser `npm update --save-dev`

À chaque nouvelle version d'une librairie JavaScript, mettre à jour :
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`
- `src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml`
- `Gruntfile.js`
- `src\assets\js\chiffon.js`
- `src\assets\js\chiffon.jquery.js`
- `src\assets\js\chiffon.views.js`

Pour les feuilles de style, mettre à jour :
- `src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml`

Cas spéciaux:          
- [jQuery](https://jquery.com/download/), télécharger et installer les 3 fichiers 
  jquery.js, jquery.min.js et jquery.min.map.
- Lo-Dash, la mise à jour se fait via nodejs ; ensuite ne pas oublier de modifier
  `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`
  et de supprimer les références à l'ancienne version dans VS.
  ATTENTION: La nouvelle version ne sera visible dans VS qu'après avoir exécuté grunt.

CSS & JavaScript
----------------

- n'utiliser que des classes dans les CSS, éviter les IDs autant que faire se peut ;
- utiliser uniquement des IDs dans les sélecteurs jQuery.

Tests d'interface
-----------------

Analyse des performances en conditions réelles :
- YSlow Extension
- PageSpeed Extension
- [PageSpeed Insights](http://developers.google.com/speed/pagespeed/insights/)

Interface :
- [Modern IE](http://www.modern.ie/)
- [IE Tester](http://my-debugbar.com/wiki/IETester/HomePage)
- [Windows XP Mode](http://windows.microsoft.com/en-us/windows7/products/features/windows-xp-mode)
