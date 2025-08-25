# Fragility Plugin
[![Watch on YouTube](https://img.youtube.com/vi/7QWodQAnxaE/hqdefault.jpg)](https://youtu.be/7QWodQAnxaE?si=VnAlIxtXh0CW00Jx)
***
### Plugin Functionality
The "Fragility" plugin will force SCP-079 to change his camera to the closest (most cost-efficient) after his main camera gets shot by a player. After that, by default, the recover time cooldown will be started, meaning the SCP-079 won't be able to switch back to the broken camera for some amount of time.

### Configuration
By default the recover time equals 10 seconds, but it can be configured in the Text-based Remote Admin by players who have access to the RA and have the `Fragility079` permission in the Exiled permissions file.

The cooldown will change dynamically after setting a new value using the RA. This means if you accidentally set a permanent block, you can easily change it to 0 seconds (no blocking at all) or more seconds to keep the temporary block.

The plugin can also be turned on/off during the round using the command `fragility` or `fgl`.

### Installation
If you haven't changed anything in your EXILED files, you will have to put the `.dll` file from the GitHub in the following folder:

C:\Users\[User]\AppData\Roaming\EXILED\Plugins


### Permissions
Permissions are managed in the following file:

C:\Users\[User]\AppData\Roaming\EXILED\Configs\permissions.yml


**Example `permissions.yml` file format:**
```yaml
user:
  default: true
  inheritance: [ ]
  permissions:
    - testplugin.user

owner:
  inheritance: [ ]
  permissions:
    - .*
    - Fragility079 // Put this in the permission of the certain role you would like to have access to the Fragility config via the RA.

admin:
  inheritance: moderator
  permissions:
    - testplugin.admin
    - testplugin.*

moderator:
  inheritance: [ ]
  permissions:
    - testplugin.moderator
Thanks for reading! Have a good time using the plugin on your server.
