# Projet Reseau Rubika 4  

Pitch : Jeux multijoueurs en "monde persistant" ou le but est de pousser les autres dans la lave ou en dehors du terrain.

Petit + personnel : Génération d'un terrain procédural et synchronisation de celui ci par la seed.

Scene : "ExampleScene"

Exemple d'objet synchronisé : Prefab/Character

Testé au maximum sur un réseau local.

## Asynchronisme et Gestion du réseau

Protocole UDP, Serveur multiclients

Abstraction de la couche réseau afin de la rendre modulaire et permettre l'ajout de composants synchronisé par un simple héritage.

Le serveur et le client partagent une grosse partie de code  et hérite du même objet pour leurs particularités.

**Classes intéressantes associées (Dans le dossier Scripts/Network)**

- Network : C'est la classe qui est appellée par le Game Manager pour démarrer le réseau. C'est elle qui est accessible aux autres membres de la scène et qui contient les informations nécessaires (serveur ou client, état de celui ci, etc...)

- NetworkObject : Contient les méthodes permettant d'envoyer ou recevoir des données en asynchrone ainsi que d'éxécuter les données sur le thread principal.

- Client & Server : Héritent de la classe précédent pour implémenter les particularités de chacun.

## Notions d'objets synchronisés

En dehors des paquets "classiques" (connexion, deconnexion etc...) tout est traité par rapport a des objets synchronisés. Chacun de ces objets possèdent un ID unique ainsi qu'un potentiel propriétaire définit par le serveur et transmit aux clients. Ces objets possèdent à leur tour des éléments synchronisés propre qui vont se charger de maintenir la synchronisation des données de l'objet (par exemple un "SynchronizedTransform" pour la position, rotation etc...)

Pour synchroniser des données il suffit donc de créer de nouveaux éléments qui n'auront qu'à implémenter trois fonctions renvoyant respectivement les données à transmettres aux clients depuis le serveur, au possesseur de l'objet depuis le serveur ou du client au serveur depuis le possesseur.

**Classes intéressantes associées (Dans le dossier Scripts/Network/SynchronizedObjects)**

- SynchronizedObject : Classe regroupant les informations réseaux de l'objet (id possesseur). Il a juste besoin d'être ajouté en "Component" sur un objet de la scène pour fonctionner. Il gère les SynchronizedElement.

- SynchronizedElement : Classe abstraite par laquelle va découler tous les différents éléments synchronisés. Elle implémente trois méthodes importantes :
    - SynchronizeFromServer : Créé et envoit la donnée depuis le serveur aux clients
    - SynchronizeFromServerToOwner : Créé en envoit la donnée depuis le serveur au client posesseur de l'objet
    - SynchronizeFromClient : Créé et envoit la données depuis le posseur de l'objet au serveur

- SynchronizedTransform : Un exemple de synchronisation pour le transform d'un objet. Applicable à n'importe quel objet de la scène.

##  Sérialisation des informations à transmettre

J'ai choisit d'abstraire complètement le protocole réseau en passant la sérialisation. C'est à dire que chaque type d'informations (comme le transform par exemple) sera représenté sous forme de classes qui seront toutes hérités du même modèle. Ces classes possèdent des attributs sérialisables qui seront transmisent par le réseau ainsi que des méthodes de validation des données et d'éxécutions qui seront éxécutées de l'autre coté.

Cette méthode me permet pour chaque paquet reçu de m'assurer des types de données que je reçoit (si les types sont incohérents je génère une exception) tout en rendant le processus simple pour n'importe quel type d'information.

**Classes intéressantes associées (Dans le dossier Scripts/Network/Parsing)**

- DataParser, ServerParser & ClientParser : Les classes qui vont s'assurer que les données reçuent sont valides puis éxécuter leurs contenus.

- Data : Classe abstraite représentant n'importe quel type d'information que l'on veut transmettre. Elle implémente trois méthodes importantes :
    - Validate : Permet de vérifier que les données reçus sont cohérentes à ce que l'on attend. Si ce n'est pas le cas on renvoit une exception.
    - Execute : Actions que l'on peut éxécuter sur le thread immédiat. Renvoit "true" s'il y a une action à éxécuter sur le thread principal.
    - ExecuteOnMainThread : Actions qui seront éxécutées sur le thread principal dès que possible.

- ServerData/ServerData & ClientData/ClientData : Classes abstraites héritant de la précédente afin d'implémenter les différentes particularités des paquets envoyés du serveur et du client. 

**Note :** Les classes héritants de "ServerData" sont créées sur **le serveur** et éxécutées sur **le client**. Et inversement. Cela peut paraître parfois illogique.

- ClientData/ConnexionData & ServerData/AcceptConnexionData : Des exemples de paquets "classiques" représentant la connexion et la déconnexion au serveur.

- Les classes SynchronizedObject...Data et SynchronizedElement...Data sont elles des abstractions des informations liées aux objets et éléments synchronisés et qui vont encore une fois abstraire le code communs à toutes les classes qui en héritent.

**Note :** Pour éviter la surcharge d'opérations par frame, seul le SynchronizedElement...Data le plus récent sera gardé en mémoire à l'éxécution du thread principal.

- ServerData/TransformData : Encore une fois l'exemple de la forme qu'à l'information du transform quand il est envoyé du serveur au client.