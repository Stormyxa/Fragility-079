Plugin in use video: https://youtu.be/7QWodQAnxaE?si=VnAlIxtXh0CW00Jx

"Fragility" plugin will force SCP-079 to change his camera to the closest ( most cost-efficient ) after getting his main camera getting shot by a player. After that, by default, the recover time cooldown will be started, meaning the SCP-079 won't be able to swtich back to the broken camera for some amount of time.

By default the recover time equals 10 seconds, but it can be configured in the Text-based Remote Admin by the players who has access to the RA and has "Fragility079" permission in the Exiled permissions file.

The cooldown will change dynamically after settting a new value using the RA. Meaning if you accidently set permanent block, then you can easily change it to 0 seconds ( no blocking at all ) or more seconds to keep the temporary block.

Plugin can be also turned on/off during the round using the 'fragility' ( 'fgl' ).

If you haven't changed anything in your EXILED files, then you will have to put the .dll file from the Git Hub in the following folder:

C:\Users\[User]\AppData\Roaming\EXILED\Plugins

Permissions are managed in the next file:

C:\Users\GamerX\AppData\Roaming\EXILED\Configs\permissions.yml

Example:

user:
  default: true
  inheritance: [ ]
  permissions:
testplugin.user

owner:
  inheritance: [ ]
  permissions:
.*
Fragility079 // Put this in the permission of the certain role you would like to have access to the Fragility config via the RA.

admin:
  inheritance:
moderator
  permissions:
testplugin.admin
testplugin.*

moderator:
  inheritance: [ ]
  permissions:
testplugin.moderator

Thanks for reading! Have good time using the plugin on your server.
