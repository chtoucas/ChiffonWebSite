TODO
====

Divers
======

* configurer l'accès à la boîte mail Gandi

CSS & images
============

* nouveau filigramme pour Christine
* contrôle de qualité / h5bp

* background-color par défaut pour les motifs
* modales, croix en haut à droite pour fermer la modale
* utiliser .designer à la place de #designer
* revoir overlay pour qu'on n'aie pas à ajouter un DIV
* vérifier toutes les nuances de gris
* mutualiser les styles pour les input
* supprimer toutes les dimensions fixes
* infos + en-tête en statique
* modales, positionnement centré
* less ou sass, est-ce vraiment nécessaire ?
* créer un sprite pour les 3 images CSS en plus de l'encodage en base64 ?
* feuille de style pour un design responsive
* feuille de style pour l'impression
* utiliser une image par défaut pour les motifs & vignettes qui n'existent pas

JS
==

* améliorer le processus d'activation de Google Analytics
* validation des formulaires
* langue par défaut
* modales, ESC -> quitte la modale
* modales, événement clickoutside (via un plugin)
* modales, cliquer dans certains liens devrait laisser l'utilisateur dans la modale
* modales, attention au double clique
* utiliser Zepto ?
* alertes concernant le statut Ajax
* regarder les plugins bootstrap
* typescript ?
* charger progressivement les gros catalogues

Déploiement & Co
================

* SqlServer, configurer
* connexion SqlServer
* IIS, configurer les logs
* IIS, configurer les noms de domaine sans et avec www, s (statiques) & m (media)
* rediriger wwww
* réécriture quand l'adresse se termine en /
* problème de conf MSBuild lors de la compilation des assets
* configurer Google Analytics & Google Webmaster tools (lier les comptes ?)

* vérifier l'état du paiement Azure
* DNS : déclarer le sous-domaine des statiques
* permettre aux tâches MSBuild de prendre une liste de fichiers en entrée (Closure, YUI)
* scripts de compilation et de déploiement en ps1 ou F#
* encrypter la connexion DB
* MvcBuildViews
* script Windows d'optimisation des images
* sortir la compilation des CSS, JS et vues de MSBuild
* utiliser : Google Closure, CSS Lint, JS Lint, Uglify, ES Lint

DB
==

* pour vivi, utiliser carr4b comme avatar

Motifs
======

* référencer tous les motifs en version B

HTML
====

* verifier le tag robots

ASP.NET
=======

* prise en charge des vues alternatives pour les motifs

* IsLocalUrl
* publier un sitemap
* passer les formulaires en helpers avec validation
* vérifier les redirections (correctes + relatives)
* account cookie -> HttpOnly
* page Newsletter
* nom de la session ASP.NET
* cookies, chemin & persistence (cf. forms dans web.config)
* serveur SMTP (CritSend / SendGrid or Gandi) http://wiki.gandi.net/en/mail/standard-settings
* pour commencer utiliser MarkdownMailer
* pages d'erreur MVC & IIS (en particulier pour les images)
* Serilog, enrichir les logs avec du contenu relatif à la requête
* vérifier l'utilisation de RenderPartial (@{})
* token de vérification
* OWASP
* utiliser les DisplayModes pour le code spécifique IE ?
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
* Last-Modified-Time & co pour les images
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

Newsletter
==========

* maquette & réalisation
* MailChimp

DONE
====

2013/08/21
----------

* éléments HTML5 pour IE
* encodeur HTTP qui ne pourrit pas les attributs en UTF-8 -> normalement c'est bon via AntiXSS
* utiliser l'encodeur AntiXSS
* background-color par défaut pour les vignettes, on utilise #ddd
* DNS : déclarer les sous-domaine anglais
* lier Google Webmaster Tools
* lier Google Analytics
* Azure, paiement
* séparer les configurations DEV, PROD
* inclure le code Google Analytics
* activer Google Analytics uniquement en production
* machineKey (via IIS)

2013/08/22
----------

* informations humantxt
