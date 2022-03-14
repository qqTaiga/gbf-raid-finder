## gbf-raid-finder (WIP)

gbf raid finder is a webapp that allow users to get latest [Granblue Fantasy](https://granbluefantasy.jp/)(GBF) raid bosses in real time.

#### Implementation(TODO: add more details)

1. Use [Twitter filtered stream api](https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/introduction) to get GBF raid bosses help tweets in Twitter.
2. Backend process received tweets to get the raid code(use to enter specific raid boss room in game).
3. Use websocket to send the code to users in real time.

Tech stack:

-   .Net 6 (backend)
-   React (frontend)
