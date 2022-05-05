# Car Dealer Inventory Manager
Program written for a Software Architecture & Design Patterns class for practice designing a UI using the Model/View/Controller pattern. The entire program was written using WPF and C#.

| ![The customer interface of the app. The upper third of the window is composed of fields to search a table of vehicles, which appears in the lower two thirds. There are fields for make, model, condition (new/used), and color. There are ranged pairs of fields for mileage, price, and model-year. At the end there is a search button. There are six cars in the list at the bottom, currently unfiltered.](../_DemoImg/dealer/init.png) | ![The same image, but with a popup window named Dealer Login with a username and admin field filled out. The username is admin and the password is 13579. The popup has buttons for Login and Cancel, as well as a help button.](../_DemoImg/dealer/login.png) | ![The Dealer interface of the app. The left two thirds of the window contains the table of cars from the other view. The right side has buttons and fields for adding, editing, and deleting cars from the inventory. The window is currently in "add car" mode, with the relevant fields filled out to add a 1982 Detomaso Pantera. At the bottom are Cancel and Apply buttons.](../_DemoImg/dealer/adminView.png) |
|--|--|--|

The app provides two modes: a customer view and a dealer view. Both views show a list of available vehicles in the inventory. The customer view includes tools to filter the list by various attributes, such as make, model, year, mileage, color, etc. Multiple filters may be used together. Under the file menu is a login option to enter the dealer view (credentials: admin / 13579). The dealer view includes and interface to add, modify, or remove vehicles from the list.

| ![The first image in a series of eight images of the inventory interface, all showing use of the search filters. This image is searching for used cars, from 10k to 200k miles, from 2000 to 2020. There are two results.](../_DemoImg/dealer/filterMulti.png) | ![In this image, the search filter is for white cars. There are two results.](../_DemoImg/dealer/filterColor.png) | ![In this image, the search filter is for new, non-used cars. There are two results.](../_DemoImg/dealer/filterCondition.png) | ![In this image, the search filter is for Hyundais. There is one result.](../_DemoImg/dealer/filterMake.png) |
|--|--|--|--|
| ![In this image, the search filter is for mileage from 100,000 to 200,000. There are four results.](../_DemoImg/dealer/filterMiles.png) | ![In this image, the search filter is for Civics. There is one result.](../_DemoImg/dealer/filterModel.png) | ![In this image, the search filter is for prices from $0 to $10,000. There are three results.](../_DemoImg/dealer/filterPrice.png) | ![In this image, the search filter is cars 1980 to 1999. There are two results.](../_DemoImg/dealer/filterYear.png) |






























