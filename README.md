This is my first attempt to create a project in GitHub.
Its purpose is for me to exercise and work on certain frameworks, programing and architectural topics such as web development, ASP.NET, MVC design pattern, C# language in general, JSON formatting etc.
Being in this exercise context, the project is designed in a way that a lot of different elements are used, meaning it’s not designed necessarily in the simplest and most mainstream way.
The project is a search form for YuGiOh cards (from the known YuGiOh card game). The user can use eight fields that correspond to characteristics of cards. These fields are the name (or part of it), card type, attribute, race, archetype, level, attack and defense. Of course any combination of these characteristics can be used too.
After the user hits the search button, the application connects to this API: https://ygoprodeck.com/api-guide/ and fetches all the cards that match with the chosen characteristics. The cards are presented with all their info inside a table (each line represents one card) and there is also a url for the image of every card.


