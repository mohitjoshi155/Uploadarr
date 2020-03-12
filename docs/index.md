# Welcome to Uploadarr

> A full GUI Usenet posting tool, similar to [Sonarr](https://github.com/Sonarr/Sonarr) and [Radarr](https://github.com/Radarr/Radarr) but for posting media.

This document is still a work in progress, feedback is greatly appreciated!

A design will be drafted once the goals and features have been outlined.

## Features

- Cross platform and easy deployment by combining docker and .NET Core.
- Automatic posting of your media collection to Usenet servers
- Periodic check to see if your media has been removed or if it is incomplete, and then re-post the media
- TVDB integration to post with a specific media tags
- Supports obfuscation of posts
- NZBIndexer integration to report any uploads made
- PAR Support and Rar support
- Schedule posting of media any time of the day.
- Integration with Handbrake to convert media to specific formats before posting
- Task scheduler to perform timed actions
- Post subtitle files with the media.
- Integrated VPN

## The Goal

Create an easy to use application to allow the user to post their unique media collection, check if it is already available on Usenet and if not, then post the various media to the usenet servers. This will be an A to Z posting solution, where next to no technical knowledge should be needed to share your media collection with the world. The ease of use is inspired by Sonarr and Radarr.

## Why

Currently posting to Usenet is a cumbersome process that can be largely automated. Think how Sonarr and Radarr revolutionized the scene with the automatic media management features and with a few clicks download all your favorite media. This is what can be accomplished on the usenet posting side of things with an easy to use GUI.

## Problems that are solved

- Provide 1 clear format for naming and posting
- Media will be re-posted if removed or when falling outside the retention date.  

## Security and privacy

It is of the utmost importance that users remain anonymous and secure while using this application, regardless of what they choose to upload. Certain policies will therefore be enforced:

- All uploads must be done behind a VPN/Proxy
- All connections will be done with SSL over HTTPS
- All files posted are striped of any identifying meta data.

## Posting process

The following is done for every single media file, either a movie or a tv-show.

1. Select media file
2. Check naming and if it is valid and understood
3. Send search request to check if it already exists
4. If it exits, is it still complete?
5. If step 3 or 4 is no, then continue
6. Remove meta-data from media file
7. Add media file to a compressed and split zip file
8. Verify zip file integrity
9. Create Par2 file

## Design

COMING SOON - An Adobe XD wire-frame

## Tech

The application will be developed based on the following frameworks:

### Front-end

- Vue.js with SPA Nuxt.js
- Vuetify
- SignalR

### Back-end

- .Net Core Web API
- Docker
- SignalR
- SqlLite
- NLog
- 7Zip - For compression and splitting of media.
  
### Libraries

- [Usenet Poster Library](https://github.com/keimpema/usenet)

### Knowledge Resources

- [Binaries 4 All](https://www.binaries4all.com/index.php) - Explanation of the Usenet posting process
- [Quick guide to posting binary files](http://www.harley.com/usenet/file-sharing/quick-guide-to-posting-binary-files.html) - quick guide of the Usenet posting process

### Future Problems

- [Problem 1](https://old.reddit.com/r/usenet/comments/fh0wct/uploadarr_a_full_gui_usenet_posting_tool_similar/fk9p5jb/)
  
  > One difficulty I foresee is that usenet isn't centralized - depending on what indexer/server pair you use, you can get a very different picture of what is and isn't available/completable on usenet.
  If the chosen indexer(s) is crappy (or the server(s) to check if download is still completable is crappy) that would result in a lot of false positive uploads. Would a bunch of duplicates be super bad? Who knows - and I realize there are currently lots of duplicates on usenet, but I'd image if this tool got widely used, and the conditions above were common, it'd be several magnitudes more duplicates. And on top of that, if someone uses this tool with a server that has frequent/quick takedowns and they keep reuploading stuff, wouldn't that make the indexers and the other servers with less frequent takedowns have a crap ton of duplicates?
  Another problem is obfuscation. My understanding is just about everything these days is posted with some amount of obfuscation stuff without it is taken down almost immediately. So the tool need to obfuscate stuff (in a way that a given indexer could obfuscate).
  Would it make sense to have Uploadarr upload to a specific indexer? Or even have a separate indexer? But like an 'offline' one - I wouldn't care about indexing stuff that's already out there, just a place to throw the nzb files that Uploadarr's users upload. This would solve the first problem and most of the second.
