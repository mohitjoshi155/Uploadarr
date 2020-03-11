## Welcome to Uploadarr

> A full GUI Usenet posting tool, similar to [Sonarr](https://link](https://github.com/Sonarr/Sonarr)) and [Radarr](https://github.com/Radarr/Radarr) but for posting media. 

### Features

 - Cross platform and easy deployment with combining docker and .NET Core.
 - Automatic posting of media to usenet servers.
 - TVDB integration to post with a specific meta tags
 - Integration with Handbrake to convert media to specific formats before posting. 
 - Post subtitle files with the media.
 - Supports obfuscation of posts. 
 - NZBIndexer integration to report any uploads made. 
 - PAR Support. 
 - Schedule posting of media any time of the day.


### The Goal

Create an easy to use application to index the media collection of the casual user, check if it is already available on Usenet and if not, then post the various media to the usenet servers. This will be an A to Z posting solution, where next to no technical knowledge should be needed to share your media collection with the world. 

### Why?

Currently posting to Usenet is a cumbersome process that can be largely automated. Think how Sonarr and Radarr revolutionized the scene with the automatic media management features. This is what can be accomplished on the usenet posting side of things with an easy to use GUI.

### Problems that are solved

 - Provide 1 clear format for naming and posting
 - Media will be re-posted if removed or when falling outside the retention date.  

### Design

COMING SOON - An Adobe XD wireframe

### Tech
The application will be developed based on the following technologies: 

#### Front-end
 - Vue.js with SPA Nuxt.js
 - SignalR

#### Back-end
 - .Net Core Web API
 - Docker
 - SignalR
 - SqlLite
 - NLog