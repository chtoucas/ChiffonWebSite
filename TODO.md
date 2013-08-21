TODO
====

CSS & images
============

* nouveau filigramme pour Christine
* contrôle de qualité / h5bp

* vérifier toutes les nuances de gris
* mutualiser les styles pour les input
* supprimer toutes les dimensions fixes
* infos + en-tête en statique
* modales, positionnement centré
* less ou sass, est-ce vraiment nécessaire ?
* créer un sprite pour les 3 images CSS en plus de l'encodage en base64 ?
* feuille de style pour un design responsive
* feuille de style pour l'impression

JS
==

* validation des formulaires
* configurer Google Analytics

* langue par défaut
* modales, ESC -> quitte la modale
* modales, événement clickoutside (via un plugin)
* modales, cliquer dans certains liens devrait laisser l'utilisateur dans la modale
* modales, attention au double clique
* utiliser Zepto ?
* alertes concernant le statut Ajax
* regarder les plugins bootstrap

Déploiement & Co
================

* Azure, paiement
* SqlServer, configurer
* connexion SqlServer
* séparer les configurations dev & prod
* IIS, configurer les logs
* IIS, configurer les noms de domaine sans et avec www, s (statiques) & m (media)
* DNS : déclarer et sans www, en et s
* rediriger wwww
* réécriture quand l'adresse se termine en /

* scripts de compilation et de déploiement en ps1 ou F#
* encrypter la connexion DB
* MvcBuildViews
* script Windows d'optimisation des images
* sortir la compilation des CSS, JS et vues de MSBuild
* utiliser : Google Closure, CSS Lint, JS Lint, Uglify, ES Lint

ASP.NET
=======

* passer les formulaires en helpers avec validation
* vérifier les redirections (correctes + relatives)

* page Newsletter
* nom de la session ASP.NET
* cookies, chemin & persistence
* utiliser l'encodeur AntiXSS
* serveur SMTP
* pages d'erreur MVC & IIS (en particulier pour les images)
* Serilog, enrichir les logs avec du contenu relatif à la requête
* vérifier l'utilisation de RenderPartial (@{})

* PreserveLoginUrl et autres web.config
* vérifier la SEO
* utiliser les bonnes exceptions pour les Enums
* vérifier toutes les exceptions (raison & message)
* vérifier les en-têtes de cache
* enregistrer toutes les visites / connexions
* modales, ChildAction
* Serilog, utiliser des fonctions différéés
* passer les logs en MongoDB
* utiliser T4MVC ?
* utiliser AutoMapper ?
* utiliser HtmlTags ?
* utiliser NodaTime ?
* utiliser Microsoft.Practices.EnterpriseLibrary.Data, EF ?
* basculer AssetTag en méthode d'extension de HtmlHelper ou via ViewPageBase
* OpenGraph & co, microformats
* pagination dans les catégories
* Elmah.MVC
* module pour créer des filigrammes dans les images
* automatiser la correspondance entre RouteTable et SiteMap
* routage dépendant de la langue
* injection de modules HTTP, cf. MVC Turbine ou HttpModuleMagic
* retaillage à la volée de certaines images ?
* asset bundles via l'AssetManager
* vues spécifiques aux tablettes et mobiles
* utiliser des opérations async quand c'est utile

Admin
-----

* WebApi
* AngularJS
* développer un MRP spécifique

DONE
====

2013/08/21
----------

* éléments HTML5 pour IE
* encodeur HTTP qui ne pourrit pas les attributs en UTF-8 -> normalement c'est bon via AntiXSS
