Processus de déploiement
========================


Créer un nouveau milestone
--------------------------

Mettre à jour "etc\Milestone.config" et lancer "scripts\release.ps1 milestone".

Pour une mise à jour majeur :
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <Milestone>Major</Milestone>
</configuration>

Pour une mise à jour mineure :
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <Milestone>Minor</Milestone>
</configuration>

Pour un patch :
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <Milestone>Patch</Milestone>
</configuration>