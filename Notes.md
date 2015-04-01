Notes de développement
======================

Tâches
------

En priorité,
- revoir les bidouilles effectuées spécialement pour la mise en place du site de démo
  * désactiver l'inscription, l'envoi de mail. Authentifier automatiquement ;
  * changer "virtualdirectory" en "simone" ;
  * supprimer SHOWCASE et changer le conmportement du site en fonction d'un
    paramètre de configuration.
- ré-activer la gestion des langues multiples :
  * VSModuleContext (activer en localhost) + session ;
  * utiliser `CurrentCulture` plutôt que `CurrentUICulture`.
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

1. changer le numéro de version dans `src\AssemblyInfo.Common.cs` et `VersionInfo.xml` ;
2. lancer `build.cmd`. Le package sera créé dans le répertoire `artefacts` ;
3. tagger la nouvelle version.

Mise à jour des librairies JavaScript & CSS
-------------------------------------------

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

En ce qui concerne nodejs :
- lancer `tools\npm-check-updates` pour vérifier la disponibilité de nouvelles versions ;
- puis `tools\npm-check-updates -u` pour mettre à jour le fichier `package.json`
- et enfin `npm install` pour la mise à jour effective.

On peut aussi utiliser `npm update --save-dev`.

Dépendances mises à jour via nodejs :
- [Lo-Dash](https://lodash.com/). Attention, la nouvelle version ne sera
  disponible qu'après avoir exécuté grunt.
- [less.js](http://lesscss.org/). Attention, il ne faut pas oublier de faire
  une copie des fichiers dans `src\Chiffon.WebSite\assets\vendor`.

Dépendances mises à jour manuellement :
- [FastClick](https://github.com/ftlabs/fastclick), uniquement le js ;
- [jQuery](https://jquery.com/download/), les trois fichiers js, min.js et min.map ;
- [jQuery.validate](http://jqueryvalidation.org/), les deux fichiers js, min.js
  ainsi que les fichiers de langue ;
- [NProgress](http://ricostacruz.com/nprogress/). Attention, il faut mettre
  à jour le js et le css ;
- [jQuery.Waypoints](https://github.com/imakewebthings/waypoints), les deux
  fichiers js et min.js ;
- [normalize.css](http://necolas.github.io/normalize.css/).

Dépendances ne nécessitant pas de mises à jour :
- [yepnope](http://yepnopejs.com/), _plus de mises à jour disponibles_ ;
- [H5BP](https://github.com/h5bp/html5-boilerplate), internalisé à partir de la version 4.3 ;
- jquery.microdata, internalisé (je ne retrouve même plus la référence d'origine).

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

Tester l'interface avec des navigateurs désués :
- [Modern IE](http://www.modern.ie/)
- [IE Tester](http://my-debugbar.com/wiki/IETester/HomePage)
- [Windows XP Mode](http://windows.microsoft.com/en-us/windows7/products/features/windows-xp-mode)
