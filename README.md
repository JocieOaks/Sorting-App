# Sorting-App
This is a fairly simple ASP.NET Core MVC project I created to familiarize myself with the framework. It is inspired by websites such as TierMaker for making and organizing fun lists. The idea was instead to have a list of items and then sort them by comparing two items one at a time, and then sorting the list by making as few comparisons as possible. It allows for the creation of lists, with each item having the option of including an image and various user specified tags.

## Roadmap
* User registration and making lists only be editable by the user that created them, and having each Sort be specific to a user.
* Automatic Tier List - Taking the sorted list and then finding groupings to place all the items into tiers.
* AND Tagging - Currently using constraining the list of shown elements by tag is additive and functions as an OR statement where any element that has any of the specified tags is shown. I also want to have a functionality where the tags instead require elements to contain all the specified tags.

## License
This project is under an MIT license.
