TODO
----

En priorité,
- revoir les bidouilles effectuées spécialement pour la mise en place du site de démo
  * désactiver l'inscription, l'envoi de mail. Authentifier automatiquement
  * changer "virtualdirectory" en "simone" (chemins dans httpErrors)
  * supprimer SHOWCASE et déterminer via la configuration si on est dans un répertoire virtuel
    + inscription / login auto
- ré-activer la gestion des langues multiples :
  * VSModuleContext (activer en localhost) + session lang ;
  * Use CurrentCulture instead of CurrentUICulture for localization.
- DNS, il y a un problème avec www. et /XXX

Travail de fond :
- réduire le nombre des espaces de noms ;
- ré-activer robots.txt ;
- utiliser Attribute Routing ;
- finir `MailMerge` ;
- rajouter les scripts de déploiement aux packages ;
- encrypter les chaînes de connection ;
- utiliser des tableaux de caractères plutôt qu'une chaîne de caractères pour le mdp.

Création d'un package
---------------------

1. mettre à jour la version dans `src\AssemblyInfo.Common.cs` et `VersionInfo.xml` ;
2. lancer `build.cmd`. Le package sera créé dans le répertoire `artefacts`.

Mise à jour des librairies JavaScript
-------------------------------------

Màj des dépendances nodejs:
- `tools\npm-check-updates` pour vérifier la disponibilité de nouvelles versions ;
- `tools\npm-check-updates -u` pour mettre à jour le fichier `package.json`
  puis `npm install --save-dev`. On peut aussi utiliser `npm update --save-dev`.

À chaque nouvelle version d'une librairie JavaScript, si nécessaire, mettre à jour :
- `Gruntfile.js` ;
- `src\assets\js\chiffon.js` ;
- `src\assets\js\chiffon.views.js` ;
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml` ;
- `src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml` ;

et pour les feuilles de style, on fera attention à :
- `src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml`.
Après coup, aller dans Visual Studio, supprimer les anciennes version et
référencer les nouvelles.

Mises à jour via nodejs :
- [Lo-Dash](https://lodash.com/). Attention, la nouvelle version ne sera
  disponible qu'après avoir exécuté grunt.
- [less.js](http://lesscss.org/). Attention, il ne faut pas oublier de faire
  une copie des fichiers dans `assets\vendor`.

Mises à jour manuelles :
- [FastClick](https://github.com/ftlabs/fastclick), uniquement le js.
- [jQuery](https://jquery.com/download/), js, min.js et min.map.
- [jQuery.validate](http://jqueryvalidation.org/), js, min.js et
  les fichiers de langue.
- [jQuery.Waypoints](https://github.com/imakewebthings/waypoints), js et min.js.
- [normalize.css](http://necolas.github.io/normalize.css/)
- [NProgress](http://ricostacruz.com/nprogress/). Attention, il faut mettre
  à jour js et css.

Pas de mises à jour nécessaires :
- [yepnope](http://yepnopejs.com/), _plus de mises à jour disponibles_.
- [H5BP](https://github.com/h5bp/html5-boilerplate), internalisé à partir de la version 4.3.
- jquery.microdata, internalisé (je ne retrouve même pas la référence d'origine).

CSS & JavaScript
----------------

Quelques recommandations :
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
