# MVC Project
MVC fetch application, search form for YuGiOh cards.

## About the Project
This is my first attempt to create a project in GitHub. Its purpose is for me to practice and work on certain frameworks, programing and architectural topics such as web development, ASP.NET, MVC design pattern, C# language in general, JSON formatting etc. Being in this practice context, the project is designed in a way that a lot of different elements are used, meaning it’s not designed necessarily in the simplest and most mainstream way.

## Functionality 
The project is a search form for YuGiOh cards (from the known YuGiOh card game). The user can use eight fields that correspond to characteristics of cards. These fields are the name (or part of it), card type, attribute, race, archetype, level, attack and defense. Of course any combination of these characteristics can be used too.
![search form](<Yugioh_MVC/Images/Form_1.png>)
After the search button is clicked, the application connects to this API: https://ygoprodeck.com/api-guide/ and fetches all the cards that match the chosen characteristics. The cards are presented with all their info inside a table (each line represents one card) and there is also a url for the image of every card.
![search form](<Yugioh_MVC/Images/Results_2.png>)

## Technical Information
The application fetches information from the API: https://ygoprodeck.com/api-guide/. It is designed in the MVC architectural pattern, which means that the project is separated in Models, Views and the Controller. The Views are the pages that the user interacts with (basicaly the Search Form View and the Results View). The Models are classes that are used as blueprints for the exchange of information between the Controller and the Views and between the Controller and the JSON files. And lastly the Controller has all the logic that happens server side and manages the application’s functionality. Emphasis is also placed on the serialization and deserialization of JSON formatted information. The View files use Razor syntax and the CSS framework Bootstrap.

## Test the App
To test the app download it from code button as a zip.
