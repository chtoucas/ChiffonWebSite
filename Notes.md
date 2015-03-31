TODO
----

- Gandi
  * DNS, il y a un problème avec www. et /XXX
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

Màj package.json:
- `tools\npm-check-updates`
- `npm update --save-dev`

À chaque nouvelle version de yepnope, Lo-Dash ou FastClick, mettre à jour :
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`

Pour less, màj
- `src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml`

Dans tous les cas, màj
- `chiffon.js` & `chiffon.views.js`
- `Gruntfile.js`

Fichiers impactés :
- `Gruntfile.js`
- `src\assets\js\chiffon.js`
- `src\assets\js\chiffon.jquery.js`
- `src\assets\js\chiffon.views.js`
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`
- `src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml`
                                                
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
