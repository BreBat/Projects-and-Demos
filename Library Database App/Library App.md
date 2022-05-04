# Library Database Interface App
Created for a Database Management Systems class, I worked with a partner to design and implement a simple database for a library to manage books, book copies, members, and dues, along with a desktop application used to interact with said database. I wrote the entire app and worked with my partner (who designed the database itself) to write the appropriate SQL queries for the app to use to communicate with our SQL Server database. 

![The main screen of the library app, showing a table of books known to the system. One book is selected, and a table below the first table shows a list of copies of the selected book.](../_DemoImg/library/booksSelected.png)

The app is able to add and remove books and individual book copies, register or delete members, update relevant information on books (such as condition) or members (like phone numbers or addresses), and check books in or out to members. 

| ![The library app with a large popup window overtop of it, prompting the user to add a book to the database. There are fields for Title, ISBN, the author's name, and an optional call number field. There is a dropdown menu for the book's genre, with "miscellaneous" selected.](../_DemoImg/library/screenAddBook.png) | ![The library app with a another large popup window overtop of it. This one includes fields to register a new library member, asking for a phone number, full name, and address.](../_DemoImg/library/screenAddMember.png) |
|--|--|
|  |  |

All of these actions are reflected in the SQL Server Database and have protections on both the interface end and the database end to avoid creating invalid data.

![The book checkout screen of the library app. An error message is displayed telling the user that the book they just tried to check out is already checked out by another member.](../_DemoImg/library/checkoutErr2.png)

![More screenshots may be seen here.](../_DemoImg/library)